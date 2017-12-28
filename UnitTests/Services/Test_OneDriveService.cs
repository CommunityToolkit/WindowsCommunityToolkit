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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    using System.Threading.Tasks;
    using Microsoft.Toolkit.Uwp.Services.OneDrive;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using static Microsoft.Toolkit.Uwp.Services.OneDrive.OneDriveEnums;

    [TestClass]
    public class Test_OneDriveService
    {

        [TestCategory("Services")]
        [TestMethod]
        public async Task Test_OneDriveSignIn()
        {
            var oneDriveService = new OneDriveService();
            oneDriveService.Initialize("00000000481F2023", AccountProviderType.Msa, OneDriveScopes.AppFolder| OneDriveScopes.OfflineAccess|OneDriveScopes.WlSignin);
            var result = await oneDriveService.LoginAsync();
            Assert.IsTrue(result);

        }
    }
}