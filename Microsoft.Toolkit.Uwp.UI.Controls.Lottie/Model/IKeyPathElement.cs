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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Value;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model
{
    /// <summary>
    /// Any item that can be a part of a <see cref="KeyPath"/> should implement this.
    /// </summary>
    internal interface IKeyPathElement
    {
        /// <summary>
        /// Called recursively during keypath resolution.
        ///
        /// The overridden method should just call:
        ///     MiscUtils.ResolveKeyPath(keyPath, depth, accumulator, currentPartialKeyPath, this);
        /// </summary>
        /// <param name="keyPath">The full keypath being resolved.</param>
        /// <param name="depth">The current depth that this element should be checked at in the keypath.</param>
        /// <param name="accumulator">A list of fully resolved keypaths. If this element fully matches the keypath then it should add itself to this list.</param>
        /// <param name="currentPartialKeyPath">A keypath that contains all parent element of this one. This element should create a copy of this and append itself with KeyPath#addKey when it adds itself to the accumulator or propagates resolution to its children.</param>
        void ResolveKeyPath(KeyPath keyPath, int depth, List<KeyPath> accumulator, KeyPath currentPartialKeyPath);

        /// <summary>
        /// The overridden method should handle appropriate properties and set value callbacks on their
        /// animations.
        /// </summary>
        /// <typeparam name="T">The type that the callback will work on.</typeparam>
        /// <param name="property">The <see cref="LottieProperty"/> that the callback is listening to.</param>
        /// <param name="callback">The <see cref="ILottieValueCallback{T}"/> to add to the current <see cref="IKeyPathElement"/></param>
        void AddValueCallback<T>(LottieProperty property, ILottieValueCallback<T> callback);
    }
}
