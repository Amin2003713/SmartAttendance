using Mapster;
using SmartAttendance.Application.Base.Companies.Queries.IsExist;
using SmartAttendance.Application.Base.Discounts.Commands.CreateDiscount;
using SmartAttendance.Application.Interfaces.Tenants.Discounts;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Tenants.Discounts;

namespace SmartAttendance.RequestHandlers.Base.Discounts.Commands.CreateDiscount;

public class CreateDiscountCommandHandler(
    IDiscountCommandRepository discountCommandRepository,
    IDiscountQueryRepository discountQueryRepository,
    IMediator mediator,
    ILogger<CreateDiscountCommandHandler> logger,
    IStringLocalizer<CreateDiscountCommandHandler> localizer
)
    : IRequestHandler<CreateDiscountCommand>
{
    public async Task Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting creation of discount with Code {DiscountCode}.", request.Code);

        // Check if discount already exists.
        if (await discountQueryRepository.DiscountExists(request.Code, cancellationToken))
        {
            logger.LogWarning("Discount with Code {DiscountCode} already exists.", request.Code);
            throw SmartAttendanceException.Conflict(localizer["Discount already exists."].Value);
        }

        // Adapt the incoming request to a Discount domain entity.
        var discount = request.Adapt<Discount>();

        // Associate each tenant company with the discount.
        foreach (var tenantId in request.TenantIds)
        {
            // Check for company existence.
            if (!await mediator.Send(new IsCompanyExistByIdQuery(tenantId), cancellationToken))
            {
                logger.LogWarning("Tenant company with Id {TenantId} not found.", tenantId);
                throw SmartAttendanceException.NotFound(localizer["Tenant company not found."].Value);
            }

            discount.TenantDiscount.Add(new TenantDiscount
            {
                TenantId = tenantId,
                DiscountId = discount.Id,
                IsUsed = false
            });
        }

        // If no tenant is specified, the discount is not private.
        if (request.TenantIds.Count == 0) discount.IsPrivate = false;

        await discountCommandRepository.Add(discount, cancellationToken);
        logger.LogInformation("Discount with Code {DiscountCode} created successfully.", request.Code);
    }
}