// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Navigation
{
    [TestClass]
    public class NavigateStringUri : HostFormWebViewContextSpecification
    {
        private bool _navigationCompleted;
        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _navigationCompleted = true;
                Form.Close();
            };
        }

        protected override void When()
        {
            PerformActionAndWaitForFormClose(() =>
            {
                WebView.Navigate(TestConstants.Uris.ExampleOrg.ToString());
            });
        }

        [TestMethod]
        public void NavigationCompleted()
        {
            _navigationCompleted.ShouldBeTrue();
        }
    }

    [TestClass]
    public class NavigateRelativeUri : HostFormWebViewContextSpecification
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NavigationFailedWithArgumentException()
        {
            WebView.Navigate(new Uri("/someresource", UriKind.Relative));
        }
    }

    [TestClass]
    public class NavigateFilePath : HostFormWebViewContextSpecification
    {
        private string path;

        protected override void Given()
        {
            var fileName = Guid.NewGuid().ToString("N") + ".txt";
            path = Path.Combine(TestContext.TestRunResultsDirectory, fileName);

            File.WriteAllText(
                path,
                @"
<!DOCTYPE html>
<head><title>HTML on Disk</title></head>
<body><h1>HTML on Disk</h1></body>
</html>
");

            base.Given();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "E_ABORT expected")]
        [Ignore]
        public void Navigate()
        {
            PerformActionAndWaitForFormClose(() =>
            {
                WebView.Navigate(new Uri(path));
            });
        }
    }
}
