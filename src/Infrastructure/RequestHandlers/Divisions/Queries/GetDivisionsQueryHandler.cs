using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shifty.Application.Divisions.Queries.GetDefault;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Features.Divisions.Queries;

namespace Shifty.RequestHandlers.Divisions;

public class GetDivisionsQueryHandler(IDivisionQueriesRepository repository , ILogger<GetDivisionsQueryHandler> logger) : IRequestHandler<GetDivisionQuery , List<GetDivisionResponse>>
{
    public async Task<List<GetDivisionResponse>> Handle(GetDivisionQuery request , CancellationToken cancellationToken)
    {
        try
        {
            return (await repository.TableNoTracking.ToListAsync(cancellationToken: cancellationToken)).Adapt<List<GetDivisionResponse>>();
        }
        catch (ShiftyException e)
        {
            logger.LogError(e.Source , e);
            throw;
        }
    }
}