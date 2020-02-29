// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.Toolkit.Parsers.Markdown;
using System.Text;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;
using System;

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
        public void TestBuilderFromDocument()
        {
            var document = MarkdownDocument.CreateBuilder()
                .AddBlockParser<BlockParserA>()
                .After<BlockParserB>()
                .AddBlockParser<BlockParserB>()
                .Build();

            document.GetBuilder().Build();
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

        [TestMethod]
        public void TestAfterInline()
        {
            var document = MarkdownDocument.CreateBuilder()
                .AddInlineParser<InlineParserA>()
                .After<InlineParserB>()
                .AddInlineParser<InlineParserB>()
                .Build();

            document.Parse("TEST");

            AssertEqual(document, new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = nameof(InlineParserB) }));

            document = MarkdownDocument.CreateBuilder()
                .AddInlineParser<InlineParserA>()
                .AddInlineParser<InlineParserB>()
                .After<InlineParserA>()
                .Build();

            document.Parse("TEST");

            AssertEqual(document, new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = nameof(InlineParserA) }));
        }

        [TestMethod]
        public void TestBeforeInline()
        {
            var document = MarkdownDocument.CreateBuilder()
                .AddInlineParser<InlineParserA>()
                .Before<InlineParserB>()
                .AddInlineParser<InlineParserB>()
                .Build();

            document.Parse("TEST");

            AssertEqual(document, new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = nameof(InlineParserA) }));

            document = MarkdownDocument.CreateBuilder()
                .AddInlineParser<InlineParserA>()
                .AddInlineParser<InlineParserB>()
                .Before<InlineParserA>()
                .Build();

            document.Parse("TEST");

            AssertEqual(document, new ParagraphBlock().AddChildren(
                    new TextRunInline { Text = nameof(InlineParserB) }));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void TestCycleInline()
        {
            MarkdownDocument.CreateBuilder()
                 .AddInlineParser<InlineParserA>()
                 .Before<InlineParserB>()
                 .AddInlineParser<InlineParserB>()
                 .Before<InlineParserA>()
                 .Build();
        }

        private class BlockTestParser : MarkdownBlock.Parser<ParagraphBlock>
        {

            protected override BlockParseResult<ParagraphBlock> ParseInternal(LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                var paragraphBlock = new ParagraphBlock
                {
                    Inlines = new List<MarkdownInline>
                    {
                        new TextRunInline() { Text = this.GetType().Name }
                    }
                };
                return BlockParseResult.Create(paragraphBlock, startLine, markdown.LineCount);
            }
        }

        private class BlockParserA : BlockTestParser
        {
        }

        private class BlockParserB : BlockTestParser
        {
        }

        private class InlineParserA : InlineTestParser
        {
        }

        private class InlineParserB : InlineTestParser
        {
        }

        private class InlineTestParser : MarkdownInline.Parser<TextRunInline>
        {
            protected override InlineParseResult<TextRunInline> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                return new InlineParseResult<TextRunInline>(new TextRunInline() { Text = this.GetType().Name }, tripPos, markdown.SliceText(tripPos).TextLength);
            }
        }
    }
}