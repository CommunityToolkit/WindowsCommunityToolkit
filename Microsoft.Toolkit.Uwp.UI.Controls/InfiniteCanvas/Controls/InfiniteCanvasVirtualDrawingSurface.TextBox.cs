using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class InfiniteCanvasVirtualDrawingSurface
    {
        private const int DrawableNullIndex = -1;

        private int _selectedTextDrawableIndex = DrawableNullIndex;

        internal void UpdateSelectedTextDrawableIfSelected(Point point, Rect viewPort)
        {
            for (var i = _drawableList.Count - 1; i >= 0; i--)
            {
                var drawable = _drawableList[i];
                if (drawable is TextDrawable && drawable.Bounds.Contains(point))
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
