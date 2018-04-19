using System;

namespace Global.System.Runtime.CompilerServices
{
    /// <summary>
    /// Sealed custom attibute for use with Platform Specific Analyzer
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class PlatformSpecificAttribute : Attribute
    {
    }
}
