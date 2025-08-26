namespace Shifty.Application.Base.Companies.Queries.IsExist;

public record IsCompanyExistByIdQuery(
    string Id
) : IRequest<bool>;