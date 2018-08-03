using System.Collections;
using System.Windows.Forms.Design;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    internal class MediaPlayerElementDesigner : ControlDesigner
    {
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            var control = (MediaPlayerElement)Component;
            if (control != null)
            {
                // Set MinimumSize in the designer, so that the control doesn't go to 0-height
                control.MinimumSize = new System.Drawing.Size(100, 100);
                control.Dock = System.Windows.Forms.DockStyle.Fill;
            }
        }
    }
}