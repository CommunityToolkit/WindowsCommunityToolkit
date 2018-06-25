// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//namespace Microsoft.Windows10.Forms.Controls.FunctionalTests.WebResourceRequested
//{
//    [TestClass]
//    public class WebResourceRequested : HostFormWebViewContextSpecification
//    {
//        private Uri _initialUri;
//        private Uri _subResourceUri;
//        private bool _setEvent;

//        protected override void Given()
//        {
//            _initialUri = TestConstants.Uris.ExampleCom;
//            _subResourceUri = new Uri(TestConstants.Uris.ExampleCom, "foo.js");

//            base.Given();

//            WebView.WebResourceRequested += (o, e) =>
//            {
//                WriteLine($"Request received for {e.Request.RequestUri}");

//                var requestMessage = e.Request;
//                var deferral = e.GetDeferral();
//                var responseMessage = new HttpResponseMessage(HttpStatusCode.Ok);
//                var isInitialUri = HttpRequestUriEqualityComparer.Default.Equals(_initialUri, requestMessage.RequestUri);
//                var isSubResourceUri = HttpRequestUriEqualityComparer.Default.Equals(_subResourceUri, requestMessage.RequestUri);

//                WriteLine($"IsInitialUri: {isInitialUri}");
//                WriteLine($"IsSubResourceUri: {isSubResourceUri}");

//                // Verify Uri is expected
//                if (isInitialUri)
//                {
//                    responseMessage.Content = new HttpStringContent("<!DOCTYPE html><head><script src='foo.js'></script></head><body>Redirected response</body></html>");
//                }
//                else if (isSubResourceUri)
//                {
//                    responseMessage.Content = new HttpStringContent("");

//                    // Only set the event once we've received notification of this resource request.
//                    // This validates that our initial response was actually used by the WebView control
//                    _setEvent = true;
//                    Form.Close();
//                }
//                else
//                {
//                    Assert.Fail("RequestUri did not match any of the expected values.");
//                }

//                e.Response = responseMessage;
//                deferral.Complete();

//            };
//        }

//        protected override void When()
//        {
//            NavigateAndWaitForFormClose(_initialUri);
//        }

//        [TestMethod]
//        [Timeout(TestConstants.Timeouts.Longest)]
//        public void ARedirectedResourceIsUsedByWebView()
//        {
//            _setEvent.ShouldBeTrue();
//        }
//    }
//}
