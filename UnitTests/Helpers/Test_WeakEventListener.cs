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
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Toolkit.Uwp.Helpers;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_WeakEventListener
    {
        public class SampleClass
        {
            public event EventHandler<EventArgs> Raisevent;

            public void DoSomething()
            {
                OnRaiseEvent();
            }

            protected virtual void OnRaiseEvent()
            {
                Raisevent?.Invoke(this, EventArgs.Empty);
            }
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_WeakEventListener_Events()
        {
            bool isOnEventTriggered = false;
            bool isOnDetachTriggered = false;

            SampleClass sample = new SampleClass();

            WeakEventListener<SampleClass, object, EventArgs> weak = new WeakEventListener<SampleClass, object, EventArgs>(sample);
            weak.OnEventAction = (instance, source, eventArgs) => { isOnEventTriggered = true; };
            weak.OnDetachAction = (listener) => { isOnDetachTriggered = true; };

            sample.Raisevent += weak.OnEvent;

            sample.DoSomething();
            Assert.IsTrue(isOnEventTriggered);

            weak.Detach();
            Assert.IsTrue(isOnDetachTriggered);
        }
    }
}
