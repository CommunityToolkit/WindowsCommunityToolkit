using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Microsoft.Windows.Toolkit.SampleApp.Models;

namespace Microsoft.Windows.Toolkit.SampleApp.Controls
{
    public sealed partial class PropertyControl
    {
        public PropertyControl()
        {
            InitializeComponent();
        }

        private async void PropertyControl_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var sample = DataContext as Sample;

            RootPanel.Children.Clear();

            if (sample != null)
            {
                var propertyDesc = await sample.GetPropertyDescriptorAsync();

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
                            var comboBox = new ComboBox
                            {
                                ItemsSource = Enum.GetNames(option.DefaultValue.GetType()),
                                SelectedItem = option.DefaultValue.ToString()
                            };

                            controlToAdd = comboBox;
                            dependencyProperty = Selector.SelectedItemProperty;
                            break;
                        case PropertyKind.Bool:
                            var checkBox = new ToggleSwitch();

                            controlToAdd = checkBox;
                            dependencyProperty = ToggleSwitch.IsOnProperty;
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
                        Mode = BindingMode.TwoWay
                    };

                    controlToAdd.SetBinding(dependencyProperty, binding);
                    controlToAdd.Margin = new Thickness(0, 5, 0, 20);
                    RootPanel.Children.Add(controlToAdd);
                }
            }
        }
    }
}
