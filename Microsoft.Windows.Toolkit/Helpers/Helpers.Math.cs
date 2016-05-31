namespace Microsoft.Windows.Toolkit
{
    public static partial class Helpers
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