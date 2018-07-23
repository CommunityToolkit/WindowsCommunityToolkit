// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
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

    [TestClass]
    public class Navigate2Tests : HostFormWebViewContextSpecification
    {
        private bool _navigationCompleted;

        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _navigationCompleted = e.IsSuccess;
                Form.Close();
            };
        }

        protected override void When()
        {
            PerformActionAndWaitForFormClose(() =>
            {
                WebView.Navigate(TestConstants.Uris.HttpBin, HttpMethod.Get);
            });
        }

        [TestMethod]
        public void Explict_HTTP_GET_succeeds()
        {
            _navigationCompleted.ShouldBeTrue();
        }
    }

    [TestClass]
    public class NavigateGetWithHeaders : HostFormWebViewContextSpecification
    {
        private bool _navigationCompleted;

        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _navigationCompleted = e.IsSuccess;
                Form.Close();
            };
        }

        protected override void When()
        {
            PerformActionAndWaitForFormClose(() =>
            {
                WebView.Navigate(
                    TestConstants.Uris.HttpBin,
                    HttpMethod.Get,
                    null,
                    new[] { new KeyValuePair<string, string>("pragma", "no-cache") });
            });
        }

        [TestMethod]
        public void Explict_HTTP_GET_with_HEADERS_succeeds()
        {
            _navigationCompleted.ShouldBeTrue();
        }
    }

    [TestClass]
    public class NavigateGetWithBasicAuth : HostFormWebViewContextSpecification
    {
        private bool _navigationCompleted;

        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _navigationCompleted = e.IsSuccess;
                Form.Close();
            };
        }

        protected override void When()
        {
            PerformActionAndWaitForFormClose(() =>
            {
                const string user = "usr";
                const string password = "pwd";
                const string header = "Authorization";

                var authInfo = Convert.ToBase64String(Encoding.Default.GetBytes($"{user}:{password}"));

                WebView.Navigate(
                    new Uri(TestConstants.Uris.HttpBin, new Uri($"/basic-auth/{user}/{password}", UriKind.Relative)),
                    HttpMethod.Get,
                    null,
                    new[] { new KeyValuePair<string, string>(header, $"Basic {authInfo}") });
            });
        }

        [TestMethod]
        public void Explict_HTTP_GET_with_AUTH_BASIC_succeeds()
        {
            _navigationCompleted.ShouldBeTrue();
        }
    }

    [TestClass]
    public class NavigateOption : HostFormWebViewContextSpecification
    {
        private bool _navigationCompleted;

        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _navigationCompleted = e.IsSuccess;
                Form.Close();
            };
        }

        protected override void When()
        {
            PerformActionAndWaitForFormClose(() =>
            {


                WebView.Navigate(
                    TestConstants.Uris.ExampleCom,
                    HttpMethod.Options
                    );
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Ignore("Pops UI that stalls test")]
        public void Explict_HTTP_OPTION_fails()
        {
            _navigationCompleted.ShouldBeFalse();
        }
    }

    [TestClass]
    public class NavigatePostWithContent : HostFormWebViewContextSpecification
    {
        private bool _navigationCompleted;

        protected override void Given()
        {
            base.Given();
            WebView.NavigationCompleted += (o, e) =>
            {
                _navigationCompleted = e.IsSuccess;
                Form.Close();
            };
        }

        protected override void When()
        {
            PerformActionAndWaitForFormClose(() =>
            {
                string Foo()
                {
                    var c = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("Foo", "Bar"), });
                    return c.ReadAsStringAsync().Result;
                }

                WebView.Navigate(
                    new Uri(TestConstants.Uris.HttpBin, "/post"),
                    HttpMethod.Post,
                    Foo()
                );
            });
        }

        [TestMethod]
        public void Explict_HTTP_POST_with_data_succeeds()
        {
            _navigationCompleted.ShouldBeTrue();
        }
    }
}
