// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public class MicrosoftGraphSource<T> : Collections.IIncrementalSource<T>
    {
        private bool isFirstCall = true;

        public async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<T> items = null;

            if (isFirstCall)
            {
                if (typeof(T) == typeof(Message))
                {
                    items = (IEnumerable<T>)await MicrosoftGraphService.Instance.User.Message.GetEmailsAsync(cancellationToken, pageSize);
                }

                if (typeof(T) == typeof(Event))
                {
                    items = (IEnumerable<T>)await MicrosoftGraphService.Instance.User.Event.GetEventsAsync(cancellationToken, pageSize);
                }

                isFirstCall = false;
            }
            else
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return items;
                }

                if (typeof(T) == typeof(Message))
                {
                    items = (IEnumerable<T>)await MicrosoftGraphService.Instance.User.Message.NextPageEmailsAsync(cancellationToken);
                }

                if (typeof(T) == typeof(Event))
                {
                    items = (IEnumerable<T>)await MicrosoftGraphService.Instance.User.Event.NextPageEventsAsync(cancellationToken);
                }
            }

            return items;
        }
    }
}
