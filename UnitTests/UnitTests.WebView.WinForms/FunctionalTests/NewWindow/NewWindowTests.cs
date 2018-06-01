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

//namespace Microsoft.Windows10.Forms.Controls.FunctionalTests.NewWindow
//{
//    [TestClass]
//    public class NewWindowTests : HostFormWebViewContextSpecification
//    {
//        private bool _newWindowRequested;
//        private readonly string _content = $@"
//<!DOCTYPE html>
//<body onload=""window.open('{TestConstants.Uris.ExampleCom}', '_blank');"">
//<h1>JavaScript window.open</h1>
//</body>
//</html>
//";

//        protected override void Given()
//        {
//            base.Given();

//            WebView.NewWindowRequested += (o, e) =>
//            {
//                _newWindowRequested = true;
//                WriteLine($"NewWindowRequested: Uri:{e.Uri}, Referrer: {e.Referrer}, Handled: {e.Handled}");
//                Form.Close();
//            };
//        }

//        protected override void When()
//        {
//            NavigateToStringAndWaitForFormClose(_content);
//        }

//        [TestMethod]
//        [Timeout(TestConstants.Timeouts.Short)]
//        [Ignore("NewWindowRequested event is not firing")]
//        public void NewWindowRequestedEventFired()
//        {
//            _newWindowRequested.ShouldBeTrue();
//        }
//    }
//}
