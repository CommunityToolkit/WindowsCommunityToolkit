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
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Html;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class can set clipboard format easier.
    /// </summary>
    public static class ClipboardHelper
    {
        /// <summary>
        /// Get html from clipboard.
        /// </summary>
        /// <returns>The html string.</returns>
        public static async Task<string> GetHtmlAsync()
        {
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Html))
            {
                string htmlFormat;
                try
                {
                    htmlFormat = await dataPackageView.GetHtmlFormatAsync();
                }
                catch (ArgumentException)
                {
                    // if the clipboard html format is empty string.
                    return null;
                }

                return HtmlFormatHelper.GetStaticFragment(htmlFormat);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get image bytes from clipboard.
        /// </summary>
        /// <returns>The image bytes.</returns>
        public static async Task<byte[]> GetImageAsync()
        {
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Bitmap))
            {
                var imageReceived = await dataPackageView.GetBitmapAsync();
                using (var imageStream = await imageReceived.OpenReadAsync())
                {
                    var bytes = new byte[imageStream.Size];
                    await imageStream.ReadAsync(bytes.AsBuffer(), (uint)imageStream.Size, InputStreamOptions.None);
                    return bytes;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get rtf format text from clipboard.
        /// </summary>
        /// <returns>The rtf format text.</returns>
        public static async Task<string> GetRtfAsync()
        {
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Rtf))
            {
                try
                {
                    return await dataPackageView.GetRtfAsync();
                }
                catch (ArgumentException)
                {
                    // if the clipboard rtf is empty string.
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get text from clipboard.
        /// </summary>
        /// <returns>The text string.</returns>
        public static async Task<string> GetTextAsync()
        {
            var dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.Text))
            {
                return await dataPackageView.GetTextAsync();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Set html into clipboard.
        /// </summary>
        /// <param name="html">The html string.</param>
        /// <exception cref="ArgumentNullException">'html' is null.</exception>
        /// <exception cref="ArgumentException">'html' length is not valid.</exception>
        public static void SetHtml(string html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }
            if (html.Length < 4)
            {
                throw new ArgumentException("html length is not valid.", nameof(html));
            }

            var dataPackage = new DataPackage();
            var htmlFormat = HtmlFormatHelper.CreateHtmlFormat(html);
            var plainText = HtmlUtilities.ConvertToText(html);
            dataPackage.SetHtmlFormat(htmlFormat);
            dataPackage.SetText(plainText);
            Clipboard.SetContent(dataPackage);
            Clipboard.Flush();
        }

        /// <summary>
        /// Set image file into clipboard.
        /// </summary>
        /// <param name="file">The image file.</param>
        /// <exception cref="ArgumentNullException">'image' is null.</exception>
        public static void SetImageFromFile(IStorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var dataPackage = new DataPackage();
            var imageStreamReference = RandomAccessStreamReference.CreateFromFile(file);
            dataPackage.SetBitmap(imageStreamReference);
            Clipboard.SetContent(dataPackage);
            Clipboard.Flush();
        }

        /// <summary>
        /// Set rtf format text into clipboard.
        /// </summary>
        /// <param name="rtf">The rtf format text.</param>
        /// <exception cref="ArgumentNullException">'rtf' is null.</exception>
        /// <exception cref="ArgumentException">'rtf' is empty string.</exception>
        public static void SetRtf(string rtf)
        {
            if (rtf == null)
            {
                throw new ArgumentNullException(nameof(rtf));
            }
            if (rtf.Length <= 0)
            {
                throw new ArgumentException("rtf is empty string.", nameof(rtf));
            }

            var dataPackage = new DataPackage();
            dataPackage.SetRtf(rtf);
            Clipboard.SetContent(dataPackage);
            Clipboard.Flush();
        }

        /// <summary>
        /// Set text into clipboard.
        /// </summary>
        /// <param name="text">The text string.</param>
        /// <exception cref="ArgumentNullException">'text' is null.</exception>
        /// <exception cref="ArgumentException">'text' is empty string.</exception>
        public static void SetText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (text.Length <= 0)
            {
                throw new ArgumentException("text is empty string.", nameof(text));
            }

            var dataPackage = new DataPackage();
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
            Clipboard.Flush();
        }
    }
}