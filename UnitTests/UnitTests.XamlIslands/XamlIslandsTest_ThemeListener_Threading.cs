// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.UI.Xaml;

namespace UnitTests.XamlIslands
{
    [STATestClass]
    public partial class XamlIslandsTest_ThemeListener_Threading
    {
        private TaskCompletionSource<object> _taskCompletionSource;
        private ThemeListener _themeListener = null;

        [TestInitialize]
        public void Init()
        {
            Application.Current.RequestedTheme = ApplicationTheme.Light;

            _taskCompletionSource = new TaskCompletionSource<object>();

            _themeListener = new ThemeListener();
            _themeListener.ThemeChanged += (s) =>
            {
                _taskCompletionSource.TrySetResult(null);
            };

            Application.Current.RequestedTheme = ApplicationTheme.Dark;
        }

        [TestMethod]
        public async Task ThemeListenerDispatcherTestAsync()
        {
            _ = _themeListener.OnColorValuesChanged();

            await _taskCompletionSource.Task;
        }

        [TestMethod]
        public async Task ThemeListenerDispatcherTestFromOtherThreadAsync()
        {
            await Task.Run(async () =>
            {
                _ = _themeListener.OnColorValuesChanged();

                await _taskCompletionSource.Task;
            });
        }
    }
}
