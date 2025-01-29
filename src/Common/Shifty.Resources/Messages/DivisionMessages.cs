using Microsoft.Extensions.Localization;
using Shifty.Resources.Common;

namespace Shifty.Resources.Messages
{

    public class DivisionMessages(IStringLocalizer<DivisionMessages> localizer) : BaseLocalizer<DivisionMessages>(localizer)
    {
        public string ALREADY_EXIST => Localize(nameof(ALREADY_EXIST));
        public string FAILED_TO_CREATE => Localize(nameof(FAILED_TO_CREATE));

        public string NAME_IS_REQUIRED => Localize(nameof(NAME_IS_REQUIRED));
        public string NAME_LENGTH => Localize(nameof(NAME_LENGTH));

        public string PARENT_ID_INVALID => Localize(nameof(PARENT_ID_INVALID));
        public string PARENT_NOT_FOUND => Localize(nameof(PARENT_NOT_FOUND));

        public string SHIFT_ID_INVALID => Localize(nameof(SHIFT_ID_INVALID));
        public string SHIFT_NOT_FOUND => Localize(nameof(SHIFT_NOT_FOUND));
    }

}