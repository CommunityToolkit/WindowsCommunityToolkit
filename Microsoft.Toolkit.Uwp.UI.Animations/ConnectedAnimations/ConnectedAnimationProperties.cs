using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    internal class ConnectedAnimationProperties
    {
        public string Key { get; set; }

        public UIElement Element { get; set; }

        public List<UIElement> CoordinatedElements { get; set; }

        public string ElementName { get; set; }

        public Windows.UI.Xaml.Controls.ListViewBase ListViewBase { get; set; }

        public bool IsListAnimation { get; set; } = false;
    }
}
