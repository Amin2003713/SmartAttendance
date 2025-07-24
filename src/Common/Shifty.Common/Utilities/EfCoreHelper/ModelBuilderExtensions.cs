using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pluralize.NET;
using Shifty.Domain.Commons.BaseClasses;

namespace Shifty.Common.Utilities.EfCoreHelper;

public static class ModelBuilderExtensions
{
    public static void AddFeatureBasedSchema(this ModelBuilder modelBuilder, Type filterModel)
    {
        var serviceName = filterModel.Namespace?.Split('.').FirstOrDefault() ?? "dbo";

        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(e =>
                !e.ClrType.IsGenericTypeDefinition &&
                e.ClrType.Assembly == filterModel.Assembly &&
                !e.ClrType.IsDerivedFromGeneric(filterModel)
            )
            .ToList();

        var allNamespaces = entityTypes
            .GroupBy(e => e.ClrType.Namespace ?? "")
            .ToDictionary(g => g.Key, g => g.ToList());

        foreach (var entity in entityTypes)
        {
            var ns              = entity.ClrType.Namespace ?? "";
            var schemaName      = FindSchemaByClimbing(ns, allNamespaces, serviceName);
            var snakeCaseSchema = ToSnakeCase(schemaName);
            entity.SetSchema(snakeCaseSchema);
        }

        return;

        static string FindSchemaByClimbing(
            string ns,
            Dictionary<string, List<IMutableEntityType>> nsEntities,
            string serviceName)
        {
            var parts = ns.Split('.');

            for (var i = parts.Length; i > 0; i--)
            {
                var currentNs = string.Join('.', parts.Take(i));
                var current   = parts[i - 1];

                if (string.Equals(current, "Domain", StringComparison.OrdinalIgnoreCase))
                    continue;

                var entityCount = nsEntities
                    .Where(kvp => kvp.Key.StartsWith(currentNs + ".") || kvp.Key == currentNs)
                    .SelectMany(kvp => kvp.Value)
                    .Count();

                if (entityCount > 1)
                    return current;
            }

            return serviceName;
        }

        static string ToSnakeCase(string input)
        {
            return string.Concat(
                    input.Select((c, i) =>
                        i > 0 && char.IsUpper(c) ? "_" + c : c.ToString()))
                .ToLowerInvariant();
        }
    }

    private static bool IsDerivedFromGeneric(this Type type, Type genericBaseType)
    {
        while (type != null && type != typeof(object))
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBaseType)
                return true;

            type = type.BaseType;
        }

        return false;
    }

    /// <summary>
    ///     Adds a global query filter for all entities implementing IEntity to include only active records (IsActive = true).
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddGlobalIsActiveFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            // فقط روی entityهایی که root هستند (یعنی base type ندارند یا base آن‌ها abstract است)
            if (clrType.BaseType != null &&
                typeof(IEntity).IsAssignableFrom(clrType) &&
                typeof(IEntity).IsAssignableFrom(clrType.BaseType))
                // موجودیت مشتق‌شده است، ادامه نده
                continue;

            var isActiveProperty  = clrType.GetProperty("IsActive",  BindingFlags.Public | BindingFlags.Instance);
            var isDeletedProperty = clrType.GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance);

            // اگر هیچ‌کدام وجود نداشتند، ادامه نده
            if (isActiveProperty == null && isDeletedProperty == null)
                continue;

            var         parameter = Expression.Parameter(clrType, "e");
            Expression? condition = null;

            if (isActiveProperty is { PropertyType: var type } && type == typeof(bool))
            {
                var isActiveExpression = Expression.Equal(
                    Expression.Property(parameter, isActiveProperty),
                    Expression.Constant(true));

                condition = isActiveExpression;
            }

            if (isDeletedProperty is { PropertyType: var type2 } && type2 == typeof(bool))
            {
                var isDeletedExpression = Expression.Equal(
                    Expression.Property(parameter, isDeletedProperty),
                    Expression.Constant(false));

                condition = condition != null
                    ? Expression.AndAlso(condition, isDeletedExpression)
                    : isDeletedExpression;
            }

            if (condition != null)
            {
                var lambda = Expression.Lambda(condition, parameter);
                modelBuilder.Entity(clrType).HasQueryFilter(lambda);
            }
        }
    }

    /// <summary>
    ///     Set DeleteBehavior.Restrict by default for relations
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void AddRestrictDeleteBehaviorConvention(this ModelBuilder modelBuilder)
    {
        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    /// <summary>
    ///     Dynamicaly load all IEntityTypeConfiguration with Reflection
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterEntityTypeConfiguration(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        var applyGenericMethod = typeof(ModelBuilder).GetMethods()
            .First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration));

        var types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic);

        foreach (var type in types)
        {
            foreach (var iface in type.GetInterfaces())
            {
                if (iface.IsConstructedGenericType &&
                    iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                {
                    var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);

                    applyConcreteMethod.Invoke(modelBuilder,
                        new[]
                        {
                            Activator.CreateInstance(type)
                        });
                }
            }
        }
    }

    /// <summary>
    ///     Dynamicaly register all Entities that inherit from specific BaseType
    /// </summary>
    /// <param name="modelBuilder"></param>
    /// <param name="baseType">Base type that Entities inherit from this</param>
    /// <param name="assemblies">Assemblies contains Entities</param>
    public static void RegisterAllEntities(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        var excludedNames = new[]
        {
            "IEntity",
            "ApplicationBaseEntities",
            "BaseEntity",
            "BaseEntities"
        };

        var types = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c =>
                c.IsClass &&
                !c.IsAbstract &&
                c.IsPublic &&
                typeof(IEntity).IsAssignableFrom(c) &&
                !excludedNames.Contains(c.Name) &&
                !c.Name.Contains("BaseEntity") &&
                !c.Name.Contains("BaseEntities")
            );

        foreach (var type in types)
        {
            modelBuilder.Entity(type);
        }
    }

    public static void AddPluralizingTableNameConvention(this ModelBuilder modelBuilder)
    {
        var pluralizer = new Pluralizer();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            entityType.SetTableName(pluralizer.Pluralize(tableName));
        }
    }

    public static void AddDecimalConvention(this ModelBuilder modelBuilder)
    {
        var decimalProps = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

        foreach (var property in decimalProps)
        {
            property.SetPrecision(18);
            property.SetScale(4);
        }
    }
}