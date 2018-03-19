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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A delegate for <see cref="ImageEx"/> failed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void ImageExFailedEventHandler(object sender, ImageExFailedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="ImageEx"/> ImageFailed event.
    /// </summary>
    public class ImageExFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExFailedEventArgs"/> class.
        /// </summary>
        /// <param name="errorException">exception that caused the error condition</param>
        public ImageExFailedEventArgs(Exception errorException)
        {
            ErrorException = errorException;
            ErrorMessage = ErrorException?.Message;
        }

        /// <summary>
        /// Gets the exception that caused the error condition.
        /// </summary>
        public Exception ErrorException { get; private set; }

        /// <summary>
        /// Gets the reason for the error condition.
        /// </summary>
        public string ErrorMessage { get; private set; }
    }
}