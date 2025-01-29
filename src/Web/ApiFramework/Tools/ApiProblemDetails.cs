using System.Collections.Generic;

namespace Shifty.ApiFramework.Tools
{
    public class ApiProblemDetails
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public object Detail { get; set; }
        public Dictionary<string , List<string>> Errors { get; set; } = null!;
    }
}