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
namespace Microsoft.Toolkit.Parsers.Markdown.Enums
{
    /// <summary>
    /// Determines the type of Block the Block element is.
    /// </summary>
    public enum MarkdownBlockType
    {
        /// <summary>
        /// The root element
        /// </summary>
        Root,

        /// <summary>
        /// A paragraph element.
        /// </summary>
        Paragraph,

        /// <summary>
        /// A quote block
        /// </summary>
        Quote,

        /// <summary>
        /// A code block
        /// </summary>
        Code,

        /// <summary>
        /// A header block
        /// </summary>
        Header,

        /// <summary>
        /// A list block
        /// </summary>
        List,

        /// <summary>
        /// A list item block
        /// </summary>
        ListItemBuilder,

        /// <summary>
        /// a horizontal rule block
        /// </summary>
        HorizontalRule,

        /// <summary>
        /// A table block
        /// </summary>
        Table,

        /// <summary>
        /// A link block
        /// </summary>
        LinkReference
    }
}