namespace Shifty.Application.Companies.Queries.IsExist;

public record IsCompanyExistByIdQuery(
    string Id
) : IRequest<bool>;