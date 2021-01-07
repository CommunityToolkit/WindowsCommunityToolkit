using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public class ClickAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            if (sender is Button btn && btn.IsEnabled)
            {
                var peer = new ButtonAutomationPeer(btn);
                var invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv?.Invoke();
            }

            return null;
        }
    }
}
