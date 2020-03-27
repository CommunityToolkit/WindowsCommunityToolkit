// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if WINDOWS_UWP
using Windows.UI.Notifications;
#endif

namespace Microsoft.Toolkit.Uwp.Notifications
{
#if !WINRT
    /// <summary>
    /// Builder class used to create <see cref="ToastContent"/>
    /// </summary>
    public partial class ToastContentBuilder
    {
        private ToastVisual Visual
        {
            get
            {
                if (Content.Visual == null)
                {
                    Content.Visual = new ToastVisual();
                    Content.Visual.BindingGeneric = new ToastBindingGeneric();
                }

                return Content.Visual;
            }
        }

        private ToastGenericAppLogo AppLogoOverrideUri
        {
            get
            {
                return Visual.BindingGeneric.AppLogoOverride;
            }

            set
            {
                Visual.BindingGeneric.AppLogoOverride = value;
            }
        }

        private ToastGenericAttributionText AttributionText
        {
            get
            {
                return Visual.BindingGeneric.Attribution;
            }

            set
            {
                Visual.BindingGeneric.Attribution = value;
            }
        }

        private ToastGenericHeroImage HeroImage
        {
            get
            {
                return Visual.BindingGeneric.HeroImage;
            }

            set
            {
                Visual.BindingGeneric.HeroImage = value;
            }
        }

        private IList<IToastBindingGenericChild> VisualChildren
        {
            get
            {
                return Visual.BindingGeneric.Children;
            }
        }

#if WINDOWS_UWP

        /// <summary>
        /// Create an instance of NotificationData that can be used to update toast that has a progress bar.
        /// </summary>
        /// <param name="toast">Instance of ToastContent that contain progress bars that need to be updated</param>
        /// <param name="index">Index of the progress bar (0-based) that this notification data is updating in the case that toast has multiple progress bars. Default to 0.</param>
        /// <param name="title">Title of the progress bar.</param>
        /// <param name="value">Value of the progress bar.</param>
        /// <param name="valueStringOverride">An optional string to be displayed instead of the default percentage string. If this isn't provided, something like "70%" will be displayed.</param>
        /// <param name="status"> A status string, which is displayed underneath the progress bar on the left. Default to empty.</param>
        /// <param name="sequence">A sequence number to prevent out-of-order updates, or assign 0 to indicate "always update".</param>
        /// <returns>An instance of NotificationData that can be used to update the toast.</returns>
        public static NotificationData CreateProgressBarData(ToastContent toast, int index = 0, string title = default(string), double? value = null, string valueStringOverride = default(string), string status = default(string), uint sequence = 0)
        {
            var progressBar = toast.Visual.BindingGeneric.Children.Where(c => c is AdaptiveProgressBar).ElementAt(index) as AdaptiveProgressBar;
            if (progressBar == null)
            {
                throw new ArgumentException(nameof(toast), "Given toast does not have any progress bar");
            }

            NotificationData data = new NotificationData();
            data.SequenceNumber = sequence;

            if (progressBar.Title is BindableString bindableTitle && title != default(string))
            {
                data.Values[bindableTitle.BindingName] = title;
            }

            if (progressBar.Value is BindableProgressBarValue bindableProgressValue && value != null)
            {
                data.Values[bindableProgressValue.BindingName] = value.ToString();
            }

            if (progressBar.ValueStringOverride is BindableString bindableValueStringOverride && valueStringOverride != default(string))
            {
                data.Values[bindableValueStringOverride.BindingName] = valueStringOverride;
            }

            if (progressBar.Status is BindableString bindableStatus && status != default(string))
            {
                data.Values[bindableStatus.BindingName] = status;
            }

            return data;
        }

#endif

        /// <summary>
        /// Add an Attribution Text to be displayed on the toast.
        /// </summary>
        /// <param name="text">Text to be displayed as Attribution Text</param>
        /// <param name="language">The target locale of the XML payload, specified as a BCP-47 language tags such as "en-US" or "fr-FR".</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddAttributionText(string text, string language = default(string))
        {
            AttributionText = new ToastGenericAttributionText()
            {
                Text = text
            };

            if (language != default(string))
            {
                AttributionText.Language = language;
            }

            return this;
        }

