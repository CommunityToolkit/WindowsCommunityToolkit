﻿// ******************************************************************
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

using Microsoft.Toolkit.Uwp.Connectivity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_ConnectionHelper
    {
        public NetworkHelper NetworkHelper { get; private set; }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ConnectionHelper_IsInternetOnMeteredConnection()
        {
            Assert.IsFalse(NetworkHelper.Instance.ConnectionInformation.IsInternetOnMeteredConnection);
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public void Test_ConnectionHelper_IsInternetAvailable()
        {
            Assert.IsTrue(NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable);
        }
    }
}
