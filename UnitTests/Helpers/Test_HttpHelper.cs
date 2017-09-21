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
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_HttpHelper
    {
        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_HttpHelper_SendRequestAsync()
        {
#pragma warning disable CS0612 // Type or member is obsolete
            using (var request = new HttpHelperRequest(new Uri("http://dev.windows.com")))
            {
                using (var response = await HttpHelper.Instance.SendRequestAsync(request))
                {
                    Assert.IsTrue(response.Success);
                    Assert.IsNotNull(response.Content);
                }
            }
#pragma warning restore CS0612 // Type or member is obsolete
        }
    }
}
