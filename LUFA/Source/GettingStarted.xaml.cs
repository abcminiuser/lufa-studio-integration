using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using EnvDTE;
using Microsoft.VisualStudio.ExtensionManager;
using Microsoft.VisualStudio.Shell;
using VSHelp = Microsoft.VisualStudio.VSHelp;

namespace FourWalledCubicle.LUFA.Pages
{
    public partial class GettingStarted : UserControl
    {
        private readonly DTE mDTE;
        private readonly VSHelp.Help mHelpService;

        public GettingStarted()
        {
            mDTE = Package.GetGlobalService(typeof(DTE)) as DTE;
            mHelpService = Package.GetGlobalService(typeof(VSHelp.SVsHelp)) as VSHelp.Help;

            InitializeComponent();

            IVsExtensionManager extensionManagerService = Package.GetGlobalService(typeof(SVsExtensionManager)) as IVsExtensionManager;
            if (extensionManagerService != null)
            {
                IInstalledExtension lufaExt = null;
                if (extensionManagerService.TryGetInstalledExtension(GuidList.guidLUFAVSIXManifestString, out lufaExt))
                {
                    string[] lufaVersionSegments = lufaExt.Header.Version.ToString().Split('.');

                    Run versionTextRun = new Run();
                    versionTextRun.FontWeight = System.Windows.FontWeights.Bold;
                    versionTextRun.FontSize = 12;

                    if (lufaVersionSegments.First().Equals("0"))
                    {
                        versionTextRun.Text += " (Test Release " + lufaVersionSegments[1] + ")";
                    }
                    else
                    {
                        versionTextRun.Text += " (Version " + lufaVersionSegments.Last() + ")";
                    }

                    FooterText.Inlines.InsertAfter(FooterText.Inlines.LastInline, new LineBreak());
                    FooterText.Inlines.InsertAfter(FooterText.Inlines.LastInline, versionTextRun);
                }
            }
        }

        private void NewExampleWizard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                mDTE.ExecuteCommand("File.ExampleProject", "");
            }
            catch { }
        }

        private void AuthorBlog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mDTE.ItemOperations.Navigate(@"http://www.fourwalledcubicle.com/blog");
        }

        private void AuthorWebsite_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mDTE.ItemOperations.Navigate(@"http://www.fourwalledcubicle.com");
        }

        private void LUFAMailingList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mDTE.ItemOperations.Navigate(@"http://www.lufa-lib.org/support");
        }

        private void OpenIntHelp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mHelpService.DisplayTopicFromF1Keyword("Atmel.Language.C.LUFA.Index");
        }

        private void ReinstallIntHelp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.REINSTALL_HELP);
        }

        private void OnlineHelp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mDTE.ItemOperations.Navigate(@"http://www.lufa-lib.org/documentation");
        }

        private void ChangeLog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mHelpService.DisplayTopicFromF1Keyword("Atmel.Language.C.LUFA.Page.ChangeLog");
        }

        private void MigrationNotes_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mHelpService.DisplayTopicFromF1Keyword("Atmel.Language.C.LUFA.Page.Migration");
        }

        private void KnownIssues_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mHelpService.DisplayTopicFromF1Keyword("Atmel.Language.C.LUFA.Page.KnownIssues");
        }

        private void License_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mHelpService.DisplayTopicFromF1Keyword("Atmel.Language.C.LUFA.Page.LicenseInfo");
        }

        private void Donate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mHelpService.DisplayTopicFromF1Keyword("Atmel.Language.C.LUFA.Page.Donating");
        }

        private void DockPanel_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => PageScroller.ScrollToTop()));
        }
    }
}
