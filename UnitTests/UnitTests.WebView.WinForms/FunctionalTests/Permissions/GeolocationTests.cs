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
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Permissions
{
    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class GeolocationPermissionRequestImmediatelyGranted : HostFormWebViewContextSpecification
    {
        private WebViewControlPermissionRequest _permissionRequest;
//        private string _content = @"
//<h1>TEST</h1>
//<div id=""out""></div>
//<script>
//var output = document.getElementById('out');
//if ('geolocation' in navigator) {  
//  navigator.geolocation.getCurrentPosition(function(position) {
//    var lat = position.coords.latitude;
//    var long = position.coords.longitude;
    
//    output.innerHTML = 'Latitude is ' + lat + '<br />Longitude is ' + long + '<br />';    
//  });
//} else {
//  output.innerHTML = '<b>Geolocation is not supported by your browser</b>';
//}
//</script>
//";

        protected override void Given()
        {
            base.Given();

            WebView.IsJavaScriptEnabled = true;

            WebView.PermissionRequested += (o, e) =>
            {
                e.PermissionRequest.ShouldNotBeNull();

                _permissionRequest = e.PermissionRequest;

                WriteLine($"Permission Request: Id: {e.PermissionRequest.Id}, PermissionType: {e.PermissionRequest.PermissionType}");
                e.PermissionRequest.PermissionType.ShouldEqual(WebViewControlPermissionType.Geolocation);

                if (e.PermissionRequest.State == WebViewControlPermissionState.Defer)
                {
                    WebView.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Allow();
                }
                else
                {
                    e.PermissionRequest.Allow();
                }

                Form.Close();
            };
        }

        protected override void When()
        {
            //BUG: Geolocation request not working with NavigateToString
            //NavigateToStringAndWaitForFormClose(_content);
            NavigateAndWaitForFormClose(new Uri("https://codepen.io/rjmurillo/pen/MVaKbJ"));
        }

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Long)]
        public void PermissionRequestReceived()
        {
            _permissionRequest.ShouldNotBeNull();
            _permissionRequest.State.ShouldEqual(WebViewControlPermissionState.Allow);
        }
    }

    [TestClass]
    [TestCategory(TestConstants.Categories.Nav)]
    public class GeolocationPermissionRequestDeferredThenGranted : HostFormWebViewContextSpecification
    {
        private uint _permissionRequest;

        protected override void Given()
        {
            base.Given();

            WebView.IsJavaScriptEnabled = true;

            WebView.PermissionRequested += (o, e) =>
            {
                e.PermissionRequest.ShouldNotBeNull();

                _permissionRequest = e.PermissionRequest.Id;

                WriteLine($"Permission Request: Id: {e.PermissionRequest.Id}, PermissionType: {e.PermissionRequest.PermissionType}");
                e.PermissionRequest.PermissionType.ShouldEqual(WebViewControlPermissionType.Geolocation);

                if (e.PermissionRequest.State == WebViewControlPermissionState.Defer)
                {
                }
                else
                {
                    e.PermissionRequest.Defer();
                }

                WebView.GetDeferredPermissionRequestById(_permissionRequest)?.Allow();
                Form.Close();
            };
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(new Uri("https://codepen.io/rjmurillo/pen/MVaKbJ"));
        }

        // TODO: Verify there are no more deferred permission requests

        [TestMethod]
        [Timeout(TestConstants.Timeouts.Long)]
        public void PermissionRequestReceived()
        {
            _permissionRequest.ShouldNotEqual<uint>(0);
        }
    }

}
