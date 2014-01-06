using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Atmel.Studio.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace FourWalledCubicle.LUFA
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(GuidList.guidLUFAPkgString)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    [ProvideOptionPageAttribute(typeof(OptionsPage), "Extensions", "LUFA Library", 15600, 1912, true)]
    [ProvideToolWindow(typeof(GettingStartedPageToolWindow), Style = VsDockStyle.MDI, MultiInstances = false)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class LUFAPackage : AtmelVsixPackage
    {
        private readonly DTE _DTE;
        private readonly DTEEvents _DTEEvents;

        private HelpToolbarEntries _helpLinks;
        private EasterEgg _easterEgg;

        private bool _isFirstRun = false;

        public LUFAPackage() : base(GuidList.guidLUFAVSIXManifestString)
        {
            _DTE = Package.GetGlobalService(typeof(DTE)) as DTE;

            _DTEEvents = _DTE.Events.DTEEvents;
            _DTEEvents.OnStartupComplete += new _dispDTEEvents_OnStartupCompleteEventHandler(DTEEvents_OnStartupComplete);      
        }

        protected override void DoInstallActions()
        {
            base.DoInstallActions();

            _isFirstRun = true;
        }

        protected override void DoUninstallActions()
        {
            base.DoUninstallActions();

            HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.UNINSTALL_HELP);
        }

        protected override void PackageInitialize()
        {
            base.PackageInitialize();

            OptionsPage settings;

            try
            {
                settings = GetDialogPage(typeof(OptionsPage)) as OptionsPage;
            }
            catch
            {
                settings = new OptionsPage();
            }

            OleMenuCommandService menuService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            
            _helpLinks = new HelpToolbarEntries(menuService, this);
            _easterEgg = new EasterEgg(settings);
        }

        private void DTEEvents_OnStartupComplete()
        {
            if (ExtensionInformation.IsUpdated() || _isFirstRun)
            {
                ShowGettingStartedPage();

                if (_isFirstRun)
                    HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.INSTALL_HELP);
            }
        }

        public void ShowGettingStartedPage()
        {
            GettingStartedPageToolWindow gettingStartedWindow =
                (FindToolWindow(typeof(GettingStartedPageToolWindow), 0, true) as GettingStartedPageToolWindow);

            if ((gettingStartedWindow != null) && (gettingStartedWindow.Frame != null))
            {
                IVsWindowFrame gettingStartedWindowFrame = (IVsWindowFrame)gettingStartedWindow.Frame;

                gettingStartedWindowFrame.Show();
                gettingStartedWindow.ForceMDIDock();
                gettingStartedWindow.ResetScrollPosition();
            }
        }
    }
}
