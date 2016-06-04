namespace Microsoft.Windows.Toolkit
{
    internal static partial class ControlHelpers
    {
        public static int Mod(this int value, int module)
        {
            int result = value % module;
            return result >= 0 ? result : (result + module) % module;
        }

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