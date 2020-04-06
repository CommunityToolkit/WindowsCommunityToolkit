using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static class Assert
    {
        internal static void AreEqual(object expected, object actual)
        {
            if (!expected.Equals(actual))
            {
                throw new Exception();
            }
        }
    }
}
