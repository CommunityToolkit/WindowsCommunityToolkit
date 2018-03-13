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

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content
{
    /// <summary>
    /// Content that may want to absorb and take ownership of the content around it.
    /// For example, merge paths will absorb the shapes above it and repeaters will absorb the content
    /// above it.
    /// </summary>
    internal interface IGreedyContent
    {
        /// <summary>
        /// An iterator of contents that can be used to take ownership of contents. If ownership is taken,
        /// the content should be removed from the iterator.
        ///
        /// The contents should be iterated by calling hasPrevious() and previous() so that the list of
        /// contents is traversed from bottom to top which is the correct order for handling AE logic.
        /// </summary>
        void AbsorbContent(List<IContent> contents);
    }
}
