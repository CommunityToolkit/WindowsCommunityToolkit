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

using System.Drawing;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
  public struct RECT
  {
    public int left;
    public int top;
    public int right;
    public int bottom;

    public RECT(int left, int top, int right, int bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    public RECT(Rectangle r)
    {
      this.left = r.Left;
      this.top = r.Top;
      this.right = r.Right;
      this.bottom = r.Bottom;
    }

    public static RECT FromXYWH(int x, int y, int width, int height)
    {
      return new RECT(x, y, x + width, y + height);
    }

    public Size Size
    {
      get
      {
        return new Size(this.right - this.left, this.bottom - this.top);
      }
    }
  }
}
