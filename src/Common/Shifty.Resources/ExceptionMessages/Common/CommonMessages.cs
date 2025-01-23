using Microsoft.Extensions.Localization;
using Shifty.Resources.Resources.Common;

namespace Shifty.Resources.ExceptionMessages.Common
{
    public class CommonMessages(IStringLocalizer<CommonResources> localizer) : BaseLocalizer<CommonResources>(localizer)

    {
        public   string Code_Generator()=> Localize(nameof(Code_Generator));
        public   string Server_Error() => Localize(nameof(Server_Error));
        public  string Refresh_Token_Found() => Localize(nameof(Refresh_Token_Found));
        public   string Unauthorized_Access() => Localize(nameof(Unauthorized_Access));
    }
}