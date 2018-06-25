// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Drawing;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.Win32
{
  internal struct RECT
  {
    public int bottom;
    public int left;
    public int right;
    public int top;

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

    public Size Size
    {
      get
      {
        return new Size(this.right - this.left, this.bottom - this.top);
      }
    }

    public static RECT FromXYWH(int x, int y, int width, int height)
    {
      return new RECT(x, y, x + width, y + height);
    }
  }
}