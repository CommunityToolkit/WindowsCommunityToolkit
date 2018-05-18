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
