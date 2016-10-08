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

#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Microsoft.Toolkit.Uwp.Notifications;
using System;

namespace UnitTests.Notifications
{
    [TestClass]
    public class Test_Adaptive_Xml
    {
        [TestMethod]
        public void Test_Adaptive_Text_Defaults()
        {
            AssertAdaptiveChild("<text />", new AdaptiveText());
        }

        [TestMethod]
        public void Test_Adaptive_Text_Text()
        {
            AssertAdaptiveChild("<text>Hello &amp; Goodbye</text>", new AdaptiveText()
            {
                Text = "Hello & Goodbye"
            });
        }
        
        [TestMethod]
        public void Test_Adaptive_Text_HintStyle_Values()
        {
            AssertAdaptiveTextStyle("caption", AdaptiveTextStyle.Caption);
            AssertAdaptiveTextStyle("captionSubtle", AdaptiveTextStyle.CaptionSubtle);
            AssertAdaptiveTextStyle("base", AdaptiveTextStyle.Base);
            AssertAdaptiveTextStyle("baseSubtle", AdaptiveTextStyle.BaseSubtle);
            AssertAdaptiveTextStyle("body", AdaptiveTextStyle.Body);
            AssertAdaptiveTextStyle("bodySubtle", AdaptiveTextStyle.BodySubtle);
            AssertAdaptiveTextStyle("subtitle", AdaptiveTextStyle.Subtitle);
            AssertAdaptiveTextStyle("subtitleSubtle", AdaptiveTextStyle.SubtitleSubtle);
            AssertAdaptiveTextStyle("title", AdaptiveTextStyle.Title);
            AssertAdaptiveTextStyle("titleSubtle", AdaptiveTextStyle.TitleSubtle);
            AssertAdaptiveTextStyle("titleNumeral", AdaptiveTextStyle.TitleNumeral);
            AssertAdaptiveTextStyle("subheader", AdaptiveTextStyle.Subheader);
            AssertAdaptiveTextStyle("subheaderSubtle", AdaptiveTextStyle.SubheaderSubtle);
            AssertAdaptiveTextStyle("subheaderNumeral", AdaptiveTextStyle.SubheaderNumeral);
            AssertAdaptiveTextStyle("header", AdaptiveTextStyle.Header);
            AssertAdaptiveTextStyle("headerSubtle", AdaptiveTextStyle.HeaderSubtle);
            AssertAdaptiveTextStyle("headerNumeral", AdaptiveTextStyle.HeaderNumeral);
        }

