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
    /// Data class describing an image asset exported by bodymovin.
    /// </summary>
    public class LottieImageAsset
    {
        internal LottieImageAsset(int width, int height, string id, string fileName, string dirName)
        {
            Width = width;
            Height = height;
            Id = id;
            FileName = fileName;
            DirName = dirName;
        }

        /// <summary>
        /// Gets the width that the image asset should have
        /// </summary>
        public virtual int Width { get; }

        /// <summary>
        /// Gets the height that the image asset should have
        /// </summary>
        public virtual int Height { get; }

        /// <summary>
        /// Gets the id of the image asset
        /// </summary>
        public virtual string Id { get; }

        /// <summary>
        /// Gets the file name of the image asset
        /// </summary>
        public virtual string FileName { get; }

        /// <summary>
        /// Gets the directory name of the image asset
        /// </summary>
        public virtual string DirName { get; }
    }
}