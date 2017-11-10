// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Toolkit.Uwp.SampleApp.Common;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    public sealed partial class PropertyControl
    {
        private static Dictionary<Color, string> _colorNames;

        private Sample _currentSample;

        public PropertyControl()
        {
            if (_colorNames == null)
            {
                _colorNames = new Dictionary<Color, string>();
                foreach (var color in typeof(Colors).GetRuntimeProperties())
                {
                    _colorNames[(Color)color.GetValue(null)] = color.Name;
                }
            }

            InitializeComponent();
        }

        private void PropertyControl_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue == _currentSample)
            {
                return;
            }

            _currentSample = DataContext as Sample;

            RootPanel.Children.Clear();

            if (_currentSample != null)
            {
                var propertyDesc = _currentSample.PropertyDescriptor;

                if (propertyDesc == null)
                {
                    return;
                }

                foreach (var option in propertyDesc.Options)
                {
                    // Label
                    var label = new TextBlock
                    {
                        Text = option.Label + ":",
                        Foreground = new SolidColorBrush(Colors.Black)
                    };
                    RootPanel.Children.Add(label);

                    // Control
                    Control controlToAdd;
                    DependencyProperty dependencyProperty;
                    IValueConverter converter = null;

                    IDictionary<string, object> propertyDict = propertyDesc.Expando;

                    switch (option.Kind)
                    {
                        case PropertyKind.Slider:
                        case PropertyKind.DoubleSlider:
                            var slider = new Slider();
                            var sliderOption = option as SliderPropertyOptions;
                            if (sliderOption != null)
                            {
                                slider.Minimum = sliderOption.MinValue;
                                slider.Maximum = sliderOption.MaxValue;
                                slider.StepFrequency = sliderOption.Step;
                            }

                            if (option.Kind == PropertyKind.DoubleSlider)
                            {
                                slider.StepFrequency = 0.01;
                            }

                            if ((propertyDict[option.Name] as ValueHolder).Value is double value)
                            {
                                slider.Value = value;
                            }

                            controlToAdd = slider;
                            dependencyProperty = RangeBase.ValueProperty;

                            break;
                        case PropertyKind.Enum:
                            var enumType = option.DefaultValue.GetType();
                            var comboBox = new ComboBox
                            {
                                ItemsSource = Enum.GetNames(enumType),
                                SelectedItem = (propertyDict[option.Name] as ValueHolder).Value.ToString()
                            };

                            converter = new EnumConverter(enumType);
                            controlToAdd = comboBox;
                            dependencyProperty = Selector.SelectedItemProperty;
                            break;
                        case PropertyKind.Bool:
                            var checkBox = new ToggleSwitch();

                            if ((propertyDict[option.Name] as ValueHolder).Value is bool isOn)
                            {
                                checkBox.IsOn = isOn;
                            }

                            controlToAdd = checkBox;
                            dependencyProperty = ToggleSwitch.IsOnProperty;
                            break;
                        case PropertyKind.Brush:
                            var colorComboBox = new ComboBox();
                            var dataSource = typeof(Colors).GetTypeInfo().DeclaredProperties.Select(p => p.Name).ToList();
                            colorComboBox.ItemsSource = dataSource;

                            if ((propertyDict[option.Name] as ValueHolder).Value is SolidColorBrush brush &&
                                _colorNames.TryGetValue(brush.Color, out var color))
                            {
                                colorComboBox.SelectedIndex = dataSource.IndexOf(color);
                            }
                            else
                            {
                                colorComboBox.SelectedIndex = dataSource.IndexOf(option.DefaultValue.ToString());
                            }

                            converter = new SolidColorBrushConverter();

                            controlToAdd = colorComboBox;
                            dependencyProperty = Selector.SelectedItemProperty;
                            break;
                        default:
                            var textBox = new TextBox { Text = (propertyDict[option.Name] as ValueHolder).Value.ToString() };

                            controlToAdd = textBox;
                            dependencyProperty = TextBox.TextProperty;
                            break;
                    }

                    var binding = new Binding
                    {
                        Source = propertyDesc.Expando,
                        Path = new PropertyPath(option.Name + ".Value"),
                        Mode = BindingMode.TwoWay,
                        Converter = converter
                    };

                    controlToAdd.SetBinding(dependencyProperty, binding);
                    controlToAdd.Margin = new Thickness(0, 5, 0, 20);
                    RootPanel.Children.Add(controlToAdd);
                }
            }
        }
    }
}
