using System;
using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Divisions.Requests.Create;

/// <summary>
/// Example request for creating a division.
/// </summary>
public class CreateDivisionRequestExample : IExamplesProvider<CreateDivisionRequest>
{
    public CreateDivisionRequest GetExamples()
    {
        return new CreateDivisionRequest
        {
            Name     = "معاونت فنی",
            ShiftId  = Guid.NewGuid(),
            ParentId = Guid.NewGuid()
        };
    }
}