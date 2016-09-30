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

using System;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using NotificationsVisualizerLibrary;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class LiveTilePage : Page
    {
        private TileContent _tileContent;

        public LiveTilePage()
        {
            InitializeComponent();
            Initialize();
        }

        public static TileContent GenerateTileContent(string username, string avatarLogoSource)
        {
            return new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = GenerateTileBindingMedium(username, avatarLogoSource),
                    TileWide = GenerateTileBindingWide(username, avatarLogoSource),
                    TileLarge = GenerateTileBindingLarge(username, avatarLogoSource)
                }
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }
        }

        private static TileBinding GenerateTileBindingMedium(string username, string avatarLogoSource)
        {
            return new TileBinding()
            {
                Content = new TileBindingContentAdaptive()
                {
                    PeekImage = new TilePeekImage()
                    {
                        Source = avatarLogoSource,
                        HintCrop = TilePeekImageCrop.Circle
                    },

                    TextStacking = TileTextStacking.Center,

                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "Hi,",
                            HintAlign = AdaptiveTextAlign.Center,
                            HintStyle = AdaptiveTextStyle.Base
                        },

                        new AdaptiveText()
                        {
                            Text = username,
                            HintAlign = AdaptiveTextAlign.Center,
                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                        }
                    }
                }
            };
        }

        private static TileBinding GenerateTileBindingWide(string username, string avatarLogoSource)
        {
            return new TileBinding()
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
                                            Source = avatarLogoSource,
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
                                            Text = username,
                                            HintStyle = AdaptiveTextStyle.SubtitleSubtle
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private static TileBinding GenerateTileBindingLarge(string username, string avatarLogoSource)
        {
            return new TileBinding()
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
                                new AdaptiveSubgroup()
                                {
                                    HintWeight = 1
                                },

                                // We surround the image by two subgroups so that it doesn't take the full width
                                new AdaptiveSubgroup()
                                {
                                    HintWeight = 2,
                                    Children =
                                    {
                                        new AdaptiveImage()
                                        {
                                            Source = avatarLogoSource,
                                            HintCrop = AdaptiveImageCrop.Circle
                                        }
                                    }
                                },

                                new AdaptiveSubgroup()
                                {
                                    HintWeight = 1
                                }
                            }
                        },

                        new AdaptiveText()
                        {
                            Text = "Hi,",
                            HintAlign = AdaptiveTextAlign.Center,
                            HintStyle = AdaptiveTextStyle.Title
                        },

                        new AdaptiveText()
                        {
                            Text = username,
                            HintAlign = AdaptiveTextAlign.Center,
                            HintStyle = AdaptiveTextStyle.SubtitleSubtle
                        }
                    }
                }
            };
        }

        private void ButtonPinTile_Click(object sender, RoutedEventArgs e)
        {
            PinTile();
        }

        private async void PinTile()
        {
            SecondaryTile tile = new SecondaryTile(DateTime.Now.Ticks.ToString())
            {
                DisplayName = "Xbox",
                Arguments = "args"
            };
            tile.VisualElements.Square150x150Logo = Constants.Square150x150Logo;
            tile.VisualElements.Wide310x150Logo = Constants.Wide310x150Logo;
            tile.VisualElements.Square310x310Logo = Constants.Square310x310Logo;
            tile.VisualElements.ShowNameOnSquare150x150Logo = true;
            tile.VisualElements.ShowNameOnSquare310x310Logo = true;
            tile.VisualElements.ShowNameOnWide310x150Logo = true;

            if (!await tile.RequestCreateAsync())
            {
                return;
            }

            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId).Update(new TileNotification(_tileContent.GetXml()));
        }

        private void Initialize()
        {
            // Generate the tile notification content
            _tileContent = GenerateTileContent("MasterHip", "Assets/Photos/Owl.png");

            // Prepare and update the preview tiles
            var previewTiles = new PreviewTile[] { MediumPreviewTile, WidePreviewTile, LargePreviewTile };
            foreach (var tile in previewTiles)
            {
                tile.DisplayName = "Xbox";
                tile.VisualElements.BackgroundColor = Constants.ApplicationBackgroundColor;
                tile.VisualElements.ShowNameOnSquare150x150Logo = true;
                tile.VisualElements.ShowNameOnSquare310x310Logo = true;
                tile.VisualElements.ShowNameOnWide310x150Logo = true;
                tile.VisualElements.Square44x44Logo = Constants.Square44x44Logo;
                tile.VisualElements.Square150x150Logo = Constants.Square150x150Logo;
                tile.VisualElements.Wide310x150Logo = Constants.Wide310x150Logo;
                tile.VisualElements.Square310x310Logo = Constants.Square310x310Logo;
                var dontWait = tile.UpdateAsync(); // Commit changes (no need to await)

                tile.CreateTileUpdater().Update(new TileNotification(_tileContent.GetXml()));
            }
        }
    }
}
