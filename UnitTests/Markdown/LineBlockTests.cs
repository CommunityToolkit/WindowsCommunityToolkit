using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Markdown
{
    [TestClass]
    public class LineBlockTests
    {
        const string text = @"Hallo
Welt
Hello
World";

        [TestMethod]
        public void CheckLineCount()
        {
            var l = new LineBlock(text.AsSpan());
            Assert.AreEqual(4, l.LineCount);
        }

        [TestMethod]
        public void TextLength()
        {
            var l = new LineBlock(text.AsSpan());
            Assert.AreEqual(22, l.TextLength);
        }

        [TestMethod]
        public void CheckToString()
        {
            var l = new LineBlock(text.AsSpan());

            Assert.AreEqual(text, l.ToString());
        }

        [TestMethod]
        public void CheckLines()
        {
            var l = new LineBlock(text.AsSpan());
            Assert.AreEqual("Hallo", l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hello", l[2].ToString());
            Assert.AreEqual("World", l[3].ToString());
        }

        [TestMethod]
        public void SliceStart()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(2);

            Assert.AreEqual("llo", l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hello", l[2].ToString());
            Assert.AreEqual("World", l[3].ToString());
        }

        [TestMethod]
        public void SliceStart_2()
        {
            var testString = "Hallo";
            var l = new LineBlock(text.AsSpan());
            for (int i = 0; i < 6; i++)
            {
                l = l.SliceText(1);
                if (testString.Length > 0)
                {
                    testString = testString.Substring(1);
                }

                if (i == 5)
                {
                    Assert.AreEqual(3, l.LineCount);
                    Assert.AreEqual("Welt", l[0].ToString());
                    Assert.AreEqual("Hello", l[1].ToString());
                    Assert.AreEqual("World", l[2].ToString());
                }
                else
                {
                    Assert.AreEqual(4, l.LineCount);
                    Assert.AreEqual(testString, l[0].ToString());
                    Assert.AreEqual("Welt", l[1].ToString());
                    Assert.AreEqual("Hello", l[2].ToString());
                    Assert.AreEqual("World", l[3].ToString());
                }
            }
        }

        [TestMethod]
        public void SliceStartTwice()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(1);
            l = l.SliceText(1);

            Assert.AreEqual("llo", l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hello", l[2].ToString());
            Assert.AreEqual("World", l[3].ToString());
        }

        [TestMethod]
        public void SliceStartFirstLineLine()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(5);

            Assert.AreEqual(4, l.LineCount);
            Assert.AreEqual(string.Empty, l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hello", l[2].ToString());
            Assert.AreEqual("World", l[3].ToString());
        }

        [TestMethod]
        public void SliceStartCompleteLine()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(6);

            Assert.AreEqual(3, l.LineCount);
            Assert.AreEqual("Welt", l[0].ToString());
            Assert.AreEqual("Hello", l[1].ToString());
            Assert.AreEqual("World", l[2].ToString());
        }

        [TestMethod]
        public void SliceStartMoreThen1Line()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(7);

            Assert.AreEqual(3, l.LineCount);
            Assert.AreEqual("elt", l[0].ToString());
            Assert.AreEqual("Hello", l[1].ToString());
            Assert.AreEqual("World", l[2].ToString());
        }

        [TestMethod]
        public void SliceEnd()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(0, l.TextLength - 1);

            Assert.AreEqual("Hallo", l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hello", l[2].ToString());
            Assert.AreEqual("Worl", l[3].ToString());
        }

        [TestMethod]
        public void SliceEndTwice()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(0, l.TextLength - 1);
            l = l.SliceText(0, l.TextLength - 1);

            Assert.AreEqual("Hallo", l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hello", l[2].ToString());
            Assert.AreEqual("Wor", l[3].ToString());
        }

        [TestMethod]
        public void SliceEndCompleteLine()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(0, l.TextLength - 6);

            Assert.AreEqual(3, l.LineCount);
            Assert.AreEqual("Hallo", l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hello", l[2].ToString());
        }

        [TestMethod]
        public void SliceEndEmptyLine()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(0, l.TextLength - 5);

            Assert.AreEqual(4, l.LineCount);
            Assert.AreEqual("Hallo", l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hello", l[2].ToString());
            Assert.AreEqual(string.Empty, l[3].ToString());
        }

        [TestMethod]
        public void SliceEndMoreThen1Line()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(0, l.TextLength - 7);

            Assert.AreEqual(3, l.LineCount);
            Assert.AreEqual("Hallo", l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hell", l[2].ToString());
        }

        [TestMethod]
        public void SliceBoth()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(1, l.TextLength - 2);

            Assert.AreEqual("allo", l[0].ToString());
            Assert.AreEqual("Welt", l[1].ToString());
            Assert.AreEqual("Hello", l[2].ToString());
            Assert.AreEqual("Worl", l[3].ToString());
        }

        [TestMethod]
        public void RemoveFromLineStart()
        {
            var l = new LineBlock(text.AsSpan());

            l = l.RemoveFromLineStart(2);

            Assert.AreEqual("llo", l[0].ToString());
            Assert.AreEqual("lt", l[1].ToString());
            Assert.AreEqual("llo", l[2].ToString());
            Assert.AreEqual("rld", l[3].ToString());
        }

        [TestMethod]
        public void RemoveFromLineEnd()
        {
            var l = new LineBlock(text.AsSpan());

            l = l.RemoveFromLineEnd(2);

            Assert.AreEqual("Hal", l[0].ToString());
            Assert.AreEqual("We", l[1].ToString());
            Assert.AreEqual("Hel", l[2].ToString());
            Assert.AreEqual("Wor", l[3].ToString());
        }

        [TestMethod]
        public void RemoveFromLineStartAfterSlice()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(1, l.TextLength - 2);
            l = l.RemoveFromLineStart(2);

            Assert.AreEqual("lo", l[0].ToString());
            Assert.AreEqual("lt", l[1].ToString());
            Assert.AreEqual("llo", l[2].ToString());
            Assert.AreEqual("rl", l[3].ToString());
        }

        [TestMethod]
        public void RemoveFromLineEndAfterSlice()
        {
            var l = new LineBlock(text.AsSpan());
            l = l.SliceText(1, l.TextLength - 2);
            l = l.RemoveFromLineEnd(2);

            Assert.AreEqual("al", l[0].ToString());
            Assert.AreEqual("We", l[1].ToString());
            Assert.AreEqual("Hel", l[2].ToString());
            Assert.AreEqual("Wo", l[3].ToString());
        }

        [TestMethod]
        public void RemoveLastCharStart()
        {
            var l = new LineBlock("1".AsSpan());
            l = l.SliceText(1);
            Assert.AreEqual(1, l.LineCount);
            Assert.AreEqual(0, l[0].Length);
        }

        [TestMethod]
        public void RemoveLastCharEnd()
        {
            var l = new LineBlock("1".AsSpan());
            l = l.SliceText(0, 0);
            Assert.AreEqual(1, l.LineCount);
            Assert.AreEqual(0, l[0].Length);
        }

        [TestMethod]
        public void RemoveMoreCharactersThenLineLengthFromStart()
        {
            var l = new LineBlock("1\nHalloWorld\n2".AsSpan());
            l = l.RemoveFromLineStart(2);
            Assert.AreEqual(3, l.LineCount);
            Assert.AreEqual(string.Empty, l[0].ToString());
            Assert.AreEqual("lloWorld", l[1].ToString());
            Assert.AreEqual(string.Empty, l[2].ToString());
        }

        [TestMethod]
        public void RemoveMoreCharactersThenLineLengthFromEnd()
        {
            var l = new LineBlock("1\nHalloWorld\n2".AsSpan());
            l = l.RemoveFromLineEnd(2);
            Assert.AreEqual(3, l.LineCount);
            Assert.AreEqual(string.Empty, l[0].ToString());
            Assert.AreEqual("HalloWor", l[1].ToString());
            Assert.AreEqual(string.Empty, l[2].ToString());
        }
    }
}
