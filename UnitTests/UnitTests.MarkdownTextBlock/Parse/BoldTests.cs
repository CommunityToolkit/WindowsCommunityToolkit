using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse.Elements;
using UITestMethodAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class BoldTests : ParseTestBase
    {
        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Simple()
        {
            AssertEqual("**bold**", new ParagraphBlock().AddChildren(
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "bold" })));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Simple_Alt()
        {
            AssertEqual("__bold__", new ParagraphBlock().AddChildren(
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "bold" })));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Inline()
        {
            AssertEqual("This is **bold** text", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "This is " },
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "bold" }),
                new TextRunInline { Text = " text" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Inline_Alt()
        {
            AssertEqual("This is __bold__ text", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "This is " },
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "bold" }),
                new TextRunInline { Text = " text" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Inside_Word()
        {
            AssertEqual("before**middle**end", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "before" },
                new BoldTextInline().AddChildren(
                    new TextRunInline { Text = "middle" }),
                new TextRunInline { Text = "end" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Negative_1()
        {
            AssertEqual("before** middle **end", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "before** middle **end" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Negative_2()
        {
            AssertEqual("before** middle**end", new ParagraphBlock().AddChildren(
                new TextRunInline { Text = "before** middle**end" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Bold_Negative_CannotBeEmpty()
        {
            AssertEqual("before ****** after",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before ****** after" }));
        }
    }
}
