// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace UnitTests.UWP.UI.Triggers
{
    [TestClass]
    [TestCategory("Test_IsNullOrEmptyStateTrigger")]
    public class Test_IsNullOrEmptyStateTrigger : VisualUITestBase
    {
        [TestMethod]
        public async Task IsNullOrEmptyStateTrigger_SelectedItem()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var listView = new ListView { Name="myListView"};
                await SetTestContentAsync(listView);
                
                var trigger = new IsNullOrEmptyStateTrigger();
                Binding nullOrEmptyStateBinding = new Binding();
                nullOrEmptyStateBinding.Source = this;
                nullOrEmptyStateBinding.Path = new PropertyPath("SelectedItem");
                nullOrEmptyStateBinding.Mode = BindingMode.OneWay;
                BindingOperations.SetBinding(listView, ListView.SelectedItemProperty, nullOrEmptyStateBinding);

                var bol = IsNullOrEmptyStateTrigger.IsActive;

                Assert.AreEqual(true, bol);
            });
        }
    }
}
