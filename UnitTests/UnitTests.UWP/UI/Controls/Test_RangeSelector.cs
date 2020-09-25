// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace UnitTests.UI.Controls
{
    [TestClass]
    public class Test_RangeSelector
    {
        [UITestMethod]
        [TestCategory("RangeSelector")]
        public void StepFrequency_SmallValue_RangeMinJumpsToProperStep()
        {
            var selector = new RangeSelector();
            selector.Minimum = 0;
            selector.Maximum = 100;
            selector.StepFrequency = 5;

            selector.RangeMin = 2;
            Assert.AreEqual(selector.RangeMin, 0);

            selector.RangeMin = 4;
            Assert.AreEqual(selector.RangeMin, 5);

            selector.RangeMin = 94;
            Assert.AreEqual(selector.RangeMin, 95);

            selector.RangeMin = 99;
            Assert.AreEqual(selector.RangeMin, 100);
        }

        [UITestMethod]
        [TestCategory("RangeSelector")]
        public void StepFrequency_SmallValue_RangeMaxJumpsToProperStep()
        {
            var selector = new RangeSelector();
            selector.Minimum = 0;
            selector.Maximum = 100;
            selector.StepFrequency = 5;

            selector.RangeMax = 99;
            Assert.AreEqual(selector.RangeMax, 100);

            selector.RangeMax = 94;
            Assert.AreEqual(selector.RangeMax, 95);

            selector.RangeMax = 6;
            Assert.AreEqual(selector.RangeMax, 5);

            selector.RangeMax = 4;
            Assert.AreEqual(selector.RangeMax, 5);

            selector.RangeMax = 1;
            Assert.AreEqual(selector.RangeMax, 0);
        }

        [UITestMethod]
        [TestCategory("RangeSelector")]
        public void StepFrequency_LargeValue_RangeMinJumpsToProperStep()
        {
            var selector = new RangeSelector();
            selector.Minimum = 0;
            selector.Maximum = 100;
            selector.StepFrequency = 30;

            selector.RangeMin = 2;
            Assert.AreEqual(selector.RangeMin, 0);

            selector.RangeMin = 14;
            Assert.AreEqual(selector.RangeMin, 0);

            selector.RangeMin = 16;
            Assert.AreEqual(selector.RangeMin, 30);

            selector.RangeMin = 29;
            Assert.AreEqual(selector.RangeMin, 30);

            selector.RangeMin = 36;
            Assert.AreEqual(selector.RangeMin, 30);

            selector.RangeMin = 65;
            Assert.AreEqual(selector.RangeMin, 60);

            selector.RangeMin = 86;
            Assert.AreEqual(selector.RangeMin, 90);

            selector.RangeMin = 95;
            Assert.AreEqual(selector.RangeMin, 90);

            selector.RangeMin = 99;
            Assert.AreEqual(selector.RangeMin, 90);

            selector.RangeMin = 100;
            Assert.AreEqual(selector.RangeMin, 90);
        }

        [UITestMethod]
        [TestCategory("RangeSelector")]
        public void StepFrequency_LargeValue_RangeMaxJumpsToProperStep()
        {
            var selector = new RangeSelector();
            selector.Minimum = 0;
            selector.Maximum = 100;
            selector.StepFrequency = 30;

            selector.RangeMax = 99;
            Assert.AreEqual(selector.RangeMax, 100);

            selector.RangeMax = 94;
            Assert.AreEqual(selector.RangeMax, 100);

            selector.RangeMax = 78;
            Assert.AreEqual(selector.RangeMax, 70);

            selector.RangeMax = 65;
            Assert.AreEqual(selector.RangeMax, 70);

            selector.RangeMax = 50;
            Assert.AreEqual(selector.RangeMax, 40);

            selector.RangeMax = 38;
            Assert.AreEqual(selector.RangeMax, 40);

            selector.RangeMax = 20;
            Assert.AreEqual(selector.RangeMax, 10);

            selector.RangeMax = 8;
            Assert.AreEqual(selector.RangeMax, 10);

            selector.RangeMax = 1;
            Assert.AreEqual(selector.RangeMax, 10);

            selector.RangeMax = 0;
            Assert.AreEqual(selector.RangeMax, 10);
        }
    }
}
