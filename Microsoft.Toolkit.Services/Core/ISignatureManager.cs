namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// Provides platform specific logic to sign request for OAuth communication
    /// </summary>
    public interface ISignatureManager
    {
        /// <summary>
        /// Generate request signature
        /// </summary>
        /// <param name="baseString">String to sign</param>
        /// <param name="secret">Secret to use to sign</param>
        /// <param name="append">If true append &amp; to the base string</param>
        /// <returns>The signed baseString to use in the OAuth requests</returns>
        string GetSignature(string baseString, string secret, bool append = false);
    }
}
