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
    internal class FullPathIterator : PathIterator
    {
        private readonly Path _path;

        private int _index;

        public FullPathIterator(Path path)
        {
            _path = path;
        }

        public override bool Next()
        {
            _index++;
            if (_index > _path.Contours.Count)
            {
                return false;
            }

            return true;
        }

        public override bool Done => _index >= _path.Contours.Count;

        public override ContourType CurrentSegment(float[] points)
        {
            var contour = _path.Contours[_index];

            for (var i = 0; i < contour.Points.Length; i++)
            {
                points[i] = contour.Points[i];
            }

            return contour.Type;
        }
    }
}