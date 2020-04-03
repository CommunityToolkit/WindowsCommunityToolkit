// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI;

namespace UnitTests.XamlIslands
{
    [STATestClass]
    public partial class XamlIslandsTest_Eyedropper
    {
        [TestMethod]
        public async Task Eyedropper_DoesntCrash()
        {
            Eyedropper eyedropper = null;
            Color? color = null;
            _ = Program.Dispatcher.ExecuteOnUIThreadAsync(async () =>
            {
                var xamlRoot = Program.MainFormInstance.xamlHost.Child.XamlRoot;
                eyedropper = new Eyedropper(xamlRoot);
                color = await eyedropper.Open();
            });

            await Program.Dispatcher.ExecuteOnUIThreadAsync(async () =>
            {
                var pos = new Point(100, 100);
                await eyedropper.InternalPointerPressedAsync(1, pos, PointerDeviceType.Mouse);
                await Task.Delay(5000);

                await eyedropper.InternalPointerReleasedAsync(1, pos);

                await Task.Delay(1000);

                Assert.AreEqual(Colors.CornflowerBlue, color);
            });
        }
    }
}