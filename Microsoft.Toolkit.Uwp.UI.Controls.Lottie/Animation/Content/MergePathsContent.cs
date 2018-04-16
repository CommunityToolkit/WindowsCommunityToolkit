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
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Content;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Animation.Content
{
    internal class MergePathsContent : IPathContent, IGreedyContent
    {
        private readonly Path _firstPath = new Path();
        private readonly Path _remainderPath = new Path();
        private readonly Path _path = new Path();

        private readonly List<IPathContent> _pathContents = new List<IPathContent>();
        private readonly MergePaths _mergePaths;

        internal MergePathsContent(MergePaths mergePaths)
        {
            Name = mergePaths.Name;
            _mergePaths = mergePaths;
        }

        public void AbsorbContent(List<IContent> contents)
        {
            var index = contents.Count;

            // Fast forward the iterator until after this content.
            while (index > 0)
            {
                index--;
                if (contents[index] == this)
                {
                    break;
                }
            }

            while (index > 0)
            {
                index--;
                var content = contents[index];
                if (content is IPathContent pathContent)
                {
                    _pathContents.Add(pathContent);
                    contents.RemoveAt(index);
                }
            }
        }

        public void SetContents(List<IContent> contentsBefore, List<IContent> contentsAfter)
        {
            for (var i = 0; i < _pathContents.Count; i++)
            {
                _pathContents[i].SetContents(contentsBefore, contentsAfter);
            }
        }

        public virtual Path Path
        {
            get
            {
                _path.Reset();

                switch (_mergePaths.Mode)
                {
                    case MergePaths.MergePathsMode.Merge:
                        AddPaths();
                        break;
                    case MergePaths.MergePathsMode.Add:
                        OpFirstPathWithRest(CanvasGeometryCombine.Union);
                        break;
                    case MergePaths.MergePathsMode.Subtract:
                        OpFirstPathWithRest(CanvasGeometryCombine.Exclude);
                        break;
                    case MergePaths.MergePathsMode.Intersect:
                        OpFirstPathWithRest(CanvasGeometryCombine.Intersect);
                        break;
                    case MergePaths.MergePathsMode.ExcludeIntersections:
                        OpFirstPathWithRest(CanvasGeometryCombine.Xor);
                        break;
                }

                return _path;
            }
        }

        public string Name { get; }

        private void AddPaths()
        {
            for (var i = 0; i < _pathContents.Count; i++)
            {
                _path.AddPath(_pathContents[i].Path);
            }
        }

        private void OpFirstPathWithRest(CanvasGeometryCombine op)
        {
            _remainderPath.Reset();
            _firstPath.Reset();

            for (var i = _pathContents.Count - 1; i >= 1; i--)
            {
                var content = _pathContents[i];

                if (content is ContentGroup contentGroup)
                {
                    var pathList = contentGroup.PathList;
                    for (var j = pathList.Count - 1; j >= 0; j--)
                    {
                        var path = pathList[j].Path;
                        path.Transform(contentGroup.TransformationMatrix);
                        _remainderPath.AddPath(path);
                    }
                }
                else
                {
                    _remainderPath.AddPath(content.Path);
                }
            }

            var lastContent = _pathContents[0];
            if (lastContent is ContentGroup group)
            {
                var pathList = group.PathList;
                for (var j = 0; j < pathList.Count; j++)
                {
                    var path = pathList[j].Path;
                    path.Transform(group.TransformationMatrix);
                    _firstPath.AddPath(path);
                }
            }
            else
            {
                _firstPath.Set(lastContent.Path);
            }

            _path.Op(_firstPath, _remainderPath, op);
        }
    }
}