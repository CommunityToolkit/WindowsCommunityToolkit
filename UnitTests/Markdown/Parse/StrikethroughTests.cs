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

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse;
using UITestMethodAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class StrikethroughTests : ParseTestBase
    {
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Simple()
        {
            AssertEqual("~~strike~~",
                new ParagraphBlock().AddChildren(
                    new StrikethroughTextInline().AddChildren(
                        new TextRunInline { Text = "strike" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Inline()
        {
            AssertEqual("This is ~~strike~~ text",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "This is " },
                    new StrikethroughTextInline().AddChildren(
                        new TextRunInline { Text = "strike" }),
                    new TextRunInline { Text = " text" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Negative_1()
        {
            AssertEqual(@"~~ strike~~",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "~~ strike~~" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Negative_2()
        {
            AssertEqual(@"~~strike ~~",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "~~strike ~~" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Negative_CannotBeEmpty()
        {
            AssertEqual(@"~~~~",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "~~~~" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Escape_1()
        {
            AssertEqual(@"\~~strike~~",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "~~strike~~" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Escape_2()
        {
            AssertEqual(@"~~strike\~~",
                new ParagraphBlock().AddChildren(
                    new StrikethroughTextInline().AddChildren(
                        new TextRunInline { Text = @"strike\" })));
        }
    }
}