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
    [TestClass]
    public class TestToastContentBuilder
    {
        public void ToastContentBuilderTest()
        {
            // Arrange
            string testToastHeaderId = "Test Header ID";
            string testToastTitle = "Test Toast Title";
            string testToastArguments = "Test Toast Arguments";

            string testToastLaunchArugments = "Test Toast Launch Args";
            ToastActivationType testToastActivationType = ToastActivationType.Background;
            ToastDuration testToastDuration = ToastDuration.Long;
            ToastScenario testToastScenario = ToastScenario.Default;

            Uri testAudioUriSrc = new Uri("ms-appx:///just-a-test-uri.mp3");
            string testAttributionText = "Test Attribution Text";
            Uri testAppLogoOverrideUri = new Uri("ms-appx:///just-a-test-uri.jpg");
            Uri testHeroImageUri = new Uri("ms-appx:///just-a-test-uri.png");
            Uri testInlineImageUri = new Uri("ms-appx:///just-a-test-uri.jpeg");
            string testText = "Test inline text";

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

            string testButtonContent = "Test Button Content";
            ToastActivationType testButtonActivationType = ToastActivationType.Foreground;
            string testButtonLaunchArgs = "Test Toast Button Launch Args";

            string testInputTextBoxId = "Test Input TextBox Id";

            string testInputTextBoxButtonContent = "Test Input TextBox Button Content";
            ToastActivationType testInputTextBoxButtonActivationType = ToastActivationType.Foreground;
            string testInputTextBoxButtonLaunchArgs = "Test Input TextBox Button Launch Args";
            Uri testInputTextBoxButtonImageUri = new Uri("ms-appx:///just-a-test-uri.bmp");

            string testComboBoxId = "Test Combo Box Id";
            string testComboBoxTitle = "Test ComboBox Title";

            var testComboBoxOption1 = ("TestComboBoxOption1", "TestComboBoxOption1Content");
            var testComboBoxOption2 = ("TestComboBoxOption2", "TestComboBoxOption2Content");

            // Act
            ToastContentBuilder builder = new ToastContentBuilder();

            builder.AddCustomTimeStamp(DateTime.Today)
                .AddHeader(testToastHeaderId, testToastTitle, testToastArguments)
                .AddToastActivationInfo(testToastLaunchArugments, testToastActivationType)
                .SetToastDuration(testToastDuration)
                .SetToastScenario(testToastScenario)
                .AddAudio(testAudioUriSrc)
                .AddAttributionText(testAttributionText)
                .AddAppLogoOverride(testAppLogoOverrideUri)
                .AddHeroImage(testHeroImageUri)
                .AddInlineImage(testInlineImageUri)
                .AddProgressBar()
                .AddText(testText)
                .AddVisualChild(group)
                .AddButton(testButtonContent, testButtonActivationType, testButtonLaunchArgs)
                .AddInputTextBox(testInputTextBoxId)
                .AddInputTextBoxButton(testInputTextBoxId, testInputTextBoxButtonContent, testInputTextBoxButtonActivationType, testInputTextBoxButtonLaunchArgs, testInputTextBoxButtonImageUri)
                .AddComboBox(testComboBoxId, testComboBoxTitle, null, testComboBoxOption1, testComboBoxOption2);

            // Assert
        }

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
    }
}
