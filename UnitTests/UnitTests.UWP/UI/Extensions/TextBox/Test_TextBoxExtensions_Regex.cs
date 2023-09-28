// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.UWP.UI
{
    [TestClass]
    public class Test_TextBoxExtensions_Regex : VisualUITestBase
    {
        [TestCategory("TextBoxExtensionsRegex")]
        [TestMethod]
        public async Task Test_SetAndCheckValidationModeNormal()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
                xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                xmlns:ui=""using:Microsoft.Toolkit.Uwp.UI"">
                <Grid x:Name=""OuterGrid"" Width=""200"">
                        <TextBox x:Name=""InnerTextBox""
                            ui:TextBoxExtensions.ValidationMode =""Dynamic"" 
                            ui:TextBoxExtensions.ValidationType = ""Number"" />
                </Grid>
            </Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");
                await SetTestContentAsync(treeRoot);
            });
        }
    }
}
