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
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Html;
using Windows.Storage;
using Windows.Storage.Streams;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_ClipboardHelper
    {
        private string _testHtml;

        private string _testRtf;

        private string _testText;

        [TestInitialize]
        public void SetUp()
        {
            _testText = "Hello world";

            _testRtf = @"{\rtf1\ansi\ansicpg936\deff0\nouicompat\deflang1033\deflangfe2052{\fonttbl{\f0\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}
{\*\generator Riched20 10.0.14393}\viewkind4\uc1
\pard\sa200\sl276\slmult1\f0\fs22\lang2052 HelloWorld\par
}
 ";

            _testHtml = "<div style=\"color:red;\">Hello world</div>";
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_ClipboardHelper_GetImageAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                Clipboard.Clear();
                var nullResult = await ClipboardHelper.GetImageAsync();
                Assert.IsNull(nullResult);

                var dataPackage = new DataPackage();
                var testImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/StoreLogo.png"));
                var imageReference = RandomAccessStreamReference.CreateFromFile(testImage);
                dataPackage.SetBitmap(imageReference);
                Clipboard.SetContent(dataPackage);

                var testImageBytes = await testImage.ReadBytesAsync();
                var result = await ClipboardHelper.GetImageAsync();
                CollectionAssert.AreEqual(testImageBytes, result);
            });
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_ClipboardHelper_GetRawHtmlAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                Clipboard.Clear();
                var nullResult = await ClipboardHelper.GetHtmlAsync();
                Assert.IsNull(nullResult);

                var dataPackage = new DataPackage();
                var htmlFormat = HtmlFormatHelper.CreateHtmlFormat(_testHtml);
                dataPackage.SetHtmlFormat(htmlFormat);
                Clipboard.SetContent(dataPackage);

                var result = await ClipboardHelper.GetHtmlAsync();
                Assert.AreEqual(_testHtml, result);
            });
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_ClipboardHelper_GetRtfAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                Clipboard.Clear();
                var nullResult = await ClipboardHelper.GetRtfAsync();
                Assert.IsNull(nullResult);

                var dataPackage = new DataPackage();
                dataPackage.SetRtf(_testRtf);
                Clipboard.SetContent(dataPackage);

                var result = await ClipboardHelper.GetRtfAsync();
                Assert.AreEqual(_testRtf, result);
            });
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_ClipboardHelper_GetTextAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                Clipboard.Clear();
                var nullResult = await ClipboardHelper.GetTextAsync();
                Assert.IsNull(nullResult);

                var dataPackage = new DataPackage();
                dataPackage.SetText(_testText);
                Clipboard.SetContent(dataPackage);

                var result = await ClipboardHelper.GetTextAsync();
                Assert.AreEqual(_testText, result);
            });
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_ClipboardHelper_SetRawHtml()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    ClipboardHelper.SetHtml(null);
                });

                ClipboardHelper.SetHtml(_testHtml);
                var dataPackageView = Clipboard.GetContent();
                var htmlFormat = await dataPackageView.GetHtmlFormatAsync();
                var htmlResult = HtmlFormatHelper.GetStaticFragment(htmlFormat);
                var textResult = await dataPackageView.GetTextAsync();
                Assert.AreEqual(_testHtml, htmlResult);
                Assert.AreEqual(HtmlUtilities.ConvertToText(_testHtml), textResult);
            });
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_ClipboardHelper_SetRtf()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
           {
               Assert.ThrowsException<ArgumentNullException>(() =>
               {
                   ClipboardHelper.SetRtf(null);
               });

               ClipboardHelper.SetRtf(_testRtf);
               var dataPackageView = Clipboard.GetContent();
               var result = await dataPackageView.GetRtfAsync();
               Assert.AreEqual(_testRtf, result);
           });
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_ClipboardHelper_SetText()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    ClipboardHelper.SetText(null);
                });

                ClipboardHelper.SetText(string.Empty);
                var emptyContent = Clipboard.GetContent();
                Assert.AreEqual(string.Empty, await emptyContent.GetTextAsync());

                ClipboardHelper.SetText(_testText);
                var dataPackageView = Clipboard.GetContent();
                var result = await dataPackageView.GetTextAsync();
                Assert.AreEqual(_testText, result);
            });
        }
    }
}