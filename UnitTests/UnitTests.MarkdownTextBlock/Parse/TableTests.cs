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
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse.Elements;
using UITestMethodAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class TableTests : ParseTestBase
    {
        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_Simple()
        {
            AssertEqual(CollapseWhitespace(@"
                | Column 1 | Column 2 | Column 3 |
                |----------|----------|----------|
                | A **cat**| Bob      | Chow     |"),
                new TableBlock
                {
                    ColumnDefinitions = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                    }
                }.AddChildren(
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "Column 1" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "Column 2" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "Column 3" })),
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(
                            new TextRunInline { Text = "A " },
                            new BoldTextInline().AddChildren(new TextRunInline { Text = "cat" })),
                        new TableCell().AddChildren(new TextRunInline { Text = "Bob" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "Chow" }))));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_WithAlignment()
        {
            AssertEqual(CollapseWhitespace(@"
                | Column 1   | Column 2    | Column 3     |
                |:-----------|------------:|:------------:|
                | You        |          You|     You     
                  can align  |    can align|  can align   |
                | left       |        right|   center     "),
                new TableBlock
                {
                    ColumnDefinitions = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Alignment = ColumnAlignment.Left },
                        new TableColumnDefinition { Alignment = ColumnAlignment.Right },
                        new TableColumnDefinition { Alignment = ColumnAlignment.Center },
                    }
                }.AddChildren(
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "Column 1" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "Column 2" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "Column 3" })),
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "You" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "You" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "You" })),
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "can align" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "can align" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "can align" })),
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "left" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "right" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "center" }))));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_Dividers()
        {
            // Too many column dividers is okay.
            AssertEqual(CollapseWhitespace(@"
                        Column A | Column B | Column C
                        -|-|-|-
                        A1 | B1 | C1"),
                new TableBlock
                {
                    ColumnDefinitions = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                    }
                }.AddChildren(
                        new TableRow().AddChildren(
                            new TableCell().AddChildren(new TextRunInline { Text = "Column A" }),
                            new TableCell().AddChildren(new TextRunInline { Text = "Column B" }),
                            new TableCell().AddChildren(new TextRunInline { Text = "Column C" })),
                        new TableRow().AddChildren(
                            new TableCell().AddChildren(new TextRunInline { Text = "A1" }),
                            new TableCell().AddChildren(new TextRunInline { Text = "B1" }),
                            new TableCell().AddChildren(new TextRunInline { Text = "C1" }))));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_Minimal_1()
        {
            AssertEqual(CollapseWhitespace(@"
                |c
                -"),
                new TableBlock
                {
                    ColumnDefinitions = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                    }
                }.AddChildren(
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "c" }))));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_Minimal_2()
        {
            AssertEqual(CollapseWhitespace(@"
                c|
                -"),
                new TableBlock
                {
                    ColumnDefinitions = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                    }
                }.AddChildren(
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "c" }))));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_Minimal_3()
        {
            AssertEqual(CollapseWhitespace(@"
                a|b
                -|-"),
                new TableBlock
                {
                    ColumnDefinitions = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                    }
                }.AddChildren(
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "a" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "b" }))));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_Minimal_4()
        {
            AssertEqual(CollapseWhitespace(@"
                a|b
                :|:"),
                new TableBlock
                {
                    ColumnDefinitions = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Alignment = ColumnAlignment.Left },
                        new TableColumnDefinition { Alignment = ColumnAlignment.Left },
                    }
                }.AddChildren(
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "a" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "b" }))));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_PrematureEnding()
        {
            AssertEqual(CollapseWhitespace(@"
                a|b
                -|-
                A|B
                test"),
                new TableBlock
                {
                    ColumnDefinitions = new List<TableColumnDefinition>
                    {
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                        new TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                    }
                }.AddChildren(
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "a" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "b" })),
                    new TableRow().AddChildren(
                        new TableCell().AddChildren(new TextRunInline { Text = "A" }),
                        new TableCell().AddChildren(new TextRunInline { Text = "B" }))),
                new ParagraphBlock().AddChildren(new TextRunInline { Text = "test" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_Negative_NewParagraph()
        {
            // Must start on a new paragraph.
            AssertEqual(CollapseWhitespace(@"
                before
                | Column 1 | Column 2 | Column 3 |
                |----------|----------|----------|
                | A        | B        | C        |
                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before | Column 1 | Column 2 | Column 3 | |----------|----------|----------| | A        | B        | C        | after" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_Negative_TooFewDividers()
        {
            // Too few dividers doesn't work.
            AssertEqual(CollapseWhitespace(@"
                Column A | Column B | Column C
                -|-
                A1 | B1 | C1"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Column A | Column B | Column C -|- A1 | B1 | C1" }));
        }

        [UITestMethod]
        [TestCategory("Parse - block")]
        public void Table_Negative_MissingDashes()
        {
            // The dashes are normally required
            AssertEqual(CollapseWhitespace(@"
                Column A | Column B | Column C
                ||
                A1 | B1 | C1"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "Column A | Column B | Column C || A1 | B1 | C1" }));
        }

    }
}
