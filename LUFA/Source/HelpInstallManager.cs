using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

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

        private static Version HelpVersion = new Version(2, 2);

        private static string GetHelpManagerPath()
        {
            string helpRootFolder = string.Format(@"C:\Program Files\Microsoft Help Viewer\v{0}.{1}\", HelpVersion.Major, HelpVersion.Minor);

            try
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(string.Format(@"Software\\Microsoft\\Help\\v{0}.{1}", HelpVersion.Major, HelpVersion.Minor));
                helpRootFolder = (string)registryKey.GetValue("AppRoot");
            }
            catch { }

            return Path.Combine(helpRootFolder, "HlpCtntMgr.exe");
        }

        private static void AddRemoveHelp(HelpAction action)
        {
            string shellName = ExtensionInformation.Shell.Name;
            string shellVersion = ExtensionInformation.Shell.Version;
            string helpPackagePath = ExtensionInformation.GetContentLocation("MSHelp");

            if (helpPackagePath == null)
                return;

            string helpManagerArguments = string.Format(@"/catalogName {0}{1} /locale en-us", shellName, shellVersion.Replace(".",""));

            switch (action)
            {
                case HelpAction.INSTALL_HELP:
                    helpManagerArguments += string.Format(@" /operation install /sourceuri ""{0}""", helpPackagePath);
                    break;
                case HelpAction.UNINSTALL_HELP:
                    helpManagerArguments += string.Format(@" /operation uninstall /sourceuri ""{0}""", helpPackagePath);
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
                @"LUFA contains an integrated Atmel Studio help package, however it is unsigned and installation must be manually confirmed." +
                Environment.NewLine + Environment.NewLine +
                @"If you wish to install the LUFA help package now, accept the following security prompts from the Microsoft Help Manager" +
                @"that will show when this box is closed. If cancelled, this can be manually re-attemped at a later time from the Help menu.",
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
