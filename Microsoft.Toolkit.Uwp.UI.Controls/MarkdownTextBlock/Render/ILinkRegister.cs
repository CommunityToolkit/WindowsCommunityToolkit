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

using Windows.UI.Xaml.Documents;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render
{
    /// <summary>
    /// An interface used to handle links in the markdown.
    /// </summary>
    public interface ILinkRegister
    {
        /// <summary>
        /// Registers a Hyperlink with a LinkUrl.
        /// </summary>
        /// <param name="newHyperlink">Hyperlink to Register.</param>
        /// <param name="linkUrl">Url to Register.</param>
        void RegisterNewHyperLink(Hyperlink newHyperlink, string linkUrl);
    }
}