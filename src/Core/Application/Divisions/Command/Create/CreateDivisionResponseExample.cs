using System;
using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Example response for creating a division.
/// </summary>
public class CreateDivisionResponseExample : IExamplesProvider<CreateDivisionResponse>
{
    public CreateDivisionResponse GetExamples()
    {
        return new CreateDivisionResponse
        {
            Id       = Guid.NewGuid(),
            Name     = "واحد اجرایی",
            ShiftId  = Guid.NewGuid(),
            ParentId = Guid.NewGuid()
        };
    }
}