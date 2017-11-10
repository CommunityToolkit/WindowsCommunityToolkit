﻿// ******************************************************************
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
    public class CodeTests : ParseTestBase
    {
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Code_Inline()
        {
            AssertEqual("Here is some `inline code` lol",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Here is some " },
                    new CodeInline { Text = "inline code" },
                    new TextRunInline { Text = " lol" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Code_Inline_Boundary()
        {
            AssertEqual("before` middle `after",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" },
                    new CodeInline { Text = "middle" },
                    new TextRunInline { Text = "after" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Code_Inline_Formatting()
        {
            // Formatting is ignored inside code.
            AssertEqual("Here is some `ignored **formatting** inside code`",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Here is some " },
                    new CodeInline { Text = "ignored **formatting** inside code" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Code_Inline_EscapedStartChar()
        {
            AssertEqual(@"Here is some \`escaped code`",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Here is some `escaped code`" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Code_Inline_WhiteSpace()
        {
            AssertEqual(@"Some     `   spacy     text    `    with spaces",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Some     " },
                    new CodeInline { Text = "spacy     text" },
                    new TextRunInline { Text = "    with spaces" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Code_Inline_InnerEscapeSequence()
        {
            AssertEqual(@"`one\\two`",
                new ParagraphBlock().AddChildren(
                    new CodeInline { Text = @"one\\two" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Code_Inline_AlternateSyntax()
        {
            AssertEqual(@"``There is a literal backtick (`) here.``",
                new ParagraphBlock().AddChildren(
                    new CodeInline { Text = @"There is a literal backtick (`) here." }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Code_Inline_Negative_CannotBeEmpty()
        {
            AssertEqual("before ``` after",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before ``` after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Code_Block()
        {
            // Multi-line code block.  Should have a border and scroll, not wrap!
            AssertEqual(CollapseWhitespace(@"
                before

                    Code line 1

                    Code line 2

                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new CodeBlock { Text = "Code line 1\r\n\r\nCode line 2" },
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Code_Block_LeadingSpace()
        {
            // Code blocks win over quotes.
            AssertEqual("      2 leading spaces",
                new CodeBlock { Text = "  2 leading spaces" });
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Code_Block_WinsOverQuote()
        {
            // Code blocks win over quotes.
            AssertEqual("    >not quoted",
                new CodeBlock { Text = ">not quoted" });
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Code_Block_With_Indent()
        {
            // Multi-line code block.  Should have a border and scroll, not wrap!
            AssertEqual(CollapseWhitespace(@"
                before

                    Code
                      More code with **stars** and   spacing
                    Even more code

                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new CodeBlock { Text = "Code\r\n  More code with **stars** and   spacing\r\nEven more code" },
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Code_Block_WithTabs()
        {
            // A tab character can start a code block.
            // Tab characters inside the code are converted to 1-4 spaces.
            AssertEqual(CollapseWhitespace(@"
                before

                " + "\t" + @"Code
                " + "\t\t" + @"can
                " + "\tbe\t" + @"tabbed
                " + " \thole\t" + @"tabbed

                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new CodeBlock { Text = "Code\r\n    can\r\nbe  tabbed\r\nhole    tabbed" },
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Code_Block_WithLeadingBlankLine()
        {
            // Leading lines that are purely whitespace should be ignored.
            AssertEqual(CollapseWhitespace(@"
                before

                     
                    line 1

                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new CodeBlock { Text = "line 1" },
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Code_Block_WithTrailingBlankLine()
        {
            // Trailing lines that are purely whitespace should be ignored.
            AssertEqual(CollapseWhitespace(@"
                before

                    line 1
                     

                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new CodeBlock { Text = "line 1" },
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "after" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Code_Block_Negative()
        {
            // Multi-line code blocks must start with a new paragraph.
            AssertEqual(CollapseWhitespace(@"
                before
                    Code
                        More code
                    Even more code
                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before     Code         More code     Even more code after" }));
        }
    }
}
