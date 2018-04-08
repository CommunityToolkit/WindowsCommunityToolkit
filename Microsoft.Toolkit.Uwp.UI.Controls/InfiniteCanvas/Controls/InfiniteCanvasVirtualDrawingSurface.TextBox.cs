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
            for (var i = _visibleList.Count - 1; i >= 0; i--)
            {
                var drawable = _visibleList[i];
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

            return (TextDrawable)_visibleList.ElementAt(_selectedTextDrawableIndex);
        }

        internal void ResetSelectedTextDrawable()
        {
            _selectedTextDrawableIndex = DrawableNullIndex;
        }

        internal void UpdateSelectedTextDrawable()
        {
            _selectedTextDrawableIndex = _visibleList.Count - 1;
        }
    }
}