        /// <summary>
        /// Override the app logo with custom image of choice that will be displayed on the toast.
        /// </summary>
        /// <param name="uri">The URI of the image. Can be from your application package, application data, or the internet. Internet images must be less than 200 KB in size.</param>
        /// <param name="hintCrop">Specify how the image should be cropped.</param>
        /// <param name="alternateText">A description of the image, for users of assistive technologies.</param>
        /// <param name="addImageQuery">A value whether Windows is allowed to append a query string to the image URI supplied in the Tile notification.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddAppLogoOverride(Uri uri, ToastGenericAppLogoCrop? hintCrop = null, string alternateText = default(string), bool? addImageQuery = default(bool?))
        {
            AppLogoOverrideUri = new ToastGenericAppLogo()
            {
                Source = uri.OriginalString
            };

            if (hintCrop != null)
            {
                AppLogoOverrideUri.HintCrop = hintCrop.Value;
            }

            if (alternateText != default(string))
            {
                AppLogoOverrideUri.AlternateText = alternateText;
            }

            if (addImageQuery != default(bool?))
            {
                AppLogoOverrideUri.AddImageQuery = addImageQuery;
            }

            return this;
        }

        /// <summary>
        /// Add a hero image to the toast.
        /// </summary>
        /// <param name="uri">The URI of the image. Can be from your application package, application data, or the internet. Internet images must be less than 200 KB in size.</param>
        /// <param name="alternateText">A description of the image, for users of assistive technologies.</param>
        /// <param name="addImageQuery">A value whether Windows is allowed to append a query string to the image URI supplied in the Tile notification.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddHeroImage(Uri uri, string alternateText = default(string), bool? addImageQuery = default(bool?))
        {
            HeroImage = new ToastGenericHeroImage()
            {
                Source = uri.OriginalString
            };

            if (alternateText != default(string))
            {
                HeroImage.AlternateText = alternateText;
            }

            if (addImageQuery != default(bool?))
            {
                HeroImage.AddImageQuery = addImageQuery;
            }

            return this;
        }

        /// <summary>
        /// Add an image inline with other toast content.
        /// </summary>
        /// <param name="uri">The URI of the image. Can be from your application package, application data, or the internet. Internet images must be less than 200 KB in size.</param>
        /// <param name="alternateText">A description of the image, for users of assistive technologies.</param>
        /// <param name="addImageQuery">A value whether Windows is allowed to append a query string to the image URI supplied in the Tile notification.</param>
        /// <param name="hintCrop">A value whether a margin is removed. images have an 8px margin around them.</param>
        /// <param name="hintRemoveMargin">the horizontal alignment of the image.This is only supported when inside an <see cref="AdaptiveSubgroup"/>.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddInlineImage(Uri uri, string alternateText = default(string), bool? addImageQuery = default(bool?), AdaptiveImageCrop? hintCrop = null, bool? hintRemoveMargin = default(bool?))
        {
            var inlineImage = new AdaptiveImage()
            {
                Source = uri.OriginalString
            };

            if (hintCrop != null)
            {
                inlineImage.HintCrop = hintCrop.Value;
            }

            if (alternateText != default(string))
            {
                inlineImage.AlternateText = alternateText;
            }

            if (addImageQuery != default(bool?))
            {
                inlineImage.AddImageQuery = addImageQuery;
            }

            if (hintRemoveMargin != default(bool?))
            {
                inlineImage.HintRemoveMargin = hintRemoveMargin;
            }

            return AddVisualChild(inlineImage);
        }

        /// <summary>
        /// Add a progress bar to the toast.
        /// </summary>
        /// <param name="title">Title of the progress bar.</param>
        /// <param name="value">Value of the progress bar. Default is 0</param>
        /// <param name="isIndeterminate">Determine if the progress bar value should be indeterminate. Default to false.</param>
        /// <param name="valueStringOverride">An optional string to be displayed instead of the default percentage string. If this isn't provided, something like "70%" will be displayed.</param>
        /// <param name="status">A status string which is displayed underneath the progress bar. This string should reflect the status of the operation, like "Downloading..." or "Installing...". Default to empty.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        /// <remarks>More info at: https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/toast-progress-bar </remarks>
        public ToastContentBuilder AddProgressBar(string title = default(string), double? value = null, bool isIndeterminate = false, string valueStringOverride = default(string), string status = default(string))
        {
            int index = VisualChildren.Count(c => c is AdaptiveProgressBar);

            var progressBar = new AdaptiveProgressBar()
            {
            };

            if (title == default(string))
            {
                progressBar.Title = new BindableString($"progressBarTitle_{index}");
            }
            else
            {
                progressBar.Title = title;
            }

            if (isIndeterminate)
            {
                progressBar.Value = AdaptiveProgressBarValue.Indeterminate;
            }
            else if (value == null)
            {
                progressBar.Value = new BindableProgressBarValue($"progressValue_{index}");
            }
            else
            {
                progressBar.Value = value.Value;
            }

            if (valueStringOverride == default(string))
            {
                progressBar.ValueStringOverride = new BindableString($"progressValueString_{index}");
            }
            else
            {
                progressBar.ValueStringOverride = valueStringOverride;
            }

            if (status == default(string))
            {
                progressBar.Status = new BindableString($"progressStatus_{index}");
            }
            else
            {
                progressBar.Status = status;
            }

            return AddVisualChild(progressBar);
        }

