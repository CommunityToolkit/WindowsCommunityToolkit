using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarFormats;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages.TextToolbar
{
    public class SampleFormatter : Formatter
    {
        public SampleFormatter(UI.Controls.TextToolbar model)
            : base(model)
        {
        }

        public override ButtonMap DefaultButtons
        {
            get
            {
                var bold = Model.CommonButtons.Bold;
                bold.Activation = item => Selected.Text = "BOLD!!!";

                return new ButtonMap
                {
                    bold
                };
            }
        }
    }
}