using Microsoft.Extensions.Localization;

namespace Shifty.Resources.ExceptionMessages.Common
{
    public class BaseLocalizer<TLocalizer>(IStringLocalizer<TLocalizer> localizer)
    {
        /// <summary>
        /// Localizes a message based on the provided key.
        /// </summary>
        /// <param name="key">The resource key for the message.</param>
        /// <param name="args">Optional arguments for formatting the message.</param>
        /// <returns>The localized string.</returns>
        public string Localize(string key , params object[] args)
        {
            var aa = localizer.GetAllStrings();
            var a  = localizer[key , args];
            return a;
        }
    }
}