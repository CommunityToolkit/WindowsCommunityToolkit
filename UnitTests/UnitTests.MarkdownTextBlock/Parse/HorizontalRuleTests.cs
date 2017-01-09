using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse.Elements;
using UITestMethodAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace UnitTests.Markdown.Parse
{ 
    [TestClass]
    public class HorizontalRuleTests : ParseTestBase
    {
        [UITestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_Simple()
        {
            AssertEqual("***",
                new HorizontalRuleBlock());
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_StarsAndSpaces()
        {
            AssertEqual("* * * * *",
                new HorizontalRuleBlock());
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalRule_Alt()
        {
            AssertEqual("---",
                new HorizontalRuleBlock());
        }

        [UITestMethod]
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

        [UITestMethod]
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

        [UITestMethod]
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

        [UITestMethod]
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

        [UITestMethod]
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