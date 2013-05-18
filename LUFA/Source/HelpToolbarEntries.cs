using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.ExtensionManager;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    class HelpToolbarEntries
    {
        private readonly DTE mDTE;
        private readonly OleMenuCommandService mMenuService;

        internal static class CommandIDs
        {
            public const int btnGettingStarted = 0x0103;
            public const int btnShowLocalHelp = 0x0104;
            public const int btnProjectPage = 0x0105;
            public const int btnMailingList = 0x0106;
            public const int btnDocumentation = 0x0107;
            public const int btnReinstallLocalHelp = 0x0108;
        }

        public HelpToolbarEntries(DTE dte, OleMenuCommandService menuService)
        {
            mDTE = dte;
            mMenuService = menuService;

            AddToolbarButtonHandler(
                    CommandIDs.btnGettingStarted,
                    (c, a) => { ShowGettingStarted(); }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnShowLocalHelp,
                    (c, a) => { ShowLocalHelp(); }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnProjectPage,
                    (c, a) => { mDTE.ItemOperations.Navigate(@"http://www.lufa-lib.org"); }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnMailingList,
                    (c, a) => { mDTE.ItemOperations.Navigate(@"http://www.lufa-lib.org/support"); }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnDocumentation,
                    (c, a) => { mDTE.ItemOperations.Navigate(@"http://www.lufa-lib.org/documentation"); }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnReinstallLocalHelp,
                    (c, a) => { ReinstallLocalHelp(); }
                );        
        }

        private void AddToolbarButtonHandler(int commandID, EventHandler callback)
        {
            CommandID btnCommandID = new CommandID(GuidList.guidLUFACmdSet, commandID);
            if (btnCommandID == null)
                return;

            MenuCommand btnMenuCommand = new MenuCommand(callback, btnCommandID);
            if (btnMenuCommand == null)
                return;

            mMenuService.AddCommand(btnMenuCommand);
        }

        public void ShowGettingStarted()
        {
            IVsExtensionManager extensionManagerService = Package.GetGlobalService(typeof(SVsExtensionManager)) as IVsExtensionManager;
            if (extensionManagerService == null)
                return;

            string gettingStartedPath = extensionManagerService.GetEnabledExtensionContentLocations("GettingStarted").FirstOrDefault();

            if (File.Exists(gettingStartedPath))
            {
                mDTE.ItemOperations.Navigate(@"file:///" + gettingStartedPath, vsNavigateOptions.vsNavigateOptionsNewWindow);
            }
            else
            {
                MessageBox.Show(new ModalDialogHandle(), "Could not find LUFA Getting Started guide.", "LUFA Library", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowLocalHelp()
        {
            try
            {
                System.Diagnostics.Process.Start(@"ms-xhelp://?method=page&id=LUFALUFA&product=ATMELStudio&productVersion=6.1");
            }
            catch { }
        }

        private void ReinstallLocalHelp()
        {
            HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.REINSTALL_HELP);
        }
    }
}
