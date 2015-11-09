using System;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.ExtensionManager;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;

namespace FourWalledCubicle.LUFA
{
    public abstract class ExtensionInformation
    {
        public abstract class Shell
        {
            public static string Name
            {
                get
                {
                    string productName = @"AtmelStudio";

                    try
                    {
                        DTE packageDTE = Package.GetGlobalService(typeof(DTE)) as DTE;
                        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(packageDTE.RegistryRoot + "_Config");
                        productName = (string)registryKey.GetValue("AppName");
                    }
                    catch { }

                    return productName;
                }
            }

            public static string Version
            {
                get
                {
                    string productName = @"6.1";

                    try
                    {
                        DTE packageDTE = Package.GetGlobalService(typeof(DTE)) as DTE;
                        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(packageDTE.RegistryRoot + "_Config");
                        productName = (string)registryKey.GetValue("ProductVersion");
                    }
                    catch { }

                    return productName;
                }
            }
        }

        public abstract class LUFA
        {
            public enum ReleaseTypes
            {
                Unknown,
                Normal,
                Test,
            };

            public static bool Updated
            {
                get
                {
                    bool isUpdated = false;

                    const string lufaInstalledVersionKeyName = @"Software\LUFA\AtmelStudioExtension\InstalledVersion";

                    ReleaseTypes currentReleaseType;
                    string currentVersion = GetVersion(out currentReleaseType);

                    try
                    {
                        RegistryKey versionNode = Registry.CurrentUser.OpenSubKey(lufaInstalledVersionKeyName, true);

                        if (versionNode != null)
                        {
                            isUpdated = (versionNode.GetValue("Version").ToString().Equals(currentVersion) == false) ||
                                        (versionNode.GetValue("Type").ToString().Equals(currentReleaseType.ToString()) == false);
                        }
                        else
                        {
                            versionNode = Registry.CurrentUser.CreateSubKey(lufaInstalledVersionKeyName);
                        }

                        versionNode.SetValue("Version", currentVersion);
                        versionNode.SetValue("Type", currentReleaseType.ToString());
                    }
                    catch (Exception e)
                    {
                        Logging.Log(Logging.Severity.Error, "Could not get/set LUFA version in registry: {0}", e.Message);
                    }

                    return isUpdated;
                }
            }

            public static string GetVersion(out ReleaseTypes versionType)
            {
                versionType = ReleaseTypes.Unknown;

                IVsExtensionManager extensionManagerService = Package.GetGlobalService(typeof(SVsExtensionManager)) as IVsExtensionManager;
                if (extensionManagerService == null)
                    return null;

                IInstalledExtension lufaExt = null;
                if (extensionManagerService.TryGetInstalledExtension(GuidList.guidLUFAVSIXManifestString, out lufaExt) == false)
                    return null;

                string[] lufaVersionSegments = lufaExt.Header.Version.ToString().Split('.');

                if (lufaVersionSegments.First().Equals("0"))
                {
                    versionType = ReleaseTypes.Test;
                    return lufaVersionSegments[1];
                }
                else
                {
                    versionType = ReleaseTypes.Normal;
                    return lufaVersionSegments.Last();
                }
            }
        }

        public abstract class ASF
        {
            public static readonly Version Mininimum = new Version(3, 14);

            public static Version Version
            {
                get
                {
                    IVsExtensionManager extensionManagerService = Package.GetGlobalService(typeof(SVsExtensionManager)) as IVsExtensionManager;
                    if (extensionManagerService == null)
                    {
                        Logging.Log(Logging.Severity.Error, "Unable to obtain extension manager service");
                        return null;
                    }

                    IInstalledExtension asfExt = null;
                    if (extensionManagerService.TryGetInstalledExtension(GuidList.guidASFVSIXManifestString, out asfExt) == false)
                    {
                        Logging.Log(Logging.Severity.Error, "Unable to obtain ASF extension information");
                        return null;
                    }

                    return asfExt.Header.Version;
                }
            }
        }

        public static string GetContentLocation(string contentName)
        {
            IVsExtensionManager extensionManagerService = Package.GetGlobalService(typeof(SVsExtensionManager)) as IVsExtensionManager;
            if (extensionManagerService == null)
            {
                Logging.Log(Logging.Severity.Error, "Unable to obtain extension manager service");
                return null;
            }

            IInstalledExtension lufaExt = null;
            if (extensionManagerService.TryGetInstalledExtension(GuidList.guidLUFAVSIXManifestString, out lufaExt) == false)
            {
                Logging.Log(Logging.Severity.Error, "Unable to obtain LUFA extension information");
                return null;
            }

            string contentPath = null;

            try
            {
                string contentRelativePath = lufaExt.Content.Where(c => c.ContentTypeName == contentName).First().RelativePath;
                contentPath = Path.Combine(lufaExt.InstallPath, contentRelativePath);
            }
            catch (Exception e)
            {
                Logging.Log(Logging.Severity.Error, "Could not get content location: {0}", e.Message);
            }

            return contentPath;
        }
    }
}
