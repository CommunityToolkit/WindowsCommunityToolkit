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

using Microsoft.Toolkit.Uwp.UI.Helpers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp.Styles
{
    public static class ThemeInjector
    {
        private static ResourceDictionary darkTheme;

        private static ResourceDictionary lightTheme;

        public static void InjectThemeResources(ResourceDictionary dict)
        {
            var themes = dict.MergedDictionaries[0];
            darkTheme = themes.ThemeDictionaries["Dark"] as ResourceDictionary;
            lightTheme = themes.ThemeDictionaries["Light"] as ResourceDictionary;

            if (VisualHelpers.SupportsFluentAcrylic)
            {
                AddAcrylic(new ThemeAcrylic
                {
                    Name = "Background-AboutPage-SidePane",
                    DarkAcrylic = new AcrylicBrush
                    {
                        TintColor = VisualHelpers.ColorFromHex("#FF333333"),
                        TintOpacity = 0.8,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop
                    },
                    LightAcrylic = new AcrylicBrush
                    {
                        TintColor = Colors.White,
                        TintOpacity = 0.8,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    }
                });

                AddAcrylic(new ThemeAcrylic
                {
                    Names = new[] { "Brush-SampleInfo-Background", "Commands-Background" },
                    DarkAcrylic = new AcrylicBrush
                    {
                        TintColor = VisualHelpers.ColorFromHex("#FF111111"),
                        TintOpacity = 0.7,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop
                    },
                    LightAcrylic = new AcrylicBrush
                    {
                        TintColor = Colors.White,
                        TintOpacity = 0.6,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    }
                });

                AddAcrylic(new ThemeAcrylic
                {
                    Name = "Brush-Sample-HostAcrylic",
                    DarkAcrylic = new AcrylicBrush
                    {
                        TintColor = Colors.Black,
                        TintOpacity = 0.5,
                        BackgroundSource = AcrylicBackgroundSource.HostBackdrop
                    },
                    LightAcrylic = new AcrylicBrush
                    {
                        TintColor = Colors.White,
                        TintOpacity = 0.3,
                        BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    }
                });

                AddAcrylic(new ThemeAcrylic
                {
                    Name = "Brush-Sample-AppAcrylic",
                    DarkAcrylic = new AcrylicBrush
                    {
                        TintColor = Colors.Black,
                        TintOpacity = 0.6,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop
                    },
                    LightAcrylic = new AcrylicBrush
                    {
                        TintColor = Colors.White,
                        TintOpacity = 0.4,
                        BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    }
                });
            }
        }

        private static void AddAcrylic(ThemeAcrylic resource)
        {
            var light = resource?.LightAcrylic;
            var dark = resource?.DarkAcrylic;

            var names = resource.Names;
            if (names == null)
            {
                names = new string[] { resource.Name };
            }

            foreach (var res in names)
            {
                if (light != null)
                {
                    if (light.FallbackColor == null && lightTheme[res] is SolidColorBrush brush)
                    {
                        light.FallbackColor = brush.Color;
                    }

                    lightTheme[res] = light;
                }

                if (dark != null)
                {
                    if (dark.FallbackColor == null && darkTheme[res] is SolidColorBrush brush)
                    {
                        dark.FallbackColor = brush.Color;
                    }

                    darkTheme[res] = dark;
                }
            }
        }

        private class ThemeAcrylic
        {
            public string Name { get; set; }

            public string[] Names { get; set; }

            public AcrylicBrush LightAcrylic { get; set; }

            public AcrylicBrush DarkAcrylic { get; set; }
        }
    }
}