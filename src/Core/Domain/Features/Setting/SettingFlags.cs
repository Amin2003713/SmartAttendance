using System;

namespace Shifty.Domain.Features.Companies;

[Flags]
public enum SettingFlags
{
    CompanyEnabled = 1 << 0 , 
    InitialStepper = 1 << 1 ,
}

