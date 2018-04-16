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

using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content
{
    internal class MergePaths : IContentModel
    {
        public enum MergePathsMode
        {
            Merge = 1,
            Add = 2,
            Subtract = 3,
            Intersect = 4,
            ExcludeIntersections = 5
        }

        private readonly MergePathsMode _mode;

        public MergePaths(string name, MergePathsMode mode)
        {
            Name = name;
            _mode = mode;
        }

        public virtual string Name { get; }

        internal virtual MergePathsMode Mode => _mode;

        public IContent ToContent(LottieDrawable drawable, BaseLayer layer)
        {
            if (!drawable.EnableMergePaths())
            {
                Debug.WriteLine("Animation contains merge paths but they are disabled.", LottieLog.Tag);
                return null;
            }

            return new MergePathsContent(this);
        }

        public override string ToString()
        {
            return "MergePaths{" + "mode=" + _mode + '}';
        }
    }
}