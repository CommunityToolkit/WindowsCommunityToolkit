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
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse.Elements;
using UITestMethodAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class ItalicTests : ParseTestBase
    {
        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Simple()
        {
            AssertEqual("*italic*",
                new ParagraphBlock().AddChildren(
                    new ItalicTextInline().AddChildren(
                        new TextRunInline { Text = "italic" })));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Simple_Alt()
        {
            AssertEqual("_italic_",
                new ParagraphBlock().AddChildren(
                    new ItalicTextInline().AddChildren(
                        new TextRunInline { Text = "italic" })));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Inline()
        {
            AssertEqual("This is *italic* text",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "This is " },
                    new ItalicTextInline().AddChildren(
                        new TextRunInline { Text = "italic" }),
                    new TextRunInline { Text = " text" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Inline_Alt()
        {
            AssertEqual("This is _italic_ text",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "This is " },
                    new ItalicTextInline().AddChildren(
                        new TextRunInline { Text = "italic" }),
                    new TextRunInline { Text = " text" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Inside_Word()
        {
            AssertEqual("before*middle*end",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" },
                    new ItalicTextInline().AddChildren(
                        new TextRunInline { Text = "middle" }),
                    new TextRunInline { Text = "end" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_MultiLine()
        {
            // Does work across lines.
            AssertEqual("italics *does\r\n" +
                "work* across line breaks",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "italics " },
                    new ItalicTextInline().AddChildren(
                        new TextRunInline { Text = "does work" }),
                    new TextRunInline { Text = " across line breaks" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Escape()
        {
            AssertEqual(@"\*escape the formatting syntax\*",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "*escape the formatting syntax*" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Negative_1()
        {
            AssertEqual("before* middle *end",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before* middle *end" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Negative_2()
        {
            AssertEqual("before* middle*end",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before* middle*end" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Negative_3()
        {
            // There must be a valid end italics marker otherwise the whole thing is ignored.
            AssertEqual("This is *not italics * text",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "This is *not italics * text" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Negative_MultiParagraph()
        {
            // Doesn't work across paragraphs.
            AssertEqual(CollapseWhitespace(@"
                italics *doesn't

                apply* across paragraphs"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "italics *doesn't" }),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "apply* across paragraphs" }));
        }

        [UITestMethod]
        [TestCategory("Parse - inline")]
        public void Italic_Negative_CannotBeEmpty()
        {
            AssertEqual("before *** after",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before *** after" }));
        }
    }
}
