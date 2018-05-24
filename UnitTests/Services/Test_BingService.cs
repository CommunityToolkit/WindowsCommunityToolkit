// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Bing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Services
{
    [TestClass]
    public class Test_BingService
    {
        private const string Query = @"Windows 10";

        [TestCategory("Services")]
        [TestMethod]
        public async Task Test_BingServiceSearch_Request()
        {
            BingSearchConfig config = new BingSearchConfig();
            config.Country = BingCountry.UnitedStates;
            config.QueryType = BingQueryType.Search;
            config.Query = Query;

            var results = await BingService.Instance.RequestAsync(config, 50);

            Assert.AreEqual(results.Count, 50);
        }

        [TestCategory("Services")]
        [TestMethod]
        public async Task Test_BingServiceNews_Request()
        {
            BingSearchConfig config = new BingSearchConfig();
            config.Country = BingCountry.UnitedStates;
            config.QueryType = BingQueryType.News;
            config.Query = Query;

            var results = await BingService.Instance.RequestAsync(config, 50);

            Assert.IsTrue(results.Count > 5);
        }
    }
}
