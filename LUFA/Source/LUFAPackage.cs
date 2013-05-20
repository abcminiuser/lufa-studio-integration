using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Atmel.Studio.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSHelp = Microsoft.VisualStudio.VSHelp;

namespace FourWalledCubicle.LUFA
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(GuidList.guidLUFAPkgString)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    [ProvideOptionPageAttribute(typeof(OptionsPage), "Extensions", "LUFA Library", 15600, 1912, true)]
    [ProvideToolWindow(typeof(GettingStartedPageToolWindow), Style = VsDockStyle.MDI, Window = "C02047B1-FD51-456B-95D1-6FF77A2A1894", MultiInstances = false)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class LUFAPackage : AtmelVsixPackage
    {
        private readonly DTE mDTE;
        private readonly DTEEvents mDTEEvents;

        private HelpToolbarEntries mHelpLinks;
        private EasterEgg mEasterEgg;

        private bool isFirstRun = false;

        protected override void DoInstallActions()
        {
            base.DoInstallActions();

            isFirstRun = true;
        }

        protected override void DoUninstallActions()
        {
            base.DoUninstallActions();

            HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.UNINSTALL_HELP);
        }

        public LUFAPackage() : base(GuidList.guidLUFAVSIXManifestString)
        {
            mDTE = Package.GetGlobalService(typeof(DTE)) as DTE;

            mDTEEvents = mDTE.Events.DTEEvents;
            mDTEEvents.OnStartupComplete += new _dispDTEEvents_OnStartupCompleteEventHandler(mDTEEvents_OnStartupComplete);      
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
            VSHelp.Help helpService = GetGlobalService(typeof(VSHelp.SVsHelp)) as VSHelp.Help;

            mHelpLinks = new HelpToolbarEntries(mDTE, menuService, helpService, this);
            mEasterEgg = new EasterEgg(mDTE, settings);
        }

        private void mDTEEvents_OnStartupComplete()
        {
            if (isFirstRun)
            {
                ShowGettingStartedPage();

                HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.INSTALL_HELP);
            }
        }

        public void ShowGettingStartedPage()
        {
            GettingStartedPageToolWindow gettingStartedWindow =
                (FindToolWindow(typeof(GettingStartedPageToolWindow), 0, true) as GettingStartedPageToolWindow);

            if ((gettingStartedWindow != null) && (gettingStartedWindow.Frame != null))
            {
                ((IVsWindowFrame)gettingStartedWindow.Frame).Show();
                gettingStartedWindow.ResetScrollPosition();
            }
        }
    }
}
