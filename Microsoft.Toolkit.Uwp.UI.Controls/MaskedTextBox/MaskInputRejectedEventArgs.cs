using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Describes a delegate for an event that has a MaskInputRejectedEventArgs as a parameter.
    /// </summary>
    public delegate void MaskInputRejectedEventHandler(object sender, MaskInputRejectedEventArgs e);

    /// <summary>
    /// Provides data for the MaskInputRejected event.
    /// </summary>
    public class MaskInputRejectedEventArgs : EventArgs
    {
        public MaskInputRejectedEventArgs(int position, MaskedTextResultHint rejectionHint)
        {
            Position = position;
            RejectionHint = rejectionHint;
        }

        /// <summary>
        /// The position where the test failed the mask constraint.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Retreives a hint on why the input is rejected.
        /// </summary>
        public MaskedTextResultHint RejectionHint { get; }
    }
}