// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.UI
{
    [TestClass]
    public class Test_IncrementalLoadingCollection
    {
        [TestMethod]
        public async Task SequentialRequests()
        {
            const int pageSize = 20;
            const int pages = 5;
            var collection = new IncrementalLoadingCollection<DataSource<int>, int>(new DataSource<int>(Enumerable.Range(0, pageSize * pages)), pageSize);

            for (int pageNum = 1; pageNum <= pages; pageNum++)
            {
                var rez1 = await collection.LoadMoreItemsAsync(0);
                Assert.AreEqual((uint)pageSize, rez1.Count);
                CollectionAssert.AreEquivalent(Enumerable.Range(0, pageSize * pageNum).ToArray(), collection);
            }
        }

        [TestMethod]
        public void ConcurentRequests()
        {
            const int pageSize = 20;
            const int pages = 5;
            var collection = new IncrementalLoadingCollection<DataSource<int>, int>(new DataSource<int>(Enumerable.Range(0, pageSize * pages)), pageSize);

            var requests = new List<Task>();

            for (int pageNum = 1; pageNum <= pages; pageNum++)
            {
                requests.Add(collection.LoadMoreItemsAsync(0).AsTask().ContinueWith(t =>
                {
                    Assert.AreEqual(TaskStatus.RanToCompletion, t.Status);
                    Assert.AreEqual((uint)pageSize, t.Result.Count);
                }));
            }

            Task.WaitAll(requests.ToArray());
            CollectionAssert.AreEquivalent(Enumerable.Range(0, pageSize * pages).ToArray(), collection);
        }
    }
}