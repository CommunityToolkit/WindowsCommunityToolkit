// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Extensions;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.XamlIslands.UWPApp
{
    [STATestClass]
    public partial class XamlIslandsTest_StringExtensions
    {
        [TestMethod]
        public async Task StringExtensions_GetViewLocalized()
        {
            await App.Dispatcher.ExecuteOnUIThreadAsync(() =>
            {
                var xamlRoot = App.XamlRoot;
                var str = StringExtensions.GetViewLocalized("abc", xamlRoot.UIContext);
                Assert.AreEqual("ABCDEF", str);
            });
        }

        [TestMethod]
        public async Task StringExtensions_GetLocalized()
        {
            await App.Dispatcher.ExecuteOnUIThreadAsync(() =>
            {
                var xamlRoot = App.XamlRoot;
                var str = StringExtensions.GetLocalized("abc", xamlRoot.UIContext);
                Assert.AreEqual("ABCDEF", str);
            });
        }

        [TestMethod]
        public void StringExtensions_GetLocalizedWithResourcePath()
        {
            var str = StringExtensions.GetLocalized("TextToolbarStrings_OkLabel", "Microsoft.Toolkit.Uwp.UI.Controls/Resources");
            Assert.AreEqual("Ok", str);
        }
    }
}