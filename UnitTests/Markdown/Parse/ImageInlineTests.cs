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

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace UnitTests.Markdown.Parse
{
    [TestClass]
    public class ImageInlineTests : ParseTestBase
    {
        [TestMethod]
        [TestCategory("Parse - inline")]
        public void ImageInline_WithWidth()
        {
            AssertEqual(
                "![SVG logo](https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg =1)",
                new ParagraphBlock().AddChildren(
                    new ImageInline
                    {
                        Url = "https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg",
                        Text = "![SVG logo](https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg =1)",
                        Tooltip = "SVG logo",
                        ImageWidth = 1,
                        ImageHeight = 0
                    }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void ImageInline_WithHeight()
        {
            AssertEqual(
                "![SVG logo](https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg =x1)",
                new ParagraphBlock().AddChildren(
                    new ImageInline
                    {
                        Url = "https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg",
                        Text = "![SVG logo](https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg =x1)",
                        Tooltip = "SVG logo",
                        ImageWidth = 0,
                        ImageHeight = 1
                    }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void ImageInline_WithWidthAndHeight()
        {
            AssertEqual(
                "![SVG logo](https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg =128x64)",
                new ParagraphBlock().AddChildren(
                    new ImageInline
                    {
                        Url = "https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg",
                        Tooltip = "SVG logo",
                        Text = "![SVG logo](https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg =128x64)",
                        ImageWidth = 128,
                        ImageHeight = 64
                    }));
        }

        [TestMethod]
        [TestCategory("Parse - inline")]
        public void ImageInline_ParseEncodedUrl()
        {
            AssertEqual(
                "![SVG logo](https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg%20=32)",
                new ParagraphBlock().AddChildren(
                    new ImageInline
                    {
                        Url = "https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg%20=32",
                        Tooltip = "SVG logo",
                        Text = "![SVG logo](https://upload.wikimedia.org/wikipedia/commons/0/02/SVG_logo.svg%20=32)",
                        ImageWidth = 0,
                        ImageHeight = 0
                    }));
        }
    }
}