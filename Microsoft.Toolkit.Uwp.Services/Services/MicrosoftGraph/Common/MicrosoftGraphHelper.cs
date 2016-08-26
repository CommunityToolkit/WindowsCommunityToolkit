// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************

using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.Services.AzureAD;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    /// 
    /// </summary>
    public class MicrosoftGraphHelper
    {
        public static async Task<GraphServiceClient> GetGraphClientForCurrentSigninUserAsync(string appClientId)
        {
            return new GraphServiceClient(
                  new DelegateAuthenticationProvider(
                     async (requestMessage) =>
                     {
                         // requestMessage.Headers.Add("outlook.timezone", "Romance Standard Time");
                         requestMessage.Headers.Authorization =
                                            new AuthenticationHeaderValue(
                                                     "bearer",
                                                     await AuthenticationHelper.Instance.GetUserTokenAsync(appClientId).ConfigureAwait(false));
                         return;
                     }));
        }
    }
}
