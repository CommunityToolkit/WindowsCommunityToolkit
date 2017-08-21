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

using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Bing;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

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
