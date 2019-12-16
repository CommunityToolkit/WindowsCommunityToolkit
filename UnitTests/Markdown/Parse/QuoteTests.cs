// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class QuoteTests : ParseTestBase
    {
        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_SingleLine()
        {
            AssertEqual(">Quoted text",
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quoted text" })));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_MultiLine_1()
        {
            AssertEqual(CollapseWhitespace(@"
                > Single line

                >Quote  
                with line break


                 > Spaces
                  > and line continuation

                normal text"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Single line" })),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote\r\nwith line break" })),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Spaces and line continuation" })),
                new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "normal text" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_MultiLine_2()
        {
            AssertEqual(CollapseWhitespace(@"
                > Single line
                >"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Single line" })));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Empty()
        {
            AssertEqual(CollapseWhitespace(@"
                >"),
                new QuoteBlock() { Blocks = System.Array.Empty<MarkdownBlock>() });
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_MultiLine_Simple()
        {
            AssertEqual(CollapseWhitespace(@"
                before
                >Quoted
                >Quoted, line 2
                after"),
                new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = "before" }),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quoted Quoted, line 2 after" })));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_WithHeader()
        {
            AssertEqual(CollapseWhitespace(@"
                >#header
                > #header
                >  #not a header
                >
                >    text
                >
                >     code"),
                new QuoteBlock().AddChildren(
                    new HeaderBlock { HeaderLevel = 1 }.AddChildren(
                        new TextRunInline { Text = "header" }),
                    new HeaderBlock { HeaderLevel = 1 }.AddChildren(
                        new TextRunInline { Text = "header" }),
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = " #not a header" }),
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "   text" }),
                    new CodeBlock { Text = "code" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Nested_1()
        {
            AssertEqual(CollapseWhitespace(@"
                >Quote1.1
                >>Quote1.Nest1.1
                >Quote1.Nest1.2"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.1" }),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.1 Quote1.Nest1.2" }))));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Nested_2()
        {
            AssertEqual(CollapseWhitespace(@"
                >Quote1.1
                >>Quote1.Nest1.1
                >
                >Quote1.2"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.1" }),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.1" })),
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.2" })));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Nested_3()
        {
            AssertEqual(CollapseWhitespace(@"
                >Quote1.1
                >>Quote1.Nest1.1
                >>
                >Quote1.Nest1.2"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.1" }),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.1" }),
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.2" }))));
        }

        // This was the previous behavior that is different from CommonMark
        [Ignore]
        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Nested_6_NonBreaking()
        {
            AssertEqual(CollapseWhitespace(@"
                >Quote1.1
                >>Quote1.Nest1.1
                >
                >>Quote1.Nest1.2"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.1" }),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.1" }),
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.2" }))));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Nested_6()
        {
            AssertEqual(CollapseWhitespace(@"
                >Quote1.1
                >>Quote1.Nest1.1
                >
                >>Quote1.Nest2.1"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.1" }),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.1" })),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest2.1" }))));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Nested_5()
        {
            AssertEqual(CollapseWhitespace(@"
                >Quote1.1
                >>Quote1.Nest1.1
                >
                >
                >>Quote1.Nest2.1"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.1" }),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.1" })),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest2.1" }))));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Nested_4()
        {
            AssertEqual(CollapseWhitespace(@"
                >Quote1.1
                >>Quote1.Nest1.1
                >

                >>Quote2.Nest1.1"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.1" }),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.1" }))),
                new QuoteBlock().AddChildren(
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote2.Nest1.1" }))));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Nested_7()
        {
            AssertEqual(CollapseWhitespace(@"
                >Quote1.1
                >>Quote1.Nest1.1
                
                >Quote2.1"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.1" }),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.1" }))),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote2.1" })));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Nested_8()
        {
            AssertEqual(CollapseWhitespace(@"
                >Quote1.1
                >>Quote1.Nest1.1
                
                >>Quote2.Nest1.1"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "Quote1.1" }),
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote1.Nest1.1" }))),
                new QuoteBlock().AddChildren(
                    new QuoteBlock().AddChildren(
                        new ParagraphBlock().AddChildren(
                            new TextRunInline { Text = "Quote2.Nest1.1" }))));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_WithTable()
        {
            AssertEqual(CollapseWhitespace(@"
                This doesn't work:

                > a|b
                -|-
                1|2

                But this does:

                > a|b
                > -|-
                > 1|2"),

                new ParagraphBlock().AddChildren(new TextRunInline { Text = "This doesn't work:" }),
                new TableBlock
                {
                    ColumnDefinitions = new List<TableBlock.TableColumnDefinition>
                    {
                        new TableBlock.TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                        new TableBlock.TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                    }
                }.AddChildren(
                    new TableBlock.TableRow().AddChildren(
                        new TableBlock.TableCell().AddChildren(new TextRunInline { Text = "> a" }),
                        new TableBlock.TableCell().AddChildren(new TextRunInline { Text = "b" })),
                    new TableBlock.TableRow().AddChildren(
                        new TableBlock.TableCell().AddChildren(new TextRunInline { Text = "1" }),
                        new TableBlock.TableCell().AddChildren(new TextRunInline { Text = "2" }))),

                new ParagraphBlock().AddChildren(new TextRunInline { Text = "But this does:" }),
                new QuoteBlock().AddChildren(
                    new TableBlock
                    {
                        ColumnDefinitions = new List<TableBlock.TableColumnDefinition>
                        {
                            new TableBlock.TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                            new TableBlock.TableColumnDefinition { Alignment = ColumnAlignment.Unspecified },
                        }
                    }.AddChildren(
                        new TableBlock.TableRow().AddChildren(
                            new TableBlock.TableCell().AddChildren(new TextRunInline { Text = "a" }),
                            new TableBlock.TableCell().AddChildren(new TextRunInline { Text = "b" })),
                        new TableBlock.TableRow().AddChildren(
                            new TableBlock.TableCell().AddChildren(new TextRunInline { Text = "1" }),
                            new TableBlock.TableCell().AddChildren(new TextRunInline { Text = "2" })))));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_Invalid_Table()
        {
            AssertEqual(CollapseWhitespace(@"
                >a|b"),
                new QuoteBlock().AddChildren(
                    new ParagraphBlock().AddChildren(
                        new TextRunInline { Text = "a|b" })));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_WithCode()
        {
            AssertEqual(CollapseWhitespace(@"
                >     code, line 1.1
                >
                >     code, line 1.3
                
                >     code, line 2.1
                >"),

                new QuoteBlock().AddChildren(
                    new CodeBlock { Text = "code, line 1.1\r\n\r\ncode, line 1.3" }),
                new QuoteBlock().AddChildren(
                    new CodeBlock { Text = "code, line 2.1" }));
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void Quote_WithList()
        {
            AssertEqual(CollapseWhitespace(@"
                > + List item 1
                > + List item 2
                > + List item 3"),
            new QuoteBlock().AddChildren(
                new ListBlock { Style = ListStyle.Bulleted }.AddChildren(
                    new ListItemBlock().AddChildren(new ParagraphBlock().AddChildren(new TextRunInline { Text = "List item 1" })),
                    new ListItemBlock().AddChildren(new ParagraphBlock().AddChildren(new TextRunInline { Text = "List item 2" })),
                    new ListItemBlock().AddChildren(new ParagraphBlock().AddChildren(new TextRunInline { Text = "List item 3" })))));
        }
    }
}