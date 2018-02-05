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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse
{
    /// <summary>
    /// Implemented by all inline link elements.
    /// </summary>
    internal interface ILinkElement
    {
        /// <summary>
        /// Gets the link URL.  This can be a relative URL, but note that subreddit links will always
        /// have the leading slash (i.e. the Url will be "/r/baconit" even if the text is
        /// "r/baconit").
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Gets a tooltip to display on hover.  Can be <c>null</c>.
        /// </summary>
        string Tooltip { get; }
    }
}
