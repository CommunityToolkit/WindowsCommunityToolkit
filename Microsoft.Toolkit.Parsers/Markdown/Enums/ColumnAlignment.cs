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

namespace Microsoft.Toolkit.Parsers.Markdown
{
    /// <summary>
    /// The alignment of content in a table column.
    /// </summary>
    public enum ColumnAlignment
    {
        /// <summary>
        /// The alignment was not specified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Content should be left aligned.
        /// </summary>
        Left,

        /// <summary>
        /// Content should be right aligned.
        /// </summary>
        Right,

        /// <summary>
        /// Content should be centered.
        /// </summary>
        Center,
    }
}