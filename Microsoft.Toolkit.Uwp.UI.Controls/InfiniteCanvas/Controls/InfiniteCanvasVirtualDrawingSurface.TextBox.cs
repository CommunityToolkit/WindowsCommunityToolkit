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

using System.Linq;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The virtual Drawing surface renderer used to render the ink and text.
    /// </summary>
    internal partial class InfiniteCanvasVirtualDrawingSurface
    {
        private const int DrawableNullIndex = -1;

        private int _selectedTextDrawableIndex = DrawableNullIndex;

        internal void UpdateSelectedTextDrawableIfSelected(Point point, Rect viewPort)
        {
            for (var i = _drawableList.Count - 1; i >= 0; i--)
            {
                var drawable = _drawableList[i];
                if (drawable is TextDrawable && drawable.IsActive && drawable.Bounds.Contains(point))
                {
                    _selectedTextDrawableIndex = i;
                    return;
                }
            }

            _selectedTextDrawableIndex = DrawableNullIndex;
        }

        internal TextDrawable GetSelectedTextDrawable()
        {
            if (_selectedTextDrawableIndex == DrawableNullIndex)
            {
                return null;
            }

            return (TextDrawable)_drawableList.ElementAt(_selectedTextDrawableIndex);
        }

        internal void ResetSelectedTextDrawable()
        {
            _selectedTextDrawableIndex = DrawableNullIndex;
        }

        internal void UpdateSelectedTextDrawable()
        {
            _selectedTextDrawableIndex = _drawableList.Count - 1;
        }
    }
}
