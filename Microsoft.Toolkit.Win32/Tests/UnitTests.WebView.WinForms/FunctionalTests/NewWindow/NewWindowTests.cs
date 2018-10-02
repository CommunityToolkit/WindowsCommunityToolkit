// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
