using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Microsoft.Toolkit.Uwp.SampleApp
{
    internal static class Tools
    {
        internal static async Task<bool> CheckInternetConnectionAsync()
        {
            if (!ConnectionHelper.IsInternetAvailable)
            {
                var dialog = new MessageDialog("Internet connection not detected. Please try again later.");
                await dialog.ShowAsync();

                return false;
            }

            return true;
        }
    }
}
