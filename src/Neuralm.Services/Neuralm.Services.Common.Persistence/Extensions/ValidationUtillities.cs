using System;

namespace Neuralm.Services.Common.Persistence.Extensions
{
    /// <summary>
    /// Represents the static <see cref="ValidationUtilities"/> class
    /// </summary>
    public static class ValidationUtilities
    {
        /// <summary>
        /// Verifies if the url is valid.
        /// </summary>
        /// <param name="url">The url as string.</param>
        /// <returns>Returns <c>true</c> if the url is valid; otherwise, returns <c>false</c>.</returns>
        public static bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
