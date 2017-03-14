// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Text;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Html clipboard format.
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms649015(v=vs.85).aspx
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlClipboardFormat"/> class.
        /// </summary>
        /// <param name="html">The html string you want to copy into the clipboard.</param>
        /// <exception cref="ArgumentNullException">'html' is null.</exception>
        public HtmlClipboardFormat(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            _html = html;
        }

        /// <summary>
        /// Cast to html clipboard format string.<see cref="ToString"/>
        /// </summary>
        /// <param name="htmlClipboardFormat">Html clipboard format instance.</param>
        public static implicit operator string(HtmlClipboardFormat htmlClipboardFormat)
        {
            if (htmlClipboardFormat == null)
            {
                throw new ArgumentNullException(nameof(htmlClipboardFormat));
            }

            return htmlClipboardFormat.ToString();
        }

        /// <summary>
        /// Cast to html cliboard format string.
        /// </summary>
        /// <returns>The html string in clipboard format.</returns>
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