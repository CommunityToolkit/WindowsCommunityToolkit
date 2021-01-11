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
    public class VoiceCommandTrigger : Trigger
    {
        private static readonly Dictionary<string, List<VoiceCommandTrigger>> _triggers = new Dictionary<string, List<VoiceCommandTrigger>>(StringComparer.InvariantCultureIgnoreCase);
        private static ISpeechRecognizer _speechRecognizer;

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

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(VoiceCommandTrigger), new PropertyMetadata(default(string), OnTextPropertyChanged));

        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof(IsEnabled), typeof(bool), typeof(VoiceCommandTrigger), new PropertyMetadata(true));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VoiceCommandTrigger source)
            {
                var newValue = (string)e.NewValue;
                var oldValue = (string)e.OldValue;
                if (!string.IsNullOrEmpty(oldValue)) source.Remove(oldValue);
                if (!string.IsNullOrEmpty(newValue)) source.Add(newValue);
            }
        }


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
