using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Services.Bing;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTests.Services
{
    [TestClass]
    public class Test_BingService
    {
        [TestCategory("Services")]
        [TestMethod]
        public async Task Test_BingService_Request()
        {
            const string QUERY = @"Windows 10";

            BingSearchConfig config = new BingSearchConfig();
            config.Country = BingCountry.France;
            config.Query = QUERY;

            var results = await BingService.Instance.RequestAsync(config, 50);

            Assert.AreEqual(results.Count(), 50);
        }
    }
}
