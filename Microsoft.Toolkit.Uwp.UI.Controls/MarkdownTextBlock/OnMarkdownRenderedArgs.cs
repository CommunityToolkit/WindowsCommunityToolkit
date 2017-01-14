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
    /// Arguments for the OnMarkdownRendered event which indicates when the markdown has been
    /// rendered.
    /// </summary>
    public class OnMarkdownRenderedArgs : EventArgs
    {
        internal OnMarkdownRenderedArgs(bool hadError, Exception ex)
        {
            HadError = hadError;
            Exception = ex;
        }

        /// <summary>
        /// Gets a value indicating whether there was an error with the markdown.
        /// </summary>
        public bool HadError { get; }

        /// <summary>
        /// Gets the exception if there was one.
        /// </summary>
        public Exception Exception { get; }
    }
}
