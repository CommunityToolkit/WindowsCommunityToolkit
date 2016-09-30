﻿// ******************************************************************
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

namespace UnitTests.Notifications
{
    [TestClass]
    public class TextXboxModern
    {
        [TestCategory("EndToEnd/XboxModern")]
        [TestMethod]
        public void TestXboxModernTile()
        {
            TileBinding medium = new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    TextStacking = TileTextStacking.Center,

                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "Hi,",
                            HintStyle = AdaptiveTextStyle.Base,
                            HintAlign = AdaptiveTextAlign.Center
                        },

                        new AdaptiveText()
                        {
                            Text = "MasterHip",
                            HintStyle = AdaptiveTextStyle.CaptionSubtle,
                            HintAlign = AdaptiveTextAlign.Center
                        }
                    }
                }
            };


            TileBinding wide = new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    Children =
                    {
                        new AdaptiveGroup()
                        {
                            Children =
                            {
                                new AdaptiveSubgroup()
                                {
                                    HintWeight = 33,
                                    Children =
                                    {
                                        new AdaptiveImage()
                                        {
                                            Source = "http://xbox.com/MasterHip/profile.jpg",
                                            HintCrop = AdaptiveImageCrop.Circle
                                        }
                                    }
                                },

                                new AdaptiveSubgroup()
                                {
                                    HintTextStacking = AdaptiveSubgroupTextStacking.Center,
                                    Children =
                                    {
                                        new AdaptiveText()
                                        {
                                            Text = "Hi,",
                                            HintStyle = AdaptiveTextStyle.Title
                                        },

                                        new AdaptiveText()
                                        {
                                            Text = "MasterHip",
                                            HintStyle = AdaptiveTextStyle.SubtitleSubtle
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };




            TileBinding large = new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    TextStacking = TileTextStacking.Center,
                    Children =
                    {
                        new AdaptiveGroup()
                        {
                            Children =
                            {
                                new AdaptiveSubgroup() { HintWeight = 1 },
                                new AdaptiveSubgroup()
                                {
                                    HintWeight = 2,
                                    Children =
                                    {
                                        new AdaptiveImage()
                                        {
                                            Source = "http://xbox.com/MasterHip/profile.jpg",
                                            HintCrop = AdaptiveImageCrop.Circle
                                        }
                                    }
                                },
                                new AdaptiveSubgroup() { HintWeight = 1 }
                            }
                        },

                        new AdaptiveText()
                        {
                            Text = "Hi,",
                            HintStyle = AdaptiveTextStyle.Title,
                            HintAlign = AdaptiveTextAlign.Center
                        },

                        new AdaptiveText()
                        {
                            Text = "MasterHip",
                            HintStyle = AdaptiveTextStyle.SubtitleSubtle,
                            HintAlign = AdaptiveTextAlign.Center
                        }
                    }
                }
            };



            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    Branding = TileBranding.NameAndLogo,

                    TileMedium = medium,
                    TileWide = wide,
                    TileLarge = large
                }
            };



            string expectedXml = $@"<?xml version=""1.0"" encoding=""utf-8""?><tile><visual branding=""nameAndLogo"">";

            // Medium
            expectedXml += @"<binding template=""TileMedium"" hint-textStacking=""center""><text hint-align=""center"" hint-style=""base"">Hi,</text><text hint-align=""center"" hint-style=""captionSubtle"">MasterHip</text></binding>";


            // Wide
            expectedXml += @"<binding template=""TileWide""><group><subgroup hint-weight=""33""><image src=""http://xbox.com/MasterHip/profile.jpg"" hint-crop=""circle"" /></subgroup><subgroup hint-textStacking=""center""><text hint-style=""title"">Hi,</text><text hint-style=""subtitleSubtle"">MasterHip</text></subgroup></group></binding>";


            // Large
            expectedXml += @"<binding template=""TileLarge"" hint-textStacking=""center""><group><subgroup hint-weight=""1"" /><subgroup hint-weight=""2""><image src=""http://xbox.com/MasterHip/profile.jpg"" hint-crop=""circle"" /></subgroup><subgroup hint-weight=""1"" /></group><text hint-align=""center"" hint-style=""title"">Hi,</text><text hint-align=""center"" hint-style=""subtitleSubtle"">MasterHip</text></binding>";


            expectedXml += "</visual></tile>";

            
            AssertHelper.AssertTile(expectedXml, content);
        }
    }
}
