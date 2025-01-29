using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;

/// <summary>
/// Example response for retrieving a list of divisions.
/// </summary>
public class GetDivisionResponseExample : IExamplesProvider<List<GetDivisionResponse>>
{
    public List<GetDivisionResponse> GetExamples()
    {
        return
        [
            new GetDivisionResponse
            {
                Name     = "هیات مدیره" ,
                ShiftId  = null ,
                ParentId = null
            } ,

            new GetDivisionResponse
            {
                Name     = "مدیر عامل" ,
                ShiftId  = null ,
                ParentId = Guid.NewGuid() // Assuming it's linked to "هیات مدیره"
            } ,

            new GetDivisionResponse
            {
                Name     = "معاونت فنی" ,
                ShiftId  = Guid.NewGuid() ,
                ParentId = Guid.NewGuid() // Assuming it's linked to "مدیر عامل"
            } ,

            new GetDivisionResponse
            {
                Name     = "واحد اجرایی" ,
                ShiftId  = Guid.NewGuid() ,
                ParentId = Guid.NewGuid() // Assuming it's linked to "معاونت فنی"
            } ,
        ];
    }
}