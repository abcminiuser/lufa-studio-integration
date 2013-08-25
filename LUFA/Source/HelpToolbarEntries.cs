using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.VSHelp;

namespace FourWalledCubicle.LUFA
{
    class HelpToolbarEntries
    {
        private readonly DTE mDTE;
        private readonly Help mHelpService;
        private readonly OleMenuCommandService mMenuService;
        private readonly LUFAPackage mLUFAPkg;

        internal static class CommandIDs
        {
            public const int btnGettingStarted = 0x0103;
            public const int btnShowLocalHelp = 0x0104;
            public const int btnProjectPage = 0x0105;
            public const int btnMailingList = 0x0106;
            public const int btnDocumentation = 0x0107;
            public const int btnReinstallLocalHelp = 0x0108;
        }

        public HelpToolbarEntries(OleMenuCommandService menuService, LUFAPackage LUFAPkg)
        {
            mDTE = Package.GetGlobalService(typeof(DTE)) as DTE;
            mHelpService = Package.GetGlobalService(typeof(SVsHelp)) as Help;
            mMenuService = menuService;
            mLUFAPkg = LUFAPkg;

            AddToolbarButtonHandler(
                    CommandIDs.btnGettingStarted,
                    (c, a) => { mLUFAPkg.ShowGettingStartedPage(); }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnShowLocalHelp,
                    (c, a) => { mHelpService.DisplayTopicFromF1Keyword("Atmel.Language.C.LUFA.Index"); }
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
                    (c, a) => { HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.REINSTALL_HELP); }
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
    }
}
