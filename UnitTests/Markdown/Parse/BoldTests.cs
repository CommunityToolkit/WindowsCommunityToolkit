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
    public class BoldTests : ParseTestBase
    {
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Simple()
        {
            AssertEqual("**bold**", new ParagraphBlock().AddChildren(
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "bold" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Simple_Alt()
        {
            AssertEqual("__bold__", new ParagraphBlock().AddChildren(
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "bold" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Inline()
        {
            AssertEqual("This is **bold** text", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "This is " },
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "bold" }),
                new TextRunInline { Text = " text" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Inline_Alt()
        {
            AssertEqual("This is __bold__ text", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "This is " },
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "bold" }),
                new TextRunInline { Text = " text" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Inside_Word()
        {
            AssertEqual("before**middle**end", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "before" },
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "middle" }),
                new TextRunInline { Text = "end" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Negative_1()
        {
            AssertEqual("before** middle **end", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "before** middle **end" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Negative_2()
        {
            AssertEqual("before** middle**end", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "before** middle**end" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Negative_CannotBeEmpty()
        {
            AssertEqual("before ****** after",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before ****** after" }));
        }
    }
}
