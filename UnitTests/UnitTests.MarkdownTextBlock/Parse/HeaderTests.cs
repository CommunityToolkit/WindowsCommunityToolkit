using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse.Elements;
using UITestMethodAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class HeaderTests : ParseTestBase
    {
        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_1()
        {
            AssertEqual("#Header 1",
                new HeaderBlock { HeaderLevel = 1 }.AddChildren(
                    new TextRunInline { Text = "Header 1" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_1_Alt()
        {
            AssertEqual(CollapseWhitespace(@"
                Header 1
                ="),
                new HeaderBlock { HeaderLevel = 1 }.AddChildren(
                    new TextRunInline { Text = "Header 1" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_1_Alt_WithHashes()
        {
            AssertEqual(CollapseWhitespace(@"
                Header 1##
                ="),
                new HeaderBlock { HeaderLevel = 1 }.AddChildren(
                    new TextRunInline { Text = "Header 1##" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_2()
        {
            AssertEqual("##Header 2",
                new HeaderBlock { HeaderLevel = 2 }.AddChildren(
                    new TextRunInline { Text = "Header 2" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_2_Alt()
        {
            // Note: trailing spaces on the second line are okay.
            AssertEqual(CollapseWhitespace(@"
                Header 2
                --  "),
                new HeaderBlock { HeaderLevel = 2 }.AddChildren(
                    new TextRunInline { Text = "Header 2" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_3()
        {
            AssertEqual("###Header 3",
                new HeaderBlock { HeaderLevel = 3 }.AddChildren(
                    new TextRunInline { Text = "Header 3" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_4()
        {
            AssertEqual("####Header 4",
                new HeaderBlock { HeaderLevel = 4 }.AddChildren(
                    new TextRunInline { Text = "Header 4" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_5()
        {
            AssertEqual("#####Header 5",
                new HeaderBlock { HeaderLevel = 5 }.AddChildren(
                    new TextRunInline { Text = "Header 5" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_6()
        {
            AssertEqual("######Header 6",
                new HeaderBlock { HeaderLevel = 6 }.AddChildren(
                    new TextRunInline { Text = "Header 6" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_6_WithTrailingHashSymbols()
        {
            AssertEqual("###### Header 6 ######",
                new HeaderBlock { HeaderLevel = 6 }.AddChildren(
                    new TextRunInline { Text = " Header 6 " }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_7()
        {
            AssertEqual("#######Header 6",
                new HeaderBlock { HeaderLevel = 6 }.AddChildren(
                    new TextRunInline { Text = "#Header 6" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_1_Empty()
        {
            AssertEqual("#",
                new HeaderBlock { HeaderLevel = 1, Inlines = new List<MarkdownInline>() });
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_Hashes()
        {
            AssertEqual("## # # ##",
                new HeaderBlock { HeaderLevel = 2 }.AddChildren(
                    new TextRunInline { Text = " # # " }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_6_Empty()
        {
            AssertEqual("#######",
                new HeaderBlock { HeaderLevel = 6, Inlines = new List<MarkdownInline>() });
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_1_Inline()
        {
            AssertEqual(CollapseWhitespace(@"
                before
                #Header
                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new HeaderBlock { HeaderLevel = 1 }.AddChildren(
                    new TextRunInline { Text = "Header" }),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "after" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_Negative_RogueCharacter()
        {
            // The second line after a heading must be all === or all ---
            AssertEqual(CollapseWhitespace(@"
                Header 1
                =f"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Header 1 =f" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Header_Negative_ExtraSpace()
        {
            // The second line after a heading must not start with a space
            AssertEqual(CollapseWhitespace(@"
                Header 1
                  ="),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Header 1   =" }));
        }
    }
}
