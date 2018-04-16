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

using System;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Lottie
{
    internal static class RectExt
    {
        public static void Set(ref Rect rect, double left, double top, double right, double bottom)
        {
            rect.X = left;
            rect.Y = top;
            rect.Width = Math.Abs(right - left);
            rect.Height = Math.Abs(bottom - top);
        }

        public static void Set(ref Rect rect, Rect newRect)
        {
            rect.X = newRect.X;
            rect.Y = newRect.Y;
            rect.Width = newRect.Width;
            rect.Height = newRect.Height;
        }
    }
}
