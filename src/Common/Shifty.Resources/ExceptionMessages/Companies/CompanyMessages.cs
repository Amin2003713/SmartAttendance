using Microsoft.Extensions.Localization;
using Shifty.Resources.ExceptionMessages.Common;
using Shifty.Resources.Resources.Companies;

namespace Shifty.Resources.ExceptionMessages.Companies;

public class CompanyMessages(IStringLocalizer<CompanyResources> localizer) : BaseLocalizer<CompanyResources>(localizer)
{

    public string Company_Admin_Not_Created()=>Localize(nameof(Company_Admin_Not_Created));
    public string Tenant_Exists()=>Localize(nameof(Tenant_Exists));
    public string Company_Not_Created()=>Localize(nameof(Company_Not_Created));
    public string Company_Not_Found()=>Localize(nameof(Company_Not_Found));
    public string Tenant_Is_Valid()=>Localize(nameof(Tenant_Is_Valid));
    public string Tenant_Is_Not_Valid()=>Localize(nameof(Tenant_Is_Not_Valid));
    public string Tenant_Admin_Not_Found()=>Localize(nameof(Tenant_Admin_Not_Found));
    public string Tenant_Admin_Not_Created()=>Localize(nameof(Tenant_Admin_Not_Created));

}