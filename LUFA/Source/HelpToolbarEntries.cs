using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.VSHelp;

namespace FourWalledCubicle.LUFA
{
    class HelpToolbarEntries
    {
        private readonly DTE _DTE;
        private readonly Help _helpService;
        private readonly OleMenuCommandService _menuService;
        private readonly LUFAPackage _LUFAPkg;

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
            _DTE = Package.GetGlobalService(typeof(DTE)) as DTE;
            _helpService = Package.GetGlobalService(typeof(SVsHelp)) as Help;
            _menuService = menuService;
            _LUFAPkg = LUFAPkg;

            AddToolbarButtonHandler(
                    CommandIDs.btnGettingStarted,
                    (c, a) => {
                        _LUFAPkg.ShowGettingStartedPage();
                    }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnShowLocalHelp,
                    (c, a) => {
                        _helpService.DisplayTopicFromF1Keyword("LUFA.Index");
                    }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnProjectPage,
                    (c, a) => {
                        _DTE.ItemOperations.Navigate(@"http://www.lufa-lib.org");
                    }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnMailingList,
                    (c, a) => {
                        _DTE.ItemOperations.Navigate(@"http://www.lufa-lib.org/support");
                    }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnDocumentation,
                    (c, a) => {
                        ExtensionInformation.LUFA.ReleaseTypes releaseType;
                        string versionString = ExtensionInformation.LUFA.GetVersion(out releaseType);

                        _DTE.ItemOperations.Navigate(string.Format(@"http://www.lufa-lib.org/documentation/{0}/html", versionString));
                    }
                );

            AddToolbarButtonHandler(
                    CommandIDs.btnReinstallLocalHelp,
                    (c, a) => {
                        HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.REINSTALL_HELP);
                    }
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

            _menuService.AddCommand(btnMenuCommand);
        }
    }
}
