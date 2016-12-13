using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    internal interface IElement_ToastActivatable
    {
        Element_ToastActivationType ActivationType { get; set; }

        string ProtocolActivationTargetApplicationPfn { get; set; }

        ToastAfterActivationBehavior AfterActivationBehavior { get; set; }
    }
}
