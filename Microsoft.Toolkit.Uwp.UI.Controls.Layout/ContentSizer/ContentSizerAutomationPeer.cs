using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.ContentSizer
{
    public class ContentSizerAutomationPeer : FrameworkElementAutomationPeer
    {
        public ContentSizerAutomationPeer(SplitBase owner) : base(owner)
        {

        }

        private ContentSizer OwningContentSizer
        {
            get
            {
                return Owner as ContentSizer;
            }
        }
    }
}
