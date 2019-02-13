// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Notifications
{
#if !WINRT
    [TestClass]
    public class TestToastContentBuilder
    {
        [TestMethod]
        public void AddCustomTimeStampTest_WithCustomTimeStamp_ReturnSelfWithCustomTimeStampAdded()
        {
            // Arrange
            DateTime testCustomTimeStamp = DateTime.UtcNow;

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddCustomTimeStamp(testCustomTimeStamp);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testCustomTimeStamp, builder.Content.DisplayTimestamp);
        }

        [TestMethod]
        public void AddHeaderTest_WithExpectedArgs_ReturnSelfWithHeaderAdded()
        {
            // Arrange
            string testToastHeaderId = "Test Header ID";
            string testToastTitle = "Test Toast Title";
            string testToastArguments = "Test Toast Arguments";

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddHeader(testToastHeaderId, testToastTitle, testToastArguments);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testToastHeaderId, builder.Content.Header.Id);
            Assert.AreEqual(testToastTitle, builder.Content.Header.Title);
            Assert.AreEqual(testToastArguments, builder.Content.Header.Arguments);
        }

        [TestMethod]
        public void AddToastActivationInfoTest_WithExpectedArgs_ReturnSelfWithActivationInfoAdded()
        {
            // Arrange
            string testToastLaunchArugments = "Test Toast Launch Args";
            ToastActivationType testToastActivationType = ToastActivationType.Background;

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddToastActivationInfo(testToastLaunchArugments, testToastActivationType);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testToastLaunchArugments, builder.Content.Launch);
            Assert.AreEqual(testToastActivationType, builder.Content.ActivationType);
        }

        [TestMethod]
        public void SetToastDurationTest_WithCustomToastDuration_ReturnSelfWithCustomToastDurationSet()
        {
            // Arrange
            ToastDuration testToastDuration = ToastDuration.Long;

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.SetToastDuration(testToastDuration);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testToastDuration, builder.Content.Duration);
        }

        [TestMethod]
        public void SetToastScenarioTest_WithCustomToastScenario_ReturnSelfWithCustomToastScenarioSet()
        {
            // Arrange
            ToastScenario testToastScenario = ToastScenario.Default;

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.SetToastScenario(testToastScenario);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testToastScenario, builder.Content.Scenario);
        }

        [TestMethod]
        public void AddAudioTest_WithAudioUriOnly_ReturnSelfWithCustomAudioAdded()
        {
            // Arrange
            Uri testAudioUriSrc = new Uri("C:/justatesturi.mp3");

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddAudio(testAudioUriSrc);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testAudioUriSrc.OriginalString, builder.Content.Audio.Src.OriginalString);
        }

        [TestMethod]
        public void AddAudioTest_WithFullArgs_ReturnSelfWithCustomAudioAddedWithAllOptionsSet()
        {
            // Arrange
            Uri testAudioUriSrc = new Uri("C:/justatesturi.mp3");
            bool testToastAudioLoop = true;
            bool testToastAudioSilent = true;

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddAudio(testAudioUriSrc, testToastAudioLoop, testToastAudioSilent);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testAudioUriSrc.OriginalString, builder.Content.Audio.Src.OriginalString);
            Assert.AreEqual(testToastAudioLoop, builder.Content.Audio.Loop);
            Assert.AreEqual(testToastAudioSilent, builder.Content.Audio.Silent);
        }

        [TestMethod]
        public void AddAttributionTextTest_WithSimpleText_ReturnSelfWithCustomAttributionTextAdded()
        {
            // Arrange
            string testAttributionText = "Test Attribution Text";

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddAttributionText(testAttributionText);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testAttributionText, builder.Content.Visual.BindingGeneric.Attribution.Text);
        }

        [TestMethod]
        public void AddAttributionTextTest_WithTextAndLanguage_ReturnSelfWithCustomAttributionTextAndLanguageAdded()
        {
            // Arrange
            string testAttributionText = "Test Attribution Text";
            string testAttributionTextLanguage = "en-US";

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddAttributionText(testAttributionText, testAttributionTextLanguage);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testAttributionText, builder.Content.Visual.BindingGeneric.Attribution.Text);
            Assert.AreEqual(testAttributionTextLanguage, builder.Content.Visual.BindingGeneric.Attribution.Language);
        }

        [TestMethod]
        public void AddAppLogoOverrideTest_WithLogoUriOnly_ReturnSelfWithCustomLogoAdded()
        {
            // Arrange
            Uri testAppLogoUriSrc = new Uri("C:/justatesturi.jpg");

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddAppLogoOverride(testAppLogoUriSrc);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testAppLogoUriSrc.OriginalString, builder.Content.Visual.BindingGeneric.AppLogoOverride.Source);
        }

        [TestMethod]
        public void AddAppLogoOverrideTest_WithCustomLogoAndFullOptions_ReturnSelfWithCustomLogoAndOptionsAdded()
        {
            // Arrange
            Uri testAppLogoUriSrc = new Uri("C:/justatesturi.jpg");
            ToastGenericAppLogoCrop testCropOption = ToastGenericAppLogoCrop.Circle;
            string testLogoAltText = "Test Logo Alt Text";
            bool testLogoAddImageQuery = true;

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddAppLogoOverride(testAppLogoUriSrc, testCropOption, testLogoAltText, testLogoAddImageQuery);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testAppLogoUriSrc.OriginalString, builder.Content.Visual.BindingGeneric.AppLogoOverride.Source);
            Assert.AreEqual(testCropOption, builder.Content.Visual.BindingGeneric.AppLogoOverride.HintCrop);
            Assert.AreEqual(testLogoAltText, builder.Content.Visual.BindingGeneric.AppLogoOverride.AlternateText);
            Assert.AreEqual(testLogoAddImageQuery, builder.Content.Visual.BindingGeneric.AppLogoOverride.AddImageQuery);
        }

        [TestMethod]
        public void AddHeroImageTest_WithHeroImageUriOnly_ReturnSelfWithHeroImageAdded()
        {
            // Arrange
            Uri testHeroImageUriSrc = new Uri("C:/justatesturi.jpg");

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddHeroImage(testHeroImageUriSrc);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testHeroImageUriSrc.OriginalString, builder.Content.Visual.BindingGeneric.HeroImage.Source);
        }

        [TestMethod]
        public void AddHeroImageTest_WithHeroImageUriAndFullOptions_ReturnSelfWithHeroImageAndOptionsAdded()
        {
            // Arrange
            Uri testHeroImageUriSrc = new Uri("C:/justatesturi.jpg");
            string testHeroImageAltText = "Test Hero Image Text";
            bool testHeroImageAddImageQuery = true;

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddHeroImage(testHeroImageUriSrc, testHeroImageAltText, testHeroImageAddImageQuery);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testHeroImageUriSrc.OriginalString, builder.Content.Visual.BindingGeneric.HeroImage.Source);
            Assert.AreEqual(testHeroImageAltText, builder.Content.Visual.BindingGeneric.HeroImage.AlternateText);
            Assert.AreEqual(testHeroImageAddImageQuery, builder.Content.Visual.BindingGeneric.HeroImage.AddImageQuery);
        }

        [TestMethod]
        public void AddInlineImageTest_WithInlineImageUriOnly_ReturnSelfWithInlineImageAdded()
        {
            // Arrange
            Uri testInlineImageUriSrc = new Uri("C:/justatesturi.jpg");

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddInlineImage(testInlineImageUriSrc);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.AreEqual(testInlineImageUriSrc.OriginalString, (builder.Content.Visual.BindingGeneric.Children.First() as AdaptiveImage).Source);
        }

        [TestMethod]
        public void AddInlineImageTest_WithInlineImageAndFullOptions_ReturnSelfWithInlineImageAndOptionsAdded()
        {
            // Arrange
            Uri testInlineImageUriSrc = new Uri("C:/justatesturi.jpg");
            string testInlineImageAltText = "Test Inline Image Text";
            bool testInlineImageAddImageQuery = true;

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();
            ToastContentBuilder anotherReference = builder.AddInlineImage(testInlineImageUriSrc, testInlineImageAltText, testInlineImageAddImageQuery);

            // Assert
            Assert.AreSame(builder, anotherReference);

            var image = (builder.Content.Visual.BindingGeneric.Children.First() as AdaptiveImage);

            Assert.AreEqual(testInlineImageUriSrc.OriginalString, image.Source);
            Assert.AreEqual(testInlineImageAltText, image.AlternateText);
            Assert.AreEqual(testInlineImageAddImageQuery, image.AddImageQuery);
        }

        [TestMethod]
        public void AddProgressBarTest_WithoutInputArgs_ReturnSelfWithNonIndeterminateBindableProgressBarAdded()
        {
            // Arrange
            ToastContentBuilder builder = new ToastContentBuilder();

            // Act
            ToastContentBuilder anotherReference = builder.AddProgressBar();

            // Assert
            Assert.AreSame(builder, anotherReference);
            var progressBar = (builder.Content.Visual.BindingGeneric.Children.First() as AdaptiveProgressBar);

            Assert.IsNotNull(progressBar.Title.BindingName);
            Assert.IsNotNull(progressBar.Value.BindingName);
            Assert.IsNotNull(progressBar.ValueStringOverride.BindingName);
            Assert.IsNotNull(progressBar.Status.BindingName);
        }

        [TestMethod]
        public void AddProgressBarTest_WithFixedPropertiesAndDeterminateValue_ReturnSelfWithFixedValueAndPropertiesProgressBarAdded()
        {
            // Arrange
            string testProgressBarTitle = "Test Progress Bar Title";
            double testProgressBarValue = 0.25;
            string testValueStringOverride = "Test Value String Override";
            string testProgressBarStatus = "Test Progress Bar Status";

            ToastContentBuilder builder = new ToastContentBuilder();

            // Act
            ToastContentBuilder anotherReference = builder.AddProgressBar(testProgressBarTitle, testProgressBarValue, false, testValueStringOverride, testProgressBarStatus);

            // Assert
            Assert.AreSame(builder, anotherReference);
            var progressBar = (builder.Content.Visual.BindingGeneric.Children.First() as AdaptiveProgressBar);

            Assert.IsNull(progressBar.Title.BindingName);
            Assert.AreEqual(testProgressBarTitle, (string)progressBar.Title);

            Assert.IsNull(progressBar.Value.BindingName);
            Assert.AreEqual(testProgressBarValue, ((AdaptiveProgressBarValue)progressBar.Value).Value);

            Assert.IsNull(progressBar.ValueStringOverride.BindingName);
            Assert.AreEqual(testValueStringOverride, (string)progressBar.ValueStringOverride);

            Assert.IsNull(progressBar.Status.BindingName);
            Assert.AreEqual(testProgressBarStatus, (string)progressBar.Status);
        }

        [TestMethod]
        public void AddProgressBarTest_WithIndeterminateValue_ReturnSelfWithIndeterminateProgressBarAdded()
        {
            // Arrange
            ToastContentBuilder builder = new ToastContentBuilder();

            // Act
            ToastContentBuilder anotherReference = builder.AddProgressBar(isIndeterminate: true);

            // Assert
            Assert.AreSame(builder, anotherReference);
            var progressBar = (builder.Content.Visual.BindingGeneric.Children.First() as AdaptiveProgressBar);

            Assert.IsTrue(((AdaptiveProgressBarValue)progressBar.Value).IsIndeterminate);
        }

        [TestMethod]
        public void AddTextTest_WithSimpleText_ReturnSelfWithTextAdded()
        {
            // Arrange
            string testText = "Test Text";
            ToastContentBuilder builder = new ToastContentBuilder();

            // Act
            ToastContentBuilder anotherReference = builder.AddText(testText);

            // Assert
            Assert.AreSame(builder, anotherReference);
            var text = (builder.Content.Visual.BindingGeneric.Children.First() as AdaptiveText);

            Assert.AreEqual(testText, (string)text.Text);
        }

        [TestMethod]
        public void AddTextTest_WithMultipleTexts_ReturnSelfWithAllTextsAdded()
        {
            // Arrange
            string testText1 = "Test Header";
            string testText2 = "Test Text";
            string testText3 = "Test Text Again";
            ToastContentBuilder builder = new ToastContentBuilder();

            // Act
            ToastContentBuilder anotherReference = builder.AddText(testText1)
                .AddText(testText2)
                .AddText(testText3);

            // Assert
            Assert.AreSame(builder, anotherReference);
            var texts = builder.Content.Visual.BindingGeneric.Children.Take(3).Cast<AdaptiveText>().ToList();

            Assert.AreEqual(testText1, (string)(texts[0].Text));
            Assert.AreEqual(testText2, (string)(texts[1].Text));
            Assert.AreEqual(testText3, (string)(texts[2].Text));
        }

        [TestMethod]
        public void AddTextTest_WithTextAndFullOptions_ReturnSelfWithTextAndAllOptionsAdded()
        {
            // Arrange
            string testText = "Test Text";
            AdaptiveTextStyle testStyle = AdaptiveTextStyle.Header;
            bool testWrapHint = true;
            int testHintMaxLine = 2;
            int testHintMinLine = 1;
            AdaptiveTextAlign testAlign = AdaptiveTextAlign.Auto;
            string testLanguage = "en-US";

            ToastContentBuilder builder = new ToastContentBuilder();

            // Act
            ToastContentBuilder anotherReference = builder.AddText(testText, testStyle, testWrapHint, testHintMaxLine, testHintMinLine, testAlign, testLanguage);

            // Assert
            Assert.AreSame(builder, anotherReference);
            var text = (builder.Content.Visual.BindingGeneric.Children.First() as AdaptiveText);

            Assert.AreEqual(testText, (string)text.Text);
            Assert.AreEqual(testStyle, text.HintStyle);
            Assert.AreEqual(testWrapHint, text.HintWrap);
            Assert.AreEqual(testHintMaxLine, text.HintMaxLines);
            Assert.AreEqual(testHintMinLine, text.HintMinLines);
            Assert.AreEqual(testAlign, text.HintAlign);
            Assert.AreEqual(testLanguage, text.Language);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTextTest_WithMoreThan4LinesOfText_ThrowInvalidOperationException()
        {
            // Arrange
            string testText1 = "Test Header";
            string testText2 = "Test Text";
            string testText3 = "Test Text Again";
            string testText4 = "Test Text Again x2";
            ToastContentBuilder builder = new ToastContentBuilder();

            // Act
            ToastContentBuilder anotherReference = builder.AddText(testText1)
                .AddText(testText2)
                .AddText(testText3)
                .AddText(testText4);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddTextTest_WithMaxLinesValueLargerThan2_ThrowArgumentOutOfRangeException()
        {
            // Arrange
            string testText1 = "Test Header";
            ToastContentBuilder builder = new ToastContentBuilder();

            // Act
            ToastContentBuilder anotherReference = builder.AddText(testText1, hintMaxLines: 3);
        }

        [TestMethod]
        public void AddVisualChildTest_AddCustomVisual_ReturnSelfWithCustomVisualAdded()
        {
            // Arrange
            // Taken from : https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/adaptive-interactive-toasts#adaptive-content
            AdaptiveGroup group = new AdaptiveGroup()
            {
                Children =
                {
                    new AdaptiveSubgroup()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "52 attendees",
                                HintStyle = AdaptiveTextStyle.Base
                            },
                            new AdaptiveText()
                            {
                                Text = "23 minute drive",
                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }
                        }
                    },
                    new AdaptiveSubgroup()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "1 Microsoft Way",
                                HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                HintAlign = AdaptiveTextAlign.Right
                            },
                            new AdaptiveText()
                            {
                                Text = "Bellevue, WA 98008",
                                HintStyle = AdaptiveTextStyle.CaptionSubtle,
                                HintAlign = AdaptiveTextAlign.Right
                            }
                        }
                    }
                }
            };
            ToastContentBuilder builder = new ToastContentBuilder();

            // Act
            ToastContentBuilder anotherReference = builder.AddVisualChild(group);

            // Assert
            Assert.AreSame(builder, anotherReference);
            Assert.IsInstanceOfType(builder.Content.Visual.BindingGeneric.Children.First(), typeof(AdaptiveGroup));
        }
    }

#endif
}
