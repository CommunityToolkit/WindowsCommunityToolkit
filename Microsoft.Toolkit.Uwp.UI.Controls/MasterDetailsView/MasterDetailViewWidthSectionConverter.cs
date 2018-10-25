using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class MasterDetailViewWidthSectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            GridLength ret = new GridLength(0, GridUnitType.Star);

            if (value is MasterDetailsView masterDetail)
            {
                MasterDetailsViewHighlightedSection highlightedSection = masterDetail.HighlightedSection;

                switch (parameter?.ToString())
                {
                    case "MasterColumn":
                        if (highlightedSection == MasterDetailsViewHighlightedSection.Details)
                        {
                            ret = new GridLength(masterDetail.MasterPaneWidth);
                        }
                        else
                        {
                            ret = new GridLength(100, GridUnitType.Star);
                        }

                        break;

                    case "DetailsColumn":
                        if (highlightedSection == MasterDetailsViewHighlightedSection.Details)
                        {
                            ret = new GridLength(100, GridUnitType.Star);
                        }
                        else
                        {
                            ret = new GridLength(masterDetail.MasterPaneWidth);
                        }

                        break;
                }

                return ret;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
