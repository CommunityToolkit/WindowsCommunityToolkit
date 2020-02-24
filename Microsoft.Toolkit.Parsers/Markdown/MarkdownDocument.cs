// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
                      new CodeBlock.Parser()
                  }),
                  blockEdges: null,
                  inlineParsers: TopologicalSort(new MarkdownInline.Parser[]
                  {
                      new BoldItalicTextInline.Parser(),
                      new BoldTextInline.Parser(),
                      new CodeInline.Parser(),
                      new CommentInline.Parser(),
                      new EmojiInline.Parser(),
                      new HyperlinkInline.AngleBracketLinkParser(),
                      new HyperlinkInline.EmailAddressParser(),
                      new HyperlinkInline.PartialLinkParser(),
                      new HyperlinkInline.ReditLinkParser(),
                      new HyperlinkInline.UrlParser(),
                      new ImageInline.Parser(),
                      new ItalicTextInline.Parser(),
                      new LinkAnchorInline.Parser(),
                      new MarkdownLinkInline.Parser(),
                      new StrikethroughTextInline.Parser(),
                      new SubscriptTextInline.Parser(),
                      new SuperscriptTextInline.Parser()
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
            Blocks = ParseBlocks(markdownText, 0, markdownText.Length, actualEnd: out _);

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
        public List<MarkdownBlock> ParseBlocks(string markdown, int start, int end, out int actualEnd)
        {
            // We need to parse out the list of blocks.
            // Some blocks need to start on a new paragraph (code, lists and tables) while other
            // blocks can start on any line (headers, horizontal rules and quotes).
            // Text that is outside of any other block becomes a paragraph.
            var blocks = new List<MarkdownBlock>();

            // Where the first character starts
            int startOfLine = start;

            // Is this the beginning of a paragraph
            bool lineStartsNewParagraph = true;

            // We need to remember what text is not yet part of a block
            int startOfParagrapgh = start;

            // text already "parsed" but not yet part of a block will be transformed to ParagraphBlock
            void AddParagraph(int endOfText)
            {
                // End the current paragraph.
                if (startOfParagrapgh < endOfText)
                {
                    var block = ParagraphBlock.Parse(markdown, startOfParagrapgh, endOfText, this);
                    if (block != null)
                    {
                        blocks.Add(block);
                    }
                }

                // We need to mark all text as parsed.
                startOfParagrapgh = endOfText;
            }

            // Go line by line.
            while (startOfLine < end)
            {
                char nonSpaceChar = '\0';

                // Find the end of the current line.
                int endOfLine = Common.FindNextSingleNewLine(markdown, startOfLine, end, out int startOfNextLine);

                // Find the first non-whitespace character.
                var nonSpacePos = Common.FindNextNoneWhiteSpace(markdown, startOfLine, endOfLine, false);
                if (nonSpacePos != -1)
                {
                    nonSpaceChar = markdown[nonSpacePos];
                }

                if (nonSpaceChar == '\0')
                {
                    // The line is empty or nothing but whitespace.
                    lineStartsNewParagraph = true;
                    AddParagraph(startOfLine);
                }
                else
                {
                    BlockParseResult parsedBlock = null;

                    foreach (var parser in this.parsersBlock)
                    {
                        parsedBlock = parser.Parse(markdown, startOfLine, nonSpacePos, endOfLine, startOfParagrapgh, end, lineStartsNewParagraph, this);
                        if (parsedBlock != null)
                        {
                            startOfNextLine = parsedBlock.End;
                            break;
                        }
                    }

                    // Block elements start new paragraphs.
                    lineStartsNewParagraph = parsedBlock != null;

                    if (parsedBlock == null)
                    {
                        // Add the last paragraph if we are at the end of the input text.
                        if (startOfNextLine >= end)
                        {
                            AddParagraph(end);
                        }
                    }
                    else
                    {
                        AddParagraph(parsedBlock.Start);

                        // Skip the paragraph text
                        startOfParagrapgh = parsedBlock.End;

                        blocks.Add(parsedBlock.ParsedElement);
                    }
                }

                // Repeat.
                startOfLine = startOfNextLine;
            }

            actualEnd = startOfLine;
            return blocks;
        }

        /// <summary>
        /// This function can be called by any element parsing. Given a start and stopping point this will
        /// parse all found elements out of the range.
        /// </summary>
        /// <returns> A list of parsed inlines. </returns>
        public List<MarkdownInline> ParseInlineChildren(LineBlock markdown, IEnumerable<Type> ignoredParsers)
        {
            int currentParsePosition = startingPos;

            var inlines = new List<MarkdownInline>();
            while (currentParsePosition < maxEndingPos)
            {
                // Find the next inline element.
                var parseResult = FindNextInlineElement(markdown, currentParsePosition, maxEndingPos, ignoredParsers);

                // there were no more inlines.
                if (parseResult is null)
                {
                    // If we didn't find any elements we have a normal text block.
                    // Let us consume the entire range.
                    var textRun = TextRunInline.Parse(markdown, currentParsePosition, maxEndingPos, inlines.Count == 0, true);

                    // The textblock may contain only linebreaks.
                    if (textRun != null)
                    {
                        parseResult = InlineParseResult.Create(textRun, currentParsePosition, maxEndingPos);
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
                if (parseResult.StartLine != currentParsePosition)
                {
                    var textRun = TextRunInline.Parse(markdown, currentParsePosition, parseResult.StartLine, inlines.Count == 0, false);

                    if (textRun != null)
                    {
                        inlines.Add(textRun);
                    }
                }

                // Add the parsed element.
                inlines.Add(parseResult.ParsedElement);

                // Update the current position.
                currentParsePosition = parseResult.EndLine;
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
        private InlineParseResult FindNextInlineElement(string markdown, int start, int end, IEnumerable<Type> ignoredParsers)
        {
            var parsers = this.parsersInline;
            if (ignoredParsers.Any())
            {
                parsers = parsers.Where(x => !ignoredParsers.Contains(x.GetType())).ToArray();
            }

            var canUseTripChar = parsers.All(x => x.TripChar.Any());

            var foundInline = canUseTripChar
                ? this.FindNextInlineWithTripChar(markdown, start, end, ignoredParsers, parsers)
                : this.FindNextInlineSlow(markdown, start, end, ignoredParsers, parsers);

            return foundInline;
        }

        private InlineParseResult FindNextInlineSlow(string markdown, int start, int end, IEnumerable<Type> ignoredParsers, MarkdownInline.Parser[] parsers)
        {
            // Search for the next inline sequence.
            for (int pos = start; pos < end; pos++)
            {
                // IndexOfAny should be the fastest way to skip characters we don't care about.

                // Don't match if the previous character was a backslash.
                if (pos > start && markdown[pos - 1] == '\\')
                {
                    continue;
                }

                // Find the trigger(s) that matched.
                char currentChar = markdown[pos];
                foreach (var parser in parsers.Where(x => !x.TripChar.Any() || x.TripChar.Contains(currentChar)))
                {
                    // If we are here we have a possible match. Call into the inline class to verify.
                    var parseResult = parser.Parse(markdown, pos, end, ignoredParsers);
                    if (parseResult != null)
                    {
                        return parseResult;
                    }
                }
            }

            return null;
        }

        private InlineParseResult FindNextInlineWithTripChar(string markdown, int start, int end, IEnumerable<Type> ignoredParsers, MarkdownInline.Parser[] parsers)
        {
            var tripCharacters = parsers.SelectMany(x => x.TripChar).Distinct().ToArray();

            // Search for the next inline sequence.
            for (int pos = start; pos < end; pos++)
            {
                // IndexOfAny should be the fastest way to skip characters we don't care about.
                pos = markdown.IndexOfAny(tripCharacters, pos, end - pos);
                if (pos < 0)
                {
                    break;
                }

                // Don't match if the previous character was a backslash.
                if (pos > start && markdown[pos - 1] == '\\')
                {
                    continue;
                }

                // Find the trigger(s) that matched.
                char currentChar = markdown[pos];
                foreach (var parser in parsers.Where(x => x.TripChar.Contains(currentChar)))
                {
                    // If we are here we have a possible match. Call into the inline class to verify.
                    var parseResult = parser.Parse(markdown, pos, end, ignoredParsers);
                    if (parseResult != null)
                    {
                        return parseResult;
                    }
                }
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