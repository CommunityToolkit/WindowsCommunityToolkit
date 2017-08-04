using Microsoft.Toolkit.Uwp.Services.Facebook;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            FacebookService.Instance.Initialize("687964081409306", FacebookPermissions.PublicProfile | FacebookPermissions.UserPosts | FacebookPermissions.PublishActions | FacebookPermissions.UserPhotos | FacebookPermissions.ManagePages);
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