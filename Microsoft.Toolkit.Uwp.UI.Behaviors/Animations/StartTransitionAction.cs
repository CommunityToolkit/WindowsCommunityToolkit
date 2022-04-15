// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// An <see cref="IAction"/> implementation that can trigger a target <see cref="TransitionHelper"/> instance.
    /// </summary>
    public sealed class StartTransitionAction : DependencyObject, IAction
    {
        /// <summary>
        /// Gets or sets the linked <see cref="TransitionHelper"/> instance to invoke.
        /// </summary>
        public TransitionHelper Transition
        {
            get
            {
                return (TransitionHelper)this.GetValue(TransitionProperty);
            }

            set
            {
                this.SetValue(TransitionProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <seealso cref="Transition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TransitionProperty = DependencyProperty.Register(
            nameof(Transition),
            typeof(TransitionHelper),
            typeof(StartTransitionAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the source control of the <seealso cref="Transition"/>.
        /// </summary>
        public FrameworkElement Source
        {
            get
            {
                return (FrameworkElement)this.GetValue(SourceProperty);
            }

            set
            {
                this.SetValue(SourceProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <seealso cref="Source"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            nameof(Source),
            typeof(FrameworkElement),
            typeof(StartTransitionAction),
            new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the target control of the <seealso cref="Transition"/>.
        /// </summary>
        public FrameworkElement Target
        {
            get
            {
                return (FrameworkElement)this.GetValue(TargetProperty);
            }

            set
            {
                this.SetValue(TargetProperty, value);
            }
        }

        /// <summary>
        /// Identifies the <seealso cref="Target"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            nameof(Target),
            typeof(FrameworkElement),
            typeof(StartTransitionAction),
            new PropertyMetadata(null));

        /// <inheritdoc/>
        public object Execute(object sender, object parameter)
        {
            if (this.Transition is null)
            {
                throw new ArgumentNullException(nameof(this.Transition));
            }

            if (this.Source is null)
            {
                throw new ArgumentNullException(nameof(this.Source));
            }

            if (this.Target is null)
            {
                throw new ArgumentNullException(nameof(this.Target));
            }

            this.Transition.Source = this.Source;
            this.Transition.Target = this.Target;
            _ = this.Transition.StartAsync();

            return null;
        }
    }
}