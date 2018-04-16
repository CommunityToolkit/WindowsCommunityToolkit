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

using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Lottie
{
    [TestClass]
    public class MeanCalculatorTest
    {
        private MeanCalculator _meanCalculator;

        [TestInitialize]
        public void Init()
        {
            _meanCalculator = new MeanCalculator();
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestMeanWithNoNumbers()
        {
            Assert.AreEqual(0f, _meanCalculator.Mean);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestMeanWithOneNumber()
        {
            _meanCalculator.Add(2);
            Assert.AreEqual(2f, _meanCalculator.Mean);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestMeanWithTwoNumbers()
        {
            _meanCalculator.Add(2);
            _meanCalculator.Add(4);
            Assert.AreEqual(3f, _meanCalculator.Mean);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestMeanWithTwentyNumbers()
        {
            for (int i = 1; i <= 20; i++)
            {
                _meanCalculator.Add(i);
            }

            Assert.AreEqual(10.5f, _meanCalculator.Mean);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestMeanWithHugeNumber()
        {
            _meanCalculator.Add(int.MaxValue - 1);
            _meanCalculator.Add(int.MaxValue - 1);
            Assert.AreEqual(int.MaxValue - 1, _meanCalculator.Mean);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestMeanWithHugeNumberAndNegativeHugeNumber()
        {
            _meanCalculator.Add(int.MaxValue - 1);
            _meanCalculator.Add(int.MaxValue - 1);
            _meanCalculator.Add(-int.MaxValue + 1);
            _meanCalculator.Add(-int.MaxValue + 1);
            Assert.AreEqual(0f, _meanCalculator.Mean);
        }
    }
}