        private static void AssertAdaptiveTextStyle(string expectedPropertyValue, AdaptiveTextStyle style)
        {
            AssertAdaptiveTextPropertyValue("hint-style", expectedPropertyValue, new AdaptiveText()
            {
                HintStyle = style
            });
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintAlign_Values()
        {
            AssertAdaptiveTextAlign("auto", AdaptiveTextAlign.Auto);
            AssertAdaptiveTextAlign("left", AdaptiveTextAlign.Left);
            AssertAdaptiveTextAlign("center", AdaptiveTextAlign.Center);
            AssertAdaptiveTextAlign("right", AdaptiveTextAlign.Right);
        }
        
        private static void AssertAdaptiveTextAlign(string expectedPropertyValue, AdaptiveTextAlign align)
        {
            AssertAdaptiveTextPropertyValue("hint-align", expectedPropertyValue, new AdaptiveText()
            {
                HintAlign = align
            });
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMaxLines_MinValue()
        {
            AssertAdaptiveTextPropertyValue("hint-maxLines", "1", new AdaptiveText()
            {
                HintMaxLines = 1
            });
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMaxLines_NormalValue()
        {
            AssertAdaptiveTextPropertyValue("hint-maxLines", "3", new AdaptiveText()
            {
                HintMaxLines = 3
            });
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMaxLines_MaxValue()
        {
            AssertAdaptiveTextPropertyValue("hint-maxLines", int.MaxValue.ToString(), new AdaptiveText()
            {
                HintMaxLines = int.MaxValue
            });
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMaxLines_BelowMin()
        {
            try
            {
                new AdaptiveText() { HintMaxLines = 0 };
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }

            // We can't use the ExpectedException attribute since the UWP testing toolkit doesn't support it :(
            Assert.Fail("ArgumentOutOfRangeExceptions should have been thrown.");
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMaxLines_AboveMax()
        {
            try
            { 
                new AdaptiveText() { HintMaxLines = -54 };
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }

            Assert.Fail("ArgumentOutOfRangeExceptions should have been thrown.");
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMinLines_MinValue()
        {
            AssertAdaptiveTextPropertyValue("hint-minLines", "1", new AdaptiveText()
            {
                HintMinLines = 1
            });
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMinLines_NormalValue()
        {
            AssertAdaptiveTextPropertyValue("hint-minLines", "3", new AdaptiveText()
            {
                HintMinLines = 3
            });
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMinLines_MaxValue()
        {
            AssertAdaptiveTextPropertyValue("hint-minLines", int.MaxValue.ToString(), new AdaptiveText()
            {
                HintMinLines = int.MaxValue
            });
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMinLines_BelowMin()
        {
            try
            { 
                new AdaptiveText() { HintMinLines = 0 };
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }

            Assert.Fail("ArgumentOutOfRangeExceptions should have been thrown.");
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintMinLines_AboveMax()
        {
            try
            { 
                new AdaptiveText() { HintMinLines = -54 };
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }

            Assert.Fail("ArgumentOutOfRangeExceptions should have been thrown.");
        }

        [TestMethod]
        public void Test_Adaptive_Text_HintWrap_Values()
        {
            AssertAdaptiveTextPropertyValue("hint-wrap", "false", new AdaptiveText()
            {
                HintWrap = false
            });

            AssertAdaptiveTextPropertyValue("hint-wrap", "true", new AdaptiveText()
            {
                HintWrap = true
            });
        }

        [TestMethod]
        public void Test_Adaptive_Text_DefaultNullValues()
        {
            AssertAdaptiveChild("<text />", new AdaptiveText()
            {
                HintAlign = AdaptiveTextAlign.Default,
                HintStyle = AdaptiveTextStyle.Default,
                HintMaxLines = null,
                HintMinLines = null,
                HintWrap = null,
                Language = null,
                Text = null
            });
        }

        private static void AssertAdaptiveTextPropertyValue(string expectedPropertyName, string expectedPropertyValue, AdaptiveText text)
        {
            AssertAdaptiveChild($"<text {expectedPropertyName}='{expectedPropertyValue}'/>", text);
        }

        [TestMethod]
        public void Test_Adaptive_Image_Defaults()
        {
            try
            { 
                AssertAdaptiveChild("exception should be thrown", new AdaptiveImage());
            }
            catch (NullReferenceException)
            {
                return;
            }

            Assert.Fail("NullReferenceException should have been thrown.");
        }

        [TestMethod]
        public void Test_Adaptive_Image_Source()
        {
            AssertAdaptiveImagePropertyValue("src", "ms-appdata:///local/MyImage.png", new AdaptiveImage()
            {
                Source = "ms-appdata:///local/MyImage.png"
            });

            AssertAdaptiveImagePropertyValue("src", "ms-appx:///Assets/MyImage.png", new AdaptiveImage()
            {
                Source = "ms-appx:///Assets/MyImage.png"
            });

            AssertAdaptiveImagePropertyValue("src", "http://msn.com/img.png", new AdaptiveImage()
            {
                Source = "http://msn.com/img.png"
            });

            AssertAdaptiveImagePropertyValue("src", "Assets/MyImage.png", new AdaptiveImage()
            {
                Source = "Assets/MyImage.png"
            });
        }

        [TestMethod]
        public void Test_Adaptive_Image_Source_Null()
        {
            try
            { 
                new AdaptiveImage()
                {
                    Source = null
                };
            }
            catch (ArgumentNullException)
            {
                return;
            }

            Assert.Fail("ArgumentNullException should have been thrown.");
        }

        [TestMethod]
        public void Test_Adaptive_Image_AddImageQuery()
        {
            AssertAdaptiveImagePropertyValue("addImageQuery", "false", new AdaptiveImage()
            {
                AddImageQuery = false
            });

            AssertAdaptiveImagePropertyValue("addImageQuery", "true", new AdaptiveImage()
            {
                AddImageQuery = true
            });
        }

        [TestMethod]
        public void Test_Adaptive_Image_AlternateText()
        {
            AssertAdaptiveImagePropertyValue("alt", "image of puppies", new AdaptiveImage()
            {
                AlternateText = "image of puppies"
            });
        }

        [TestMethod]
        public void Test_Adaptive_Image_HintAlign()
        {
            AssertAdaptiveImageAlign("stretch", AdaptiveImageAlign.Stretch);
            AssertAdaptiveImageAlign("left", AdaptiveImageAlign.Left);
            AssertAdaptiveImageAlign("center", AdaptiveImageAlign.Center);
            AssertAdaptiveImageAlign("right", AdaptiveImageAlign.Right);
        }

        private static void AssertAdaptiveImageAlign(string expectedValue, AdaptiveImageAlign align)
        {
            AssertAdaptiveImagePropertyValue("hint-align", expectedValue, new AdaptiveImage()
            {
                HintAlign = align
            });
        }

        [TestMethod]
        public void Test_Adaptive_Image_HintCrop()
        {
            AssertAdaptiveImageCrop("none", AdaptiveImageCrop.None);
            AssertAdaptiveImageCrop("circle", AdaptiveImageCrop.Circle);
        }

        private static void AssertAdaptiveImageCrop(string expectedValue, AdaptiveImageCrop crop)
        {
            AssertAdaptiveImagePropertyValue("hint-crop", expectedValue, new AdaptiveImage()
            {
                HintCrop = crop
            });
        }

        [TestMethod]
        public void Test_Adaptive_Image_HintRemoveMargin()
        {
            AssertAdaptiveImagePropertyValue("hint-removeMargin", "false", new AdaptiveImage()
            {
                HintRemoveMargin = false
            });

            AssertAdaptiveImagePropertyValue("hint-removeMargin", "true", new AdaptiveImage()
            {
                HintRemoveMargin = true
            });
        }

        [TestMethod]
        public void Test_Adaptive_Image_DefaultNullValues()
        {
            AssertAdaptiveChild("<image src='img.png'/>", new AdaptiveImage()
            {
                Source = "img.png",
                AddImageQuery = null,
                AlternateText = null,
                HintAlign = AdaptiveImageAlign.Default,
                HintCrop = AdaptiveImageCrop.Default,
                HintRemoveMargin = null
            });
        }

        private static void AssertAdaptiveImagePropertyValue(string expectedPropertyName, string expectedPropertyValue, AdaptiveImage image)
        {
            bool addedSource = false;
            if (image.Source == null)
            {
                image.Source = "img.png";
                addedSource = true;
            }

            string xml = $"<image {expectedPropertyName}='{expectedPropertyValue}'";

            if (addedSource)
                xml += " src='img.png'";

            xml += "/>";

            AssertAdaptiveChild(xml, image);
        }

        [TestMethod]
        public void Test_Adaptive_Group_Defaults()
        {
            try
            {
                AssertAdaptiveChild("exception should be thrown since groups need at least one subgroup child", new AdaptiveGroup());
            }
            catch (InvalidOperationException)
            {
                return;
            }

            Assert.Fail("InvalidOperationException should have been thrown.");
        }

        [TestMethod]
        public void Test_Adaptive_Group_OneChild()
        {
            AssertAdaptiveChild("<group><subgroup /></group>", new AdaptiveGroup()
            {
                Children =
                {
                    new AdaptiveSubgroup()
                }
            });
        }

        [TestMethod]
        public void Test_Adaptive_Group_TwoChildren()
        {
            AssertAdaptiveChild("<group><subgroup /><subgroup /></group>", new AdaptiveGroup()
            {
                Children =
                {
                    new AdaptiveSubgroup(),
                    new AdaptiveSubgroup()
                }
            });
        }

        [TestMethod]
        public void Test_Adaptive_Group_ThreeChildren()
        {
            AssertAdaptiveChild("<group><subgroup /><subgroup /><subgroup /></group>", new AdaptiveGroup()
            {
                Children =
                {
                    new AdaptiveSubgroup(),
                    new AdaptiveSubgroup(),
                    new AdaptiveSubgroup()
                }
            });
        }

        [TestMethod]
        public void Test_Adaptive_Subgroup_Defaults()
        {
            AssertAdaptiveSubgroup("<subgroup />", new AdaptiveSubgroup());
        }

        [TestMethod]
        public void Test_Adaptive_Subgroup_HintWeight_MinValue()
        {
            AssertAdaptiveSubgroupProperty("hint-weight", "1", new AdaptiveSubgroup()
            {
                HintWeight = 1
            });
        }

        [TestMethod]
        public void Test_Adaptive_Subgroup_HintWeight_NormalValue()
        {
            AssertAdaptiveSubgroupProperty("hint-weight", "20", new AdaptiveSubgroup()
            {
                HintWeight = 20
            });
        }

        [TestMethod]
        public void Test_Adaptive_Subgroup_HintWeight_MaxValue()
        {
            AssertAdaptiveSubgroupProperty("hint-weight", int.MaxValue.ToString(), new AdaptiveSubgroup()
            {
                HintWeight = int.MaxValue
            });
        }

        [TestMethod]
        public void Test_Adaptive_Subgroup_HintWeight_JustBelowMin()
        {
            try
            {
                AssertAdaptiveSubgroup("exception should be thrown", new AdaptiveSubgroup()
                {
                    HintWeight = 0
                });
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }

            Assert.Fail("ArgumentOutOfRangeException should have been thrown.");
        }

        [TestMethod]
        public void Test_Adaptive_Subgroup_HintWeight_BelowMin()
        {
            try
            {
                AssertAdaptiveSubgroup("exception should be thrown", new AdaptiveSubgroup()
                {
                    HintWeight = -53
                });
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }

            Assert.Fail("ArgumentOutOfRangeException should have been thrown.");
        }

        [TestMethod]
        public void Test_Adaptive_Subgroup_HintTextStacking()
        {
            AssertAdaptiveSubgroupTextStacking("top", AdaptiveSubgroupTextStacking.Top);
            AssertAdaptiveSubgroupTextStacking("center", AdaptiveSubgroupTextStacking.Center);
            AssertAdaptiveSubgroupTextStacking("bottom", AdaptiveSubgroupTextStacking.Bottom);
        }

        private static void AssertAdaptiveSubgroupTextStacking(string expectedValue, AdaptiveSubgroupTextStacking textStacking)
        {
            AssertAdaptiveSubgroupProperty("hint-textStacking", expectedValue, new AdaptiveSubgroup()
            {
                HintTextStacking = textStacking
            });
        }

        [TestMethod]
        public void Test_Adaptive_Subgroup_DefaultNullValues()
        {
            AssertAdaptiveSubgroup("<subgroup />", new AdaptiveSubgroup()
            {
                HintTextStacking = AdaptiveSubgroupTextStacking.Default,
                HintWeight = null
            });
        }

        private static void AssertAdaptiveSubgroupProperty(string expectedPropertyName, string expectedPropertyValue, AdaptiveSubgroup subgroup)
        {
            AssertAdaptiveSubgroup($"<subgroup {expectedPropertyName}='{expectedPropertyValue}' />", subgroup);
        }

        private static void AssertAdaptiveSubgroup(string expectedSubgroupXml, AdaptiveSubgroup subgroup)
        {
            AdaptiveGroup group = new AdaptiveGroup()
            {
                Children =
                {
                    subgroup
                }
            };

            AssertAdaptiveChild("<group>" + expectedSubgroupXml + "</group>", group);
        }

        private static void AssertAdaptiveChild(string expectedAdaptiveChildXml, IAdaptiveChild child)
        {
            AssertAdaptiveChildInToast(expectedAdaptiveChildXml, child);
            AssertAdaptiveChildInTile(expectedAdaptiveChildXml, child);

            // Also assert them within group/subgroup if possible!
            if (child is IAdaptiveSubgroupChild)
            {
                AdaptiveGroup group = new AdaptiveGroup()
                {
                    Children =
                    {
                        new AdaptiveSubgroup()
                        {
                            Children =
                            {
                                child as IAdaptiveSubgroupChild
                            }
                        }
                    }
                };

                string expectedGroupXml = "<group><subgroup>" + expectedAdaptiveChildXml + "</subgroup></group>";

                AssertAdaptiveChildInToast(expectedGroupXml, group);
                AssertAdaptiveChildInTile(expectedGroupXml, group);
            }
        }

        private static void AssertAdaptiveChildInToast(string expectedAdaptiveChildXml, IAdaptiveChild child)
        {
            var binding = new ToastBindingGeneric();

            // If the child isn't text, we need to add a text element so notification is valid
            if (!(child is AdaptiveText))
            {
                binding.Children.Add(new AdaptiveText()
                {
                    Text = "Required text element"
                });

                expectedAdaptiveChildXml = "<text>Required text element</text>" + expectedAdaptiveChildXml;
            }

            binding.Children.Add((IToastBindingGenericChild)child);

            var content = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = binding
                }
            };

            string expectedFinalXml = "<toast><visual><binding template='ToastGeneric'>" + expectedAdaptiveChildXml + "</binding></visual></toast>";

            AssertHelper.AssertToast(expectedFinalXml, content);
        }

        private static void AssertAdaptiveChildInTile(string expectedAdaptiveChildXml, IAdaptiveChild child)
        {
            var content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                            {
                                (ITileBindingContentAdaptiveChild)child
                            }
                        }
                    }
                }
            };

            string expectedFinalXml = "<tile><visual><binding template='TileMedium'>" + expectedAdaptiveChildXml + "</binding></visual></tile>";

            AssertHelper.AssertTile(expectedFinalXml, content);
        }
    }
}
