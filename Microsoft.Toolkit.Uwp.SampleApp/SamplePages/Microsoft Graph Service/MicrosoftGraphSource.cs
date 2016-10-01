// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.Services.MicrosoftGraph;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public class MicrosoftGraphSource : IIncrementalSource<Message>
    {
        private bool isFirstCall = true;

        public async Task<IEnumerable<Message>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            IUserMessagesCollectionPage messages = null;

            if (isFirstCall)
            {
                messages = await MicrosoftGraphService.Instance.User.Message.GetEmailsAsync(cancellationToken, pageSize);

                isFirstCall = false;
            }
            else
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return messages;
                }

                messages = await MicrosoftGraphService.Instance.User.Message.NextPageEmailsAsync();
            }

            return messages;
        }
    }
}
