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

using System.Linq;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace UnitTests.UI.Controls
{
    [TestClass]
    #pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
    public class Test_UniformGrid_FreeSpots
    {
        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_Basic()
        {
            var grid = new UniformGrid();

            bool[,] test = new bool[4, 5]
                {
                    { false,  true, false,  true, false },
                    { false,  true,  true,  true, false },
                    { false,  true, false,  true, false },
                    { false, false,  true, false, false },
                };

            var results = UniformGrid.GetFreeSpot(test, 0, false).ToArray();

            var expected = new(int row, int column)[]
                {
                    (0, 0),       (0, 2),       (0, 4),
                    (1, 0),                     (1, 4),
                    (2, 0),       (2, 2),       (2, 4),
                    (3, 0),(3, 1),       (3, 3),(3, 4)
                };

            CollectionAssert.AreEqual(
                expected,
                results, 
                "GetFreeSpot failed.  Expected:\n{0}.\nActual:\n{1}", 
                expected.ToArrayString(), 
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_FirstColumn()
        {
            var grid = new UniformGrid();

            bool[,] test = new bool[4, 5]
                {
                    { true,  false, false,  true, false },
                    { false,  true,  true,  true, false },
                    { false,  true,  true,  true, false },
                    { false, false,  true, false, false },
                };

            var results = UniformGrid.GetFreeSpot(test, 2, false).ToArray();

            var expected = new(int row, int column)[]
                {
                                  (0, 2),       (0, 4),
                    (1, 0),                     (1, 4),
                    (2, 0),                     (2, 4),
                    (3, 0),(3, 1),       (3, 3),(3, 4)
                };

            CollectionAssert.AreEqual(
                expected,
                results,
                "GetFreeSpot failed FirstColumn.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_Reverse()
        {
            var grid = new UniformGrid();

            bool[,] test = new bool[4, 5]
                {
                    { false, false, false,  true, false },
                    { false,  true,  true, false, false },
                    { true,  false, false,  true, false },
                    { false, false,  true, false, false },
                };

            var results = UniformGrid.GetFreeSpot(test, 0, true).ToArray();

            // right to left, so this should be reversed
            var expected = new(int row, int column)[]
                {
                    (0, 4),       (0, 2),(0, 1),(0, 0),
                    (1, 4),(1, 3),              (1, 0),
                    (2, 4),       (2, 2),(2, 1),
                    (3, 4),(3, 3),       (3, 1),(3, 0)
                };

            CollectionAssert.AreEqual(
                expected,
                results,
                "GetFreeSpot failed RightToLeft.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                results.ToArrayString());
        }
    }
    #pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}