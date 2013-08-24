using System.IO;
using System.Linq;
using Microsoft.VisualStudio.ExtensionManager;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    abstract class ExtensionInformation
    {
        public enum LUFAReleaseTypes
        {
            Unknown,
            Normal,
            Test,
        };

        public static string GetVersion(out LUFAReleaseTypes versionType)
        {
            versionType = LUFAReleaseTypes.Unknown;

            IVsExtensionManager extensionManagerService = Package.GetGlobalService(typeof(SVsExtensionManager)) as IVsExtensionManager;
            if (extensionManagerService == null)
                return null;

            IInstalledExtension lufaExt = null;
            if (extensionManagerService.TryGetInstalledExtension(GuidList.guidLUFAVSIXManifestString, out lufaExt) == false)
                return null;

            string[] lufaVersionSegments = lufaExt.Header.Version.ToString().Split('.');

            if (lufaVersionSegments.First().Equals("0"))
            {
                versionType = LUFAReleaseTypes.Test;
                return lufaVersionSegments[1];
            }
            else
            {
                versionType = LUFAReleaseTypes.Normal;
                return lufaVersionSegments.Last();
            }
        }

        public static string GetContentLocation(string contentName)
        {
            IVsExtensionManager extensionManagerService = Package.GetGlobalService(typeof(SVsExtensionManager)) as IVsExtensionManager;
            if (extensionManagerService == null)
                return null;

            IInstalledExtension lufaExt = null;
            if (extensionManagerService.TryGetInstalledExtension(GuidList.guidLUFAVSIXManifestString, out lufaExt) == false)
                return null;

            string contentPath = null;

            try
            {
                string contentRelativePath = lufaExt.Content.Where(c => c.ContentTypeName == contentName).First().RelativePath;
                contentPath = Path.Combine(lufaExt.InstallPath, contentRelativePath);
            }
            catch { }

            return contentPath;
        }
    }
}
