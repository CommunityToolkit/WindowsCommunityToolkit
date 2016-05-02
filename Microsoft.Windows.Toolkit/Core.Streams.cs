using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace Microsoft.Windows.Toolkit
{
    public static partial class Core
    {
        /// <summary>
        /// Get the response stream returned by a HTTP get request.
        /// </summary>
        /// <param name="uri">Uri to request.</param>
        /// <returns>Response stream</returns>
        public static async Task<IRandomAccessStream> GetHTTPStreamAsync(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException();
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    var outputStream = new InMemoryRandomAccessStream();

                    using (var content = response.Content)
                    {
                        var responseBuffer = await content.ReadAsBufferAsync();
                        await outputStream.WriteAsync(responseBuffer);

                        outputStream.Seek(0);

                        return outputStream;
                    }
                }
            }
        }
    }
}
