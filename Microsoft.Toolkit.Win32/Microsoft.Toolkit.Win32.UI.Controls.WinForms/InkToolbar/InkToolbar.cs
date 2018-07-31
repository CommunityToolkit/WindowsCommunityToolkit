using Microsoft.Toolkit.Win32.UI.Interop;
using Microsoft.Toolkit.Win32.UI.Interop.WinForms;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms.InkToolbar
{
    public class InkToolbar : WindowsXamlHostBase
    {
        internal global::Windows.UI.Xaml.Controls.InkToolbar UwpControl { get; set; }

        private readonly string initialTypeName;

        public InkToolbar()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbar).Name)
        {
        }

        public InkToolbar(string name)
        {
            initialTypeName = name;
            UwpControl = UWPTypeFactory.CreateXamlContentByType(initialTypeName) as global::Windows.UI.Xaml.Controls.InkToolbar;
            desktopWindowXamlSource.Content = UwpControl;
        }
    }
}
