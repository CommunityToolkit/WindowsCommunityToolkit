using System;
using Windows.ApplicationModel.DataTransfer;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class can set clipboard format easier.
    /// </summary>
    public static class ClipboardHelper
    {
        /// <summary>
        /// Set html into clipboard.
        /// </summary>
        /// <param name="html">Html content.</param>
        public static void SetRawHtml(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            var content = new DataPackage();
            content.SetHtmlFormat(new HtmlClipboardFormat(html));
            Clipboard.SetContent(content);
        }
    }
}