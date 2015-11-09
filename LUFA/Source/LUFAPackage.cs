using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace FourWalledCubicle.LUFA
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(GuidList.guidLUFAPkgString)]
    [ProvideAutoLoad(UIContextGuids.NoSolution)]
    [ProvideOptionPage(typeof(OptionsPage), "Extensions", "LUFA Library", 15600, 1912, true)]
    [ProvideToolWindow(typeof(GettingStartedPageToolWindow), Style = VsDockStyle.MDI, MultiInstances = false)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class LUFAPackage : Package, IDisposable
    {
        private readonly DTE _DTE;
        private readonly DTEEvents _DTEEvents;

        private HelpToolbarEntries _helpLinks;
        private EasterEgg _easterEgg;

        public LUFAPackage()
        {
            _DTE = Package.GetGlobalService(typeof(DTE)) as DTE;

            _DTEEvents = _DTE.Events.DTEEvents;
            _DTEEvents.OnStartupComplete += new _dispDTEEvents_OnStartupCompleteEventHandler(DTEEvents_OnStartupComplete);

            Logging.Log(Logging.Severity.Information, "LUFA package created");
        }

        public void Dispose()
        {
            _easterEgg.Dispose();
        }

        protected override void Initialize()
        {
            base.Initialize();

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

            Logging.Log(Logging.Severity.Information, "LUFA package initialized");
        }

        private void DTEEvents_OnStartupComplete()
        {
            ExtensionInformation.LUFA.ReleaseTypes releaseType;
            string versionString = ExtensionInformation.LUFA.GetVersion(out releaseType);

            if (ExtensionInformation.LUFA.Updated)
            {
                Logging.Log(Logging.Severity.Information, "LUFA updated to version {0} ({1}), showing getting started and installing help", versionString, releaseType);

                WarnIfOldASFVersion();
                ShowGettingStartedPage();

                HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.INSTALL_HELP);
            }
            else
            {
                Logging.Log(Logging.Severity.Information, "LUFA not updated, installed version {0} ({1})", versionString, releaseType);
            }
        }

        public void WarnIfOldASFVersion()
        {
            Version asfVersion = ExtensionInformation.ASF.Version;
            Version recommendedASFVersion = ExtensionInformation.ASF.Mininimum;

            if (asfVersion == null)
            {
                Logging.Log(Logging.Severity.Information, "No installed ASF extension found, showing user warning");
    
                MessageBox.Show(new ModalDialogHandle(),
                    @"LUFA relies on the Atmel Software Framework (ASF) extension for its project and module management." +
                    Environment.NewLine + Environment.NewLine +
                    @"An installed ASF version was not found; please install the ASF extension from the Atmel Gallery.",
                    @"LUFA Library",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (asfVersion < recommendedASFVersion)
            {
                Logging.Log(Logging.Severity.Information, "ASF extension found, {0} < {1}, showing user warning", asfVersion.ToString(), recommendedASFVersion.ToString());

                MessageBox.Show(new ModalDialogHandle(),
                    @"LUFA relies on the Atmel Software Framework (ASF) extension for its project and module management." +
                    Environment.NewLine + Environment.NewLine +
                    string.Format(@"An installed ASF version of {0}.{1} or later is recommended, however you have version {2}.{3} installed.",
                        recommendedASFVersion.Major, recommendedASFVersion.Minor,
                        asfVersion.Major, asfVersion.Minor) +
                    Environment.NewLine + Environment.NewLine +
                    @"Using this version of ASF with LUFA may result in issues with project management; please update if possible from the Atmel Gallery.",
                    @"LUFA Library",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Logging.Log(Logging.Severity.Information, "ASF extension found, {0} >= {1}, no action required", asfVersion.ToString(), recommendedASFVersion.ToString());
            }
        }

        public void ShowGettingStartedPage()
        {
            GettingStartedPageToolWindow gettingStartedWindow =
                (FindToolWindow(typeof(GettingStartedPageToolWindow), 0, true) as GettingStartedPageToolWindow);

            if ((gettingStartedWindow != null) && (gettingStartedWindow.Frame != null))
            {
                IVsWindowFrame gettingStartedWindowFrame = (IVsWindowFrame)gettingStartedWindow.Frame;

                Logging.Log(Logging.Severity.Information, "Showing Getting Started page");

                gettingStartedWindowFrame.Show();
                gettingStartedWindow.ForceMDIDock();
                gettingStartedWindow.ResetScrollPosition();
            }
            else
            {
                Logging.Log(Logging.Severity.Error, "Unable to show Getting Started page");
            }
        }
    }
}
