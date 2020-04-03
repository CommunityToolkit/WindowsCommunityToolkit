// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace UnitTests.XamlIslands
{
    [STATestClass]
    public partial class XamlIslandsTest_SystemInformation
    {
        [TestMethod]
        public async Task SystemInformationTrackAppUse()
        {
            await Program.Dispatcher.ExecuteOnUIThreadAsync(() =>
            {
                var e = new FakeArgs
                {
                    PreviousExecutionState = ApplicationExecutionState.NotRunning
                };
                var xamlRoot = Program.MainFormInstance.xamlHost.Child.XamlRoot;
                SystemInformation.Instance.TrackAppUse(e, xamlRoot);
            });
        }

        class FakeArgs : IActivatedEventArgs
        {
            public ActivationKind Kind { get; set; }

            public ApplicationExecutionState PreviousExecutionState { get; set; }

            public SplashScreen SplashScreen { get; set; }
        }
    }
}
