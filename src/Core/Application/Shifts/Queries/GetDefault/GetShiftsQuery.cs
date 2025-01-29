using MediatR;
using System.Collections.Generic;

namespace Shifty.Application.Shifts.Queries.GetDefault
{
  public class GetShiftsQuery : IRequest<List<GetShiftsQueryResponse>>;
}