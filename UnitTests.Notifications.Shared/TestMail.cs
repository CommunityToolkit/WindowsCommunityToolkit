using System;
#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Microsoft.Windows.Toolkit.Notifications;

namespace UnitTests.Notifications
{
    [TestClass]
    public class TestMail
    {
        private const string FIRST_FROM = "Jennifer Parker";
        private const string FIRST_SUBJECT = "Photos from our trip";
        private const string FIRST_BODY = "Check out these awesome photos I took while in New Zealand!";

        private const string SECOND_FROM = "Steve Bosniak";
        private const string SECOND_SUBJECT = "Build 2015 Dinner";
        private const string SECOND_BODY = "Want to go out for dinner after Build tonight?";

        [TestCategory("EndToEnd/Mail")]
        [TestMethod]
        public void TestMailTile()
        {
            TileBinding small = new TileBinding()
            {
                Content = new TileBindingContentIconic()
                {
                    Icon = new TileBasicImage() { Source = "Assets\\Mail.png" }
                }
            };


            TileBinding medium = new TileBinding()
            {
                Branding = TileBranding.Logo,

                Content = new TileBindingContentAdaptive()
                {
                    Children =
                    {
                        GenerateFirstMessage(false),
                        GenerateSpacer(),
                        GenerateSecondMessage(false)
                    }
                }
            };


            TileBinding wideAndLarge = new TileBinding()
            {
                Branding = TileBranding.NameAndLogo,

                Content = new TileBindingContentAdaptive()
                {
                    Children =
                    {
                        GenerateFirstMessage(true),
                        GenerateSpacer(),
                        GenerateSecondMessage(true)
                    }
                }
            };



            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileSmall = small,
                    TileMedium = medium,
                    TileWide = wideAndLarge,
                    TileLarge = wideAndLarge
                }
            };

            string expectedXml = $@"<?xml version=""1.0"" encoding=""utf-8""?><tile><visual><binding template=""TileSquare71x71IconWithBadge""><image id=""1"" src=""Assets\Mail.png"" /></binding>";


            // Medium
            expectedXml += @"<binding template=""TileMedium"" branding=""logo"">";
            expectedXml += GenerateXmlGroups(false);
            expectedXml += "</binding>";


            // Wide
            expectedXml += @"<binding template=""TileWide"" branding=""nameAndLogo"">";
            expectedXml += GenerateXmlGroups(true);
            expectedXml += "</binding>";


            // Large
            expectedXml += @"<binding template=""TileLarge"" branding=""nameAndLogo"">";
            expectedXml += GenerateXmlGroups(true);
            expectedXml += "</binding>";

            expectedXml += "</visual></tile>";


            

            AssertHelper.AssertTile(expectedXml, content);
        }


        private static string GenerateXmlGroups(bool makeLarge)
        {
            return GenerateXmlGroup(FIRST_FROM, FIRST_SUBJECT, FIRST_BODY, makeLarge) + "<text />" + GenerateXmlGroup(SECOND_FROM, SECOND_SUBJECT, SECOND_BODY, makeLarge);
        }

        private static string GenerateXmlGroup(string from, string subject, string body, bool makeLarge)
        {
            string xml = "<group><subgroup><text";

            if (makeLarge)
                xml += @" hint-style=""subtitle""";
            else
                xml += " hint-style='caption'";

            xml += $@">{from}</text><text hint-style=""captionSubtle"">{subject}</text><text hint-style=""captionSubtle"">{body}</text></subgroup></group>";

            return xml;
        }

        private static AdaptiveText GenerateSpacer()
        {
            return new AdaptiveText();
        }

        private static AdaptiveGroup GenerateFirstMessage(bool makeLarge)
        {
            return GenerateMessage(FIRST_FROM, FIRST_SUBJECT, FIRST_BODY, makeLarge);
        }

        private static AdaptiveGroup GenerateSecondMessage(bool makeLarge)
        {
            return GenerateMessage(SECOND_FROM, SECOND_SUBJECT, SECOND_BODY, makeLarge);
        }

        private static AdaptiveGroup GenerateMessage(string from, string subject, string body, bool makeLarge)
        {
            return new AdaptiveGroup()
            {
                Children =
                {
                    new AdaptiveSubgroup()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = from,
                                HintStyle = makeLarge ? AdaptiveTextStyle.Subtitle : AdaptiveTextStyle.Caption
                            },

                            new AdaptiveText()
                            {
                                Text = subject,
                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                            },

                            new AdaptiveText()
                            {
                                Text = body,
                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }
                        }
                    }
                }
            };
        }
    }
}
