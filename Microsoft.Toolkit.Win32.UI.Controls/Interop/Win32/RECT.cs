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
