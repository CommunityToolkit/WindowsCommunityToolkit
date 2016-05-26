using Microsoft.Windows.Toolkit.SampleApp.Models;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    using System.Collections.ObjectModel;

    using global::Windows.UI.Xaml;

    public sealed partial class PivoramaPage
    {
        /// <summary>
        /// Defines the <see cref="PivoramaItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PivoramaItemsProperty =
            DependencyProperty.Register(
                nameof(PivoramaItems),
                typeof(ObservableCollection<object>),
                typeof(PivoramaPage),
                new PropertyMetadata(null));

        public PivoramaPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the pivorama items collection.
        /// </summary>
        public ObservableCollection<object> PivoramaItems
        {
            get
            {
                return (ObservableCollection<object>)GetValue(PivoramaItemsProperty);
            }
            set
            {
                SetValue(PivoramaItemsProperty, value);
            }
        }

        /// <summary>
        /// Called on navigating to the pivorama page.
        /// </summary>
        /// <param name="e">
        /// The navigation event arguments.
        /// </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            // ToDo, populate
            PivoramaItems = new ObservableCollection<object>();
        }
    }
}