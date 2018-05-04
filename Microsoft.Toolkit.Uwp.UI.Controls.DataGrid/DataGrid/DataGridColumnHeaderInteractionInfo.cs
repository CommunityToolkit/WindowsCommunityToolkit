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

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Primitives
{
    internal class DataGridColumnHeaderInteractionInfo
    {
        internal Pointer CapturedPointer
        {
            get;
            set;
        }

        internal DataGridColumn DragColumn
        {
            get;
            set;
        }

        internal DataGridColumnHeader.DragMode DragMode
        {
            get;
            set;
        }

        internal uint DragPointerId
        {
            get;
            set;
        }

        internal Point? DragStart
        {
            get;
            set;
        }

        internal double FrozenColumnsWidth
        {
            get;
            set;
        }

        internal bool HasUserInteraction
        {
            get
            {
                return this.DragMode != DataGridColumnHeader.DragMode.None;
            }
        }

        internal Point? LastPointerPositionHeaders
        {
            get;
            set;
        }

        internal CoreCursor OriginalCursor
        {
            get;
            set;
        }

        internal double OriginalHorizontalOffset
        {
            get;
            set;
        }

        internal double OriginalWidth
        {
            get;
            set;
        }

        internal Point? PressedPointerPositionHeaders
        {
            get;
            set;
        }

        internal uint ResizePointerId
        {
            get;
            set;
        }
    }
}