using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Loading control allows to show an loading animation with some xaml in it.
    /// </summary>
    [TemplateVisualState(Name = "LoadingIn", GroupName = "CommonStates")]
    [TemplateVisualState(Name = "LoadingOut", GroupName = "CommonStates")]
    [TemplatePart(Name = "RootGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "BackgroundGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "ContentGrid", Type = typeof(ContentPresenter))]
    public sealed partial class Loading : ContentControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Loading"/> class.
        /// </summary>
        public Loading()
        {
            DefaultStyleKey = typeof(Loading);
        }

        protected override void OnApplyTemplate()
        {
            _rootGrid = GetTemplateChild("RootGrid") as Grid;
            _backgroundGrid = GetTemplateChild("BackgroundGrid") as Grid;
            _contentGrid = GetTemplateChild("ContentGrid") as ContentPresenter;

            CreateLoadingControl();

            base.OnApplyTemplate();
        }

        private void CreateLoadingControl()
        {
            if (IsLoading)
            {
                if (LoadingBackground == null && LoadingOpacity == 0d)
                {
                    _backgroundGrid = null;
                }
                else
                {
                    _backgroundGrid.Background = LoadingBackground;
                    _backgroundGrid.Opacity = LoadingOpacity;
                }

                VisualStateManager.GoToState(this, "LoadingIn", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "LoadingOut", true);
            }
        }
    }
}
