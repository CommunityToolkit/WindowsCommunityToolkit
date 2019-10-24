// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

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

        private readonly MarkdownBlock.Factory[] factorys;

        private Dictionary<string, LinkReferenceBlock> _references;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownDocument"/> class.
        /// </summary>
        public MarkdownDocument()
            : base(MarkdownBlockType.Root)
        {
            this.factorys = new Factory[]
            {
                new YamlHeaderBlock.Factory(),
                new TableBlock.Factory(),
                new QuoteBlock.Factory(),
                new ListBlock.Factory(),
                new LinkReferenceBlock.Factory(),
                new HorizontalRuleBlock.Factory(),
                new HeaderBlock.HashFactory(),
                new HeaderBlock.UnderlineFactory(),
                new CodeBlock.Factory()
            };
        }

        internal MarkdownDocument(IEnumerable<Factory> blockFactorys)
            : this()
        {
            this.factorys = blockFactorys.ToArray();
        }

        /// <summary>
        /// Gets or sets the list of block elements.
        /// </summary>
        public IList<MarkdownBlock> Blocks { get; set; }

        /// <summary>
        /// Parses markdown document text.
        /// </summary>
        /// <param name="markdownText"> The markdown text. </param>
        public void Parse(string markdownText)
        {
            Blocks = Parse(markdownText, 0, markdownText.Length, quoteDepth: 0, actualEnd: out int actualEnd);

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


                    foreach (var factory in this.factorys)
                    {
                        newBlockElement = factory.Parse(markdown, startOfLine, nonSpacePos, realStartOfLine, endOfLine, end, quoteDepth, out var endOfBlock, paragraphText, lineStartsNewParagraph, this);
                        if (newBlockElement != null)
                        {
                            startOfNextLine = endOfBlock;
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
    }
    public interface IDocumentBuilder
    {
        DocumentBuilder.DocumentBuilderConfigurator<TFactory> AddParser<TFactory>(Action<TFactory> configurationCallback = null) where TFactory : MarkdownBlock.Factory, new();
        MarkdownDocument Build();
        DocumentBuilder RemoveParser<TFactory>()
            where TFactory : MarkdownBlock.Factory, new();
    }
    public class DocumentBuilder : IDocumentBuilder
    {

        private readonly Dictionary<Type, MarkdownBlock.Factory> factoryInstances = new Dictionary<Type, MarkdownBlock.Factory>();
        private readonly Dictionary<Type, HashSet<Type>> aAfterBRelation = new Dictionary<Type, HashSet<Type>>();

        public DocumentBuilder RemoveParser<TFactory>()
            where TFactory : MarkdownBlock.Factory, new()
        {
            this.factoryInstances.Remove(typeof(TFactory));
            return this;
        }

        public DocumentBuilderConfigurator<TFactory> AddParser<TFactory>(Action<TFactory> configurationCallback = null)
            where TFactory : MarkdownBlock.Factory, new()
        {
            var data = new TFactory();
            configurationCallback?.Invoke(data);
            this.factoryInstances.Add(typeof(TFactory), data);

            foreach (var item in data.DefaultAfterFactorys)
            {
                this.AddRelation(item, data.GetType());
            }
            foreach (var item in data.DefaultBeforeFactorys)
            {
                this.AddRelation(data.GetType(), item);
            }

            return new DocumentBuilderConfigurator<TFactory>(this);
        }


        public MarkdownDocument Build()
        {

            // we need to get rid of all edges that are related to removed nodes

            foreach (var item in this.aAfterBRelation.Keys.Where(x => !this.factoryInstances.ContainsKey(x)).ToArray())
            {
                this.aAfterBRelation.Remove(item);
            }

            foreach (var keyValuePair in this.aAfterBRelation)
            {
                foreach (var item in keyValuePair.Value.Where(x => !this.factoryInstances.ContainsKey(x)).ToArray())
                {
                    keyValuePair.Value.Remove(item);
                }
            }


            // we want to order all elements to get a deterministic result
            var orderedSource = this.factoryInstances.Values.OrderBy(x => x.GetType().FullName).ToArray();

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


            var L = new List<MarkdownBlock.Factory>();
            var S = new Queue<MarkdownBlock.Factory>(orderedSource.Where(x => !this.aAfterBRelation.ContainsKey(x.GetType())));

            while (S.Count > 0)
            {
                var n = S.Dequeue();
                L.Add(n);
                foreach (var m in orderedSource)
                {
                    if (this.aAfterBRelation.ContainsKey(m.GetType()) && this.aAfterBRelation[m.GetType()].Contains(n.GetType()))
                    {
                        this.RemoveRelation(m.GetType(), n.GetType());
                        if (!this.aAfterBRelation.ContainsKey(m.GetType()))
                            S.Enqueue(m);
                    }
                }
            }

            if (this.aAfterBRelation.Count > 0)
                throw new InvalidOperationException("Graph contains cycles");

            return new MarkdownDocument(L);



        }

        private void AddRelation(Type before, Type after)
        {
            if (this.aAfterBRelation.ContainsKey(before))
            {
                this.aAfterBRelation[before].Add(after);
            }
            else
            {
                this.aAfterBRelation.Add(before, new HashSet<Type> { after });
            }
        }
        private void RemoveRelation(Type before, Type after)
        {
            if (this.aAfterBRelation.ContainsKey(before))
            {
                this.aAfterBRelation[before].Remove(after);
                if (this.aAfterBRelation[before].Count == 0)
                    this.aAfterBRelation.Remove(before);
            }
        }

        public class DocumentBuilderConfigurator<TFactory> : IDocumentBuilder
                where TFactory : MarkdownBlock.Factory, new()
        {
            private readonly DocumentBuilder parent;


            internal DocumentBuilderConfigurator(DocumentBuilder documentBuilder)
            {
                this.parent = documentBuilder;
            }

            public DocumentBuilderConfigurator<TFactory1> AddParser<TFactory1>(Action<TFactory1> configurationCallback = null) where TFactory1 : MarkdownBlock.Factory, new() => this.parent.AddParser(configurationCallback);

            public DocumentBuilderConfigurator<TFactory> After<TFactory2>()
                where TFactory2 : MarkdownBlock.Factory, new()
            {
                this.parent.AddRelation(typeof(TFactory), typeof(TFactory2));
                return this;
            }

            public DocumentBuilderConfigurator<TFactory> Before<TFactory2>()
                where TFactory2 : MarkdownBlock.Factory, new()
            {
                this.parent.AddRelation(typeof(TFactory2), typeof(TFactory));
                return this;
            }

            public MarkdownDocument Build() => this.parent.Build();


            public DocumentBuilder RemoveParser<TFactory1>()
                where TFactory1 : MarkdownBlock.Factory, new() => this.parent.RemoveParser<TFactory1>();

        }


    }

}