
using System;
using System.Threading.Tasks;
using Microsoft.Toolkit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest_Microsoft.Toolkit
{
    [TestClass]
    public class UnitTestHttp
    {
        private HttpHelper httpHelper = null;

        public UnitTestHttp()
        {
            httpHelper = new HttpHelper(1);
        }

        [TestCategory("HttpHelper")]
        [TestMethod]
        public async Task Test_HttpHelper_SendRequestAsync()
        {
            using (var request = new HttpHelperRequest(new Uri("http://dev.windows.com")))
            {
                using (var response = await httpHelper.SendRequestAsync(request))
                {
                    Assert.IsTrue(response.Success);
                    Assert.IsNotNull(response.Content);
                }
            }
        }

        [TestCategory("HttpHelper")]
        [TestMethod]
        public async Task Test_HttpHelper_GetStreamAsync()
        {
            using (var request = new HttpHelperRequest(new Uri("http://dev.windows.com")))
            {
                using (var response = await httpHelper.GetStreamAsync(request))
                {
                    Assert.IsNotNull(response.Content);

                    response.Content.Dispose();
                }
            }
        }
    }
}
