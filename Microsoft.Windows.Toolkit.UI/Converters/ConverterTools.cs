using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Converters
{
    internal static class ConverterTools
    {
        internal static bool SafeParseBool(object parameter)
        {
            var parsed = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out parsed);
            }

            return parsed;
        }

        internal static Visibility Opposite(Visibility target)
        {
            if (target == Visibility.Visible)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }
    }
}
