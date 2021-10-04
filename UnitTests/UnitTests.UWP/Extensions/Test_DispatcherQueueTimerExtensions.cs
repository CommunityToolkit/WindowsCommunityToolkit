using Microsoft.Toolkit.Uwp.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_DispatcherQueueTimerExtensions
    {
        [TestCategory("DispatcherQueueTimerExtensions")]
        [TestMethod]
        public async Task Test_DispatcherQueueTimerExtensions_Debounce()
        {
            var debounceTimer = App.DispatcherQueue.CreateTimer();

            var triggeredCount = 0;
            string triggeredValue = null;

            var value = "He";
            debounceTimer.Debounce(
                () =>
                {
                    triggeredCount++;
                    triggeredValue = value;
                },
                TimeSpan.FromMilliseconds(60));

            await Task.Delay(TimeSpan.FromMilliseconds(10));

            value = "Hello";
            debounceTimer.Debounce(
                () =>
                {
                    triggeredCount++;
                    triggeredValue = value;
                },
                TimeSpan.FromMilliseconds(60));

            await Task.Delay(TimeSpan.FromMilliseconds(110));

            Assert.AreEqual(false, debounceTimer.IsRunning, "Expected to stop the timer.");
            Assert.AreEqual(value, triggeredValue, "Expected to execute the last action.");
            Assert.AreEqual(1, triggeredCount, "Expected to postpone execution.");
        }
    }
}
