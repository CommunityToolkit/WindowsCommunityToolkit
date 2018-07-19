namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    /// <summary>
    /// <see cref="System.Numerics.Matrix4x4"/>
    /// </summary>
    public class Matrix4x4
    {
        internal global::System.Numerics.Matrix4x4 UwpInstance { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4x4"/> class, a
        /// Wpf-enabled wrapper for <see cref="System.Numerics.Matrix4x4"/>
        /// </summary>
        public Matrix4x4(System.Numerics.Matrix4x4 instance)
        {
            this.UwpInstance = instance;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="System.Numerics.Matrix4x4.IsIdentity"/>
        /// </summary>
        public bool IsIdentity
        {
            get => UwpInstance.IsIdentity;
        }

        /*
        /// <summary>
        /// Gets or sets <see cref="System.Numerics.Matrix4x4.Translation"/>
        /// </summary>
        public System.Numerics.Vector3 Translation
        {
            get => UwpInstance.Translation;
            set => UwpInstance.Translation = value;
        }
        */

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Numerics.Matrix4x4"/> to <see cref="Microsoft.Toolkit.Win32.UI.Controls.WPF.Matrix4x4"/>.
        /// </summary>
        /// <param name="args">The <see cref="System.Numerics.Matrix4x4"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Matrix4x4(
            System.Numerics.Matrix4x4 args)
        {
            return FromMatrix4x4(args);
        }

        /// <summary>
        /// Creates a <see cref="Matrix4x4"/> from <see cref="System.Numerics.Matrix4x4"/>.
        /// </summary>
        /// <param name="args">The <see cref="System.Numerics.Matrix4x4"/> instance containing the event data.</param>
        /// <returns><see cref="Matrix4x4"/></returns>
        public static Matrix4x4 FromMatrix4x4(System.Numerics.Matrix4x4 args)
        {
            return new Matrix4x4(args);
        }
    }
}