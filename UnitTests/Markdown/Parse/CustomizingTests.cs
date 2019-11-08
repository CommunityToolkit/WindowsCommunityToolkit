// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.Toolkit.Parsers.Markdown;
using System.Text;
using System.Collections.Generic;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class CustomizingTests : ParseTestBase
    {
        [TestMethod]
        public void TestAfter()
        {
            var document = MarkdownDocument.CreateBuilder()
                .AddBlockParser<BlockParserA>()
                .After<BlockParserB>()
                .AddBlockParser<BlockParserB>()
                .Build();

            document.Parse("TEST");

            AssertEqual(document, new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = nameof(BlockParserB) }));

            document = MarkdownDocument.CreateBuilder()
                .AddBlockParser<BlockParserA>()
                .AddBlockParser<BlockParserB>()
                .After<BlockParserA>()
                .Build();

            document.Parse("TEST");

            AssertEqual(document, new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = nameof(BlockParserA) }));
        }

        [TestMethod]
        public void TestBefore()
        {
            var document = MarkdownDocument.CreateBuilder()
                .AddBlockParser<BlockParserA>()
                .Before<BlockParserB>()
                .AddBlockParser<BlockParserB>()
                .Build();

            document.Parse("TEST");

            AssertEqual(document, new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = nameof(BlockParserA) }));

            document = MarkdownDocument.CreateBuilder()
                .AddBlockParser<BlockParserA>()
                .AddBlockParser<BlockParserB>()
                .Before<BlockParserA>()
                .Build();

            document.Parse("TEST");

            AssertEqual(document, new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = nameof(BlockParserB) }));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestCycle()
        {
           MarkdownDocument.CreateBuilder()
                .AddBlockParser<BlockParserA>()
                .Before<BlockParserB>()
                .AddBlockParser<BlockParserB>()
                .Before<BlockParserA>()
                .Build();
        }

        private class BlockTestParser : MarkdownBlock.Parser<ParagraphBlock>
        {
            protected override ParagraphBlock ParseInternal(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                actualEnd = maxEnd;
                var paragraphBlock = new ParagraphBlock
                {
                    Inlines = new List<MarkdownInline>
                    {
                        new TextRunInline() { Text = this.GetType().Name }
                    }
                };
                return paragraphBlock;
            }
        }

        private class BlockParserA : BlockTestParser
        {
        }

        private class BlockParserB : BlockTestParser
        {
        }
    }
}