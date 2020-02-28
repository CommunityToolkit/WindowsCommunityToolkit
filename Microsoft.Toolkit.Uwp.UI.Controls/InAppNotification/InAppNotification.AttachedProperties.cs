// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// In App Notification defines a control to show local notification in the app.
    /// </summary>
    public partial class InAppNotification
    {
        /// <summary>
        /// Gets the value of the KeyFrameDuration attached Property
        /// </summary>
        /// <param name="obj">the KeyFrame where the duration is set</param>
        /// <returns>Value of KeyFrameDuration</returns>
        public static Duration GetKeyFrameDuration(DependencyObject obj)
        {
            return (Duration)obj.GetValue(KeyFrameDurationProperty);
        }

        /// <summary>
        /// Sets the value of the KeyFrameDuration attached property
        /// </summary>
        /// <param name="obj">The KeyFrame object where the property is attached</param>
        /// <param name="value">The Duration value to be set as duration</param>
        public static void SetKeyFrameDuration(DependencyObject obj, Duration value)
        {
            obj.SetValue(KeyFrameDurationProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for KeyFrameDuration. This enables animation, styling, binding, etc
        /// </summary>
        public static readonly DependencyProperty KeyFrameDurationProperty =
            DependencyProperty.RegisterAttached("KeyFrameDuration", typeof(Duration), typeof(InAppNotification), new PropertyMetadata(DurationHelper.FromTimeSpan(TimeSpan.Zero), OnKeyFrameAnimationChanged));

        private static void OnKeyFrameAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Duration dur)
            {
                var keyTimeFromAnimationDuration = KeyTimeHelper.FromTimeSpan(dur.TimeSpan);
                if (d is DoubleKeyFrame dkf)
                {
                    dkf.KeyTime = keyTimeFromAnimationDuration;
                }
                else if (d is ObjectKeyFrame okf)
                {
                    okf.KeyTime = keyTimeFromAnimationDuration;
                }
            }
        }
    }
}
