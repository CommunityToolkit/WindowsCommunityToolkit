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
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class can set clipboard format easier.
    /// </summary>
    public static class ClipboardHelper
    {
        /// <summary>
        /// Get image bytes from clipboard.
        /// </summary>
        /// <returns>Image.</returns>
        public static async Task<byte[]> GetImageAsync()
        {
            var content = Clipboard.GetContent();
            var streamReference = await content.GetBitmapAsync();
            var stream = await streamReference.OpenReadAsync();
            using (var memoryStream = new MemoryStream())
            {
                await stream.AsStreamForRead().CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Get text from clipboard.
        /// </summary>
        /// <returns>Text.</returns>
        public static async Task<string> GetTextAsync()
        {
            var content = Clipboard.GetContent();
            return await content.GetTextAsync();
        }

        /// <summary>
        /// Set image bytes into clipboard.
        /// </summary>
        /// <param name="image">Image bytes.</param>
        public static void SetImage(byte[] image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            var content = new DataPackage();
            content.SetBitmap(RandomAccessStreamReference.CreateFromStream(new MemoryStream(image).AsRandomAccessStream()));
            Clipboard.SetContent(content);
        }

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

        /// <summary>
        /// Set text into clipboard.
        /// </summary>
        /// <param name="text">Text.</param>
        public static void SetText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var content = new DataPackage();
            content.SetText(text);
            Clipboard.SetContent(content);
        }
    }
}