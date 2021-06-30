using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.Input.GazeControls
{
    internal class KeyboardPage
    {
        public FrameworkElement Page;
        public KeyboardPage Parent;
        public List<string> ChildrenNames;
        public List<KeyboardPage> Children;
        public KeyboardPage CurrentChild;
        public KeyboardPage PrevChild;

        public KeyboardPage(FrameworkElement page, KeyboardPage parent)
        {
            Page = page;
            Parent = parent;
            ChildrenNames = new List<string>();
            Children = new List<KeyboardPage>();
        }
    }
}
