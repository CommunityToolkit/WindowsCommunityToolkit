using Microsoft.Graph;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class Common
    {
        const string GraphAPIBaseUrl = "https://graph.microsoft.com/v1.0";

        internal static GraphServiceClient GetAuthenticatedClient(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken)) return null;

            var graphClient = new GraphServiceClient(
                    GraphAPIBaseUrl,
                    new DelegateAuthenticationProvider(
                        (requestMessage) =>
                        {
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                            return Task.FromResult(0);
                        }));
            return graphClient;
        }

        internal static BitmapImage GetBitmapImagFromEmbedResource(string path)
        {
            BitmapImage bitmapImage = null;
            Assembly assembly = typeof(Common).GetTypeInfo().Assembly;
            using (var imageStream = assembly.GetManifestResourceStream(path))
            using (var memStream = new MemoryStream())
            {
                if (imageStream == null) return null;
                imageStream.CopyTo(memStream);
                memStream.Position = 0;
                using (var raStream = memStream.AsRandomAccessStream())
                {
                    bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(raStream);
                }
            }
            return bitmapImage;
        }
    }
}
