// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//namespace Microsoft.Windows10.Forms.Controls.FunctionalTests.UnsupportedUriScheme
//{
//    [TestClass]
//    public class UnsupportedUriSchemeTests : HostFormWebViewContextSpecification
//    {
//        private bool _eventFired;

//        protected override void Given()
//        {
//            base.Given();

//            WebView.UnsupportedUriSchemeIdentified += (o, e) =>
//            {
//                WriteLine($"UnsupportedUriSchemeIdentified: Uri: {e.Uri}, Handled: {e.Handled}");
//                _eventFired = true;
//                Form.Close();
//            };
//        }

//        protected override void When()
//        {
//            // Protocol exists as part of RFC6733
//            var uri = new UriBuilder("aaa", "localhost", 1813, ";transport=tcp").Uri;
//            WriteLine(uri.ToString());
//            NavigateAndWaitForFormClose(uri);
//        }

//        [TestMethod]
//        [Timeout(Constants.Timeouts.Medium)]
//        [Ignore("E_ABORT (0x80004004) is raised when an unhandled protocol is received")]
//        public void UnsupportedUriSchemeEventFiredForAAA()
//        {
//            _eventFired.ShouldBeTrue();
//        }
//    }
//}
