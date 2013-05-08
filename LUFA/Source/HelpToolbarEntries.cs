using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using Atmel.Studio.Services;
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
            public const int btnProjectPage = 0x0104;
            public const int btnMailingList = 0x0105;
            public const int btnDocumentation = 0x0106;
            public const int btnReinstallLocalHelp = 0x0107;
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
                IDialogService dialogService = ATServiceProvider.DialogService;
                if (dialogService == null)
                    return;

                dialogService.ShowDialog(null,
                    "Could not find LUFA Getting Started guide.", "LUFA Library",
                    DialogButtonSet.Ok, DialogIcon.Error);
            }
        }

        public void ReinstallLocalHelp()
        {
            HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.REINSTALL_HELP);
        }
    }
}
