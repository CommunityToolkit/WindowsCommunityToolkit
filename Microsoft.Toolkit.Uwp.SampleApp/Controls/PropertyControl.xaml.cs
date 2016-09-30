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
        private Sample _currentSample;

        public PropertyControl()
        {
            InitializeComponent();
        }

        private async void PropertyControl_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue == _currentSample)
            {
                return;
            }

            _currentSample = DataContext as Sample;

            RootPanel.Children.Clear();

            if (_currentSample != null)
            {
                var propertyDesc = await _currentSample.GetPropertyDescriptorAsync();

                if (propertyDesc == null)
                {
                    return;
                }

                foreach (var option in propertyDesc.Options)
                {
                    // Label
                    var label = new TextBlock
                    {
                        Text = option.Name + ":",
                        Foreground = new SolidColorBrush(Colors.Black)
                    };
                    RootPanel.Children.Add(label);

                    // Control
                    Control controlToAdd;
                    DependencyProperty dependencyProperty;
                    IValueConverter converter = null;

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
                            }

                            if (option.Kind == PropertyKind.DoubleSlider)
                            {
                                slider.StepFrequency = 0.01;
                            }

                            controlToAdd = slider;
                            dependencyProperty = RangeBase.ValueProperty;

                            break;
                        case PropertyKind.Enum:
                            var enumType = option.DefaultValue.GetType();
                            var comboBox = new ComboBox
                            {
                                ItemsSource = Enum.GetNames(enumType),
                                SelectedItem = option.DefaultValue.ToString()
                            };

                            converter = new EnumConverter(enumType);
                            controlToAdd = comboBox;
                            dependencyProperty = Selector.SelectedItemProperty;
                            break;
                        case PropertyKind.Bool:
                            var checkBox = new ToggleSwitch();

                            controlToAdd = checkBox;
                            dependencyProperty = ToggleSwitch.IsOnProperty;
                            break;
                        case PropertyKind.Brush:
                            var colorComboBox = new ComboBox();
                            var dataSource = typeof(Colors).GetTypeInfo().DeclaredProperties.Select(p => p.Name).ToList();
                            colorComboBox.ItemsSource = dataSource;
                            colorComboBox.SelectedIndex = dataSource.IndexOf(option.DefaultValue.ToString());

                            converter = new SolidColorBrushConverter();

                            controlToAdd = colorComboBox;
                            dependencyProperty = Selector.SelectedItemProperty;
                            break;
                        default:
                            var textBox = new TextBox { Text = option.DefaultValue.ToString() };

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
