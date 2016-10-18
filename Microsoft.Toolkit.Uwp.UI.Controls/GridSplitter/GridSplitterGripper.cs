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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class GridSplitterGripper : Grid
    {
        // Symbol GripperBarVertical in Segoe MDL2 Assets
        private const string GripperBarVertical = "\xE784";

        // Symbol GripperBarHorizontal in Segoe MDL2 Assets
        private const string GripperBarHorizontal = "\xE76F";
        private const string GripperDisplayFont = "Segoe MDL2 Assets";
        private readonly TextBlock _gripperDisplay;

        internal Brush GripperForeground
        {
            get
            {
                return _gripperDisplay.Foreground;
            }

            set
            {
                _gripperDisplay.Foreground = value;
            }
        }

        internal GridSplitterGripper(
            GridSplitter.GridResizeDirection gridSplitterDirection,
            Brush gripForeground)
        {
            _gripperDisplay = new TextBlock();
            _gripperDisplay.FontFamily = new FontFamily(GripperDisplayFont);
            _gripperDisplay.HorizontalAlignment = HorizontalAlignment.Center;
            _gripperDisplay.VerticalAlignment = VerticalAlignment.Center;
            _gripperDisplay.Foreground = gripForeground;

            if (gridSplitterDirection == GridSplitter.GridResizeDirection.Columns)
            {
                _gripperDisplay.Text = GripperBarVertical;
            }
            else if (gridSplitterDirection == GridSplitter.GridResizeDirection.Rows)
            {
                _gripperDisplay.Text = GripperBarHorizontal;
            }

            Children.Add(_gripperDisplay);
        }
    }
}