        /// <summary>
        /// Add text to the toast.
        /// </summary>
        /// <param name="text">Custom text to display on the tile.</param>
        /// <param name="hintStyle">Style that controls the text's font size, weight, and opacity.</param>
        /// <param name="hintWrap">Indicating whether text wrapping is enabled. For Tiles, this is false by default.</param>
        /// <param name="hintMaxLines">The maximum number of lines the text element is allowed to display. For Tiles, this is infinity by default</param>
        /// <param name="hintMinLines">The minimum number of lines the text element must display.</param>
        /// <param name="hintAlign">The horizontal alignment of the text</param>
        /// <param name="language">
        /// The target locale of the XML payload, specified as a BCP-47 language tags such as "en-US" or "fr-FR". The locale specified here overrides any other specified locale, such as that in binding or visual.
        /// </param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        /// <exception cref="InvalidOperationException">Throws when attempting to add/reserve more than 4 lines on a single toast. </exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="hintMaxLines"/> value is larger than 2. </exception>
        /// <remarks>More info at: https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/adaptive-interactive-toasts#text-elements</remarks>
        public ToastContentBuilder AddText(string text, AdaptiveTextStyle? hintStyle = null, bool? hintWrap = default(bool?), int? hintMaxLines = default(int?), int? hintMinLines = default(int?), AdaptiveTextAlign? hintAlign = null, string language = default(string))
        {
            int lineCount = GetCurrentTextLineCount();
            if (GetCurrentTextLineCount() == 4)
            {
                // Reached maximum, we can't go further.
                throw new InvalidOperationException("We have reached max lines allowed (4) per toast");
            }

            AdaptiveText adaptive = new AdaptiveText()
            {
                Text = text
            };

            if (hintStyle != null)
            {
                adaptive.HintStyle = hintStyle.Value;
            }

            if (hintAlign != null)
            {
                adaptive.HintAlign = hintAlign.Value;
            }

            if (hintWrap != default(bool?))
            {
                adaptive.HintWrap = hintWrap;
            }

            if (hintMaxLines != default(int?))
            {
                if (hintMaxLines > 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(hintMaxLines), "max line can't go more than 2 lines.");
                }
                else if ((lineCount + hintMaxLines) > 4)
                {
                    throw new InvalidOperationException($"Can't exceed more than 4 lines of text per toast. Current line count : {lineCount} | Requesting line count: {lineCount + hintMaxLines}");
                }

                adaptive.HintMaxLines = hintMaxLines;
            }

            if (hintMinLines != default(int?) && hintMinLines > 0)
            {
                adaptive.HintMinLines = hintMinLines;
            }

            if (language != default(string))
            {
                adaptive.Language = language;
            }

            return AddVisualChild(adaptive);
        }

        /// <summary>
        /// Add a visual element to the toast.
        /// </summary>
        /// <param name="child">An instance of a class that implement <see cref="IToastBindingGenericChild"/>.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddVisualChild(IToastBindingGenericChild child)
        {
            VisualChildren.Add(child);

            return this;
        }

        private int GetCurrentTextLineCount()
        {
            if (!VisualChildren.Any(c => c is AdaptiveText))
            {
                return 0;
            }

            var textList = VisualChildren.Where(c => c is AdaptiveText).Select(c => c as AdaptiveText).ToList();

            // First one is already the header.
            // https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/adaptive-interactive-toasts#text-elements
            // The default (and maximum) is up to 2 lines of text for the title, and up to 4 lines (combined) for the two additional description elements (the second and third AdaptiveText).
            AdaptiveText text = textList.First();
            int count = 0;
            count += text.HintMaxLines ?? 2;

            for (int i = 1; i < textList.Count; i++)
            {
                text = textList[i];
                count += text.HintMaxLines ?? 1;
            }

            return count;
        }
    }

#endif
}
