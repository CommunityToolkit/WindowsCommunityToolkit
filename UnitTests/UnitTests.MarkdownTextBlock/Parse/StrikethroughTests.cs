using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse.Elements;
using UITestMethodAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class StrikethroughTests : ParseTestBase
    {
        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Simple()
        {
            AssertEqual("~~strike~~",
                new ParagraphBlock().AddChildren(
                    new StrikethroughTextInline().AddChildren(
                        new TextRunInline { Text = "strike" })));
        }

        [UITestMethod]
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

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Negative_1()
        {
            AssertEqual(@"~~ strike~~",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "~~ strike~~" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Negative_2()
        {
            AssertEqual(@"~~strike ~~",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "~~strike ~~" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Negative_CannotBeEmpty()
        {
            AssertEqual(@"~~~~",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "~~~~" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Strikethrough_Escape_1()
        {
            AssertEqual(@"\~~strike~~",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "~~strike~~" }));
        }

        [UITestMethod]
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