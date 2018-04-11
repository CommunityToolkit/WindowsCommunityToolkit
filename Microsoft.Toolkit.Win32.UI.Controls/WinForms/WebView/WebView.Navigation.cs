using System;
using System.ComponentModel;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    // Navigation Journaling
    public partial class WebView
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanGoBack => _webViewControl?.CanGoBack ?? false;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanGoForward => _webViewControl?.CanGoForward ?? false;

        public bool GoBack()
        {
            var retval = true;
            try
            {
                _webViewControl.GoBack();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException()) throw;

                retval = false;
            }

            return retval;
        }
        public bool GoForward()
        {
            var retval = true;
            try
            {
                _webViewControl.GoForward();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException()) throw;
                retval = false;
            }

            return retval;
        }

        public override void Refresh()
        {
            try
            {
                _webViewControl?.Refresh();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException()) throw;
            }
        }

        public void Stop()
        {
            try
            {
                _webViewControl?.Stop();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException()) throw;
            }
        }
    }
}