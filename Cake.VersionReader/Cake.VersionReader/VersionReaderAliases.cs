using System.Reflection;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.VersionReader
{
    /// <summary>
    /// Version Reader Aliases
    /// </summary>
    [CakeAliasCategory("Version Reader")]
    public static class VersionReaderAliases
    {
        /// <summary>
        /// Get the version number from an assembly.
        /// </summary>
        /// <param name="context">The context</param>
        /// <param name="file">The binary to read from</param>
        /// <returns>Version number in the format '0.0.0.0'</returns>
        [CakeMethodAlias]
        public static string GetVersionNumber(this ICakeContext context, FilePath file)
        {
            var filePath = file.MakeAbsolute(context.Environment).FullPath;
            var version = AssemblyName.GetAssemblyName(filePath).Version;

            return $"{version.Major}.{version.Minor}.{version.Build}";
        }

        /// <summary>
        /// Get the version number with the current build number appended.
        /// This is based on the article found here: http://www.xavierdecoster.com/semantic-versioning-auto-incremented-nuget-package-versions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="file"></param>
        /// <param name="buildNumber"></param>
        /// <returns>Version number in the format '0.0.0.0-CI00000'</returns>
        public static string GetVersionNumberWithContinuesIntegrationNumberAppended(this ICakeContext context, FilePath file, int buildNumber)
        {
            var filePath = file.MakeAbsolute(context.Environment).FullPath;
            var version = AssemblyName.GetAssemblyName(filePath).Version;
            var adjustedVersion = $"{version.Major}.{version.Minor}.{version.Build}-CI{buildNumber.ToString("00000")}";
            return adjustedVersion;
        }
    }
}