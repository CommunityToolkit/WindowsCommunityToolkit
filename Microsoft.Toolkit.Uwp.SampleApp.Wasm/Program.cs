using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Uno.UI;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.Wasm
{
    public static class Program
    {
        private static App _app;

        private static void Main(string[] args)
        {
            FeatureConfiguration.UIElement.AssignDOMXamlName = true;
            Application.Start(e => new App());
        }
    }
}
