using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Uno.Extensions;
using Windows.UI.Xaml;
using Uno.UI;

namespace Microsoft.Toolkit.Uwp.SampleApp.Wasm
{
	public static class Program
	{
		private static App _app;

		static void Main(string[] args)
		{
			FeatureConfiguration.UIElement.AssignDOMXamlName = true;
			Application.Start(e => new App());
		}
	}
}
