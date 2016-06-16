using $ProjectDefaultNamespace$;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using AppStudio.DataProviders;
using AppStudio.DataProviders. ???;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.Services
{
    public sealed class DataProviderConnector : Control
    {
        public DataProviderConnector()
        {
            this.DefaultStyleKey = typeof(DataProviderConnector);
            Loaded += DataProviderConnector_Loaded;
        }

        private async void DataProviderConnector_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public List$Name$Schema> Source
        {
            get { return List<$ServiceInstance.Name$Schema>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(List<$Name$Schema>), typeof(DataProviderConnector), new PropertyMetadata(null));

        public $Name$DataConfig SourceDataConfig
        {
            get { return ($ServiceInstance.Name$DataConfig)GetValue(SourceDataConfigProperty); }
            set { SetValue(SourceDataConfigProperty, value); }
        }

        public static readonly DependencyProperty SourceDataConfigProperty =
            DependencyProperty.Register("SourceDataConfig", typeof($Name$DataConfig), typeof(DataProviderConnector), new PropertyMetadata(null, SourceDataConfigChangedCallback));

        private async static void SourceDataConfigChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var config = (d as DataProviderConnector).SourceDataConfig;

            $ServiceInstance.Name$ProviderHelper.Instance.Config = config;

            if(config!=null)
            {
                var that = d as DataProviderConnector;
                that.Source = await $ServiceInstance.Name$ProviderHelper.Instance.RequestAsync();
            }
        }

        public string SourceQuery
        {
            get { return (string)GetValue(SourceQueryProperty); }
            set { SetValue(SourceQueryProperty, value); }
        }

        public static readonly DependencyProperty SourceQueryProperty =
            DependencyProperty.Register("SourceQuery", typeof(string), typeof(DataProviderConnector), new PropertyMetadata(null, SourceQueryChangedCallback));

        private async static void SourceQueryChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var query = (d as DataProviderConnector).SourceQuery;
            var config = (d as DataProviderConnector).SourceDataConfig;

            $ServiceInstance.Name$ProviderHelper.Instance.Config = config;

            if (config == null)
            {
                $ServiceInstance.Name$ProviderHelper.Instance.Config = new $ServiceInstance.Name$DataConfig();
                $ServiceInstance.Name$ProviderHelper.Instance.Config.$QUERY_PARAM_PROPERTY_NAME$ = query;
            }

            var that = d as DataProviderConnector;
            that.Source = await $ServiceInstance.Name$ProviderHelper.Instance.RequestAsync();
        }

    }
}
