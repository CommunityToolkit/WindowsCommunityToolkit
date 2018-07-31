using Microsoft.Toolkit.Win32.UI.Interop;
using Microsoft.Toolkit.Win32.UI.Interop.WinForms;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms.InkCanvas
{
    public class InkCanvas : WindowsXamlHostBase
    {
        internal global::Windows.UI.Xaml.Controls.InkCanvas UwpControl { get; set; }

        private readonly string initialTypeName;

        public InkCanvas()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkCanvas).Name)
        {
        }

        public InkCanvas(string name)
        {
            initialTypeName = name;
            UwpControl = UWPTypeFactory.CreateXamlContentByType(initialTypeName) as global::Windows.UI.Xaml.Controls.InkCanvas;
            desktopWindowXamlSource.Content = UwpControl;
        }
    }
}
