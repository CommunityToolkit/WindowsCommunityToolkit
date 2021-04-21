// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI;
using System.Numerics;

namespace UnitTests.UWP.UI
{
    [TestClass]
    [TestCategory("Test_VisualExtensions")]
    public class Test_VisualExtensions : VisualUITestBase
    {
        [TestMethod]
        public async Task SetAndGetTranslation()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var button = new Button();
                var grid = new Grid() { Children = { button } };

                VisualExtensions.SetTranslation(button, "80, 20, 0");

                await SetTestContentAsync(grid);

                Assert.AreEqual(button.GetVisual().TransformMatrix.Translation, new Vector3(80, 20, 0));

                string translation = VisualExtensions.GetTranslation(button);

                Assert.AreEqual(translation, new Vector3(80, 20, 0).ToString());
            });
        }
    }
}