using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using VSHelp = Microsoft.VisualStudio.VSHelp;

namespace FourWalledCubicle.LUFA.Pages
{
    public partial class GettingStarted : UserControl
    {
        private readonly DTE _DTE;
        private readonly VSHelp.Help _helpService;

        public GettingStarted()
        {
            _DTE = Package.GetGlobalService(typeof(DTE)) as DTE;
            _helpService = Package.GetGlobalService(typeof(VSHelp.SVsHelp)) as VSHelp.Help;

            InitializeComponent();

            ExtensionInformation.LUFA.ReleaseTypes releaseType;
            string versionString = ExtensionInformation.LUFA.GetVersion(out releaseType);

            if (versionString != null)
            {
                Run versionTextRun = new Run();
                versionTextRun.FontWeight = FontWeights.Bold;
                versionTextRun.FontSize = 12;

                versionTextRun.Text = String.Format("({0} {1})",
                    (releaseType == ExtensionInformation.LUFA.ReleaseTypes.Test) ? "Test Release" : "Version",
                    versionString);

                FooterText.Inlines.InsertAfter(FooterText.Inlines.LastInline, new LineBreak());
                FooterText.Inlines.InsertAfter(FooterText.Inlines.LastInline, versionTextRun);
            }
        }

        private void NewExampleWizard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                _DTE.ExecuteCommand("File.ExampleProject", "");
            }
            catch { }
        }

        private void AuthorBlog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _DTE.ItemOperations.Navigate(@"http://www.fourwalledcubicle.com/blog");
        }

        private void AuthorWebsite_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _DTE.ItemOperations.Navigate(@"http://www.fourwalledcubicle.com");
        }

        private void LUFAMailingList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _DTE.ItemOperations.Navigate(@"http://www.lufa-lib.org/support");
        }

        private void OpenIntHelp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _helpService.DisplayTopicFromF1Keyword("LUFA.Index");
        }

        private void ReinstallIntHelp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.REINSTALL_HELP);
        }

        private void OnlineHelp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ExtensionInformation.LUFA.ReleaseTypes releaseType;
            string versionString = ExtensionInformation.LUFA.GetVersion(out releaseType);

            _DTE.ItemOperations.Navigate(string.Format(@"http://www.lufa-lib.org/documentation/{0}/html", versionString));
        }

        private void ChangeLog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _helpService.DisplayTopicFromF1Keyword("LUFA.Page.ChangeLog");
        }

        private void MigrationNotes_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _helpService.DisplayTopicFromF1Keyword("LUFA.Page.Migration");
        }

        private void KnownIssues_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _helpService.DisplayTopicFromF1Keyword("LUFA.Page.KnownIssues");
        }

        private void License_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _helpService.DisplayTopicFromF1Keyword("LUFA.Page.LicenseInfo");
        }

        private void Donate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _helpService.DisplayTopicFromF1Keyword("LUFA.Page.Donating");
        }

        private void DockPanel_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => PageScroller.ScrollToTop()));
        }
    }
}
