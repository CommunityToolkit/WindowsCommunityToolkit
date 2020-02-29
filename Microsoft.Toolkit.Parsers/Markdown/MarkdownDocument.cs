// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown
{
    /// <summary>
    /// Represents a Markdown Document. <para/>
    /// Initialize an instance and call <see cref="Parse(string)"/> to parse the Raw Markdown Text.
    /// </summary>
    public class MarkdownDocument : MarkdownBlock
    {
        /// <summary>
        /// Gets a list of URL schemes.
        /// </summary>
        public static List<string> KnownSchemes { get; private set; } = new List<string>()
        {
            "http",
            "https",
            "ftp",
            "steam",
            "irc",
            "news",
            "mumble",
            "ssh",
            "ms-windows-store",
            "sip"
        };

        private readonly MarkdownBlock.Parser[] parsersBlock;
        private readonly Dictionary<Type, HashSet<Type>> parserDependencysBlock;

        private readonly MarkdownInline.Parser[] parsersInline;
        private readonly Dictionary<Type, HashSet<Type>> parserDependencysInline;

        private Dictionary<string, LinkReferenceBlock> _references;

        // A list of supported HTML entity names, along with their corresponding code points.
        private readonly Dictionary<string, int> _entities = new Dictionary<string, int>
        {
            { "quot", 0x0022 }, // "
            { "amp", 0x0026 }, // &
            { "apos", 0x0027 }, // '
            { "lt", 0x003C }, // <
            { "gt", 0x003E }, // >
            { "nbsp", 0x00A0 }, // <space>
            { "#160", 0x00A0 }, // ?
            { "iexcl", 0x00A1 }, // ¡
            { "cent", 0x00A2 }, // ¢
            { "pound", 0x00A3 }, // £
            { "curren", 0x00A4 }, // ¤
            { "yen", 0x00A5 }, // ¥
            { "brvbar", 0x00A6 }, // ¦
            { "sect", 0x00A7 }, // §
            { "uml", 0x00A8 }, // ¨
            { "copy", 0x00A9 }, // ©
            { "ordf", 0x00AA }, // ª
            { "laquo", 0x00AB }, // «
            { "not", 0x00AC }, // ¬
            { "shy", 0x00AD }, // ?
            { "reg", 0x00AE }, // ®
            { "macr", 0x00AF }, // ¯
            { "deg", 0x00B0 }, // °
            { "plusmn", 0x00B1 }, // ±
            { "sup2", 0x00B2 }, // ²
            { "sup3", 0x00B3 }, // ³
            { "acute", 0x00B4 }, // ´
            { "micro", 0x00B5 }, // µ
            { "para", 0x00B6 }, // ¶
            { "middot", 0x00B7 }, // ·
            { "cedil", 0x00B8 }, // ¸
            { "sup1", 0x00B9 }, // ¹
            { "ordm", 0x00BA }, // º
            { "raquo", 0x00BB }, // »
            { "frac14", 0x00BC }, // ¼
            { "frac12", 0x00BD }, // ½
            { "frac34", 0x00BE }, // ¾
            { "iquest", 0x00BF }, // ¿
            { "Agrave", 0x00C0 }, // À
            { "Aacute", 0x00C1 }, // Á
            { "Acirc", 0x00C2 }, // Â
            { "Atilde", 0x00C3 }, // Ã
            { "Auml", 0x00C4 }, // Ä
            { "Aring", 0x00C5 }, // Å
            { "AElig", 0x00C6 }, // Æ
            { "Ccedil", 0x00C7 }, // Ç
            { "Egrave", 0x00C8 }, // È
            { "Eacute", 0x00C9 }, // É
            { "Ecirc", 0x00CA }, // Ê
            { "Euml", 0x00CB }, // Ë
            { "Igrave", 0x00CC }, // Ì
            { "Iacute", 0x00CD }, // Í
            { "Icirc", 0x00CE }, // Î
            { "Iuml", 0x00CF }, // Ï
            { "ETH", 0x00D0 }, // Ð
            { "Ntilde", 0x00D1 }, // Ñ
            { "Ograve", 0x00D2 }, // Ò
            { "Oacute", 0x00D3 }, // Ó
            { "Ocirc", 0x00D4 }, // Ô
            { "Otilde", 0x00D5 }, // Õ
            { "Ouml", 0x00D6 }, // Ö
            { "times", 0x00D7 }, // ×
            { "Oslash", 0x00D8 }, // Ø
            { "Ugrave", 0x00D9 }, // Ù
            { "Uacute", 0x00DA }, // Ú
            { "Ucirc", 0x00DB }, // Û
            { "Uuml", 0x00DC }, // Ü
            { "Yacute", 0x00DD }, // Ý
            { "THORN", 0x00DE }, // Þ
            { "szlig", 0x00DF }, // ß
            { "agrave", 0x00E0 }, // à
            { "aacute", 0x00E1 }, // á
            { "acirc", 0x00E2 }, // â
            { "atilde", 0x00E3 }, // ã
            { "auml", 0x00E4 }, // ä
            { "aring", 0x00E5 }, // å
            { "aelig", 0x00E6 }, // æ
            { "ccedil", 0x00E7 }, // ç
            { "egrave", 0x00E8 }, // è
            { "eacute", 0x00E9 }, // é
            { "ecirc", 0x00EA }, // ê
            { "euml", 0x00EB }, // ë
            { "igrave", 0x00EC }, // ì
            { "iacute", 0x00ED }, // í
            { "icirc", 0x00EE }, // î
            { "iuml", 0x00EF }, // ï
            { "eth", 0x00F0 }, // ð
            { "ntilde", 0x00F1 }, // ñ
            { "ograve", 0x00F2 }, // ò
            { "oacute", 0x00F3 }, // ó
            { "ocirc", 0x00F4 }, // ô
            { "otilde", 0x00F5 }, // õ
            { "ouml", 0x00F6 }, // ö
            { "divide", 0x00F7 }, // ÷
            { "oslash", 0x00F8 }, // ø
            { "ugrave", 0x00F9 }, // ù
            { "uacute", 0x00FA }, // ú
            { "ucirc", 0x00FB }, // û
            { "uuml", 0x00FC }, // ü
            { "yacute", 0x00FD }, // ý
            { "thorn", 0x00FE }, // þ
            { "yuml", 0x00FF }, // ÿ
            { "OElig", 0x0152 }, // Œ
            { "oelig", 0x0153 }, // œ
            { "Scaron", 0x0160 }, // Š
            { "scaron", 0x0161 }, // š
            { "Yuml", 0x0178 }, // Ÿ
            { "fnof", 0x0192 }, // ƒ
            { "circ", 0x02C6 }, // ˆ
            { "tilde", 0x02DC }, // ˜
            { "Alpha", 0x0391 }, // Α
            { "Beta", 0x0392 }, // Β
            { "Gamma", 0x0393 }, // Γ
            { "Delta", 0x0394 }, // Δ
            { "Epsilon", 0x0395 }, // Ε
            { "Zeta", 0x0396 }, // Ζ
            { "Eta", 0x0397 }, // Η
            { "Theta", 0x0398 }, // Θ
            { "Iota", 0x0399 }, // Ι
            { "Kappa", 0x039A }, // Κ
            { "Lambda", 0x039B }, // Λ
            { "Mu", 0x039C }, // Μ
            { "Nu", 0x039D }, // Ν
            { "Xi", 0x039E }, // Ξ
            { "Omicron", 0x039F }, // Ο
            { "Pi", 0x03A0 }, // Π
            { "Rho", 0x03A1 }, // Ρ
            { "Sigma", 0x03A3 }, // Σ
            { "Tau", 0x03A4 }, // Τ
            { "Upsilon", 0x03A5 }, // Υ
            { "Phi", 0x03A6 }, // Φ
            { "Chi", 0x03A7 }, // Χ
            { "Psi", 0x03A8 }, // Ψ
            { "Omega", 0x03A9 }, // Ω
            { "alpha", 0x03B1 }, // α
            { "beta", 0x03B2 }, // β
            { "gamma", 0x03B3 }, // γ
            { "delta", 0x03B4 }, // δ
            { "epsilon", 0x03B5 }, // ε
            { "zeta", 0x03B6 }, // ζ
            { "eta", 0x03B7 }, // η
            { "theta", 0x03B8 }, // θ
            { "iota", 0x03B9 }, // ι
            { "kappa", 0x03BA }, // κ
            { "lambda", 0x03BB }, // λ
            { "mu", 0x03BC }, // μ
            { "nu", 0x03BD }, // ν
            { "xi", 0x03BE }, // ξ
            { "omicron", 0x03BF }, // ο
            { "pi", 0x03C0 }, // π
            { "rho", 0x03C1 }, // ρ
            { "sigmaf", 0x03C2 }, // ς
            { "sigma", 0x03C3 }, // σ
            { "tau", 0x03C4 }, // τ
            { "upsilon", 0x03C5 }, // υ
            { "phi", 0x03C6 }, // φ
            { "chi", 0x03C7 }, // χ
            { "psi", 0x03C8 }, // ψ
            { "omega", 0x03C9 }, // ω
            { "thetasym", 0x03D1 }, // ϑ
            { "upsih", 0x03D2 }, // ϒ
            { "piv", 0x03D6 }, // ϖ
            { "ensp", 0x2002 }, //  ?
            { "emsp", 0x2003 }, //  ?
            { "thinsp", 0x2009 }, //  ?
            { "zwnj", 0x200C }, //  ?
            { "zwj", 0x200D }, //  ?
            { "lrm", 0x200E }, //  ?
            { "rlm", 0x200F }, //  ?
            { "ndash", 0x2013 }, // –
            { "mdash", 0x2014 }, // —
            { "lsquo", 0x2018 }, // ‘
            { "rsquo", 0x2019 }, // ’
            { "sbquo", 0x201A }, // ‚
            { "ldquo", 0x201C }, // “
            { "rdquo", 0x201D }, // ”
            { "bdquo", 0x201E }, // „
            { "dagger", 0x2020 }, // †
            { "Dagger", 0x2021 }, // ‡
            { "bull", 0x2022 }, // •
            { "hellip", 0x2026 }, // …
            { "permil", 0x2030 }, // ‰
            { "prime", 0x2032 }, // ′
            { "Prime", 0x2033 }, // ″
            { "lsaquo", 0x2039 }, // ‹
            { "rsaquo", 0x203A }, // ›
            { "oline", 0x203E }, // ‾
            { "frasl", 0x2044 }, // ⁄
            { "euro", 0x20AC }, // €
            { "image", 0x2111 }, // ℑ
            { "weierp", 0x2118 }, // ℘
            { "real", 0x211C }, // ℜ
            { "trade", 0x2122 }, // ™
            { "alefsym", 0x2135 }, // ℵ
            { "larr", 0x2190 }, // ←
            { "uarr", 0x2191 }, // ↑
            { "rarr", 0x2192 }, // →
            { "darr", 0x2193 }, // ↓
            { "harr", 0x2194 }, // ↔
            { "crarr", 0x21B5 }, // ↵
            { "lArr", 0x21D0 }, // ⇐
            { "uArr", 0x21D1 }, // ⇑
            { "rArr", 0x21D2 }, // ⇒
            { "dArr", 0x21D3 }, // ⇓
            { "hArr", 0x21D4 }, // ⇔
            { "forall", 0x2200 }, // ∀
            { "part", 0x2202 }, // ∂
            { "exist", 0x2203 }, // ∃
            { "empty", 0x2205 }, // ∅
            { "nabla", 0x2207 }, // ∇
            { "isin", 0x2208 }, // ∈
            { "notin", 0x2209 }, // ∉
            { "ni", 0x220B }, // ∋
            { "prod", 0x220F }, // ∏
            { "sum", 0x2211 }, // ∑
            { "minus", 0x2212 }, // −
            { "lowast", 0x2217 }, // ∗
            { "radic", 0x221A }, // √
            { "prop", 0x221D }, // ∝
            { "infin", 0x221E }, // ∞
            { "ang", 0x2220 }, // ∠
            { "and", 0x2227 }, // ∧
            { "or", 0x2228 }, // ∨
            { "cap", 0x2229 }, // ∩
            { "cup", 0x222A }, // ∪
            { "int", 0x222B }, // ∫
            { "there4", 0x2234 }, // ∴
            { "sim", 0x223C }, // ∼
            { "cong", 0x2245 }, // ≅
            { "asymp", 0x2248 }, // ≈
            { "ne", 0x2260 }, // ≠
            { "equiv", 0x2261 }, // ≡
            { "le", 0x2264 }, // ≤
            { "ge", 0x2265 }, // ≥
            { "sub", 0x2282 }, // ⊂
            { "sup", 0x2283 }, // ⊃
            { "nsub", 0x2284 }, // ⊄
            { "sube", 0x2286 }, // ⊆
            { "supe", 0x2287 }, // ⊇
            { "oplus", 0x2295 }, // ⊕
            { "otimes", 0x2297 }, // ⊗
            { "perp", 0x22A5 }, // ⊥
            { "sdot", 0x22C5 }, // ⋅
            { "lceil", 0x2308 }, // ⌈
            { "rceil", 0x2309 }, // ⌉
            { "lfloor", 0x230A }, // ⌊
            { "rfloor", 0x230B }, // ⌋
            { "lang", 0x2329 }, // 〈
            { "rang", 0x232A }, // 〉
            { "loz", 0x25CA }, // ◊
            { "spades", 0x2660 }, // ♠
            { "clubs", 0x2663 }, // ♣
            { "hearts", 0x2665 }, // ♥
            { "diams", 0x2666 }, // ♦
        };

        // A list of characters that can be escaped.
        private readonly char[] _escapeCharacters = new char[] { '\\', '`', '*', '_', '{', '}', '[', ']', '(', ')', '#', '+', '-', '.', '!', '|', '~', '^', '&', ':', '<', '>', '/' };


        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownDocument"/> class.
        /// </summary>
        public MarkdownDocument()
            : this(
                  blockParsers: TopologicalSort(new Parser[]
                  {
                      new YamlHeaderBlock.Parser(),
                      new TableBlock.Parser(),
                      new QuoteBlock.Parser(),
                      new ListBlock.Parser(),
                      new LinkReferenceBlock.Parser(),
                      new HorizontalRuleBlock.Parser(),
                      new HeaderBlock.HashParser(),
                      new HeaderBlock.UnderlineParser(),
                      new CodeBlock.ParserTicked(),
                      new CodeBlock.ParserIndented(),
                  }),
                  blockEdges: null,
                  inlineParsers: TopologicalSort(new MarkdownInline.Parser[]
                  {
                      new BoldItalicTextInline.ParserUnderscore(),
                      new BoldItalicTextInline.ParserAsterix(),
                      new BoldTextInline.ParserUnderscore(),
                      new BoldTextInline.ParserAsterix(),
                      new CodeInline.Parser(),
                      new CommentInline.Parser(),
                      new EmojiInline.Parser(),
                      new HyperlinkInline.AngleBracketLinkParser(),
                      new HyperlinkInline.EmailAddressParser(),
                      new HyperlinkInline.PartialLinkParser(),
                      new HyperlinkInline.ReditLinkParser(),
                      new HyperlinkInline.UrlParser(),
                      new ImageInline.Parser(),
                      new ItalicTextInline.ParserUnderscore(),
                      new ItalicTextInline.ParserAsterix(),
                      new LinkAnchorInline.Parser(),
                      new MarkdownLinkInline.Parser(),
                      new StrikethroughTextInline.Parser(),
                      new SubscriptTextInline.Parser(),
                      new SuperscriptTextInline.ParserTags(),
                      new SuperscriptTextInline.ParserTop(),
                  }),
                  inlineEdges: null)
        {
        }

        internal MarkdownDocument(IEnumerable<MarkdownBlock.Parser> blockParsers, Dictionary<Type, HashSet<Type>> blockEdges, IEnumerable<MarkdownInline.Parser> inlineParsers, Dictionary<Type, HashSet<Type>> inlineEdges)
            : base(MarkdownBlockType.Root)
        {
            this.parsersBlock = blockParsers.ToArray();
            this.parserDependencysBlock = blockEdges ?? new Dictionary<Type, HashSet<Type>>();

            this.parsersInline = inlineParsers.ToArray();
            this.parserDependencysInline = inlineEdges ?? new Dictionary<Type, HashSet<Type>>();
        }

        /// <summary>
        /// Creates a new Builder.
        /// </summary>
        /// <returns>A Builder.</returns>
        public static IDocumentBuilder CreateBuilder()
        {
            return new DocumentBuilder();
        }

        /// <summary>
        /// Gets the BlockParsers this Document will use.
        /// </summary>
        public ReadOnlySpan<MarkdownBlock.Parser> BlockParsers => new ReadOnlySpan<Parser>(this.parsersBlock);

        /// <summary>
        /// Gets the InlineParsers this Document will use.
        /// </summary>
        public ReadOnlySpan<MarkdownInline.Parser> InlineParsers => new ReadOnlySpan<MarkdownInline.Parser>(this.parsersInline);

        /// <summary>
        /// Gets or sets the list of block elements.
        /// </summary>
        public IList<MarkdownBlock> Blocks { get; set; }

        /// <summary>
        /// Returns a builder with the same configuraiton as the one that created this Document.
        /// </summary>
        /// <returns>A Builder.</returns>
        public DocumentBuilder GetBuilder() => new DocumentBuilder(this.parsersBlock, this.parserDependencysBlock, this.parsersInline, this.parserDependencysInline);

        /// <summary>
        /// Parses markdown document text.
        /// </summary>
        /// <param name="markdownText"> The markdown text. </param>
        public void Parse(string markdownText)
        {
            Blocks = ParseBlocks(new LineBlock(markdownText.AsSpan()));

            // Remove any references from the list of blocks, and add them to a dictionary.
            for (int i = Blocks.Count - 1; i >= 0; i--)
            {
                if (Blocks[i].Type == MarkdownBlockType.LinkReference)
                {
                    var reference = (LinkReferenceBlock)Blocks[i];
                    if (_references == null)
                    {
                        _references = new Dictionary<string, LinkReferenceBlock>(StringComparer.OrdinalIgnoreCase);
                    }

                    if (!_references.ContainsKey(reference.Id))
                    {
                        _references.Add(reference.Id, reference);
                    }

                    Blocks.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Parses text to Bloks.
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The position to start parsing. </param>
        /// <param name="end"> The position to stop parsing. </param>
        /// <param name="actualEnd"> Set to the position at which parsing ended.  This can be
        /// different from <paramref name="end"/> when the parser is being called recursively.
        /// </param>
        /// <returns> A list of parsed blocks. </returns>
        public List<MarkdownBlock> ParseBlocks(LineBlock markdown)
        {
            // We need to parse out the list of blocks.
            // Some blocks need to start on a new paragraph (code, lists and tables) while other
            // blocks can start on any line (headers, horizontal rules and quotes).
            // Text that is outside of any other block becomes a paragraph.
            var blocks = new List<MarkdownBlock>();

            // Is this the beginning of a paragraph
            bool lineStartsNewParagraph = true;

            int currentLineIndex = 0;

            // text already "parsed" but not yet part of a block will be transformed to ParagraphBlock
            void AddParagraph(ref LineBlock markdown, int lineCount)
            {
                // End the current paragraph.
                if (lineCount > 0)
                {
                    var block = ParagraphBlock.Parse(markdown.SliceLines(0, lineCount), this);
                    if (block != null)
                    {
                        blocks.Add(block);
                    }
                }

                // We need to mark all text as parsed.
                markdown = markdown.SliceLines(lineCount);
                currentLineIndex = 0;
            }

            // Go line by line.
            while (currentLineIndex < markdown.LineCount)
            {
                var currentLine = markdown[currentLineIndex];

                // if line is empty we have a new Paragraph
                if (currentLine.IsWhiteSpace())
                {
                    // The line is empty or nothing but whitespace.
                    lineStartsNewParagraph = true;
                    AddParagraph(ref markdown, currentLineIndex);

                    // remove the empty line in the bgeinning
                    markdown = markdown.SliceLines(1);
                }
                else
                {
                    BlockParseResult parsedBlock = null;

                    foreach (var parser in this.parsersBlock)
                    {
                        parsedBlock = parser.Parse(markdown, currentLineIndex, lineStartsNewParagraph, this);
                        if (parsedBlock != null)
                        {
                            // add the last paragaraph
                            AddParagraph(ref markdown, parsedBlock.Start);

                            blocks.Add(parsedBlock.ParsedElement);

                            // remove the parsed lines
                            markdown = markdown.SliceLines(parsedBlock.LineCount);
                            break;
                        }
                    }

                    // Block elements start new paragraphs.
                    lineStartsNewParagraph = parsedBlock != null;

                    // Repeat.
                    if (parsedBlock is null)
                    {
                        currentLineIndex++;
                    }
                }
            }

            // Add the last paragraph if we are at the end of the input text.
            AddParagraph(ref markdown, markdown.LineCount);
            return blocks;
        }

        public List<MarkdownInline> ParseInlineChildren(ReadOnlySpan<char> markdown, IEnumerable<Type> ignoredParsers = null) => this.ParseInlineChildren(new LineBlock(markdown), ignoredParsers);

        /// <summary>
        /// This function can be called by any element parsing. Given a start and stopping point this will
        /// parse all found elements out of the range.
        /// </summary>
        /// <returns> A list of parsed inlines. </returns>
        public List<MarkdownInline> ParseInlineChildren(LineBlock markdown, IEnumerable<Type> ignoredParsers = null)
        {
            ignoredParsers ??= Array.Empty<Type>();
            LineBlockPosition currentParsePosition = default;

            var inlines = new List<MarkdownInline>();
            while (currentParsePosition.IsIn(markdown))
            {
                // Find the next inline element.
                var parseResult = FindNextInlineElement(markdown, ignoredParsers);

                // there were no more inlines.
                if (parseResult is null)
                {
                    // If we didn't find any elements we have a normal text block.
                    // Let us consume the entire range.
                    var textRun = TextRunInline.Parse(markdown, inlines.Count == 0, true, this);

                    // The textblock may contain only linebreaks.
                    if (textRun != null)
                    {
                        parseResult = InlineParseResult.Create(textRun, currentParsePosition, markdown.TextLength);
                    }
                }

                // there where no inline to parse
                if (parseResult is null)
                {
                    break;
                }

                // If the element we found doesn't start at the position we are looking for there
                // is text between the element and the start of the parsed element. We need to wrap
                // it into a text run.
                if (parseResult.Start != currentParsePosition)
                {
                    var textRun = TextRunInline.Parse(markdown.SliceText(0, parseResult.Start.FromStart), inlines.Count == 0, false, this);

                    if (textRun != null)
                    {
                        inlines.Add(textRun);
                    }
                }

                // Add the parsed element.
                inlines.Add(parseResult.ParsedElement);

                // Update the current position.
                currentParsePosition = parseResult.Start.Add(parseResult.Length, markdown);
                markdown = markdown.SliceText(currentParsePosition);
                currentParsePosition = default;
            }

            return inlines;
        }

        /// <summary>
        /// Finds the next inline element by matching trip chars and verifying the match.
        /// </summary>
        /// <param name="markdown"> The markdown text to parse. </param>
        /// <param name="start"> The position to start parsing. </param>
        /// <param name="end"> The position to stop parsing. </param>
        /// <param name="ignoredParsers">Supress specific parsers. (e.g don't parse link in link).</param>
        /// <returns>Returns the next element.</returns>
        private InlineParseResult FindNextInlineElement(LineBlock markdown, IEnumerable<Type> ignoredParsers)
        {
            var parsers = this.parsersInline;
            if (ignoredParsers.Any())
            {
                parsers = parsers.Where(x => !ignoredParsers.Contains(x.GetType())).ToArray();
            }

            var canUseTripChar = parsers.All(x => x.TripChar.Any());

            var foundInline = canUseTripChar
                ? this.FindNextInlineWithTripChar(markdown, ignoredParsers, parsers)
                : this.FindNextInlineSlow(markdown, ignoredParsers, parsers);

            return foundInline;
        }

        private InlineParseResult FindNextInlineSlow(in LineBlock markdown, IEnumerable<Type> ignoredParsers, MarkdownInline.Parser[] parsers)
        {
            // Search for the next inline sequence.
            for (int lineIndex = 0; lineIndex < markdown.LineCount; lineIndex++)
            {
                for (int coulemIndex = 0; coulemIndex < markdown[lineIndex].Length; coulemIndex++)
                {
                    // Don't match if the previous character was a backslash.
                    if (coulemIndex > 0 && markdown[lineIndex, coulemIndex - 1] == '\\')
                    {
                        continue;
                    }

                    // Find the trigger(s) that matched.
                    char currentChar = markdown[lineIndex, coulemIndex];
                    foreach (var parser in parsers.Where(x => !x.TripChar.Any() || x.TripChar.Contains(currentChar)))
                    {
                        // If we are here we have a possible match. Call into the inline class to verify.
                        var parseResult = parser.Parse(markdown, new LineBlockPosition(lineIndex, coulemIndex, markdown), this, ignoredParsers);
                        if (parseResult != null)
                        {
                            return parseResult;
                        }
                    }

                }
            }

            return null;
        }

        private InlineParseResult FindNextInlineWithTripChar(in LineBlock markdown, IEnumerable<Type> ignoredParsers, MarkdownInline.Parser[] parsers)
        {
            var tripCharacters = parsers.SelectMany(x => x.TripChar).Distinct().ToArray();

            LineBlockPosition index = default;

            // Search for the next inline sequence.
            while (index.IsIn(markdown))
            {
                // IndexOfAny should be the fastest way to skip characters we don't care about.
                var pos = markdown.IndexOfAny(tripCharacters, index);
                if (pos == LineBlockPosition.NotFound)
                {
                    break;
                }

                // Don't match if the previous character was a backslash.
                if (pos.Column > 0 && markdown[pos.Line][pos.Column - 1] == '\\')
                {
                    index = pos.Add(1, markdown);
                    continue;
                }

                // Find the trigger(s) that matched.
                char currentChar = markdown[pos];
                foreach (var parser in parsers.Where(x => x.TripChar.Contains(currentChar)))
                {
                    // If we are here we have a possible match. Call into the inline class to verify.
                    var parseResult = parser.Parse(markdown, pos, this, ignoredParsers);
                    if (parseResult != null)
                    {
                        return parseResult;
                    }
                }

                index = index.Add(1, markdown);
            }

            return null;
        }

        /// <summary>
        /// Looks up a reference using the ID.
        /// A reference is a line that looks like this:
        /// [foo]: http://example.com/.
        /// </summary>
        /// <param name="id"> The ID of the reference (case insensitive). </param>
        /// <returns> The reference details, or <c>null</c> if the reference wasn't found. </returns>
        public LinkReferenceBlock LookUpReference(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (_references == null)
            {
                return null;
            }

            if (_references.TryGetValue(id, out LinkReferenceBlock result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Parses unformatted text.
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <returns> A parsed text span. </returns>
        public string ResolveEscapeSequences(ReadOnlySpan<char> markdown)
        {
            return this.ResolveEscapeSequences(new LineBlock(markdown));
        }

        public string ResolveEscapeSequences(LineBlock markdown)
        {
            var bufferSize = markdown.TextLength + (markdown.LineCount - 1) * System.Environment.NewLine.Length;
            char[] arrayBuffer;
            if (bufferSize <= SpanExtensions.MAX_STACK_BUFFER_SIZE)
            {
                arrayBuffer = null;
            }
            else
            {
                arrayBuffer = ArrayPool<char>.Shared.Rent(bufferSize);
            }

            Span<char> buffer = arrayBuffer != null
                ? arrayBuffer.AsSpan(0, bufferSize)
                : stackalloc char[bufferSize];

            var index = 0;
            for (int line = 0; line < markdown.LineCount; line++)
            {
                var from = markdown[line];

                for (int c = 0; c < from.Length; c++)
                {
                    var indexToHeandle = from.Slice(c).IndexOfAny('\\', '&') + c;

                    if (indexToHeandle < c)
                    {
                        var toCopy = from.Slice(c);
                        toCopy.CopyTo(buffer.Slice(index));
                        index += toCopy.Length;
                        break;
                    }

                    if (indexToHeandle > c)
                    {
                        var toCopy = from.Slice(c, indexToHeandle - c);
                        toCopy.CopyTo(buffer.Slice(index));
                        index += toCopy.Length;
                    }

                    if (from[indexToHeandle] == '\\')
                    {
                        // markdown is an escape sequence, with one more character expected.
                        if (indexToHeandle >= from.Length - 1)
                        {
                            var toCopy = from.Slice(c);
                            toCopy.CopyTo(buffer.Slice(index));
                            index += toCopy.Length;
                            break;
                        }

                        // Check if the character after the backslash can be escaped.
                        var decodedChar = from[indexToHeandle + 1];
                        if (Array.IndexOf(_escapeCharacters, decodedChar) < 0)
                        {
                            // markdown character cannot be escaped.
                            c = indexToHeandle;
                            buffer[index] = from[indexToHeandle];
                            index++;

                            continue;
                        }

                        buffer[index] = decodedChar;
                        index++;

                        c = indexToHeandle + 1;
                    }
                    else if (from[indexToHeandle] == '&')
                    {
                        // markdown is an entity e.g. "&nbsp;".

                        // Look for the semicolon.
                        var semicolongPos = from.Slice(indexToHeandle).IndexOf(';');

                        // Unterminated entity.
                        if (semicolongPos == -1)
                        {
                            c = indexToHeandle;
                            buffer[index] = from[indexToHeandle];
                            index++;
                            continue;
                        }

                        // Okay, we have an entity, but is it one we recognise?
                        string entityName = from.Slice(indexToHeandle + 1, semicolongPos - 1).ToString();

                        // Unrecognised entity.
                        if (!_entities.TryGetValue(entityName, out var decodedChar))
                        {
                            c = indexToHeandle;
                            buffer[index] = from[indexToHeandle];
                            index++;
                            continue;
                        }

                        buffer[index] = (char)decodedChar;
                        index++;

                        c = semicolongPos;
                    }
                    else
                    {
                        // it will increased by one in the for increase step.
                        c = indexToHeandle;
                    }

                }

                if (line < markdown.LineCount - 1)
                {
                    int removedSpaces = 0;
                    while (index >= 1 && buffer[index - 1] == ' ')
                    {
                        removedSpaces++;
                        index--;
                    }

                    if (removedSpaces >= 2)
                    {
                        Environment.NewLine.AsSpan().CopyTo(buffer.Slice(index));
                        index += Environment.NewLine.Length;
                    }
                    else
                    {
                        buffer[index] = ' ';
                        index++;
                    }

                }
            }

            var result = buffer.Slice(0, index).ToString();

            if (arrayBuffer != null)
            {
                ArrayPool<char>.Shared.Return(arrayBuffer, false);
            }

            return result;
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (Blocks == null)
            {
                return base.ToString();
            }

            return string.Join("\r\n", Blocks);
        }

        private static IEnumerable<MarkdownBlock.Parser> TopologicalSort(IEnumerable<MarkdownBlock.Parser> parsers)
        {
            var edges = new Dictionary<Type, HashSet<Type>>();

            void AddRelation(Type before, Type after)
            {
                if (edges.ContainsKey(before))
                {
                    edges[before].Add(after);
                }
                else
                {
                    edges.Add(before, new HashSet<Type> { after });
                }
            }

            foreach (var parser in parsers)
            {
                foreach (var item in parser.DefaultAfterParsers)
                {
                    AddRelation(parser.GetType(), item);
                }

                foreach (var item in parser.DefaultBeforeParsers)
                {
                    AddRelation(item, parser.GetType());
                }
            }

            return TopologicalSort(parsers, edges);
        }

        private static IEnumerable<MarkdownInline.Parser> TopologicalSort(IEnumerable<MarkdownInline.Parser> parsers)
        {
            var edges = new Dictionary<Type, HashSet<Type>>();

            void AddRelation(Type before, Type after)
            {
                if (edges.ContainsKey(before))
                {
                    edges[before].Add(after);
                }
                else
                {
                    edges.Add(before, new HashSet<Type> { after });
                }
            }

            foreach (var parser in parsers)
            {
                foreach (var item in parser.DefaultAfterParsers)
                {
                    AddRelation(item, parser.GetType());
                }

                foreach (var item in parser.DefaultBeforeParsers)
                {
                    AddRelation(parser.GetType(), item);
                }
            }

            return TopologicalSort(parsers, edges);
        }

        /// <summary>
        /// Performs a topolological sort on a graph.
        /// </summary>
        /// <param name="nodes">The Factores.</param>
        /// <param name="edges">A dictionary that maps a Parser to all of its incomming (must run before this) Parsers.</param>
        /// <typeparam name="T">The type to sort.</typeparam>
        /// <returns>The ordered list.</returns>
        private static IEnumerable<T> TopologicalSort<T>(IEnumerable<T> nodes, Dictionary<Type, HashSet<Type>> edges)
        {
            // we want to order all elements to get a deterministic result
            var orderedSource = nodes.OrderBy(x => x.GetType().FullName).ToArray();

            // We will manipulate edges. So we make a copy
            edges = edges.ToDictionary(x => x.Key, x => new HashSet<Type>(x.Value));

            // Kahn's algorithm [from Wikipedia](https://en.wikipedia.org/w/index.php?title=Topological_sorting&oldid=917759838)
            // L ← Empty list that will contain the sorted elements
            // S ← Set of all nodes with no incoming edge
            // while S is non-empty do
            //     remove a node n from S
            //     add n to tail of L
            //     for each node m with an edge e from n to m do
            //         remove edge e from the graph
            //         if m has no other incoming edges then
            //             insert m into S
            // if graph has edges then
            //     return error   (graph has at least one cycle)
            // else
            //     return L   (a topologically sorted order)
            var l = new List<T>();
            var s = new Queue<T>(orderedSource.Where(x => !edges.ContainsKey(x.GetType())));

            void RemoveRelation(Type before, Type after)
            {
                if (edges.ContainsKey(before))
                {
                    edges[before].Remove(after);
                    if (edges[before].Count == 0)
                    {
                        edges.Remove(before);
                    }
                }
            }

            while (s.Count > 0)
            {
                var n = s.Dequeue();
                l.Add(n);
                foreach (var m in orderedSource)
                {
                    if (edges.ContainsKey(m.GetType()) && edges[m.GetType()].Contains(n.GetType()))
                    {
                        RemoveRelation(m.GetType(), n.GetType());
                        if (!edges.ContainsKey(m.GetType()))
                        {
                            s.Enqueue(m);
                        }
                    }
                }
            }

            if (edges.Count > 0)
            {
                throw new InvalidOperationException("Graph contains cycles");
            }

            return l;
        }

        /// <summary>
        /// Document builder allows to configure a MarkdownDocument with different blocks and inline that can be parsed.
        /// </summary>
        public class DocumentBuilder : IDocumentBuilder
        {
            private readonly Dictionary<Type, MarkdownBlock.Parser> parserInstancesBlock = new Dictionary<Type, MarkdownBlock.Parser>();
            private readonly Dictionary<Type, HashSet<Type>> aAfterBRelationBlock = new Dictionary<Type, HashSet<Type>>();

            private readonly Dictionary<Type, MarkdownInline.Parser> parserInstancesInline = new Dictionary<Type, MarkdownInline.Parser>();
            private readonly Dictionary<Type, HashSet<Type>> aAfterBRelationInline = new Dictionary<Type, HashSet<Type>>();

            internal DocumentBuilder()
            {
            }

            internal DocumentBuilder(MarkdownBlock.Parser[] parsersBlock, Dictionary<Type, HashSet<Type>> parserDependencysBlock, MarkdownInline.Parser[] parsersInline, Dictionary<Type, HashSet<Type>> parserDependencysInline)
            {
                foreach (var parser in parsersBlock)
                {
                    parserInstancesBlock.Add(parser.GetType(), parser);
                }

                foreach (var dependecy in parserDependencysBlock)
                {
                    aAfterBRelationBlock.Add(dependecy.Key, dependecy.Value);
                }

                foreach (var parser in parsersInline)
                {
                    parserInstancesInline.Add(parser.GetType(), parser);
                }

                foreach (var dependecy in parserDependencysInline)
                {
                    aAfterBRelationInline.Add(dependecy.Key, dependecy.Value);
                }
            }

            /// <inheritdoc/>
            public DocumentBuilder RemoveBlockParser<TParser>()
                where TParser : MarkdownBlock.Parser, new()
            {
                this.parserInstancesBlock.Remove(typeof(TParser));
                return this;
            }

            /// <inheritdoc/>
            public DocumentBuilder RemoveInlineParser<TParser>()
                where TParser : MarkdownInline.Parser, new()
            {
                this.parserInstancesInline.Remove(typeof(TParser));
                return this;
            }

            /// <inheritdoc/>
            public DocumentBuilderBlockConfigurator<TParser> AddBlockParser<TParser>(Action<TParser> configurationCallback = null)
                where TParser : MarkdownBlock.Parser, new()
            {
                var data = new TParser();
                configurationCallback?.Invoke(data);
                this.parserInstancesBlock.Add(typeof(TParser), data);

                foreach (var item in data.DefaultAfterParsers)
                {
                    this.AddRelationBlock(item, data.GetType());
                }

                foreach (var item in data.DefaultBeforeParsers)
                {
                    this.AddRelationBlock(data.GetType(), item);
                }

                return new DocumentBuilderBlockConfigurator<TParser>(this);
            }

            /// <inheritdoc/>
            public DocumentBuilderInlineConfigurator<TParser> AddInlineParser<TParser>(Action<TParser> configurationCallback = null)
                where TParser : MarkdownInline.Parser, new()
            {
                var data = new TParser();
                configurationCallback?.Invoke(data);
                this.parserInstancesInline.Add(typeof(TParser), data);

                foreach (var item in data.DefaultAfterParsers)
                {
                    this.AddRelationInline(item, data.GetType());
                }

                foreach (var item in data.DefaultBeforeParsers)
                {
                    this.AddRelationInline(data.GetType(), item);
                }

                return new DocumentBuilderInlineConfigurator<TParser>(this);
            }

            /// <inheritdoc/>
            public MarkdownDocument Build()
            {
                // we need to get rid of all edges that are related to removed nodes
                foreach (var item in this.aAfterBRelationBlock.Keys.Where(x => !this.parserInstancesBlock.ContainsKey(x)).ToArray())
                {
                    this.aAfterBRelationBlock.Remove(item);
                }

                foreach (var keyValuePair in this.aAfterBRelationBlock)
                {
                    foreach (var item in keyValuePair.Value.Where(x => !this.parserInstancesBlock.ContainsKey(x)).ToArray())
                    {
                        keyValuePair.Value.Remove(item);
                    }
                }

                foreach (var item in this.aAfterBRelationInline.Keys.Where(x => !this.parserInstancesInline.ContainsKey(x)).ToArray())
                {
                    this.aAfterBRelationInline.Remove(item);
                }

                foreach (var keyValuePair in this.aAfterBRelationInline)
                {
                    foreach (var item in keyValuePair.Value.Where(x => !this.parserInstancesInline.ContainsKey(x)).ToArray())
                    {
                        keyValuePair.Value.Remove(item);
                    }
                }

                var valuesBlocks = this.parserInstancesBlock.Values;
                var edgesBlocks = this.aAfterBRelationBlock;
                var sortedBlocks = TopologicalSort(valuesBlocks, edgesBlocks);

                var valuesInlines = this.parserInstancesInline.Values;
                var edgesInlines = this.aAfterBRelationInline;
                var sortedInlines = TopologicalSort(valuesInlines, edgesInlines);

                return new MarkdownDocument(sortedBlocks, edgesBlocks, sortedInlines, edgesInlines);
            }

            private void AddRelationBlock(Type before, Type after)
            {
                if (this.aAfterBRelationBlock.ContainsKey(before))
                {
                    this.aAfterBRelationBlock[before].Add(after);
                }
                else
                {
                    this.aAfterBRelationBlock.Add(before, new HashSet<Type> { after });
                }
            }

            private void AddRelationInline(Type before, Type after)
            {
                if (this.aAfterBRelationInline.ContainsKey(before))
                {
                    this.aAfterBRelationInline[before].Add(after);
                }
                else
                {
                    this.aAfterBRelationInline.Add(before, new HashSet<Type> { after });
                }
            }

            /// <summary>
            /// Allows to order a Parsers relative to other Parsers.
            /// </summary>
            /// <typeparam name="TParser">The type of the Parser.</typeparam>
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public class DocumentBuilderBlockConfigurator<TParser> : IDocumentBuilder
                    where TParser : MarkdownBlock.Parser, new()
            {
                private readonly DocumentBuilder parent;

                internal DocumentBuilderBlockConfigurator(DocumentBuilder documentBuilder)
                {
                    this.parent = documentBuilder;
                }

                /// <inheritdoc/>
                public DocumentBuilderBlockConfigurator<TParser1> AddBlockParser<TParser1>(Action<TParser1> configurationCallback = null)
                    where TParser1 : MarkdownBlock.Parser, new() => this.parent.AddBlockParser(configurationCallback);

                /// <inheritdoc/>
                public DocumentBuilderInlineConfigurator<TParser1> AddInlineParser<TParser1>(Action<TParser1> configurationCallback = null)
                    where TParser1 : MarkdownInline.Parser, new() => this.parent.AddInlineParser(configurationCallback);

                /// <summary>
                /// Defines that the last added Parser will run after <typeparamref name="TParser2"/>.
                /// </summary>
                /// <typeparam name="TParser2">The Parser that will guarantee to parse before this one.</typeparam>
                /// <returns>This Instance.</returns>
                public DocumentBuilderBlockConfigurator<TParser> After<TParser2>()
                    where TParser2 : MarkdownBlock.Parser, new()
                {
                    this.parent.AddRelationBlock(typeof(TParser), typeof(TParser2));
                    return this;
                }

                /// <summary>
                /// Defines that the last added Parser will run before <typeparamref name="TParser2"/>.
                /// </summary>
                /// <typeparam name="TParser2">The Parser that will guarantee to parse after this one.</typeparam>
                /// <returns>This Instance.</returns>
                public DocumentBuilderBlockConfigurator<TParser> Before<TParser2>()
                    where TParser2 : MarkdownBlock.Parser, new()
                {
                    this.parent.AddRelationBlock(typeof(TParser2), typeof(TParser));
                    return this;
                }

                /// <inheritdoc/>
                public MarkdownDocument Build() => this.parent.Build();

                /// <inheritdoc/>
                public DocumentBuilder RemoveBlockParser<TParser1>()
                    where TParser1 : MarkdownBlock.Parser, new() => this.parent.RemoveBlockParser<TParser1>();

                /// <inheritdoc/>
                public DocumentBuilder RemoveInlineParser<TParser1>()
                    where TParser1 : MarkdownInline.Parser, new() => this.parent.RemoveInlineParser<TParser1>();
            }

            /// <summary>
            /// Allows to order a Parsers relative to other Parsers.
            /// </summary>
            /// <typeparam name="TParser">The type of the Parser.</typeparam>
            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public class DocumentBuilderInlineConfigurator<TParser> : IDocumentBuilder
                    where TParser : MarkdownInline.Parser, new()
            {
                private readonly DocumentBuilder parent;

                internal DocumentBuilderInlineConfigurator(DocumentBuilder documentBuilder)
                {
                    this.parent = documentBuilder;
                }

                /// <inheritdoc/>
                public DocumentBuilderBlockConfigurator<TParser1> AddBlockParser<TParser1>(Action<TParser1> configurationCallback = null)
                    where TParser1 : MarkdownBlock.Parser, new() => this.parent.AddBlockParser(configurationCallback);

                /// <inheritdoc/>
                public DocumentBuilderInlineConfigurator<TParser1> AddInlineParser<TParser1>(Action<TParser1> configurationCallback = null)
                    where TParser1 : MarkdownInline.Parser, new() => this.parent.AddInlineParser(configurationCallback);

                /// <summary>
                /// Defines that the last added Parser will run after <typeparamref name="TParser2"/>.
                /// </summary>
                /// <typeparam name="TParser2">The Parser that will guarantee to parse before this one.</typeparam>
                /// <returns>This Instance.</returns>
                public DocumentBuilderInlineConfigurator<TParser> After<TParser2>()
                    where TParser2 : MarkdownInline.Parser, new()
                {
                    this.parent.AddRelationInline(typeof(TParser), typeof(TParser2));
                    return this;
                }

                /// <summary>
                /// Defines that the last added Parser will run before <typeparamref name="TParser2"/>.
                /// </summary>
                /// <typeparam name="TParser2">The Parser that will guarantee to parse after this one.</typeparam>
                /// <returns>This Instance.</returns>
                public DocumentBuilderInlineConfigurator<TParser> Before<TParser2>()
                    where TParser2 : MarkdownInline.Parser, new()
                {
                    this.parent.AddRelationInline(typeof(TParser2), typeof(TParser));
                    return this;
                }

                /// <inheritdoc/>
                public MarkdownDocument Build() => this.parent.Build();

                /// <inheritdoc/>
                public DocumentBuilder RemoveBlockParser<TParser1>()
                    where TParser1 : MarkdownBlock.Parser, new() => this.parent.RemoveBlockParser<TParser1>();

                /// <inheritdoc/>
                public DocumentBuilder RemoveInlineParser<TParser1>()
                    where TParser1 : MarkdownInline.Parser, new() => this.parent.RemoveInlineParser<TParser1>();
            }
        }
    }
}