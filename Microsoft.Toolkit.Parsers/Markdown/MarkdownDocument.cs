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
        /// Gets or sets the list of block elements.
        /// </summary>
        public IList<MarkdownBlock> Blocks { get; set; }

        /// <summary>
        /// Returns a builder with the same configuraiton as the one that created this Document.
        /// </summary>
        /// <returns>A Builder</returns>
        public DocumentBuilder GetBuilder() => new DocumentBuilder(this.parsersBlock, this.parserDependencysBlock, this.parsersInline, this.parserDependencysInline);

        /// <summary>
        /// Parses markdown document text.
        /// </summary>
        /// <param name="markdownText"> The markdown text. </param>
        public void Parse(string markdownText)
        {
            Blocks = Parse(markdownText, 0, markdownText.Length, quoteDepth: 0, actualEnd: out _);

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
        /// Parses a markdown document.
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The position to start parsing. </param>
        /// <param name="end"> The position to stop parsing. </param>
        /// <param name="quoteDepth"> The current nesting level for block quoting. </param>
        /// <param name="actualEnd"> Set to the position at which parsing ended.  This can be
        /// different from <paramref name="end"/> when the parser is being called recursively.
        /// </param>
        /// <returns> A list of parsed blocks. </returns>
        internal List<MarkdownBlock> Parse(string markdown, int start, int end, int quoteDepth, out int actualEnd)
        {
            // We need to parse out the list of blocks.
            // Some blocks need to start on a new paragraph (code, lists and tables) while other
            // blocks can start on any line (headers, horizontal rules and quotes).
            // Text that is outside of any other block becomes a paragraph.
            var blocks = new List<MarkdownBlock>();

            // Where the first character starts (without quote characters)
            int startOfLine = start;

            // Is this the beginning of a paragraph
            bool lineStartsNewParagraph = true;

            // text already "parsed" but not yet part of a block
            var paragraphText = new StringBuilder();

            // These are needed to parse underline-style header blocks.
            int previousRealtStartOfLine = start;
            int previousStartOfLine = start;
            int previousEndOfLine = start;

            // Go line by line.
            while (startOfLine < end)
            {
                // Find the first non-whitespace character.
                int nonSpacePos = startOfLine;
                char nonSpaceChar = '\0';

                // actual start of line, including quote characters
                int realStartOfLine = startOfLine;  // i.e. including quotes.

                int expectedQuotesRemaining = quoteDepth;
                while (true)
                {
                    while (nonSpacePos < end)
                    {
                        char c = markdown[nonSpacePos];
                        if (c == '\r' || c == '\n')
                        {
                            // The line is either entirely whitespace, or is empty.
                            break;
                        }

                        if (c != ' ' && c != '\t')
                        {
                            // The line has content.
                            nonSpaceChar = c;
                            break;
                        }

                        nonSpacePos++;
                    }

                    // When parsing blocks in a blockquote context, we need to count the number of
                    // quote characters ('>').  If there are less than expected AND this is the
                    // start of a new paragraph, then stop parsing.
                    if (expectedQuotesRemaining == 0)
                    {
                        break;
                    }

                    if (nonSpaceChar == '>')
                    {
                        // Expected block quote characters should be ignored.
                        expectedQuotesRemaining--;
                        nonSpacePos++;
                        nonSpaceChar = '\0';
                        startOfLine = nonSpacePos;

                        // Ignore the first space after the quote character, if there is one.
                        if (startOfLine < end && markdown[startOfLine] == ' ')
                        {
                            startOfLine++;
                            nonSpacePos++;
                        }
                    }
                    else
                    {
                        int lastIndentation = 0;
                        string lastline = null;

                        // Determines how many Quote levels were in the last line.
                        if (realStartOfLine > 0)
                        {
                            lastline = markdown.Substring(previousRealtStartOfLine, previousEndOfLine - previousRealtStartOfLine);
                            lastIndentation = lastline.Count(c => c == '>');
                        }

                        var currentEndOfLine = Common.FindNextSingleNewLine(markdown, nonSpacePos, end, out _);
                        var currentline = markdown.Substring(realStartOfLine, currentEndOfLine - realStartOfLine);
                        var currentIndentation = currentline.Count(c => c == '>');
                        var firstChar = markdown[realStartOfLine];

                        // This is a quote that doesn't start with a Quote marker, but carries on from the last line.
                        if (lastIndentation == 1)
                        {
                            if (nonSpaceChar != '\0' && firstChar != '>')
                            {
                                break;
                            }
                        }

                        // Collapse down a level of quotes if the current indentation is greater than the last indentation.
                        // Only if the last indentation is greater than 1, and the current indentation is greater than 0
                        if (lastIndentation > 1 && currentIndentation > 0 && currentIndentation < lastIndentation)
                        {
                            break;
                        }

                        // This must be the end of the blockquote.  End the current paragraph, if any.
                        actualEnd = realStartOfLine;

                        if (paragraphText.Length > 0)
                        {
                            blocks.Add(ParagraphBlock.Parse(paragraphText.ToString()));
                        }

                        return blocks;
                    }
                }

                // Find the end of the current line.
                int endOfLine = Common.FindNextSingleNewLine(markdown, nonSpacePos, end, out int startOfNextLine);

                if (nonSpaceChar == '\0')
                {
                    // The line is empty or nothing but whitespace.
                    lineStartsNewParagraph = true;

                    // End the current paragraph.
                    if (paragraphText.Length > 0)
                    {
                        blocks.Add(ParagraphBlock.Parse(paragraphText.ToString()));
                        paragraphText.Clear();
                    }
                }
                else
                {
                    // This is a header if the line starts with a hash character,
                    // or if the line starts with '-' or a '=' character and has no other characters.
                    // Or a quote if the line starts with a greater than character (optionally preceded by whitespace).
                    // Or a horizontal rule if the line contains nothing but 3 '*', '-' or '_' characters (with optional whitespace).
                    MarkdownBlock newBlockElement = null;

                    foreach (var parser in this.parsersBlock)
                    {
                        newBlockElement = parser.Parse(markdown, startOfLine, nonSpacePos, realStartOfLine, endOfLine, end, quoteDepth, out var endOfBlock, paragraphText, lineStartsNewParagraph, this);
                        if (newBlockElement != null)
                        {
                            startOfNextLine = endOfBlock;
                            break;
                        }
                    }

                    // Block elements start new paragraphs.
                    lineStartsNewParagraph = newBlockElement != null;

                    if (newBlockElement == null)
                    {
                        // The line contains paragraph text.
                        if (paragraphText.Length > 0)
                        {
                            // If the previous two characters were both spaces, then append a line break.
                            if (paragraphText.Length > 2 && paragraphText[paragraphText.Length - 1] == ' ' && paragraphText[paragraphText.Length - 2] == ' ')
                            {
                                // Replace the two spaces with a line break.
                                paragraphText[paragraphText.Length - 2] = '\r';
                                paragraphText[paragraphText.Length - 1] = '\n';
                            }
                            else
                            {
                                paragraphText.Append(" ");
                            }
                        }

                        // Add the last paragraph if we are at the end of the input text.
                        if (startOfNextLine >= end)
                        {
                            if (paragraphText.Length == 0)
                            {
                                // Optimize for single line paragraphs.
                                blocks.Add(ParagraphBlock.Parse(markdown.Substring(startOfLine, endOfLine - startOfLine)));
                            }
                            else
                            {
                                // Slow path.
                                paragraphText.Append(markdown.Substring(startOfLine, endOfLine - startOfLine));
                                blocks.Add(ParagraphBlock.Parse(paragraphText.ToString()));
                            }
                        }
                        else
                        {
                            paragraphText.Append(markdown.Substring(startOfLine, endOfLine - startOfLine));
                        }
                    }
                    else
                    {
                        // The line contained a block.  End the current paragraph, if any.
                        if (paragraphText.Length > 0)
                        {
                            blocks.Add(ParagraphBlock.Parse(paragraphText.ToString()));
                            paragraphText.Clear();
                        }

                        blocks.Add(newBlockElement);
                    }
                }

                // Repeat.
                previousRealtStartOfLine = realStartOfLine;
                previousStartOfLine = startOfLine;
                previousEndOfLine = endOfLine;
                startOfLine = startOfNextLine;
            }

            actualEnd = startOfLine;
            return blocks;
        }

        /// <summary>
        /// Looks up a reference using the ID.
        /// A reference is a line that looks like this:
        /// [foo]: http://example.com/
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
                    AddRelation(item, parser.GetType());
                }

                foreach (var item in parser.DefaultBeforeParsers)
                {
                    AddRelation(parser.GetType(), item);
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
        /// <typeparam name="T">The type to sort</typeparam>
        /// <returns>The ordered list</returns>
        private static IEnumerable<T> TopologicalSort<T>(IEnumerable<T> nodes, Dictionary<Type, HashSet<Type>> edges)
        {
            // we want to order all elements to get a deterministic result
            var orderedSource = nodes.OrderBy(x => x.GetType().FullName).ToArray();

            // We will manipulate edges. So we make a copy
            edges = new Dictionary<Type, HashSet<Type>>(edges);

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
            /// <typeparam name="TParser">The type of the Parser</typeparam>
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
                /// <returns>This Instance</returns>
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
                /// <returns>This Instance</returns>
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
            /// <typeparam name="TParser">The type of the Parser</typeparam>
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
                /// <returns>This Instance</returns>
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
                /// <returns>This Instance</returns>
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