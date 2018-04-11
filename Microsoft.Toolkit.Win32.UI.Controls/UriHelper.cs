using System;
using System.Text;

namespace Microsoft.Toolkit.Win32.UI.Controls
{
    internal static class UriHelper
    {
        private const int MAX_PATH_LENGTH = 2048;
        private const int MAX_SCHEME_LENGTH = 32;
        public const int MAX_URL_LENGTH = MAX_PATH_LENGTH + MAX_SCHEME_LENGTH + 3; /*=sizeof("://")*/


        /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <see langword="null"/></exception>
        internal static string UriToString(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            return new StringBuilder(
                uri.GetComponents(
                    uri.IsAbsoluteUri ? UriComponents.AbsoluteUri : UriComponents.SerializationInfoString,
                    UriFormat.SafeUnescaped),
                MAX_URL_LENGTH).ToString();

        }
    }
}