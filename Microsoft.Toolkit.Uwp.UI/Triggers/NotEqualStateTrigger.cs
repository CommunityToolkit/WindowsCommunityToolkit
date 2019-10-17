// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Triggers
{
    /// <summary>
    /// Enables a state if the value is not equal to another value
    /// </summary>
    public class NotEqualStateTrigger : StateTriggerBase
    {
        private void UpdateTrigger() => SetActive(!EqualsStateTrigger.AreValuesEqual(Value, NotEqualTo, true));

        /// <summary>
        /// Gets or sets the value for comparison.
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Value"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(NotEqualStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (NotEqualStateTrigger)d;
            obj.UpdateTrigger();
        }

        /// <summary>
        /// Gets or sets the value to compare inequality to.
        /// </summary>
        public object NotEqualTo
        {
            get { return (object)GetValue(NotEqualToProperty); }
            set { SetValue(NotEqualToProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="NotEqualTo"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty NotEqualToProperty =
                    DependencyProperty.Register(nameof(NotEqualTo), typeof(object), typeof(NotEqualStateTrigger), new PropertyMetadata(null, OnValuePropertyChanged));
    }
}
