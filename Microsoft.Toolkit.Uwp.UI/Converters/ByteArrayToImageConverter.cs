using System;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.UI.Converters
{
    /// <summary>
    /// This class converts a byte[] value into BitmapImage.
    /// If the value is null or not a byte array, it will return null
    /// Otherwise the logic will try to convert it to BitmapImage
    /// So it can be used with the Image Control.
    /// </summary>
    public class ByteArrayToImageConverter : IValueConverter
    {
        /// <summary>
        /// Convert a byte[] value to BitmapImage.
        /// </summary>
        /// <param name="value">An image as byte array.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">Optional parameter. Not used.</param>
        /// <param name="language">The language of the conversion. Not used</param>
        /// <returns>BitmapImage</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is byte[] byteArray)
            {
                using InMemoryRandomAccessStream stream = new();
                using (DataWriter writer = new(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(byteArray);
                    writer.StoreAsync().GetAwaiter().GetResult();
                }

                BitmapImage image = new();
                image.SetSource(stream);
                return image;
            }

            return null;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">Optional parameter. Not used.</param>
        /// <param name="language">The language of the conversion. Not used.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
