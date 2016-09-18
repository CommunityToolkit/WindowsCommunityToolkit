namespace Microsoft.Toolkit.Uwp.UI.Controls.AdvancedCollectionViewSource
{
    /// <summary>
    /// Sort description
    /// </summary>
    public class SortDescription
    {
        /// <summary>
        /// Name of property to sort on
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Direction of sort
        /// </summary>
        public SortDirection Direction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortDescription"/> class.
        /// </summary>
        /// <param name="propertyName">name of property to sort on</param>
        /// <param name="direction">direction of sort</param>
        public SortDescription(string propertyName, SortDirection direction)
        {
            PropertyName = propertyName;
            Direction = direction;
        }
    }
}