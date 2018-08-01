using System.Collections;
using System.Windows.Forms.Design;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    internal class InkToolbarDesigner : ControlDesigner
    {
        public override void InitializeNewComponent(IDictionary defaultValues)
        {
            base.InitializeNewComponent(defaultValues);

            var toolbar = (InkToolbar)Component;
            if (toolbar != null)
            {
                // Set MinimumSize in the designer, so that the control doesn't go to 0-height
                toolbar.MinimumSize = new System.Drawing.Size(20, 60);
                toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            }
        }
    }
}