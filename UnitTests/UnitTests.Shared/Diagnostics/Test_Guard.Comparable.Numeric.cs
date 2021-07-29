// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Toolkit.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Diagnostics
{
    public partial class Test_Guard
    {
        [TestCategory("Guard")]
        [TestMethod]
        public void Test_Guard_IsCloseToInt_Ok()
        {
            Guard.IsCloseTo(0, 5, 10, nameof(Test_Guard_IsCloseToInt_Ok));
            Guard.IsCloseTo(0, 5, 5, nameof(Test_Guard_IsCloseToInt_Ok));
            Guard.IsCloseTo(0, int.MaxValue, int.MaxValue, nameof(Test_Guard_IsCloseToInt_Ok));
            Guard.IsCloseTo(-500, -530, 50, nameof(Test_Guard_IsCloseToInt_Ok));
            Guard.IsCloseTo(1000, 800, 200, nameof(Test_Guard_IsCloseToInt_Ok));
            Guard.IsCloseTo(int.MaxValue, int.MaxValue - 10, 10, nameof(Test_Guard_IsCloseToInt_Ok));
        }

        [TestCategory("Guard")]
        [TestMethod]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1000", Justification = "Value tuple")]
        public void Test_Guard_IsCloseToInt_Fail()
        {
            foreach (var item in new (int Value, int Target, uint Delta)[]
            {
                (0, 20, 10),
                (0, 6, 5),
                (0, int.MaxValue, 500),
                (-500, -530, 10),
                (1000, 800, 100),
                (int.MaxValue, int.MaxValue - 10, 7),
                (int.MinValue, int.MaxValue, int.MaxValue)
            })
            {
                bool fail = false;

                try
                {
                    Guard.IsCloseTo(item.Value, item.Target, item.Delta, nameof(Test_Guard_IsCloseToInt_Fail));
                }
                catch (ArgumentException)
                {
                    fail = true;
                }

                Assert.IsTrue(fail, $"IsCloseTo didn't fail with {item}");
            }
        }

        [TestCategory("Guard")]
        [TestMethod]
        public void Test_Guard_IsCloseToFloat_Ok()
        {
            Guard.IsCloseTo(0f, 5, 10, nameof(Test_Guard_IsCloseToFloat_Ok));
            Guard.IsCloseTo(0f, 5, 5, nameof(Test_Guard_IsCloseToFloat_Ok));
            Guard.IsCloseTo(0f, float.MaxValue, float.MaxValue, nameof(Test_Guard_IsCloseToFloat_Ok));
            Guard.IsCloseTo(-500f, -530, 50, nameof(Test_Guard_IsCloseToFloat_Ok));
            Guard.IsCloseTo(1000f, 800, 200, nameof(Test_Guard_IsCloseToFloat_Ok));
            Guard.IsCloseTo(float.MaxValue, float.MaxValue - 10, 10, nameof(Test_Guard_IsCloseToFloat_Ok));
        }

        [TestCategory("Guard")]
        [TestMethod]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1000", Justification = "Value tuple")]
        public void Test_Guard_IsCloseToFloat_Fail()
        {
            foreach (var item in new (float Value, float Target, float Delta)[]
            {
                (0, 20, 10),
                (0, 6, 5),
                (0, float.MaxValue, 500),
                (-500, -530, 10),
                (1000, 800, 100),
                (float.MaxValue, float.MaxValue / 2, 7),
                (float.MinValue, float.MaxValue, float.MaxValue)
            })
            {
                bool fail = false;

                try
                {
                    Guard.IsCloseTo(item.Value, item.Target, item.Delta, nameof(Test_Guard_IsCloseToFloat_Fail));
                }
                catch (ArgumentException)
                {
                    fail = true;
                }

                Assert.IsTrue(fail, $"IsCloseTo didn't fail with {item}");
            }
        }
    }
}
