using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    abstract class HelpInstallManager
    {
        public enum HelpAction
        {
            INSTALL_HELP,
            UNINSTALL_HELP,
            REINSTALL_HELP,
        };

        private static string GetHelpManagerPath()
        {
            string helpRootFolder = @"C:\Program Files\Microsoft Help Viewer\v1.0\";

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"Software\\Microsoft\\Help\\v1.0");
                helpRootFolder = (string)registryKey.GetValue("AppRoot");
            }
            catch { }

            return Path.Combine(helpRootFolder, "HelpLibManager.exe");
        }

        private static string GetShellName()
        {
            string productName = @"AtmelStudio";

            try
            {
                EnvDTE.DTE packageDTE = Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(packageDTE.RegistryRoot + "_Config");
                productName = (string)registryKey.GetValue("AppName");
            }
            catch { }

            return productName;
        }

        private static string GetShellVersion()
        {
            string productName = @"6.1";

            try
            {
                EnvDTE.DTE packageDTE = Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(packageDTE.RegistryRoot + "_Config");
                productName = (string)registryKey.GetValue("ProductVersion");
            }
            catch { }

            return productName;
        }

        private static void AddRemoveHelp(HelpAction action)
        {
            string helpPackagePath = ExtensionInformation.GetContentLocation("MSHelp");
            string helpManagerArguments = string.Format(@"/product ""{0}"" /version ""{1}"" /locale en-us", GetShellName(), GetShellVersion());

            if (helpPackagePath == null)
                return;

            switch (action)
            {
                case HelpAction.INSTALL_HELP:
                    helpManagerArguments += string.Format(@" /brandingPackage AtmelHelp3Branding.mshc /sourceMedia ""{0}""", helpPackagePath);
                    break;
                case HelpAction.UNINSTALL_HELP:
                    helpManagerArguments += @" /silent /uninstall /vendor ""FourWalledCubicle"" /ProductName ""LUFA"" /mediaBookList ""LUFA Help""";
                    break;
                default:
                    throw new NotImplementedException();
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = GetHelpManagerPath();
            startInfo.Arguments = helpManagerArguments;
            startInfo.Verb = "runas";

            try
            {
                Process p = Process.Start(startInfo);

                if ((p != null) && (action == HelpAction.UNINSTALL_HELP))
                    p.WaitForExit();
            }
            catch { }
        }

        private static void ShowHelpInstallMessage()
        {
            MessageBox.Show(new ModalDialogHandle(),
                @"LUFA contains an integrated Atmel Studio help package, however it is unsigned and must be manually installed." +
                Environment.NewLine + Environment.NewLine +
                @"If you wish to install the LUFA help package click the ""Add"" link next to the LUFA entry in the Microsoft " +
                @"Help Manager Wizard that will show when this box is closed, click the ""Update"" button and follow the wizard prompts.",
                @"LUFA Library",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void DoHelpAction(HelpAction action)
        {
            switch (action)
            {
                case HelpAction.INSTALL_HELP:
                    ShowHelpInstallMessage();
                    AddRemoveHelp(HelpAction.INSTALL_HELP);
                    break;
                case HelpAction.UNINSTALL_HELP:
                    AddRemoveHelp(HelpAction.UNINSTALL_HELP);
                    break;
                case HelpAction.REINSTALL_HELP:
                    AddRemoveHelp(HelpAction.UNINSTALL_HELP);
                    ShowHelpInstallMessage();
                    AddRemoveHelp(HelpAction.INSTALL_HELP);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
