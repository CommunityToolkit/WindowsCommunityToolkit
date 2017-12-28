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