using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImageMaskSurfaceBrushPage : Page
    {
        private Dictionary<string, string> _maskImages;
        private Dictionary<string, string> _targetImages;

        public ImageMaskSurfaceBrushPage()
        {
            this.InitializeComponent();
            this.SizeChanged += this.OnPageSizeChanged;

            _maskImages = new Dictionary<string, string>
            {
                ["Image 1"] = "ms-appx:///SamplePages/ImageMaskSurfaceBrush/MaskImage1.png",
                ["Image 2"] = "ms-appx:///SamplePages/ImageMaskSurfaceBrush/MaskImage2.png",
                ["Image 3"] = "ms-appx:///SamplePages/ImageMaskSurfaceBrush/MaskImage3.png",
            };

            MaskImages.ItemsSource = _maskImages.Keys;
            MaskImages.SelectedIndex = 0;

            _targetImages = new Dictionary<string, string>
            {
                ["Image 1"] = "ms-appx:///Assets/Photos/PaintedHillsPathway.jpg",
                ["Image 2"] = "ms-appx:///Assets/Photos/ShootingOnAutoOnTheDrone.jpg",
                ["Image 3"] = "ms-appx:///Assets/Photos/NorthernCascadesReflection.jpg",
                ["Image 4"] = "ms-appx:///Assets/Photos/Van.jpg",
            };

            TargetImages.ItemsSource = _targetImages.Keys;
            TargetImages.SelectedIndex = 0;
        }

        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //MaskImageBrush.SurfaceHeight = MaskGrid.ActualHeight;
            //MaskImageBrush.SurfaceWidth = MaskGrid.ActualWidth;
            //TargetImageBrush.SurfaceHeight = TargetGrid.ActualHeight;
            //TargetImageBrush.SurfaceWidth = TargetGrid.ActualWidth;
        }

        private void OnMaskImageChanged(object sender, SelectionChangedEventArgs e)
        {
            var image = MaskImages.SelectedValue as string;
            if (string.IsNullOrWhiteSpace(image))
            {
                return;
            }

            try
            {
                MaskImageBrush.Source = _maskImages[image];
                //ImageMaskBrush.Mask = _maskImages[image];
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        private void OnTargetImageChanged(object sender, SelectionChangedEventArgs e)
        {
            var image = TargetImages.SelectedValue as string;
            if (string.IsNullOrWhiteSpace(image))
            {
                return;
            }

            try
            {
                TargetImageBrush.Source = _targetImages[image];
                //MaskTargetImageBrush.Source = _targetImages[image];
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        private void OnBlurRadiusChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            try
            {
                MaskImageOptions.BlurRadius = e.NewValue;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }
    }
}
