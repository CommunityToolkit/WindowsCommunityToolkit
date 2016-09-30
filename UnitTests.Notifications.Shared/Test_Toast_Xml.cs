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
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Microsoft.Toolkit.Uwp.Notifications;

namespace UnitTests.Notifications
{
    [TestClass]
    public class Test_Toast_Xml
    {
        [TestMethod]
        public void Test_Toast_XML_Toast_Defaults()
        {
            AssertPayload("<toast/>", new ToastContent());
        }

        [TestMethod]
        public void Test_Toast_XML_Toast_Launch_Value()
        {
            var toast = new ToastContent()
            {
                Launch = "tacos"
            };

            AssertPayload("<toast launch='tacos'/>", toast);
        }

        [TestMethod]
        public void Test_Toast_XML_Toast_ActivationType_Foreground()
        {
            var toast = new ToastContent()
            {
                ActivationType = ToastActivationType.Foreground
            };

            AssertPayload("<toast />", toast);
        }

        [TestMethod]
        public void Test_Toast_XML_Toast_ActivationType_Background()
        {
            var toast = new ToastContent()
            {
                ActivationType = ToastActivationType.Background
            };

            AssertPayload("<toast activationType='background' />", toast);
        }

        [TestMethod]
        public void Test_Toast_XML_Toast_ActivationType_Protocol()
        {
            var toast = new ToastContent()
            {
                ActivationType = ToastActivationType.Protocol
            };

            AssertPayload("<toast activationType='protocol' />", toast);
        }

        [TestMethod]
        public void Test_Toast_XML_Toast_Duration_Short()
        {
            var toast = new ToastContent()
            {
                Duration = ToastDuration.Short
            };

            AssertPayload("<toast />", toast);
        }

        [TestMethod]
        public void Test_Toast_XML_Toast_Duration_Long()
        {
            var toast = new ToastContent()
            {
                Duration = ToastDuration.Long
            };

            AssertPayload("<toast duration='long' />", toast);
        }

        [TestMethod]
        public void Test_ToastV2_Visual_Defaults()
        {
            AssertPayload("<toast></toast>", new ToastContent());
        }

        [TestMethod]
        public void Test_ToastV2_Visual_AddImageQuery_False()
        {
            var visual = new ToastVisual()
            {
                AddImageQuery = false
            };

            AssertVisualPayloadProperties(@"addImageQuery='false'", visual);
        }

        [TestMethod]
        public void Test_ToastV2_Visual_AddImageQuery_True()
        {
            var visual = new ToastVisual()
            {
                AddImageQuery = true
            };

            AssertVisualPayloadProperties(@"addImageQuery=""true""", visual);
        }

        [TestMethod]
        public void Test_ToastV2_Visual_BaseUri_Value()
        {
            var visual = new ToastVisual()
            {
                BaseUri = new Uri("http://msn.com")
            };

            AssertVisualPayloadProperties(@"baseUri=""http://msn.com/""", visual);
        }

        [TestMethod]
        public void Test_ToastV2_Visual_Language_Value()
        {
            var visual = new ToastVisual()
            {
                Language = "en-US"
            };

            AssertVisualPayloadProperties(@"lang=""en-US""", visual);
        }

        [TestMethod]
        public void Test_ToastV2_Visual_AdaptiveText_Defaults()
        {
            AssertAdaptiveText(@"<text/>", new AdaptiveText());
        }

        [TestMethod]
        public void Test_ToastV2_Visual_AdaptiveText_All()
        {
            AssertAdaptiveText(@"<text lang=""en-US"" hint-align=""right"" hint-maxLines=""3"" hint-minLines=""2"" hint-style=""header"" hint-wrap=""true"">Hi, I am a title</text>", new AdaptiveText()
            {
                Text = "Hi, I am a title",
                Language = "en-US",
                HintAlign = AdaptiveTextAlign.Right,
                HintMaxLines = 3,
                HintMinLines = 2,
                HintStyle = AdaptiveTextStyle.Header,
                HintWrap = true
            });
        }

