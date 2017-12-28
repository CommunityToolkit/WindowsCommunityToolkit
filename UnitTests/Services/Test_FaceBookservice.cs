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

using Microsoft.Toolkit.Uwp.Services.Facebook;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    [TestClass]
    public class Test_FaceBookservice
    {
        [TestCategory("Services")]
        [TestMethod]
        public async Task Test_Facebook_GetPages_Request()
        {
            Assert.Inconclusive();
            FacebookService.Instance.Initialize("xxxxxxxxxxxxx", FacebookPermissions.PublicProfile | FacebookPermissions.UserPosts | FacebookPermissions.PublishActions | FacebookPermissions.UserPhotos | FacebookPermissions.ManagePages);
            if (!await FacebookService.Instance.LoginAsync())
            {
                return;
            }

            var item = await FacebookService.Instance.RequestAsync(FacebookDataConfig.MyFeed);

            var fields = "category,name,id";
            var item2 = await FacebookService.Instance.RequestAsync<dynamic>(FacebookDataConfig.MyPages, 20, fields);

            // var facebookService = new FacebookService
        }
    }
}