using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;

#nullable enable

namespace Microsoft.Toolkit.Helpers
{
    /// <summary>
    /// A helper class to retrieve embedded resources from the executing assembly.
    /// </summary>
    public static class EmbeddedResources
    {
        /// <summary>
        /// The sequence of available directory separators for all platforms.
        /// </summary>
        private static readonly char[] PathSeparators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        /// <summary>
        /// Returns a <see cref="Stream"/> instance for a specified manifest file.
        /// </summary>
        /// <param name="path">The relative path of the file to open, with respect of the root of the assembly.</param>
        /// <returns>A <see cref="Stream"/> for the requested file.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> does not represent a valid file.</exception>
        /// <remarks>
        /// This method will automatically get the assembly for the caller to retrieve the target file.
        /// See remarks for <see cref="GetStream(Assembly,string)"/> for more info.
        /// </remarks>
        [Pure]
        public static Stream GetStream(string path)
        {
            return GetStream(Assembly.GetCallingAssembly(), path);
        }

        /// <summary>
        /// Returns a <see cref="Stream"/> instance for a specified manifest file.
        /// </summary>
        /// <param name="assembly">The target <see cref="Assembly"/> instance.</param>
        /// <param name="path">The relative path of the file to open, with respect of the root of the assembly.</param>
        /// <returns>A <see cref="Stream"/> for the requested file.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> does not represent a valid file.</exception>
        /// <remarks>
        /// The relative path should not include the name of the given assembly. For instance, consider an embedded
        /// resource file that is located at the path <c>/Assets/myfile.txt</c> into the assembly <c>MyAssembly</c>.
        /// You can then invoke <see cref="GetStream(Assembly,string)"/> with <paramref name="path"/> equal to either
        /// <c>/Assets/myfile.txt</c>, <c>\Assets\myfile.txt</c>, <c>Assets/myfile.txt</c> or similar combinations.
        /// This method will reconstruct the full path of the target file (which is <c>MyAssembly.Assets.myfile.txt</c>
        /// in this case, retrieve the file and return a <see cref="Stream"/> to use to read from it.
        /// </remarks>
        [Pure]
        public static Stream GetStream(Assembly assembly, string path)
        {
            string[] parts = path.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);
            string filename = $"{assembly.GetName().Name}.{string.Join(".", parts)}";

            Stream? stream = assembly.GetManifestResourceStream(filename);

            if (stream is null)
            {
                static void Throw() => throw new ArgumentException("The input path was not valid or the item didn't exist", nameof(path));

                Throw();
            }

            return stream!;
        }

        /// <summary>
        /// Returns the contents of a specified manifest file, as a <see cref="string"/>.
        /// </summary>
        /// <param name="path">The relative path of the file to read, with respect of the root of the assembly.</param>
        /// <returns>The text contents of the specified manifest file.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> does not represent a valid file.</exception>
        /// <remarks>
        /// This method will automatically get the assembly for the caller to retrieve the target file.
        /// See remarks for <see cref="GetStream(Assembly,string)"/> for more info.
        /// </remarks>
        [Pure]
        public static string GetString(string path)
        {
            return GetString(Assembly.GetCallingAssembly(), path);
        }

        /// <summary>
        /// Returns the contents of a specified manifest file, as a <see cref="string"/>.
        /// </summary>
        /// <param name="assembly">The target <see cref="Assembly"/> instance.</param>
        /// <param name="path">The relative path of the file to read, with respect of the root of the assembly.</param>
        /// <returns>The text contents of the specified manifest file.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> does not represent a valid file.</exception>
        /// <remarks>
        /// This method will automatically get the assembly for the caller to retrieve the target file.
        /// See remarks for <see cref="GetStream(Assembly,string)"/> for more info.
        /// </remarks>
        [Pure]
        public static string GetString(Assembly assembly, string path)
        {
            using Stream stream = GetStream(assembly, path);
            using StreamReader reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
