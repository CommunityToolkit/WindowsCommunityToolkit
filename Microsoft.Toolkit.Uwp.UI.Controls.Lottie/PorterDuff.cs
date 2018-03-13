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

using Microsoft.Graphics.Canvas;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    /// <summary>
    /// Static PorterDuff class that holds the <see cref="Mode"/> enum.
    /// </summary>
    public static class PorterDuff
    {
        /// <summary>
        /// PorterDuff Mode enum
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Not supported
            /// </summary>
            Clear,

            /// <summary>
            /// Not supported
            /// </summary>
            DstIn,

            /// <summary>
            /// Not supported
            /// </summary>
            DstOut,

            /// <summary>
            /// Only method supported right now.
            /// </summary>
            SrcAtop
        }

        internal static CanvasComposite ToCanvasComposite(Mode mode)
        {
            switch (mode)
            {
                case Mode.SrcAtop:
                    return CanvasComposite.SourceAtop;
                case Mode.DstIn:
                    return CanvasComposite.DestinationIn;
                case Mode.DstOut:
                    return CanvasComposite.DestinationOut;

                // case Mode.Clear:
                default:
                    return CanvasComposite.Copy;
            }
        }
    }
}