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

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// Caching strategy for compositions that will be reused frequently.
    /// Weak or Strong indicates the GC reference strength of the composition in the cache.
    /// </summary>
    public enum CacheStrategy
    {
        /// <summary>
        /// Does not cache the <see cref="LottieComposition"/>
        /// </summary>
        None,

        /// <summary>
        /// Holds a weak reference to the <see cref="LottieComposition"/> once it is loaded and deserialized
        /// </summary>
        Weak,

        /// <summary>
        /// Holds a strong reference to the <see cref="LottieComposition"/> once it is loaded and deserialized
        /// </summary>
        Strong
    }
}