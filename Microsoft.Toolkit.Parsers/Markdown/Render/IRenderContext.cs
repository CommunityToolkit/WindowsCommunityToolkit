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

namespace Microsoft.Toolkit.Parsers.Markdown.Render
{
    /// <summary>
    /// Helper for holding persistent state of Renderer.
    /// </summary>
    public interface IRenderContext
    {
        /// <summary>
        /// Gets or sets a value indicating whether to trim whitespace.
        /// </summary>
        bool TrimLeadingWhitespace { get; set; }

        /// <summary>
        /// Gets or sets the parent Element for this Context.
        /// </summary>
        object Parent { get; set; }

        /// <summary>
        /// Clones the Context.
        /// </summary>
        /// <returns>Clone</returns>
        IRenderContext Clone();
    }
}