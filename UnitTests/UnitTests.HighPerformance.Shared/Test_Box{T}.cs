// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.HighPerformance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance
{
    [TestClass]
    public class Test_BoxOfT
    {
        [TestCategory("BoxOfT")]
        [TestMethod]
        public void Test_BoxOfT_PrimitiveTypes()
        {
            Test(true, false);
            Test<byte>(27, 254);
            Test('a', '$');
            Test(4221124, 1241241);
            Test(3.14f, 2342.222f);
            Test(8394324ul, 1343431241ul);
            Test(184013.234324, 14124.23423);
            Test(DateTime.Now, DateTime.FromBinary(278091429014));
            Test(Guid.NewGuid(), Guid.NewGuid());
        }

        /// <summary>
        /// Tests the <see cref="Box{T}"/> type for a given pair of values.
        /// </summary>
        /// <typeparam name="T">The type to test.</typeparam>
        /// <param name="value">The initial <typeparamref name="T"/> value.</param>
        /// <param name="test">The new <typeparamref name="T"/> value to assign and test.</param>
        private static void Test<T>(T value, T test)
            where T : struct, IEquatable<T>
        {
            Box<T> box = value;

            Assert.AreEqual(box.Value, value);

            object obj = value;

            bool success = Box<T>.TryGetFrom(obj, out box);

            Assert.IsTrue(success);
            Assert.IsNotNull(box);
            Assert.AreEqual(box.Value, value);

            box = Box<T>.DangerousGetFrom(obj);

            Assert.AreEqual(box.Value, value);

            box.Value = test;

            Assert.AreEqual(box.Value, test);
            Assert.AreEqual(obj, test);
        }
    }
}
