using Microsoft.Extensions.Localization;
using Shifty.Resources.Common;

namespace Shifty.Resources.Messages
{
    public class DivisionMessages(IStringLocalizer<DivisionMessages> localizer) : BaseLocalizer<DivisionMessages>(localizer);
}