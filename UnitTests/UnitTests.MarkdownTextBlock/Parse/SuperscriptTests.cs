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
    public class SuperscriptTests : ParseTestBase
    {
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_Simple()
        {
            AssertEqual("Using the carot sign ^will create exponentials",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Using the carot sign " },
                    new SuperscriptTextInline().AddChildren(
                        new TextRunInline { Text = "will" }),
                    new TextRunInline { Text = " create exponentials" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_Nested()
        {
            AssertEqual("A^B^C",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "A" },
                    new SuperscriptTextInline().AddChildren(
                        new TextRunInline { Text = "B" },
                        new SuperscriptTextInline().AddChildren(
                            new TextRunInline { Text = "C" }))));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_WithParentheses()
        {
            // The text to superscript can be enclosed in brackets.
            AssertEqual("This is a sentence^(This is a note in superscript).",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "This is a sentence" },
                    new SuperscriptTextInline().AddChildren(
                        new TextRunInline { Text = "This is a note in superscript" }),
                    new TextRunInline { Text = "." }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_StartOfLine()
        {
            // Start of the line is okay.
            AssertEqual("^Test",
                new ParagraphBlock().AddChildren(
                    new SuperscriptTextInline().AddChildren(
                        new TextRunInline { Text = "Test" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_TwoInARow()
        {
            AssertEqual("^a ^b",
                new ParagraphBlock().AddChildren(
                    new SuperscriptTextInline().AddChildren(
                        new TextRunInline { Text = "a" }),
                    new TextRunInline { Text = " " },
                    new SuperscriptTextInline().AddChildren(
                        new TextRunInline { Text = "b" })));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_Escape()
        {
            // Escaped caret.
            AssertEqual(@"\^Test",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "^Test" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_Negative()
        {
            AssertEqual("Using the carot sign ^ incorrectly",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Using the carot sign ^ incorrectly" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_Negative_WithParentheses()
        {
            AssertEqual("Using the carot sign ^ (incorrectly)",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Using the carot sign ^ (incorrectly)" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_Negative_WithParentheses_2()
        {
            AssertEqual("Using the carot sign ^(incorrectly",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Using the carot sign ^(incorrectly" }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void Superscript_Negative_WithParentheses_3()
        {
            AssertEqual("Using the carot sign ^(across\r\n\r\nparagraphs)",
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Using the carot sign ^(across" }),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "paragraphs)" }));
        }
    }
}
