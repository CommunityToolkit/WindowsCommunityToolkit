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
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Model.Layer;
using Microsoft.Toolkit.Uwp.UI.Controls.Lottie.Utils;
using Moq;
using Windows.Foundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Lottie
{
    [TestClass]
    public class LottieValueAnimatorUnitTest
    {
        private Mock<TestLottieValueAnimator> _mockAnimator;
        private TestLottieValueAnimator _animator;
        private volatile bool isDone;

        [TestInitialize]
        public void Init()
        {
            LottieComposition composition = new LottieComposition();
            composition.Init(default(Rect), 0, 1000, 1000, new List<Layer>(), new Dictionary<long, Layer>(0), new Dictionary<string, List<Layer>>(0), new Dictionary<string, LottieImageAsset>(0), new Dictionary<int, FontCharacter>(0), new Dictionary<string, Font>(0));
            _mockAnimator = new Mock<TestLottieValueAnimator>
            {
                CallBase = true,
            };
            _animator = _mockAnimator.Object;
            _animator.Composition = composition;

            isDone = false;
        }

        internal class TestLottieValueAnimator : LottieValueAnimator
        {
            public int OnValueChangedCount { get; set; } = 0;

            public void OnValueChanged2()
            {
                OnValueChangedCount++;
            }

            protected override void OnValueChanged()
            {
                base.OnValueChanged();
                OnValueChanged2();
            }

            protected override void PostFrameCallback()
            {
                InternalIsRunning = true;
            }

            protected internal override void RemoveFrameCallback()
            {
                InternalIsRunning = false;
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            _animator.Cancel();
            _animator = null;
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestInitialState()
        {
            Assert.AreEqual(0f, _animator.Frame);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestResumingMaintainsValue()
        {
            _animator.Frame = 500;
            _animator.ResumeAnimation();
            Assert.AreEqual(500f, _animator.Frame);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestFrameConvertsToAnimatedFraction()
        {
            _animator.Frame = 500;
            _animator.ResumeAnimation();
            Assert.AreEqual(0.5f, _animator.AnimatedFraction);
            Assert.AreEqual(0.5f, _animator.AnimatedValueAbsolute);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestPlayingResetsValue()
        {
            _animator.Frame = 500;
            _animator.PlayAnimation();
            Assert.AreEqual(0f, _animator.Frame);
            Assert.AreEqual(0f, _animator.AnimatedFraction);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestReversingMaintainsValue()
        {
            _animator.Frame = 250;
            _animator.ReverseAnimationSpeed();
            Assert.AreEqual(250, _animator.Frame);
            Assert.AreEqual(0.75f, _animator.AnimatedFraction);
            Assert.AreEqual(0.25f, _animator.AnimatedValueAbsolute);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestReversingWithMinValueMaintainsValue()
        {
            _animator.MinFrame = 100;
            _animator.Frame = 1000;
            _animator.ReverseAnimationSpeed();
            Assert.AreEqual(1000f, _animator.Frame);
            Assert.AreEqual(0f, _animator.AnimatedFraction);
            Assert.AreEqual(1f, _animator.AnimatedValueAbsolute);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestReversingWithMaxValueMaintainsValue()
        {
            _animator.MaxFrame = 900;
            _animator.ReverseAnimationSpeed();
            Assert.AreEqual(0f, _animator.Frame);
            Assert.AreEqual(1f, _animator.AnimatedFraction);
            Assert.AreEqual(0f, _animator.AnimatedValueAbsolute);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestResumeReversingWithMinValueMaintainsValue()
        {
            _animator.MaxFrame = 900;
            _animator.ReverseAnimationSpeed();
            _animator.ResumeAnimation();
            Assert.AreEqual(900f, _animator.Frame);
            Assert.AreEqual(0f, _animator.AnimatedFraction);
            Assert.AreEqual(0.9f, _animator.AnimatedValueAbsolute);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestPlayReversingWithMinValueMaintainsValue()
        {
            _animator.MaxFrame = 900;
            _animator.ReverseAnimationSpeed();
            _animator.PlayAnimation();
            Assert.AreEqual(900f, _animator.Frame);
            Assert.AreEqual(0f, _animator.AnimatedFraction);
            Assert.AreEqual(0.9f, _animator.AnimatedValueAbsolute);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestMinAndMaxBothSet()
        {
            _animator.MinFrame = 200;
            _animator.MaxFrame = 800;
            _animator.Frame = 400;
            Assert.AreEqual(0.33f, _animator.AnimatedFraction, 2);
            Assert.AreEqual(0.4f, _animator.AnimatedValueAbsolute);
            _animator.ReverseAnimationSpeed();
            Assert.AreEqual(400f, _animator.Frame);
            Assert.AreEqual(0.666f, _animator.AnimatedFraction, 2);
            Assert.AreEqual(0.4f, _animator.AnimatedValueAbsolute);
            _animator.ResumeAnimation();
            Assert.AreEqual(400f, _animator.Frame);
            Assert.AreEqual(0.666f, _animator.AnimatedFraction, 2);
            Assert.AreEqual(0.4f, _animator.AnimatedValueAbsolute);
            _animator.PlayAnimation();
            Assert.AreEqual(800f, _animator.Frame);
            Assert.AreEqual(0f, _animator.AnimatedFraction, 2);
            Assert.AreEqual(0.8f, _animator.AnimatedValueAbsolute);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestDefaultAnimator()
        {
            int state = 0;

            _mockAnimator.Setup(l => l.OnAnimationStart(false)).Callback(() =>
            {
                if (state == 0)
                {
                    state = 1;
                }
            }).Verifiable();
            _mockAnimator.Setup(l => l.OnAnimationEnd(false)).Callback(() =>
            {
                if (state == 1)
                {
                    state = 2;
                }

                _mockAnimator.Verify();
                Assert.AreEqual(2, state);
                _mockAnimator.Verify(l => l.OnAnimationCancel(), Times.Never);
                _mockAnimator.Verify(l => l.OnAnimationRepeat(), Times.Never);

                isDone = true;
            }).Verifiable();

            TestAnimator(null);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestReverseAnimator()
        {
            _animator.ReverseAnimationSpeed();

            int state = 0;

            _mockAnimator.Setup(l => l.OnAnimationStart(true)).Callback(() =>
            {
                if (state == 0)
                {
                    state = 1;
                }
            }).Verifiable();
            _mockAnimator.Setup(l => l.OnAnimationEnd(true)).Callback(() =>
            {
                if (state == 1)
                {
                    state = 2;
                }

                _mockAnimator.Verify();
                Assert.AreEqual(2, state);
                _mockAnimator.Verify(l => l.OnAnimationCancel(), Times.Never);
                _mockAnimator.Verify(l => l.OnAnimationRepeat(), Times.Never);

                isDone = true;
            }).Verifiable();

            TestAnimator(null);
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestLoopingAnimatorOnce()
        {
            _animator.RepeatCount = 1;
            TestAnimator(() =>
            {
                _mockAnimator.Verify(l => l.OnAnimationStart(false), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationRepeat(), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationEnd(false), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationCancel(), Times.Never);
            });
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestLoopingAnimatorZeroTimes()
        {
            _animator.RepeatCount = 0;
            TestAnimator(() =>
            {
                _mockAnimator.Verify(l => l.OnAnimationStart(false), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationRepeat(), Times.Never);
                _mockAnimator.Verify(l => l.OnAnimationEnd(false), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationCancel(), Times.Never);
            });
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestLoopingAnimatorTwice()
        {
            _animator.RepeatCount = 2;
            TestAnimator(() =>
            {
                _mockAnimator.Verify(l => l.OnAnimationStart(false), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationRepeat(), Times.Exactly(2));
                _mockAnimator.Verify(l => l.OnAnimationEnd(false), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationCancel(), Times.Never);
            });
        }

        [TestCategory("Lottie")]
        [TestMethod]
        public void TestLoopingAnimatorOnceReverse()
        {
            _animator.Frame = 1000;
            _animator.RepeatCount = 1;
            _animator.ReverseAnimationSpeed();
            TestAnimator(() =>
            {
                _mockAnimator.Verify(l => l.OnAnimationStart(true), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationRepeat(), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationEnd(true), Times.Once);
                _mockAnimator.Verify(l => l.OnAnimationCancel(), Times.Never);
            });
        }

        private void TestAnimator(Action verifyListener)
        {
            _animator.AnimationEnd += (s, e) =>
            {
                verifyListener?.Invoke();
                isDone = true;
            };

            _animator.PlayAnimation();
            while (!isDone)
            {
                _animator.DoFrame();
            }
        }
    }
}
