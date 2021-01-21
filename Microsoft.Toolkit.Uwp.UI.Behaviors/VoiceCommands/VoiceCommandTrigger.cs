// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xaml.Interactivity;
using Windows.Gaming.Input.ForceFeedback;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// A behavior that listens spoken voice commands and executes its actions when that event is fired.
    /// </summary>
    public class VoiceCommandTrigger : Trigger
    {
        private static readonly Dictionary<string, List<VoiceCommandTrigger>> _triggers = new Dictionary<string, List<VoiceCommandTrigger>>(StringComparer.InvariantCultureIgnoreCase);
        private static ISpeechRecognizer _speechRecognizer;

        /// <summary>
        /// Gets or sets the SpeechRecognizer object used to recongize the voice commands
        /// </summary>
        public static ISpeechRecognizer SpeechRecognizer
        {
            get => _speechRecognizer;
            set
            {
                if (value != _speechRecognizer)
                {
                    if (_speechRecognizer is object)
                    {
                        _speechRecognizer.Recognized -= SpeechRecognizer_Recognized;
                    }

                    _speechRecognizer = value;
                    if (_speechRecognizer is object)
                    {
                        _speechRecognizer.Recognized += SpeechRecognizer_Recognized;
                    }
                }
            }
        }

        private static void SpeechRecognizer_Recognized(ISpeechRecognizer sender, RecognizedEventArgs e)
        {
            Debug.WriteLine(e.Result.Text);
            if (_triggers.TryGetValue(e.Result.Text, out var list))
            {
                foreach (var trigger in list)
                {
                    _ = trigger.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        if (trigger.IsEnabled)
                        {
                            Interaction.ExecuteActions(trigger.AssociatedObject, trigger.Actions, e);
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Gets or sets the spoken Text to trigger the Actions
        /// </summary>
        /// <remarks>
        /// Use the | to seperate alternative texts
        /// </remarks>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(VoiceCommandTrigger), new PropertyMetadata(default(string), OnTextPropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether the VoiceCommand is enabled. Default is <c>true</c>
        /// </summary>
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IsEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof(IsEnabled), typeof(bool), typeof(VoiceCommandTrigger), new PropertyMetadata(true));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VoiceCommandTrigger source)
            {
                var newValue = (string)e.NewValue;
                var oldValue = (string)e.OldValue;
                if (!string.IsNullOrEmpty(oldValue))
                {
                    source.Remove(oldValue);
                }

                if (!string.IsNullOrEmpty(newValue))
                {
                    source.Add(newValue);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            Remove(Text);
            base.OnDetaching();
        }

        private void Add(string text)
        {
            foreach (var item in text.Split('|'))
            {
                if (_triggers.TryGetValue(item, out var list))
                {
                    list.Add(this);
                }
                else
                {
                    list = new List<VoiceCommandTrigger>()
                    {
                        this
                    };
                    _triggers[item] = list;
                }
            }
        }

        private void Remove(string text)
        {
            foreach (var item in text.Split('|'))
            {
                if (_triggers.TryGetValue(item, out var list))
                {
                    list.Remove(this);
                    if (list.Count == 0)
                    {
                        _triggers.Remove(item);
                    }
                }
            }
        }
    }
}
