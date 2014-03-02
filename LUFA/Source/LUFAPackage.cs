using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
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
            if (ExtensionInformation.LUFA.Updated || _isFirstRun)
            {
                WarnIfOldASFVersion();
                ShowGettingStartedPage();

                if (_isFirstRun)
                    HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.INSTALL_HELP);
                else
                    HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.REINSTALL_HELP);
            }
        }

        public void WarnIfOldASFVersion()
        {
            Version asfVersion = ExtensionInformation.ASF.Version;
            Version recommendedASFVersion = ExtensionInformation.ASF.Mininimum;

            if ((asfVersion != null) && (asfVersion < recommendedASFVersion))
            {
                DialogBroker.ShowDialog(
                    @"LUFA relies on the Atmel Software Framework (ASF) extension for its project and module management." +
                    Environment.NewLine + Environment.NewLine +
                    string.Format(@"An installed ASF version of {0}.{1} or later is recommended, however you have version {2}.{3} installed.",
                        recommendedASFVersion.Major, recommendedASFVersion.Minor,
                        asfVersion.Major, asfVersion.Minor) +
                    Environment.NewLine + Environment.NewLine +
                    @"Using this version of ASF with LUFA may result in issues with project management; please update if possible from the Atmel Gallery.",
                    DialogBroker.Icon.Warning);
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
