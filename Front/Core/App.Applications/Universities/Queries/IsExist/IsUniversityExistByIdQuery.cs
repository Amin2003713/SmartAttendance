using MediatR;

namespace App.Applications.Universities.Queries.IsExist;

public record IsUniversityExistByIdQuery(
    string Id
) : IRequest<bool>;