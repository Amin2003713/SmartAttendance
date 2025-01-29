using MediatR;
using System.Collections.Generic;

namespace Shifty.Application.Shifts.Queries.GetDefault
{
  public class ListShiftsQuery : IRequest<List<ListShiftsQueryResponse>>;
}