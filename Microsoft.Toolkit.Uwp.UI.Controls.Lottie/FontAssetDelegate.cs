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
    /// Delegate to handle the loading of fonts that are not packaged in the assets of your app or don't
    /// have the same file name.
    /// </summary>
    /// <seealso cref="LottieDrawable.FontAssetDelegate"></seealso>
    public abstract class FontAssetDelegate
    {
        /// <summary>
        /// Override this if you want to return a Typeface from a font family.
        /// </summary>
        /// <returns>The <see cref="Typeface"/> to be used for the specified fontFamily</returns>
        public abstract Typeface FetchFont(string fontFamily);

        /// <summary>
        /// Override this if you want to specify the asset path for a given font family.
        /// </summary>
        /// <returns>The path of the font to be loaded for the specified fontFamily</returns>
        public abstract string GetFontPath(string fontFamily);
    }
}
