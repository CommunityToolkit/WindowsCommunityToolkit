using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AnimationSetPage : Page
    {
        private AnimationSet _anim;

        public AnimationSetPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AnimationFunctionList.ItemsSource = Enum.GetValues(typeof(AnimationFunction));
        }

        private async void StartClicked(object sender, RoutedEventArgs e)
        {
            if (_anim == null)
            {
                _anim = Element.Offset(50, 50)
                   .Rotate(45, 50, 50)
                   .LightAsync(2)
                   .SetDuration(2000)
                   .Then()
                   .Rotate(0, 50, 50)
                   .Offset(0, 0)
                   .LightAsync(10)
                   .SetDuration(2000);

                //_anim = Element.LightAsync(2)
                //   .SetDuration(2000)
                //   .Then()
                //   .LightAsync(10)
                //   .SetDuration(2000);

                //_anim = Element.LightAsync(5).SetDuration(20000);

                _anim.Completed += Anim_Completed;

                Debug.WriteLine("animation initalized");
            }

            await _anim.StartAsync();
            Debug.WriteLine("animation completed from await");
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            Debug.WriteLine("animation completed from event");
        }

        private void StopClicked(object sender, RoutedEventArgs e)
        {
            if (_anim != null)
            {
                _anim.Stop();
            }
        }

        private void ResetClicked(object sender, RoutedEventArgs e)
        {
            _anim = null;
        }
    }

    public enum AnimationFunction
    {
        Then,
        Scale,
        Offset
    }
}
