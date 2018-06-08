using System;
using System.Reflection;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.VersionReader
{
    /// <summary>
    /// Contains functionality for retrieving version numbers from assemblies
    /// </summary>
    [CakeAliasCategory("Version Reader")]
    public static class VersionReaderAliases
    {
        /// <summary>
        /// Get the version number from an assembly in SemVer format.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="file">The binary to read from</param>
        /// <returns>Version number in the format '0.0.0'</returns>
        [CakeMethodAlias]
        public static string GetVersionNumber(this ICakeContext context, FilePath file)
        {
            var version = GetVersionFromFile(context, file);
            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        /// <summary>
        /// Get the version number from an assembly in 4-digit format.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="file">The binary to read from</param>
        /// <returns>Version number in the format '0.0.0.0'</returns>
        [CakeMethodAlias]
        public static string GetFullVersionNumber(this ICakeContext context, FilePath file)
        {
            var version = GetVersionFromFile(context, file);
            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        /// <summary>
        /// Get the version number with the current build number appended.
        /// This is based on the article found here: http://www.xavierdecoster.com/semantic-versioning-auto-incremented-nuget-package-versions
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="file">The binary to read from</param>
        /// <param name="buildNumber">The build number as provided by the build server</param>
        /// <returns>Version number in the format '0.0.0-CI00000'</returns>
        [CakeMethodAlias]
        public static string GetVersionNumberWithContinuesIntegrationNumberAppended(this ICakeContext context, FilePath file, int buildNumber)
        {
            var version = GetVersionFromFile(context, file);
            var adjustedVersion = $"{version.Major}.{version.Minor}.{version.Build}-CI{buildNumber:00000}";
            return adjustedVersion;
        }

        /// <summary>
        /// Get the version number from an assembly in 4-digit format with the current build number appended.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="file">The binary to read from</param>
        /// <param name="buildNumber">The build number as provided by the build server</param>
        /// <returns>Version number in the format '0.0.0.0-CI00000</returns>
        [CakeMethodAlias]
        public static string GetFullVersionNumberWithContinuesIntegrationNumberAppended(this ICakeContext context, FilePath file, int buildNumber)
        {
            var version = GetVersionFromFile(context, file);
            var adjustedVersion = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}-CI{buildNumber:00000}";
            return adjustedVersion;
        }

        /// <summary>
        /// Retrieve version information from a binary
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="file">The binary to read from</param>
        /// <returns>Version</returns>
        private static Version GetVersionFromFile(ICakeContext context, FilePath file)
        {
            var filePath = file.MakeAbsolute(context.Environment).FullPath;
            var version = AssemblyName.GetAssemblyName(filePath).Version;
            return version;
        }
    }
}