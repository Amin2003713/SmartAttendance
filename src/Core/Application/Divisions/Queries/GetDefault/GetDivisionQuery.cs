using System.Collections.Generic;
using MediatR;

namespace Shifty.Application.Divisions.Queries.GetDefault;

public class GetDivisionQuery : IRequest<List<GetDivisionResponse>>;