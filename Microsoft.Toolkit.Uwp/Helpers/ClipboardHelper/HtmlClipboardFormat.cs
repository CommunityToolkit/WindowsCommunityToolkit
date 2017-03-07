using System;
using System.Text;

namespace Microsoft.Toolkit.Uwp
{
    public sealed class HtmlClipboardFormat
    {
        private const string EndFragment = "EndFragment:{0:D10}";

        private const string EndHtml = "EndHTML:{0:D10}";

        private const string StartFragment = "StartFragment:{0:D10}";

        private const string StartHtml = "StartHTML:{0:D10}";

        private const int StringBuilderDefaultCapacity = 256;

        private const string Version = "Version:0.9";

        private static readonly string End = "<!--EndFragment-->" + Environment.NewLine + "</body>" + Environment.NewLine + "</html>";

        private static readonly string Start = "<html>" + Environment.NewLine + "<body>" + Environment.NewLine + "<!--StartFragment-->";

        private readonly string _html;

        public HtmlClipboardFormat(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            _html = html;
        }

        public static implicit operator string(HtmlClipboardFormat htmlClipboardFormat)
        {
            if (htmlClipboardFormat == null)
            {
                throw new ArgumentNullException(nameof(htmlClipboardFormat));
            }

            return htmlClipboardFormat.ToString();
        }

        public override string ToString()
        {
            var indexBuilder = new StringBuilder(StringBuilderDefaultCapacity);
            indexBuilder.AppendLine(Version);
            indexBuilder.AppendLine(string.Format(StartHtml, 0));
            indexBuilder.AppendLine(string.Format(EndHtml, 0));
            indexBuilder.AppendLine(string.Format(StartFragment, 0));
            indexBuilder.AppendLine(string.Format(EndFragment, 0));
            var startHtmlIndex = Encoding.UTF8.GetByteCount(indexBuilder.ToString());
            indexBuilder.Append(Start);
            var startFragmentIndex = Encoding.UTF8.GetByteCount(indexBuilder.ToString());
            indexBuilder.Append(_html);
            var endFragmentIndex = Encoding.UTF8.GetByteCount(indexBuilder.ToString());
            indexBuilder.Append(End);
            var endHtmlIndex = Encoding.UTF8.GetByteCount(indexBuilder.ToString());

            var outputBuilder = new StringBuilder(StringBuilderDefaultCapacity);
            outputBuilder.AppendLine(Version);
            outputBuilder.AppendLine(string.Format(StartHtml, startHtmlIndex));
            outputBuilder.AppendLine(string.Format(EndHtml, endHtmlIndex));
            outputBuilder.AppendLine(string.Format(StartFragment, startFragmentIndex));
            outputBuilder.AppendLine(string.Format(EndFragment, endFragmentIndex));
            outputBuilder.Append(Start);
            outputBuilder.Append(_html);
            outputBuilder.Append(End);
            return outputBuilder.ToString();
        }
    }
}