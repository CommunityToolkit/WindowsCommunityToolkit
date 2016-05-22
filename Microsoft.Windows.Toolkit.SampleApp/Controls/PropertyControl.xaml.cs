using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Windows.Toolkit.SampleApp.Models;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Microsoft.Windows.Toolkit.SampleApp.Controls
{
    public sealed partial class PropertyControl : UserControl
    {
        public PropertyControl()
        {
            this.InitializeComponent();
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
                    Control controlToAdd = null;
                    DependencyProperty dependencyProperty = null;

                    switch (option.Kind)
                    {
                        case PropertyKind.Slider:
                            var slider = new Slider();
                            var sliderOption = option as SliderPropertyOptions;
                            if (sliderOption != null)
                            {
                                slider.Minimum = sliderOption.MinValue;
                                slider.Maximum = sliderOption.MaxValue;
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
                        default:
                            var textBox = new TextBox();

                            textBox.Text = option.DefaultValue.ToString();

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
