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
    public class HorizontalRuleTests : ParseTestBase
    {
        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_Simple()
        {
            AssertEqual("***",
                new HorizontalRuleBlock());
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_StarsAndSpaces()
        {
            AssertEqual("* * * * *",
                new HorizontalRuleBlock());
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_Alt()
        {
            AssertEqual("---",
                new HorizontalRuleBlock());
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_Alt_BeforeAfter()
        {
            AssertEqual(CollapseWhitespace(@"
                before

                ---
                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new HorizontalRuleBlock(),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_Alt2()
        {
            AssertEqual(CollapseWhitespace(@"
                before
                ___
                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new HorizontalRuleBlock(),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_BeforeAfter()
        {
            // Text on other lines is okay.
            AssertEqual(CollapseWhitespace(@"
                before
                *****
                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new HorizontalRuleBlock(),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_Negative()
        {
            // Text on the same line is not.
            AssertEqual(CollapseWhitespace(@"
                before
                ****d
                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before ****d after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_Negative_FourStars()
        {
            // Also, must be at least 3 stars.
            AssertEqual(CollapseWhitespace(@"
                before
                **
                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before ** after" }));
        }
    }
}