        [TestMethod]
        public void Test_ToastV2_Xml_Attribution()
        {
            var visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "My title"
                        },

                        new AdaptiveText()
                        {
                            Text = "My body 1"
                        }
                    },

                    Attribution = new ToastGenericAttributionText()
                    {
                        Text = "cnn.com"
                    }
                }
            };

            AssertVisualPayload(@"<visual><binding template=""ToastGeneric""><text>My title</text><text>My body 1</text><text placement='attribution'>cnn.com</text></binding></visual>", visual);
        }

        [TestMethod]
        public void Test_ToastV2_Xml_Attribution_Lang()
        {
            var visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "My title"
                        },

                        new AdaptiveText()
                        {
                            Text = "My body 1"
                        }
                    },

                    Attribution = new ToastGenericAttributionText()
                    {
                        Text = "cnn.com",
                        Language = "en-US"
                    }
                }
            };

            AssertVisualPayload(@"<visual><binding template=""ToastGeneric""><text>My title</text><text>My body 1</text><text placement='attribution' lang='en-US'>cnn.com</text></binding></visual>", visual);
        }

        [TestMethod]
        public void Test_ToastV2_BindingGeneric_BaseUri()
        {
            AssertBindingGenericProperty("baseUri", "http://msn.com/images/", new ToastBindingGeneric()
            {
                BaseUri = new Uri("http://msn.com/images/", UriKind.Absolute)
            });
        }

        [TestMethod]
        public void Test_ToastV2_BindingGeneric_AddImageQuery()
        {
            AssertBindingGenericProperty("addImageQuery", "false", new ToastBindingGeneric()
            {
                AddImageQuery = false
            });

            AssertBindingGenericProperty("addImageQuery", "true", new ToastBindingGeneric()
            {
                AddImageQuery = true
            });
        }

        [TestMethod]
        public void Test_ToastV2_BindingGeneric_Language()
        {
            AssertBindingGenericProperty("lang", "en-US", new ToastBindingGeneric()
            {
                Language = "en-US"
            });
        }

        [TestMethod]
        public void Test_ToastV2_BindingGeneric_DefaultNullValues()
        {
            AssertBindingGenericPayload("<binding template='ToastGeneric' />", new ToastBindingGeneric()
            {
                AddImageQuery = null,
                AppLogoOverride = null,
                BaseUri = null,
                Language = null,
                HeroImage = null,
                Attribution = null
            });
        }

        private static void AssertBindingGenericProperty(string expectedPropertyName, string expectedPropertyValue, ToastBindingGeneric binding)
        {
            AssertBindingGenericPayload($"<binding template='ToastGeneric' {expectedPropertyName}='{expectedPropertyValue}'/>", binding);
        }

        [TestMethod]
        public void Test_ToastV2_AppLogo_Crop_None()
        {
            var appLogo = new ToastGenericAppLogo()
            {
                HintCrop = ToastGenericAppLogoCrop.None,
                Source = "img.png"
            };

            AssertAppLogoPayload(@"<image src='img.png' placement=""appLogoOverride"" hint-crop='none'/>", appLogo);
        }

        [TestMethod]
        public void Test_ToastV2_AppLogo_Crop_Circle()
        {
            var appLogo = new ToastGenericAppLogo()
            {
                HintCrop = ToastGenericAppLogoCrop.Circle,
                Source = "img.png"
            };

            AssertAppLogoPayload(@"<image src=""img.png"" placement=""appLogoOverride"" hint-crop=""circle""/>", appLogo);
        }

        [TestMethod]
        public void Test_ToastV2_AppLogo_Source_Defaults()
        {
            var appLogo = new ToastGenericAppLogo()
            {
                Source = "http://xbox.com/Avatar.jpg"
            };

            AssertAppLogoPayload(@"<image placement=""appLogoOverride"" src=""http://xbox.com/Avatar.jpg""/>", appLogo);
        }

        [TestMethod]
        public void Test_ToastV2_AppLogo_Source_Alt()
        {
            var appLogo = new ToastGenericAppLogo()
            {
                Source = "http://xbox.com/Avatar.jpg",
                AlternateText = "alternate"
            };

            AssertAppLogoPayload(@"<image placement=""appLogoOverride"" src=""http://xbox.com/Avatar.jpg"" alt=""alternate""/>", appLogo);
        }

        [TestMethod]
        public void Test_ToastV2_AppLogo_Source_AddImageQuery_False()
        {
            var appLogo = new ToastGenericAppLogo()
            {
                Source = "http://xbox.com/Avatar.jpg",
                AddImageQuery = false
            };

            AssertAppLogoPayload(@"<image placement=""appLogoOverride"" src=""http://xbox.com/Avatar.jpg"" addImageQuery='false'/>", appLogo);
        }

        [TestMethod]
        public void Test_ToastV2_AppLogo_Source_AddImageQuery_True()
        {
            var appLogo = new ToastGenericAppLogo()
            {
                Source = "http://xbox.com/Avatar.jpg",
                AddImageQuery = true
            };

            AssertAppLogoPayload(@"<image placement=""appLogoOverride"" src=""http://xbox.com/Avatar.jpg"" addImageQuery=""true""/>", appLogo);
        }

        [TestMethod]
        public void Test_ToastV2_Xml_HeroImage_Default()
        {
            var hero = new ToastGenericHeroImage();

            try
            {
                AssertHeroImagePayload("<image placement='hero'/>", hero);
            }

            catch (NullReferenceException)
            {
                return;
            }

            Assert.Fail("Exception should have been thrown since Source wasn't provided.");
        }

        [TestMethod]
        public void Test_ToastV2_Xml_HeroImage_WithSource()
        {
            var hero = new ToastGenericHeroImage()
            {
                Source = "http://food.com/peanuts.jpg"
            };

            AssertHeroImagePayload("<image placement='hero' src='http://food.com/peanuts.jpg'/>", hero);
        }

        [TestMethod]
        public void Test_ToastV2_Xml_HeroImage_Alt()
        {
            var hero = new ToastGenericHeroImage()
            {
                Source = "http://food.com/peanuts.jpg",
                AlternateText = "peanuts"
            };

            AssertHeroImagePayload("<image placement='hero' src='http://food.com/peanuts.jpg' alt='peanuts'/>", hero);
        }

        [TestMethod]
        public void Test_ToastV2_Xml_HeroImage_AddImageQuery()
        {
            var hero = new ToastGenericHeroImage()
            {
                Source = "http://food.com/peanuts.jpg",
                AddImageQuery = true
            };

            AssertHeroImagePayload("<image placement='hero' src='http://food.com/peanuts.jpg' addImageQuery='true'/>", hero);
        }

        [TestMethod]
        public void Test_ToastV2_Xml_HeroImage_AllProperties()
        {
            var hero = new ToastGenericHeroImage()
            {
                Source = "http://food.com/peanuts.jpg",
                AddImageQuery = true,
                AlternateText = "peanuts"
            };

            AssertHeroImagePayload("<image placement='hero' src='http://food.com/peanuts.jpg' addImageQuery='true' alt='peanuts'/>", hero);
        }

        private static ToastContent GenerateFromVisual(ToastVisual visual)
        {
            return new ToastContent()
            {
                Visual = visual
            };
        }

        /// <summary>
        /// Used for testing properties of visual without needing to specify the Generic binding
        /// </summary>
        /// <param name="expectedVisualProperties"></param>
        /// <param name="visual"></param>
        private static void AssertVisualPayloadProperties(string expectedVisualProperties, ToastVisual visual)
        {
            visual.BindingGeneric = new ToastBindingGeneric();

            AssertVisualPayload("<visual " + expectedVisualProperties + "><binding template='ToastGeneric'></binding></visual>", visual);
        }

        private static void AssertBindingGenericPayload(string expectedBindingXml, ToastBindingGeneric binding)
        {
            AssertVisualPayload("<visual>" + expectedBindingXml + "</visual>", new ToastVisual()
            {
                BindingGeneric = binding
            });
        }

        private static void AssertAdaptiveText(string expectedAdaptiveTextXml, AdaptiveText text)
        {
            AssertBindingGenericPayload("<binding template='ToastGeneric'>" + expectedAdaptiveTextXml + "</binding>", new ToastBindingGeneric()
            {
                Children =
                {
                    text
                }
            });
        }

        private static void AssertAppLogoPayload(string expectedAppLogoXml, ToastGenericAppLogo appLogo)
        {
            AssertVisualPayload(@"<visual><binding template=""ToastGeneric"">" + expectedAppLogoXml + "</binding></visual>", new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    AppLogoOverride = appLogo
                }
            });
        }

        private static void AssertHeroImagePayload(string expectedHeroXml, ToastGenericHeroImage heroImage)
        {
            AssertVisualPayload(@"<visual><binding template=""ToastGeneric"">" + expectedHeroXml + "</binding></visual>", new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    HeroImage = heroImage
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Audio_Defaults()
        {
            var audio = new ToastAudio();

            AssertAudioPayload("<audio />", audio);
        }

        [TestMethod]
        public void Test_Toast_Xml_Audio_Loop_False()
        {
            var audio = new ToastAudio()
            {
                Loop = false
            };

            AssertAudioPayload("<audio />", audio);
        }

        [TestMethod]
        public void Test_Toast_Xml_Audio_Loop_True()
        {
            var audio = new ToastAudio()
            {
                Loop = true
            };

            AssertAudioPayload("<audio loop='true'/>", audio);
        }

        [TestMethod]
        public void Test_Toast_Xml_Audio_Silent_False()
        {
            var audio = new ToastAudio()
            {
                Silent = false
            };

            AssertAudioPayload("<audio/>", audio);
        }

        [TestMethod]
        public void Test_Toast_Xml_Audio_Silent_True()
        {
            var audio = new ToastAudio()
            {
                Silent = true
            };

            AssertAudioPayload("<audio silent='true'/>", audio);
        }

        [TestMethod]
        public void Test_Toast_Xml_Audio_Src_Value()
        {
            var audio = new ToastAudio()
            {
                Src = new Uri("ms-appx:///Assets/audio.mp3")
            };

            AssertAudioPayload("<audio src='ms-appx:///Assets/audio.mp3'/>", audio);
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_SnoozeAndDismiss()
        {
            AssertActionsPayload("<actions hint-systemCommands='SnoozeAndDismiss'/>", new ToastActionsSnoozeAndDismiss());
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_Custom_Defaults()
        {
            AssertActionsPayload("<actions/>", new ToastActionsCustom());
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_TextBoxAndButton()
        {
            AssertActionsPayload("<actions><input id='tb1' type='text'/><action content='Click me!' arguments='clickArgs'/></actions>", new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("Click me!", "clickArgs")
                },

                Inputs =
                {
                    new ToastTextBox("tb1")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_TwoTextBoxes()
        {
            AssertActionsPayload("<actions><input id='tb1' type='text'/><input id='tb2' type='text'/></actions>", new ToastActionsCustom()
            {
                Inputs =
                {
                    new ToastTextBox("tb1"),
                    new ToastTextBox("tb2")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_FiveTextBoxes()
        {
            AssertActionsPayload("<actions><input id='tb1' type='text'/><input id='tb2' type='text'/><input id='tb3' type='text'/><input id='tb4' type='text'/><input id='tb5' type='text'/></actions>", new ToastActionsCustom()
            {
                Inputs =
                {
                    new ToastTextBox("tb1"),
                    new ToastTextBox("tb2"),
                    new ToastTextBox("tb3"),
                    new ToastTextBox("tb4"),
                    new ToastTextBox("tb5")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_SixTextBoxes()
        {
            try
            {
                new ToastActionsCustom()
                {
                    Inputs =
                    {
                        new ToastTextBox("tb1"),
                        new ToastTextBox("tb2"),
                        new ToastTextBox("tb3"),
                        new ToastTextBox("tb4"),
                        new ToastTextBox("tb5"),
                        new ToastTextBox("tb6")
                    }
                };
            }
            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_SelectionAndButton()
        {
            AssertActionsPayload("<actions><input id='s1' type='selection'><selection id='1' content='First'/><selection id='2' content='Second'/></input><action content='Click me!' arguments='clickArgs'/></actions>", new ToastActionsCustom()
            {
                Inputs =
                {
                    new ToastSelectionBox("s1")
                    {
                        Items =
                        {
                            new ToastSelectionBoxItem("1", "First"),
                            new ToastSelectionBoxItem("2", "Second")
                        }
                    }
                },

                Buttons =
                {
                    new ToastButton("Click me!", "clickArgs")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_TwoButtons()
        {
            AssertActionsPayload("<actions><action content='Button 1' arguments='1'/><action content='Button 2' arguments='2'/></actions>", new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("Button 1", "1"),
                    new ToastButton("Button 2", "2")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_FiveButtons()
        {
            AssertActionsPayload("<actions><action content='Button 1' arguments='1'/><action content='Button 2' arguments='2'/><action content='Button 3' arguments='3'/><action content='Button 4' arguments='4'/><action content='Button 5' arguments='5'/></actions>", new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("Button 1", "1"),
                    new ToastButton("Button 2", "2"),
                    new ToastButton("Button 3", "3"),
                    new ToastButton("Button 4", "4"),
                    new ToastButton("Button 5", "5")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_SixButtons()
        {
            try
            {
                new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Button 1", "1"),
                        new ToastButton("Button 2", "2"),
                        new ToastButton("Button 3", "3"),
                        new ToastButton("Button 4", "4"),
                        new ToastButton("Button 5", "5"),
                        new ToastButton("Button 6", "6")
                    }
                };
            }
            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_SixTotal()
        {
            try
            {
                AssertActionsPayload("doesn't matter", new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Button 1", "1"),
                        new ToastButton("Button 2", "2"),
                        new ToastButton("Button 3", "3"),
                        new ToastButton("Button 4", "4"),
                        new ToastButton("Button 5", "5")
                    },

                    ContextMenuItems =
                    {
                        new ToastContextMenuItem("Menu item 1", "1")
                    }
                });
            }

            catch (InvalidOperationException)
            {
                return;
            }

            Assert.Fail("Exception should have been thrown, only 5 actions are allowed.");
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_TwoContextMenuItems()
        {

            AssertActionsPayload("<actions><action placement='contextMenu' content='Menu item 1' arguments='1'/><action placement='contextMenu' content='Menu item 2' arguments='2'/></actions>", new ToastActionsCustom()
            {
                ContextMenuItems =
                {
                    new ToastContextMenuItem("Menu item 1", "1"),
                    new ToastContextMenuItem("Menu item 2", "2")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_FiveContextMenuItems()
        {

            AssertActionsPayload("<actions><action placement='contextMenu' content='Menu item 1' arguments='1'/><action placement='contextMenu' content='Menu item 2' arguments='2'/><action placement='contextMenu' content='Menu item 3' arguments='3'/><action placement='contextMenu' content='Menu item 4' arguments='4'/><action placement='contextMenu' content='Menu item 5' arguments='5'/></actions>", new ToastActionsCustom()
            {
                ContextMenuItems =
                {
                    new ToastContextMenuItem("Menu item 1", "1"),
                    new ToastContextMenuItem("Menu item 2", "2"),
                    new ToastContextMenuItem("Menu item 3", "3"),
                    new ToastContextMenuItem("Menu item 4", "4"),
                    new ToastContextMenuItem("Menu item 5", "5")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_SixContextMenuItems()
        {
            try
            {
                AssertActionsPayload("doesn't matter", new ToastActionsCustom()
                {
                    ContextMenuItems =
                    {
                        new ToastContextMenuItem("Menu item 1", "1"),
                        new ToastContextMenuItem("Menu item 2", "2"),
                        new ToastContextMenuItem("Menu item 3", "3"),
                        new ToastContextMenuItem("Menu item 4", "4"),
                        new ToastContextMenuItem("Menu item 5", "5"),
                        new ToastContextMenuItem("Menu item 6", "6")
                    }
                });
            }

            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }



        [TestMethod]
        public void Test_Toast_Xml_ActionsSnoozeDismiss_TwoContextMenuItems()
        {
            AssertActionsPayload("<actions hint-systemCommands='SnoozeAndDismiss'><action placement='contextMenu' content='Menu item 1' arguments='1'/><action placement='contextMenu' content='Menu item 2' arguments='2'/></actions>", new ToastActionsSnoozeAndDismiss()
            {
                ContextMenuItems =
                {
                    new ToastContextMenuItem("Menu item 1", "1"),
                    new ToastContextMenuItem("Menu item 2", "2")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_ActionsSnoozeDismiss_FiveContextMenuItems()
        {
            AssertActionsPayload("<actions hint-systemCommands='SnoozeAndDismiss'><action placement='contextMenu' content='Menu item 1' arguments='1'/><action placement='contextMenu' content='Menu item 2' arguments='2'/><action placement='contextMenu' content='Menu item 3' arguments='3'/><action placement='contextMenu' content='Menu item 4' arguments='4'/><action placement='contextMenu' content='Menu item 5' arguments='5'/></actions>", new ToastActionsSnoozeAndDismiss()
            {
                ContextMenuItems =
                {
                    new ToastContextMenuItem("Menu item 1", "1"),
                    new ToastContextMenuItem("Menu item 2", "2"),
                    new ToastContextMenuItem("Menu item 3", "3"),
                    new ToastContextMenuItem("Menu item 4", "4"),
                    new ToastContextMenuItem("Menu item 5", "5")
                }
            });
        }

        [TestMethod]
        public void Test_Toast_Xml_Actions_ActionsSnoozeDismiss_SixContextMenuItems()
        {
            try
            {
                AssertActionsPayload("doesn't matter", new ToastActionsSnoozeAndDismiss()
                {
                    ContextMenuItems =
                    {
                        new ToastContextMenuItem("Menu item 1", "1"),
                        new ToastContextMenuItem("Menu item 2", "2"),
                        new ToastContextMenuItem("Menu item 3", "3"),
                        new ToastContextMenuItem("Menu item 4", "4"),
                        new ToastContextMenuItem("Menu item 5", "5"),
                        new ToastContextMenuItem("Menu item 6", "6")
                    }
                });
            }

            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_Button_Defaults()
        {
            ToastButton button = new ToastButton("my content", "myArgs");

            AssertButtonPayload("<action content='my content' arguments='myArgs' />", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_Button_NullContent()
        {
            try
            {
                new ToastButton(null, "args");
            }
            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_Button_NullArguments()
        {
            try
            {
                new ToastButton("content", null);
            }
            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_Button_ActivationType_Foreground()
        {
            ToastButton button = new ToastButton("my content", "myArgs")
            {
                ActivationType = ToastActivationType.Foreground
            };

            AssertButtonPayload("<action content='my content' arguments='myArgs' />", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_Button_ActivationType_Background()
        {
            ToastButton button = new ToastButton("my content", "myArgs")
            {
                ActivationType = ToastActivationType.Background
            };

            AssertButtonPayload("<action content='my content' arguments='myArgs' activationType='background' />", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_Button_ActivationType_Protocol()
        {
            ToastButton button = new ToastButton("my content", "myArgs")
            {
                ActivationType = ToastActivationType.Protocol
            };

            AssertButtonPayload("<action content='my content' arguments='myArgs' activationType='protocol' />", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_Button_ImageUri_Value()
        {
            ToastButton button = new ToastButton("my content", "myArgs")
            {
                ImageUri = "Assets/button.png"
            };

            AssertButtonPayload("<action content='my content' arguments='myArgs' imageUri='Assets/button.png' />", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_Button_TextBoxId_Value()
        {
            ToastButton button = new ToastButton("my content", "myArgs")
            {
                TextBoxId = "myTextBox"
            };

            AssertButtonPayload("<action content='my content' arguments='myArgs' hint-inputId='myTextBox' />", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_ButtonSnooze_Defaults()
        {
            ToastButtonSnooze button = new ToastButtonSnooze();

            AssertButtonPayload("<action activationType='system' arguments='snooze' content=''/>", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_ButtonSnooze_CustomContent()
        {
            ToastButtonSnooze button = new ToastButtonSnooze("my snooze");

            AssertButtonPayload("<action activationType='system' arguments='snooze' content='my snooze'/>", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_ButtonSnooze_SelectionId()
        {
            ToastButtonSnooze button = new ToastButtonSnooze()
            {
                SelectionBoxId = "snoozeId"
            };

            AssertButtonPayload("<action activationType='system' arguments='snooze' content='' hint-inputId='snoozeId'/>", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_ButtonDismiss_Defaults()
        {
            ToastButtonDismiss button = new ToastButtonDismiss();

            AssertButtonPayload("<action activationType='system' arguments='dismiss' content=''/>", button);
        }

        [TestMethod]
        public void Test_Toast_Xml_ButtonDismiss_CustomContent()
        {
            ToastButtonDismiss button = new ToastButtonDismiss("my dismiss");

            AssertButtonPayload("<action activationType='system' arguments='dismiss' content='my dismiss'/>", button);
        }
        
        [TestMethod]
        public void Test_Toast_Xml_ContextMenuItem_Defaults()
        {
            ToastContextMenuItem item = new ToastContextMenuItem("content", "args");

            AssertContextMenuItemPayload("<action placement='contextMenu' content='content' arguments='args'/>", item);
        }

        [TestMethod]
        public void Test_Toast_Xml_ContextMenuItem_NullContent()
        {
            try
            {
                new ToastContextMenuItem(null, "args");
            }

            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_ContextMenuItem_NullArguments()
        {
            try
            {
                new ToastContextMenuItem("content", null);
            }

            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_ContextMenuItem_ActivationType_Foreground()
        {
            ToastContextMenuItem item = new ToastContextMenuItem("content", "args")
            {
                ActivationType = ToastActivationType.Foreground
            };

            AssertContextMenuItemPayload("<action placement='contextMenu' content='content' arguments='args'/>", item);
        }

        [TestMethod]
        public void Test_Toast_Xml_ContextMenuItem_ActivationType_Background()
        {
            ToastContextMenuItem item = new ToastContextMenuItem("content", "args")
            {
                ActivationType = ToastActivationType.Background
            };

            AssertContextMenuItemPayload("<action placement='contextMenu' content='content' arguments='args' activationType='background'/>", item);
        }

        [TestMethod]
        public void Test_Toast_Xml_ContextMenuItem_ActivationType_Protocol()
        {
            ToastContextMenuItem item = new ToastContextMenuItem("content", "args")
            {
                ActivationType = ToastActivationType.Protocol
            };

            AssertContextMenuItemPayload("<action placement='contextMenu' content='content' arguments='args' activationType='protocol'/>", item);
        }

        [TestMethod]
        public void Test_Toast_Xml_TextBox_Defaults()
        {
            var textBox = new ToastTextBox("myId");

            AssertInputPayload("<input id='myId' type='text' />", textBox);
        }

        [TestMethod]
        public void Test_Toast_Xml_TextBox_DefaultTextInput_Value()
        {
            var textBox = new ToastTextBox("myId")
            {
                DefaultInput = "Default text input"
            };

            AssertInputPayload("<input id='myId' type='text' defaultInput='Default text input' />", textBox);
        }

        [TestMethod]
        public void Test_Toast_Xml_TextBox_PlaceholderContent_Value()
        {
            var textBox = new ToastTextBox("myId")
            {
                PlaceholderContent = "My placeholder content"
            };

            AssertInputPayload("<input id='myId' type='text' placeHolderContent='My placeholder content' />", textBox);
        }

        [TestMethod]
        public void Test_Toast_Xml_TextBox_Title_Value()
        {
            var textBox = new ToastTextBox("myId")
            {
                Title = "My title"
            };

            AssertInputPayload("<input id='myId' type='text' title='My title' />", textBox);
        }

        [TestMethod]
        public void Test_Toast_Xml_TextBox_NullId()
        {
            try
            {
                new ToastTextBox(null);
            }
            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_TextBox_EmptyId()
        {
            var textBox = new ToastTextBox("");

            AssertInputPayload("<input id='' type='text' />", textBox);
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBox_Defaults()
        {
            var selectionBox = new ToastSelectionBox("myId");

            AssertInputPayload("<input id='myId' type='selection' />", selectionBox);
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBox_EmptyId()
        {
            var selectionBox = new ToastSelectionBox("");

            AssertInputPayload("<input id='' type='selection' />", selectionBox);
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBox_NullId()
        {
            try
            {
                new ToastSelectionBox(null);
            }
            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBox_DefaultSelectionBoxItemId_Value()
        {
            var selectionBox = new ToastSelectionBox("myId")
            {
                DefaultSelectionBoxItemId = "2"
            };

            AssertInputPayload("<input id='myId' type='selection' defaultInput='2' />", selectionBox);
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBox_Title_Value()
        {
            var selectionBox = new ToastSelectionBox("myId")
            {
                Title = "My title"
            };

            AssertInputPayload("<input id='myId' type='selection' title='My title' />", selectionBox);
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBoxItem()
        {
            var selectionBoxItem = new ToastSelectionBoxItem("myId", "My content");

            AssertSelectionPayload("<selection id='myId' content='My content' />", selectionBoxItem);
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBoxItem_NullId()
        {
            try
            {
                new ToastSelectionBoxItem(null, "My content");
            }
            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBoxItem_NullContent()
        {
            try
            {
                new ToastSelectionBoxItem("myId", null);
            }
            catch { return; }

            Assert.Fail("Exception should have been thrown.");
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBoxItem_EmptyId()
        {
            var selectionBoxItem = new ToastSelectionBoxItem("", "My content");

            AssertSelectionPayload("<selection id='' content='My content' />", selectionBoxItem);
        }

        [TestMethod]
        public void Test_Toast_Xml_SelectionBoxItem_EmptyContent()
        {
            var selectionBoxItem = new ToastSelectionBoxItem("myId", "");

            AssertSelectionPayload("<selection id='myId' content='' />", selectionBoxItem);
        }

        private static void AssertSelectionPayload(string expectedSelectionXml, ToastSelectionBoxItem selectionItem)
        {
            AssertInputPayload("<input id='myId' type='selection'>" + expectedSelectionXml + "</input>", new ToastSelectionBox("myId")
            {
                Items = { selectionItem }
            });
        }

        private static void AssertInputPayload(string expectedInputXml, IToastInput textBox)
        {
            AssertActionsPayload("<actions>" + expectedInputXml + "</actions>", new ToastActionsCustom()
            { 
                Inputs = { textBox }
            });
        }

        private static void AssertButtonPayload(string expectedButtonXml, IToastButton button)
        {
            AssertActionsPayload("<actions>" + expectedButtonXml + "</actions>", new ToastActionsCustom()
            {
                Buttons = { button }
            });
        }
        
        private static void AssertContextMenuItemPayload(string expectedContextMenuItemXml, ToastContextMenuItem item)
        {
            AssertActionsPayload("<actions>" + expectedContextMenuItemXml + "</actions>", new ToastActionsCustom()
            {
                ContextMenuItems = { item }
            });
        }

        private static void AssertActionsPayload(string expectedActionsXml, IToastActions actions)
        {
            AssertPayload("<toast>" + expectedActionsXml + "</toast>", new ToastContent()
            {
                Actions = actions
            });
        }

        private static void AssertAudioPayload(string expectedAudioXml, ToastAudio audio)
        {
            AssertPayload("<toast>" + expectedAudioXml + "</toast>", new ToastContent()
            {
                Audio = audio
            });
        }

        private static void AssertVisualPayload(string expectedVisualXml, ToastVisual visual)
        {
            AssertPayload("<toast>" + expectedVisualXml + "</toast>", new ToastContent()
            {
                Visual = visual
            });
        }

        private static void AssertPayload(string expectedXml, ToastContent toast)
        {
            AssertHelper.AssertToast(expectedXml, toast);
        }
    }
}
