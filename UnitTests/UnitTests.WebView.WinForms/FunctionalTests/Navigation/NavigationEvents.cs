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
using System.Collections.Generic;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Navigation
{
    [TestClass]
    public class NavigatingFromNavigatingStartingEvent : HostFormWebViewContextSpecification
    {
        private List<Uri> _navigatedUris;

        protected override void Given()
        {
            _navigatedUris = new List<Uri>();

            base.Given();
            WebView.NavigationStarting += (o, e) =>
            {
                if (e.Uri != TestConstants.Uris.ExampleOrg)
                {
                    WebView.Navigate(TestConstants.Uris.ExampleOrg);
                }
            };
            WebView.NavigationCompleted += (o, e) =>
            {
                _navigatedUris.Add(e.Uri);

                if (e.Uri != TestConstants.Uris.ExampleCom)
                {
                    Form.Close();
                }
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.ExampleCom);
        }

        [TestMethod]
        public void NavigatingFromNavigationStartingInterruptsOriginalNavigation()
        {
            _navigatedUris.Count.ShouldEqual(1, "Starting a new navigation while navigating cancels the original navigation");
            _navigatedUris.ShouldContain(TestConstants.Uris.ExampleOrg);
            _navigatedUris.ShouldNotContain(TestConstants.Uris.ExampleCom);
        }
    }

    [TestCategory(TestConstants.Categories.Nav)]
    public abstract class NavigationEventsFiredForHostFormContextSpecification : HostFormWebViewContextSpecification
    {
        private bool _navStarting;
        private bool _contentLoading;
        private bool _domContentLoaded;
        private bool _navCompleted;

        protected override void Given()
        {
            base.Given();

            WebView.NavigationStarting += (o, e) => { _navStarting = true; };
            WebView.ContentLoading += (o, e) => { _contentLoading = true; };
            WebView.DOMContentLoaded += (o, e) => { _domContentLoaded = true; };
            WebView.NavigationCompleted += (o, e) =>
            {
                _navCompleted = true;
                Form.Close();
            };
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Long)]
        public void NavigationEventsCompleted()
        {
            _navStarting.ShouldBeTrue();
            _contentLoading.ShouldBeTrue();
            _domContentLoaded.ShouldBeTrue();
            _navCompleted.ShouldBeTrue();
        }
    }

    [TestClass]
    public class NavigationEventsFiredForHostFormOnAboutBlank : NavigationEventsFiredForHostFormContextSpecification
    {
        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.AboutBlank);
        }
    }

    [TestClass]
    public class NavigationEventsFiredForHostFormOnNullSource : NavigationEventsFiredForHostFormContextSpecification
    {
        protected override void When()
        {
            NavigateAndWaitForFormClose((Uri)null);
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class NavigateToStringEvents : NavigationEventsFiredForHostFormContextSpecification
    {
        private readonly string _content = $@"
<html>
<body>
<h1>{nameof(NavigateToStringEvents)}</h1>
</body>
</html>
";

        protected override void When()
        {
            NavigateToStringAndWaitForFormClose(_content);
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]

    public class NavigationEventsFiredForProcessFactory : ProcessFactoryWebViewContextSpecification
    {
        private bool _navStarting;
        private bool _contentLoading;
        private bool _domContentLoaded;
        private bool _navCompleted;

        protected override void Given()
        {
            base.Given();

            WebView.NavigationStarting += (o, e) => { _navStarting = true; };
            WebView.ContentLoading += (o, e) => { _contentLoading = true; };
            WebView.DOMContentLoaded += (o, e) => { _domContentLoaded = true; };
            WebView.NavigationCompleted += (o, e) =>
            {
                _navCompleted = true;
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(TestConstants.Uris.AboutBlank);
        }

        [TestMethod]
        //[Timeout(TestConstants.Timeouts.Short)]
        public void NavigationEventsCompleted()
        {
            _navStarting.ShouldBeTrue();
            _contentLoading.ShouldBeTrue();
            _domContentLoaded.ShouldBeTrue();
            _navCompleted.ShouldBeTrue();
        }
    }
}
