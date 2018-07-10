using System.Windows;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.Compatible
{
    public interface IWebViewCompatibleAdapter : IWebViewCompatible
    {
        FrameworkElement View { get; }
    }
}
