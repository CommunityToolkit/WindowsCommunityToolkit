// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Text;
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
        [DataRow(44, 4, 16, 64)]
        [DataRow(76, 8, 16, 128)]
        [DataRow(128, 8, 16, 128)]
        [DataRow(179, 8, 32, 256)]
        [DataRow(366, 16, 32, 512)]
        [DataRow(512, 16, 32, 512)]
        [DataRow(890, 16, 64, 1024)]
        [DataRow(1280, 32, 64, 2048)]
        [DataRow(2445, 32, 128, 4096)]
        [DataRow(5000, 64, 128, 8192)]
        [DataRow(8000, 64, 128, 8192)]
        [DataRow(12442, 64, 256, 16384)]
        [DataRow(234000, 256, 1024, 262144)]
        public void Test_StringPool_Cctor_Ok(int minimumSize, int x, int y, int size)
        {
            var pool = new StringPool(minimumSize);

            Assert.AreEqual(size, pool.Size);

            Array buckets = (Array)typeof(StringPool).GetField("buckets", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(pool);

            Assert.AreEqual(x, buckets.Length);

            Type bucketType = Type.GetType("Microsoft.Toolkit.HighPerformance.Buffers.StringPool+Bucket, Microsoft.Toolkit.HighPerformance");

            int bucketSize = (int)bucketType.GetField("entriesPerBucket", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(buckets.GetValue(0));

            Assert.AreEqual(y, bucketSize);
        }

        [TestCategory("StringPool")]
        [TestMethod]
        [DataRow(0)]
        [DataRow(-3248234)]
        [DataRow(int.MinValue)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_StringPool_Cctor_Fail(int size)
        {
            var pool = new StringPool(size);

            Assert.Fail();
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_Add_Empty()
        {
            StringPool.Default.Add(string.Empty);

            bool found = StringPool.Default.TryGet(ReadOnlySpan<char>.Empty, out string? text);

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
        public void Test_StringPool_GetOrAdd_String_Empty()
        {
            string empty = StringPool.Default.GetOrAdd(string.Empty);

            Assert.AreSame(string.Empty, empty);
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_GetOrAdd_String_Misc()
        {
            var pool = new StringPool();

            string helloworld = nameof(helloworld);

            string cached = pool.GetOrAdd(helloworld);

            Assert.AreSame(helloworld, cached);

            Span<char> span = stackalloc char[helloworld.Length];

            helloworld.AsSpan().CopyTo(span);

            string helloworld2 = span.ToString();

            cached = pool.GetOrAdd(helloworld2);

            Assert.AreSame(helloworld, cached);

            cached = pool.GetOrAdd(new string(helloworld.ToCharArray()));

            Assert.AreSame(helloworld, cached);
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_GetOrAdd_ReadOnlySpan_Empty()
        {
            string empty = StringPool.Default.GetOrAdd(ReadOnlySpan<char>.Empty);

            Assert.AreSame(string.Empty, empty);
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_GetOrAdd_ReadOnlySpan_Misc()
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

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_GetOrAdd_Encoding_Empty()
        {
            string empty = StringPool.Default.GetOrAdd(ReadOnlySpan<byte>.Empty, Encoding.ASCII);

            Assert.AreSame(string.Empty, empty);
        }

        [TestCategory("StringPool")]
        [TestMethod]
        public void Test_StringPool_GetOrAdd_Encoding_Misc()
        {
            var pool = new StringPool();

            string helloworld = nameof(helloworld);

            pool.Add(helloworld);

            Span<byte> span = Encoding.UTF8.GetBytes(nameof(helloworld));

            string helloworld2 = pool.GetOrAdd(span, Encoding.UTF8);

            Assert.AreSame(helloworld, helloworld2);

            string windowsCommunityToolkit = nameof(windowsCommunityToolkit);

            Span<byte> span2 = Encoding.ASCII.GetBytes(windowsCommunityToolkit);

            string
                windowsCommunityToolkit2 = pool.GetOrAdd(span2, Encoding.ASCII),
                windowsCommunityToolkit3 = pool.GetOrAdd(windowsCommunityToolkit);

            Assert.AreSame(windowsCommunityToolkit2, windowsCommunityToolkit3);
        }
    }
}
