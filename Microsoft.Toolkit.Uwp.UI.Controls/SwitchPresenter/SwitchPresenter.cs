// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The <see cref="SwitchPresenter"/> is a <see cref="ContentPresenter"/> which can allow a developer to mimic a <c>switch</c> statement within XAML.
    /// When provided a set of <see cref="Case"/>s and a <see cref="Value"/>, it will pick the matching <see cref="Case"/> with the corresponding <see cref="Case.Value"/>.
    /// </summary>
    [ContentProperty(Name = nameof(SwitchCases))]
    public sealed partial class SwitchPresenter : ContentPresenter
    {
        /// <summary>
        /// Gets the current <see cref="Case"/> which is being displayed.
        /// </summary>
        public Case CurrentCase
        {
            get { return (Case)GetValue(CurrentCaseProperty); }
            private set { SetValue(CurrentCaseProperty, value); }
        }

        /// <summary>
        /// Indicates the <see cref="CurrentCase"/> property.
        /// </summary>
        public static readonly DependencyProperty CurrentCaseProperty =
            DependencyProperty.Register(nameof(CurrentCase), typeof(Case), typeof(SwitchPresenter), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value representing the collection of cases to evaluate.
        /// </summary>
        public CaseCollection SwitchCases
        {
            get { return (CaseCollection)GetValue(SwitchCasesProperty); }
            set { SetValue(SwitchCasesProperty, value); }
        }

        /// <summary>
        /// Indicates the <see cref="SwitchCases"/> property.
        /// </summary>
        public static readonly DependencyProperty SwitchCasesProperty =
            DependencyProperty.Register(nameof(SwitchCases), typeof(CaseCollection), typeof(SwitchPresenter), new PropertyMetadata(null, new PropertyChangedCallback(OnSwitchCasesPropertyChanged)));

        /// <summary>
        /// Gets or sets a value indicating the value to compare all cases against. When this value is bound to and changes, the presenter will automatically evaluate cases and select the new appropriate content from the switch.
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Indicates the <see cref="Value"/> property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(SwitchPresenter), new PropertyMetadata(null, new PropertyChangedCallback(OnValuePropertyChanged)));

        /// <summary>
        /// Gets or sets a value indicating which type to first cast and compare provided values against.
        /// </summary>
        public Type TargetType
        {
            get { return (Type)GetValue(DataTypeProperty); }
            set { SetValue(DataTypeProperty, value); }
        }

        /// <summary>
        /// Indicates the <see cref="TargetType"/> property.
        /// </summary>
        public static readonly DependencyProperty DataTypeProperty =
            DependencyProperty.Register(nameof(TargetType), typeof(Type), typeof(SwitchPresenter), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether the content is removed from the visual tree when switching between cases.
        /// </summary>
        public bool IsVisualTreeDisconnectedOnChange { get; set; }

        private static void OnSwitchCasesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((SwitchPresenter)e.OldValue).SwitchCases.CaseCollectionChanged -= OnCaseValuePropertyChanged;
            }

            var xswitch = (SwitchPresenter)d;

            foreach (var xcase in xswitch.SwitchCases)
            {
                // Set our parent
                xcase.Parent = xswitch;
            }

            // Will trigger on collection change and case value changed
            xswitch.SwitchCases.Parent = xswitch;
            xswitch.SwitchCases.CaseCollectionChanged += OnCaseValuePropertyChanged;
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // When our Switch's expression changes, re-evaluate.
            var xswitch = (SwitchPresenter)d;

            xswitch.EvaluateCases();
        }

        private static void OnCaseValuePropertyChanged(object sender, EventArgs e)
        {
            // When something about our collection of cases changes, re-evaluate.
            var collection = (CaseCollection)sender;

            collection.Parent.EvaluateCases();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchPresenter"/> class.
        /// </summary>
        public SwitchPresenter()
        {
            this.SwitchCases = new CaseCollection();
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            EvaluateCases();
        }

        private void EvaluateCases()
        {
            if (CurrentCase != null &&
                CurrentCase.Value != null &&
                CurrentCase.Value.Equals(Value))
            {
                // If the current case we're on already matches our current value,
                // then we don't have any work to do.
                return;
            }

            Case xdefault = null;
            Case newcase = null;

            foreach (var xcase in SwitchCases)
            {
                if (xcase.IsDefault)
                {
                    // If there are multiple default cases provided, this will override and just grab the last one, the developer will have to fix this in their XAML. We call this out in the case comments.
                    xdefault = xcase;
                    continue;
                }

                if (CompareValues(Value, xcase.Value))
                {
                    newcase = xcase;
                    break;
                }
            }

            if (newcase == null && xdefault != null)
            {
                // Inject default if we found one.
                newcase = xdefault;
            }

            // Only bother changing things around if we have a new case.
            if (newcase != CurrentCase)
            {
                // Disconnect old content from visual tree.
                if (CurrentCase != null && CurrentCase.Content != null && IsVisualTreeDisconnectedOnChange)
                {
                    // TODO: If we disconnect here, we need to recreate later??? Need to Test...
                    VisualTreeHelper.DisconnectChildrenRecursive(CurrentCase.Content);
                }

                // Hookup new content.
                Content = newcase.Content;

                CurrentCase = newcase;
            }
        }

        /// <summary>
        /// Compares two values using the TargetType.
        /// </summary>
        /// <param name="compare">Our main value in our SwitchPresenter.</param>
        /// <param name="value">The value from the case to compare to.</param>
        /// <returns>true if the two values are equal</returns>
        private bool CompareValues(object compare, object value)
        {
            if (compare == null || value == null)
            {
                return compare == value;
            }

            if (TargetType == null ||
                (TargetType == compare.GetType() &&
                 TargetType == value.GetType()))
            {
                // Default direct object comparison or we're all the proper type
                return compare.Equals(value);
            }
            else if (compare.GetType() == TargetType)
            {
                // If we have a TargetType and the first value is ther right type
                // Then our 2nd value isn't, so convert to string and coerce.
                var valueBase2 = ConvertValue(TargetType, value);

                return compare.Equals(valueBase2);
            }

            // Neither of our two values matches the type so
            // we'll convert both to a String and try and coerce it to the proper type.
            var compareBase = ConvertValue(TargetType, compare);

            var valueBase = ConvertValue(TargetType, value);

            return compareBase.Equals(valueBase);
        }

        /// <summary>
        /// Helper method to convert a value from a source type to a target type.
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted value</returns>
        internal static object ConvertValue(Type targetType, object value)
        {
            if (targetType.IsInstanceOfType(value))
            {
                return value;
            }
            else
            {
                return XamlBindingHelper.ConvertValue(targetType, value);
            }
        }
    }
}
