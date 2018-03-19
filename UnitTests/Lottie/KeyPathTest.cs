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

using System;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Lottie
{
    [TestClass]
    public class KeyPathTest
    {
        private static readonly string[] V =
        {
            "Shape Layer 1",
            "Group 1",
            "Rectangle",
            "Stroke"
        };

        private static readonly string I = "INVALID";
        private static readonly string W = "*";
        private static readonly string G = "**";

        private LottieDrawable _lottieDrawable;

        [TestInitialize]
        public async Task InitAsync()
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                _lottieDrawable = new LottieDrawable();

                try
                {
                    LottieComposition composition = LottieComposition.Factory.FromJsonSync(new JsonReader(new StringReader(Fixtures.Squares)));
                    _lottieDrawable.SetComposition(composition);
                }
                catch (IOException e)
                {
                    throw new ArgumentException(e.Message, e);
                }
            });
        }

        [TestCleanup]
        public async Task CleanUp()
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                _lottieDrawable.ClearComposition();
                _lottieDrawable = null;
            });
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestV()
        {
            AssertSize(1, V[0]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestI()
        {
            AssertSize(0, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIi()
        {
            AssertSize(0, I, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIv()
        {
            AssertSize(0, I, V[1]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVi()
        {
            AssertSize(0, V[0], I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVv()
        {
            AssertSize(1, V[0], V[1]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIii()
        {
            AssertSize(0, I, I, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIiv()
        {
            AssertSize(0, I, I, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIvi()
        {
            AssertSize(0, I, V[2], I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIvv()
        {
            AssertSize(0, I, V[2], V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVii()
        {
            AssertSize(0, V[0], I, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestViv()
        {
            AssertSize(0, V[0], I, V[2]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVvi()
        {
            AssertSize(0, V[0], V[1], I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVvv()
        {
            AssertSize(1, V[0], V[1], V[2]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIiii()
        {
            AssertSize(0, I, I, I, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIiiv()
        {
            AssertSize(0, I, I, I, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIivi()
        {
            AssertSize(0, I, I, V[2], I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIivv()
        {
            AssertSize(0, I, I, V[2], V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIvii()
        {
            AssertSize(0, I, V[1], I, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIviv()
        {
            AssertSize(0, I, V[1], I, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIvvi()
        {
            AssertSize(0, I, V[1], V[2], V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestIvvv()
        {
            AssertSize(0, I, V[1], V[2], V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestViii()
        {
            AssertSize(0, V[0], I, I, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestViiv()
        {
            AssertSize(0, V[0], I, I, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVivi()
        {
            AssertSize(0, V[0], I, V[2], I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVivv()
        {
            AssertSize(0, V[0], I, V[2], V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVvii()
        {
            AssertSize(0, V[0], V[1], I, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVviv()
        {
            AssertSize(0, V[0], V[1], I, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVvvi()
        {
            AssertSize(0, V[0], V[1], V[2], I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVvvv()
        {
            AssertSize(1, V[0], V[1], V[2], V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWvvv()
        {
            AssertSize(2, W, V[1], V[2], V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVwvv()
        {
            AssertSize(2, V[0], W, V[2], V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVvwv()
        {
            AssertSize(1, V[0], V[1], W, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestVvvw()
        {
            AssertSize(2, V[0], V[1], V[2], W);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWwvv()
        {
            AssertSize(4, W, W, V[2], V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWvwv()
        {
            AssertSize(2, W, V[1], W, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWvvw()
        {
            AssertSize(4, W, V[1], V[2], W);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWwiv()
        {
            AssertSize(0, W, W, I, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWwvi()
        {
            AssertSize(0, W, W, V[2], I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWvw()
        {
            AssertSize(2, W, V[1], W);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWww()
        {
            AssertSize(4, W, W, W);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWwwv()
        {
            AssertSize(4, W, W, W, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWwwi()
        {
            AssertSize(0, W, W, W, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWwww()
        {
            AssertSize(8, W, W, W, W);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestG()
        {
            AssertSize(18, G);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGi()
        {
            AssertSize(0, G, I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv0()
        {
            AssertSize(1, G, V[0]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv0V0()
        {
            AssertSize(0, G, V[0], V[0]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv1()
        {
            AssertSize(2, G, V[1]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv2()
        {
            AssertSize(4, G, V[2]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv3()
        {
            AssertSize(4, G, V[3]);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv0G()
        {
            AssertSize(9, G, V[0], G);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv1G()
        {
            AssertSize(8, G, V[1], G);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv2G()
        {
            AssertSize(12, G, V[2], G);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGig()
        {
            AssertSize(0, G, I, G);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWg()
        {
            AssertSize(18, W, G);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv0W()
        {
            AssertSize(2, G, V[0], W);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWv0I()
        {
            AssertSize(0, W, V[0], I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv1W()
        {
            AssertSize(2, G, V[1], W);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWv1I()
        {
            AssertSize(0, W, V[1], I);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestGv2W()
        {
            AssertSize(8, G, V[2], W);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestWv2I()
        {
            AssertSize(0, W, V[2], I);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private void AssertSize(int size, params string[] keys)
        {
            KeyPath keyPath = new KeyPath(keys);
            List<KeyPath> resolvedKeyPaths = _lottieDrawable.ResolveKeyPath(keyPath);
            Assert.AreEqual(size, resolvedKeyPaths.Count);
        }
    }
}