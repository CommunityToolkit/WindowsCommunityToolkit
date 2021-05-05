// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.UI
{
    [TestClass]
    public class Test_IncrementalLoadingCollection
    {
        private static readonly DataSource<int>.PageOperation[] FailPassSequence
            = new DataSource<int>.PageOperation[]
            {
                DataSource<int>.ThrowException, DataSource<int>.PassThrough,
                DataSource<int>.ThrowException, DataSource<int>.PassThrough,
                DataSource<int>.ThrowException, DataSource<int>.PassThrough,
                DataSource<int>.ThrowException, DataSource<int>.PassThrough,
                DataSource<int>.ThrowException, DataSource<int>.PassThrough,
            };

        [DataRow(2500, 1000, 1000, 1000, 1000)]
        [DataRow]
        [TestMethod]
        public async Task Requests(params int[] pageDelays)
        {
            const int pageSize = 20;
            const int pages = 5;

            var source = new DataSource<int>(Enumerable.Range(0, pageSize * pages), pageDelays.Select(DataSource<int>.MakeDelayOp));
            var collection = new IncrementalLoadingCollection<DataSource<int>, int>(source, pageSize);

            for (int pageNum = 1; pageNum <= pages; pageNum++)
            {
                var rez1 = await collection.LoadMoreItemsAsync(0);
                Assert.AreEqual((uint)pageSize, rez1.Count);
                CollectionAssert.AreEqual(Enumerable.Range(0, pageSize * pageNum).ToArray(), collection);
            }
        }

        [DataRow(2500, 1000, 1000, 1000, 1000)]
        [DataRow]
        [TestMethod]
        public async Task RequestsAsync(params int[] pageDelays)
        {
            const int pageSize = 20; 
            const int pages = 5;

            var source = new DataSource<int>(Enumerable.Range(0, pageSize * pages), pageDelays.Select(DataSource<int>.MakeDelayOp));
            var collection = new IncrementalLoadingCollection<DataSource<int>, int>(source, pageSize);

            var requests = new List<Task>();

            for (int pageNum = 1; pageNum <= pages; pageNum++)
            {
                requests.Add(collection.LoadMoreItemsAsync(0).AsTask().ContinueWith(t =>
                {
                    Assert.AreEqual(TaskStatus.RanToCompletion, t.Status);
                    Assert.AreEqual((uint)pageSize, t.Result.Count);
                }));
            }

            await Task.WhenAll(requests);

            CollectionAssert.AreEqual(Enumerable.Range(0, pageSize * pages).ToArray(), collection);
        }

        [TestMethod]
        public async Task FirstRequestFails()
        {
            const int pageSize = 20;
            const int pages = 5;

            var source = new DataSource<int>(Enumerable.Range(0, pageSize * pages), DataSource<int>.ThrowException);
            var collection = new IncrementalLoadingCollection<DataSource<int>, int>(source, pageSize);

            await Assert.ThrowsExceptionAsync<AggregateException>(async () => await collection.LoadMoreItemsAsync(0));

            Assert.IsTrue(!collection.Any());
            
            var requests = new List<Task>();

            for (int pageNum = 1; pageNum <= pages; pageNum++)
            {
                requests.Add(collection.LoadMoreItemsAsync(0).AsTask());
            }

            await Task.WhenAll(requests);

            CollectionAssert.AreEqual(Enumerable.Range(0, pageSize * pages).ToArray(), collection);
        }

        [TestMethod]
        public async Task EveryOtherRequestFails()
        {
            const int pageSize = 20;
            const int pages = 5;

            var source = new DataSource<int>(Enumerable.Range(0, pageSize * pages), FailPassSequence);
            var collection = new IncrementalLoadingCollection<DataSource<int>, int>(source, pageSize);

            var willFail = true;
            for (int submitedRequests = 0; submitedRequests < 10; submitedRequests++)
            {
                if (willFail)
                {
                     await collection.LoadMoreItemsAsync(0).AsTask().ContinueWith(t => Assert.AreEqual(TaskStatus.Faulted, t.Status));
                }
                else
                {
                    await collection.LoadMoreItemsAsync(0).AsTask().ContinueWith(t => Assert.AreEqual(TaskStatus.RanToCompletion, t.Status));
                }

                willFail = !willFail;
            }

            CollectionAssert.AreEquivalent(Enumerable.Range(0, pageSize * pages).ToArray(), collection);
        }

        [TestMethod]
        public async Task EveryOtherRequestFailsAsync()
        {
            const int pageSize = 20;
            const int pages = 5;

            var source = new DataSource<int>(Enumerable.Range(0, pageSize * pages), FailPassSequence);
            var collection = new IncrementalLoadingCollection<DataSource<int>, int>(source, pageSize);

            var requests = new List<Task>();

            var willFail = true;
            for (int submitedRequests = 0; submitedRequests < 10; submitedRequests++)
            {
                if (willFail)
                {
                    requests.Add(Assert.ThrowsExceptionAsync<AggregateException>(() => collection.LoadMoreItemsAsync(0).AsTask()));
                }
                else
                {
                    requests.Add(collection.LoadMoreItemsAsync(0).AsTask());
                }

                willFail = !willFail;
            }

            await Task.WhenAll(requests);

            CollectionAssert.AreEqual(Enumerable.Range(0, pageSize * pages).ToArray(), collection);
        }
    }
}