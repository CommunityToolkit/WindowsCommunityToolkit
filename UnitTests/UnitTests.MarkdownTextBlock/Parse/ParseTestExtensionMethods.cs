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

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse.Elements;

namespace UnitTests.Markdown.Parse
{
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class ParseTestExtensionMethods
    {
        /// <summary>
        /// Adds one or more child elements to the given parent object.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static T AddChildren<T>(this T parent, params object[] elements)
        {
            foreach (var child in elements)
                AddChild(parent, child);
            return parent;
        }

        private static void AddChild<T>(T parent, object child)
        {
            if (parent is MarkdownDocument)
                AddChild(() => ((MarkdownDocument)(object)parent).Blocks, (value) => ((MarkdownDocument)(object)parent).Blocks = value, (MarkdownBlock)child);
            else if (parent is HeaderBlock)
                AddChild(() => ((HeaderBlock)(object)parent).Inlines, (value) => ((HeaderBlock)(object)parent).Inlines = value, (MarkdownInline)child);
            else if (parent is ListBlock)
                AddChild(() => ((ListBlock)(object)parent).Items, (value) => ((ListBlock)(object)parent).Items = value, (ListItemBlock)child);
            else if (parent is ListItemBlock)
                AddChild(() => ((ListItemBlock)(object)parent).Blocks, (value) => ((ListItemBlock)(object)parent).Blocks = value, (MarkdownBlock)child);
            else if (parent is ParagraphBlock)
                AddChild(() => ((ParagraphBlock)(object)parent).Inlines, (value) => ((ParagraphBlock)(object)parent).Inlines = value, (MarkdownInline)child);
            else if (parent is QuoteBlock)
                AddChild(() => ((QuoteBlock)(object)parent).Blocks, (value) => ((QuoteBlock)(object)parent).Blocks = value, (MarkdownBlock)child);
            else if (parent is TableBlock)
                AddChild(() => ((TableBlock)(object)parent).Rows, (value) => ((TableBlock)(object)parent).Rows = value, (TableRow)child);
            else if (parent is TableRow)
                AddChild(() => ((TableRow)(object)parent).Cells, (value) => ((TableRow)(object)parent).Cells = value, (TableCell)child);
            else if (parent is TableCell)
                AddChild(() => ((TableCell)(object)parent).Inlines, (value) => ((TableCell)(object)parent).Inlines = value, (MarkdownInline)child);
            else if (parent is BoldTextInline)
                AddChild(() => ((BoldTextInline)(object)parent).Inlines, (value) => ((BoldTextInline)(object)parent).Inlines = value, (MarkdownInline)child);
            else if (parent is ItalicTextInline)
                AddChild(() => ((ItalicTextInline)(object)parent).Inlines, (value) => ((ItalicTextInline)(object)parent).Inlines = value, (MarkdownInline)child);
            else if (parent is MarkdownLinkInline)
                AddChild(() => ((MarkdownLinkInline)(object)parent).Inlines, (value) => ((MarkdownLinkInline)(object)parent).Inlines = value, (MarkdownInline)child);
            else if (parent is StrikethroughTextInline)
                AddChild(() => ((StrikethroughTextInline)(object)parent).Inlines, (value) => ((StrikethroughTextInline)(object)parent).Inlines = value, (MarkdownInline)child);
            else if (parent is SuperscriptTextInline)
                AddChild(() => ((SuperscriptTextInline)(object)parent).Inlines, (value) => ((SuperscriptTextInline)(object)parent).Inlines = value, (MarkdownInline)child);
            else
                throw new NotSupportedException(string.Format("Unsupported type {0}", typeof(T).Name));
        }

        private static void AddChild<T>(Func<IList<T>> getter, Action<IList<T>> setter, T child)
        {
            var list = getter();
            if (list == null)
            {
                list = new List<T>();
                setter(list);
            }
            list.Add(child);
        }
    }
}
