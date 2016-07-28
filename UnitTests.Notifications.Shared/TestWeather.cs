#if WINDOWS_UWP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Notifications
{
    [TestClass]
    public class TestWeather
    {
        private const string IMAGE_MOSTLY_CLOUDY = "Assets\\Tiles\\Mostly Cloudy.png";
        private const string IMAGE_SUNNY = "Assets\\Tiles\\Sunny.png";
        private const string IMAGE_CLOUDY = "Assets\\Tiles\\Cloudy.png";

        private const string BACKGROUND_IMAGE_MOSTLY_CLOUDY = "Assets\\Tiles\\Mostly Cloudy-Background.jpg";

        [TestMethod]
        public void TestWeatherTile()
        {
            var backgroundImage = BACKGROUND_IMAGE_MOSTLY_CLOUDY;
            int overlay = 30;

            TileBindingContentAdaptive smallContent = new TileBindingContentAdaptive()
            {
                TextStacking = TileTextStacking.Center,
                BackgroundImage = new TileBackgroundImage() { Source = backgroundImage, HintOverlay = overlay },
                Children =
                {
                    new AdaptiveText()
                    {
                        Text = "Mon",
                        HintStyle = AdaptiveTextStyle.Body,
                        HintAlign = AdaptiveTextAlign.Center
                    },

                    new AdaptiveText()
                    {
                        Text = "63°",
                        HintStyle = AdaptiveTextStyle.Base,
                        HintAlign = AdaptiveTextAlign.Center
                    }
                }
            };


            TileBindingContentAdaptive mediumContent = new TileBindingContentAdaptive()
            {
                BackgroundImage = new TileBackgroundImage() { Source = backgroundImage, HintOverlay = overlay },
                Children =
                {
                    new AdaptiveGroup()
                    {
                        Children =
                        {
                            GenerateMediumSubgroup("Mon", IMAGE_MOSTLY_CLOUDY, 63, 42),

                            GenerateMediumSubgroup("Tue", IMAGE_CLOUDY, 57, 38)
                        }
                    }
                }
            };



            TileBindingContentAdaptive wideContent = new TileBindingContentAdaptive()
            {
                BackgroundImage = new TileBackgroundImage() { Source = backgroundImage, HintOverlay = overlay },
                Children =
                {
                    new AdaptiveGroup()
                    {
                        Children =
                        {
                            GenerateWideSubgroup("Mon", IMAGE_MOSTLY_CLOUDY, 63, 42),

                            GenerateWideSubgroup("Tue", IMAGE_CLOUDY, 57, 38),

                            GenerateWideSubgroup("Wed", IMAGE_SUNNY, 59, 43),

                            GenerateWideSubgroup("Thu", IMAGE_SUNNY, 62, 42),

                            GenerateWideSubgroup("Fri", IMAGE_SUNNY, 71, 66)
                        }
                    }
                }
            };




            TileBindingContentAdaptive largeContent = new TileBindingContentAdaptive()
            {
                BackgroundImage = new TileBackgroundImage() { Source = backgroundImage, HintOverlay = overlay },
                Children =
                {
                    new AdaptiveGroup()
                    {
                        Children =
                        {
                            new AdaptiveSubgroup()
                            {
                                HintWeight = 30,
                                Children =
                                {
                                    new AdaptiveImage() { Source = IMAGE_MOSTLY_CLOUDY }
                                }
                            },

                            new AdaptiveSubgroup()
                            {
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = "Monday",
                                        HintStyle = AdaptiveTextStyle.Base
                                    },

                                    new AdaptiveText()
                                    {
                                        Text = "63° / 42°"
                                    },

                                    new AdaptiveText()
                                    {
                                        Text = "20% chance of rain",
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                    },

                                    new AdaptiveText()
                                    {
                                        Text = "Winds 5 mph NE",
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                    }
                                }
                            }
                        }
                    },

                    // For spacing
                    new AdaptiveText(),

                    new AdaptiveGroup()
                    {
                        Children =
                        {
                            GenerateLargeSubgroup("Tue", IMAGE_CLOUDY, 57, 38),

                            GenerateLargeSubgroup("Wed", IMAGE_SUNNY, 59, 43),

                            GenerateLargeSubgroup("Thu", IMAGE_SUNNY, 62, 42),

                            GenerateLargeSubgroup("Fri", IMAGE_SUNNY, 71, 66)
                        }
                    }
                }
            };




            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    DisplayName = "Seattle",

                    TileSmall = new TileBinding()
                    {
                        Content = smallContent
                    },

                    TileMedium = new TileBinding()
                    {
                        Content = mediumContent,
                        Branding = TileBranding.Name
                    },

                    TileWide = new TileBinding()
                    {
                        Content = wideContent,
                        Branding = TileBranding.NameAndLogo
                    },

                    TileLarge = new TileBinding()
                    {
                        Content = largeContent,
                        Branding = TileBranding.NameAndLogo
                    }
                }
            };

            string expectedPayload = $@"<?xml version=""1.0"" encoding=""utf-8""?><tile><visual displayName=""Seattle""><binding template=""TileSmall"" hint-textStacking=""center"">{GenerateStringBackgroundImage()}<text hint-align=""center"" hint-style=""body"">Mon</text><text hint-align=""center"" hint-style=""base"">63°</text></binding><binding template=""TileMedium"" branding=""name"">{GenerateStringBackgroundImage()}<group>";

            // Medium tile subgroups
            expectedPayload += GenerateStringMediumSubgroup("Mon", IMAGE_MOSTLY_CLOUDY, 63, 42);
            expectedPayload += GenerateStringMediumSubgroup("Tue", IMAGE_CLOUDY, 57, 38);

            expectedPayload += "</group></binding>";


            // Wide tile
            expectedPayload += @"<binding template=""TileWide"" branding=""nameAndLogo"">";
            expectedPayload += GenerateStringBackgroundImage();
            expectedPayload += "<group>";

            // Wide tile subgroups
            expectedPayload += GenerateStringWideSubgroup("Mon", IMAGE_MOSTLY_CLOUDY, 63, 42);
            expectedPayload += GenerateStringWideSubgroup("Tue", IMAGE_CLOUDY, 57, 38);
            expectedPayload += GenerateStringWideSubgroup("Wed", IMAGE_SUNNY, 59, 43);
            expectedPayload += GenerateStringWideSubgroup("Thu", IMAGE_SUNNY, 62, 42);
            expectedPayload += GenerateStringWideSubgroup("Fri", IMAGE_SUNNY, 71, 66);

            expectedPayload += "</group></binding>";



            // Large tile
            expectedPayload += @"<binding template=""TileLarge"" branding=""nameAndLogo"">";
            expectedPayload += GenerateStringBackgroundImage();
            expectedPayload += $@"<group><subgroup hint-weight=""30""><image src=""{IMAGE_MOSTLY_CLOUDY}"" /></subgroup><subgroup><text hint-style=""base"">Monday</text><text>63° / 42°</text><text hint-style=""captionSubtle"">20% chance of rain</text><text hint-style=""captionSubtle"">Winds 5 mph NE</text></subgroup></group>";

            expectedPayload += "<text />";
            expectedPayload += "<group>";

            // Large tile subgroups
            expectedPayload += GenerateStringLargeSubgroup("Tue", IMAGE_CLOUDY, 57, 38);
            expectedPayload += GenerateStringLargeSubgroup("Wed", IMAGE_SUNNY, 59, 43);
            expectedPayload += GenerateStringLargeSubgroup("Thu", IMAGE_SUNNY, 62, 42);
            expectedPayload += GenerateStringLargeSubgroup("Fri", IMAGE_SUNNY, 71, 66);

            expectedPayload += "</group></binding></visual></tile>";
            

            AssertHelper.AssertTile(expectedPayload, content);
        }

        private static string GenerateStringBackgroundImage()
        {
            return $@"<image src=""{BACKGROUND_IMAGE_MOSTLY_CLOUDY}"" placement=""background"" hint-overlay=""30""/>";
        }

        private static string GenerateStringMediumSubgroup(string day, string image, int high, int low)
        {
            return $@"<subgroup><text hint-align=""center"">{day}</text><image src=""{image}"" hint-removeMargin=""true"" /><text hint-align=""center"">{high}°</text><text hint-align=""center"" hint-style=""captionSubtle"">{low}°</text></subgroup>";
        }

        private static string GenerateStringWideSubgroup(string day, string image, int high, int low)
        {
            return $@"<subgroup hint-weight=""1""><text hint-align=""center"">{day}</text><image src=""{image}"" hint-removeMargin=""true"" /><text hint-align=""center"">{high}°</text><text hint-align=""center"" hint-style=""captionSubtle"">{low}°</text></subgroup>";
        }

        private static string GenerateStringLargeSubgroup(string day, string image, int high, int low)
        {
            return $@"<subgroup hint-weight=""1""><text hint-align=""center"">{day}</text><image src=""{image}"" /><text hint-align=""center"">{high}°</text><text hint-align=""center"" hint-style=""captionSubtle"">{low}°</text></subgroup>";
        }

        private static AdaptiveSubgroup GenerateMediumSubgroup(string day, string image, int high, int low)
        {
            return new AdaptiveSubgroup()
            {
                Children =
                {
                    new AdaptiveText()
                    {
                        Text = day,
                        HintAlign = AdaptiveTextAlign.Center
                    },

                    new AdaptiveImage()
                    {
                        Source = image,
                        HintRemoveMargin = true
                    },

                    new AdaptiveText()
                    {
                        Text = high + "°",
                        HintAlign = AdaptiveTextAlign.Center
                    },

                    new AdaptiveText()
                    {
                        Text = low + "°",
                        HintAlign = AdaptiveTextAlign.Center,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
            };
        }

        private static AdaptiveSubgroup GenerateWideSubgroup(string day, string image, int high, int low)
        {
            var subgroup = GenerateMediumSubgroup(day, image, high, low);

            subgroup.HintWeight = 1;

            return subgroup;
        }

        private static AdaptiveSubgroup GenerateLargeSubgroup(string day, string image, int high, int low)
        {
            var subgroup = GenerateWideSubgroup(day, image, high, low);

            (subgroup.Children[1] as AdaptiveImage).HintRemoveMargin = null;

            return subgroup;
        }
    }
}
