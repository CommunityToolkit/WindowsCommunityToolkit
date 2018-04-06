using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Input.Inking;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class InfiniteCanvas
    {
        public class InfiniteCanvasTextBlock
        {
            public string Text { get; set; }
            public float FontSize { get; set; }
            public bool IsBold { get; set; }
            public bool IsItalic { get; set; }
        }

        public static List<InfiniteCanvasTextBlock> processText(string text)
        {
            bool startInfo = false;
            bool startProcessing = false;
            bool isBold = false;
            bool isItalic = false;
            bool isAttribute = false;

            List<InfiniteCanvasTextBlock> textBlockList = new List<InfiniteCanvasTextBlock>();
            StringBuilder attributeBuilder = new StringBuilder();
            StringBuilder textBuilder = new StringBuilder();

            float currentFontSize = 23;
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (!startInfo && c == 'p' && i + 1 < text.Length && text[i + 1] == 'a' && i + 2 < text.Length && text[i + 2] == 'r' && i + 3 < text.Length && text[i + 3] == 'd')
                {
                    startInfo = true;
                    continue;
                }

                if (!startProcessing && startInfo && c == ' ')
                {
                    startProcessing = true;
                    continue;
                }

                if (startProcessing)
                {
                    if (isAttribute)
                    {
                        if (c == '\\' || c == ' ')
                        {
                            if (c == ' ')
                            {
                                isAttribute = false;
                            }

                            var attribute = attributeBuilder.ToString();
                            attributeBuilder.Clear();

                            // check font size
                            if (attribute.Length > 2 && attribute[0] == 'f' && attribute[1] == 's')
                            {
                                currentFontSize = float.Parse(attribute.Substring(2));
                            }
                            else if (attribute == "b")
                            {
                                isBold = true;
                            }
                            else if (attribute == "b0")
                            {
                                isBold = false;
                            }
                            else if (attribute == "i")
                            {
                                isItalic = true;
                            }
                            else if (attribute == "i0")
                            {
                                isItalic = false;
                            }
                            else if (attribute == "par")
                            {
                                // add new line
                            }
                            else
                            {
                                continue;
                            }

                            if (textBuilder.Length > 0)
                            {
                                var textBlock = new InfiniteCanvasTextBlock()
                                {
                                    IsBold = isBold,
                                    IsItalic = isItalic,
                                    FontSize = currentFontSize,
                                    Text = textBuilder.ToString()
                                };

                                textBuilder.Clear();
                                textBlockList.Add(textBlock);
                            }
                        }
                        else
                        {
                            attributeBuilder.Append(c);
                        }
                    }
                    else if (c == '\\')
                    {
                        isAttribute = true;
                        if (textBuilder.Length > 0)
                        {
                            var textBlock = new InfiniteCanvasTextBlock()
                            {
                                IsBold = isBold,
                                IsItalic = isItalic,
                                FontSize = currentFontSize,
                                Text = textBuilder.ToString()
                            };

                            textBuilder.Clear();
                            textBlockList.Add(textBlock);
                        }
                    }
                    else
                    {
                        textBuilder.Append(c);
                    }
                }
            }

            return textBlockList;
        }

        internal static InkPoint MapPointToToSessionBounds(InkPoint point, Rect sessionBounds)
        {
            const int margin = 0;
            return new InkPoint(new Point(point.Position.X - sessionBounds.X - margin, point.Position.Y - sessionBounds.Y - margin), point.Pressure, point.TiltX, point.TiltY, point.Timestamp);
        }
    }
}
