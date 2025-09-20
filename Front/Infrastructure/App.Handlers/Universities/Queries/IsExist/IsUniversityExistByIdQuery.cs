namespace App.Handlers.Universities.Queries.IsExist;

public record IsUniversityExistByIdQuery(
    string Id
) : IRequest<bool>;