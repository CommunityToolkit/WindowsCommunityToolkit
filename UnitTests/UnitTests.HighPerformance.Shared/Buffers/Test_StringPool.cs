// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.HighPerformance.Buffers
{
    [TestClass]
    public class Test_StringPool
    {
        [TestCategory("StringPool")]
        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 0)]
        [DataRow(0, 1)]
        [DataRow(-3248234, 22)]
        [DataRow(int.MinValue, int.MinValue)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_StringPool_Cctor_Fail(int buckets, int entries)
        {
            var pool = new StringPool(buckets, entries);

            Assert.Fail();
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_Add_Empty()
        {
            StringPool.Default.Add(string.Empty);

            bool found = StringPool.Default.TryGet(default, out string? text);

            Assert.IsTrue(found);
            Assert.AreSame(string.Empty, text);
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_Add_Single()
        {
            var pool = new StringPool();

            string hello = nameof(hello);

            Assert.IsFalse(pool.TryGet(hello.AsSpan(), out _));

            pool.Add(hello);

            Assert.IsTrue(pool.TryGet(hello.AsSpan(), out string? hello2));

            Assert.AreSame(hello, hello2);
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_Add_Misc()
        {
            var pool = new StringPool();

            string
                hello = nameof(hello),
                helloworld = nameof(helloworld),
                windowsCommunityToolkit = nameof(windowsCommunityToolkit);

            Assert.IsFalse(pool.TryGet(hello.AsSpan(), out _));
            Assert.IsFalse(pool.TryGet(helloworld.AsSpan(), out _));
            Assert.IsFalse(pool.TryGet(windowsCommunityToolkit.AsSpan(), out _));

            pool.Add(hello);
            pool.Add(helloworld);
            pool.Add(windowsCommunityToolkit);

            Assert.IsTrue(pool.TryGet(hello.AsSpan(), out string? hello2));
            Assert.IsTrue(pool.TryGet(helloworld.AsSpan(), out string? world2));
            Assert.IsTrue(pool.TryGet(windowsCommunityToolkit.AsSpan(), out string? windowsCommunityToolkit2));

            Assert.AreSame(hello, hello2);
            Assert.AreSame(helloworld, world2);
            Assert.AreSame(windowsCommunityToolkit, windowsCommunityToolkit2);
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_GetOrAdd_Empty()
        {
            string empty = StringPool.Default.GetOrAdd(default);

            Assert.AreSame(string.Empty, empty);
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_GetOrAdd_Misc()
        {
            var pool = new StringPool();

            string
                hello = pool.GetOrAdd(nameof(hello).AsSpan()),
                helloworld = pool.GetOrAdd(nameof(helloworld).AsSpan()),
                windowsCommunityToolkit = pool.GetOrAdd(nameof(windowsCommunityToolkit).AsSpan());

            Assert.AreEqual(nameof(hello), hello);
            Assert.AreEqual(nameof(helloworld), helloworld);
            Assert.AreEqual(nameof(windowsCommunityToolkit), windowsCommunityToolkit);

            Assert.AreSame(hello, pool.GetOrAdd(hello.AsSpan()));
            Assert.AreSame(helloworld, pool.GetOrAdd(helloworld.AsSpan()));
            Assert.AreSame(windowsCommunityToolkit, pool.GetOrAdd(windowsCommunityToolkit.AsSpan()));

            pool.Reset();

            Assert.AreEqual(nameof(hello), hello);
            Assert.AreEqual(nameof(helloworld), helloworld);
            Assert.AreEqual(nameof(windowsCommunityToolkit), windowsCommunityToolkit);

            Assert.AreNotSame(hello, pool.GetOrAdd(hello.AsSpan()));
            Assert.AreNotSame(helloworld, pool.GetOrAdd(helloworld.AsSpan()));
            Assert.AreNotSame(windowsCommunityToolkit, pool.GetOrAdd(windowsCommunityToolkit.AsSpan()));
        }
    }
}
