using System;

namespace Shifty.Application.Tenants.Command
{
    public class CreateCompanyResponse
    {
        public string? Id { get; set; } 
        public string? Identifier { get; set; }
        public string? Name { get; set; }
    }
}