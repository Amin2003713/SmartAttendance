using Microsoft.Extensions.Localization;
using Shifty.Resources.Common;

namespace Shifty.Resources.Messages
{
    public class ShiftMessages(IStringLocalizer<ShiftMessages> localizer) : BaseLocalizer<ShiftMessages>(localizer)
    {
        public string ALREADY_EXIST => Localize(nameof(ALREADY_EXIST));
        public string FAILED_TO_CREATE => Localize(nameof(FAILED_TO_CREATE));

        public string NAME_IS_REQUIRED => Localize(nameof(NAME_IS_REQUIRED));
        public string NAME_LENGTH => Localize(nameof(NAME_LENGTH));
        public string LEAVE_EARLIER_THAN_ARRIVE => Localize(nameof(LEAVE_EARLIER_THAN_ARRIVE));
        public string GRACE_LATE_ARRIVAL_NEGATIVE => Localize(nameof(GRACE_LATE_ARRIVAL_NEGATIVE));
        public string GRACE_EARLY_LEAVE_NEGATIVE => Localize(nameof(GRACE_EARLY_LEAVE_NEGATIVE));
        public string GRACE_TOTAL_EXCEEDS_PRESENCE => Localize(nameof(GRACE_TOTAL_EXCEEDS_PRESENCE));
        public string PRESENCE_DURATION_INVALID => Localize(nameof(PRESENCE_DURATION_INVALID));
    }
}