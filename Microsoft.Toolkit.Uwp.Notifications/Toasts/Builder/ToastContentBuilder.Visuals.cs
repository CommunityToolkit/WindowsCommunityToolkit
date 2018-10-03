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

        public ToastContentBuilder AddInlineImage(Uri uri, string alternateText = default(string), bool? addImageQuery = default(bool?), AdaptiveImageCrop? hintCrop = null, bool? hintRemoveMargin = default(bool?))
        {
            var InlineImage = new AdaptiveImage()
            {
                Source = uri.OriginalString
            };

            if (hintCrop != null)
            {
                InlineImage.HintCrop = hintCrop.Value;
            }

            if (alternateText != default(string))
            {
                InlineImage.AlternateText = alternateText;
            }

            if (addImageQuery != default(bool?))
            {
                InlineImage.AddImageQuery = addImageQuery;
            }

            if (hintRemoveMargin != default(bool?))
            {
                InlineImage.HintRemoveMargin = hintRemoveMargin;
            }

            return AddVisualChild(InlineImage);
        }

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

#if WINDOWS_UWP

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
