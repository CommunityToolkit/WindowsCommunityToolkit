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

        /// <summary>
        /// Increases value by 1 and finds the remainder of division of result by another.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static int IncMod(this int value, int module)
        {
            return (value + 1).Mod(module);
        }

        public static int DecMod(this int value, int module)
        {
            return (value - 1).Mod(module);
        }

        public static double Mod(this double value, double module)
        {
            double res = value % module;
            return res >= 0 ? res : (res + module) % module;
        }
    }
}