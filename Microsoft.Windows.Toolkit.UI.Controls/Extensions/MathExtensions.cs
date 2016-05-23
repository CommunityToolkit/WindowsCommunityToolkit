namespace Microsoft.Windows.Toolkit.UI.Controls.Extensions
{
    /// <summary>
    /// A collection of extensions for math functions.
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// Finds the remainder of division of an <see cref="int"/> by another.
        /// </summary>
        /// <param name="value">
        /// The value to take the remainder of.
        /// </param>
        /// <param name="module">
        /// The value that is divided with.
        /// </param>
        /// <returns>
        /// Returns the remainder.
        /// </returns>
        public static int Mod(this int value, int module)
        {
            int result = value % module;
            return result >= 0 ? result : (result + module) % module;
        }
    }
}