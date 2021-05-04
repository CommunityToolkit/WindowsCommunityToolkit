// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.UI
{
    public class DataSource<T> : IIncrementalSource<T>
    {
        private readonly IEnumerable<T> _data;

        public DataSource(IEnumerable<T> items)
        {
            _data = items;
        }

        public async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Gets items from the collection according to pageIndex and pageSize parameters.
            var result = (from p in _data
                          select p).Skip(pageIndex * pageSize).Take(pageSize);

            // Simulates a longer request...
            // Make sure the list is still in order after a refresh,
            // even if the first page takes longer to load
            if (pageIndex == 0)
            {
                await Task.Delay(2500);
            }
            else
            {
                await Task.Delay(1000);
            }

            return result;
        }
    }
}
