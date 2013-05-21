using System;
using System.Windows.Controls;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using VSHelp = Microsoft.VisualStudio.VSHelp;

namespace FourWalledCubicle.LUFA.Pages
{
    public partial class GettingStarted : UserControl
    {
        private readonly DTE mDTE;

        public GettingStarted()
        {
            mDTE = Package.GetGlobalService(typeof(DTE)) as DTE;

            InitializeComponent();
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
            VSHelp.Help helpService = Package.GetGlobalService(typeof(VSHelp.SVsHelp)) as VSHelp.Help;
            helpService.DisplayTopicFromF1Keyword("Atmel.Language.C.LUFA.Index");
        }

        private void ReinstallIntHelp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            HelpInstallManager.DoHelpAction(HelpInstallManager.HelpAction.REINSTALL_HELP);
        }

        private void OnlineHelp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mDTE.ItemOperations.Navigate(@"http://www.lufa-lib.org/documentation");
        }

        private void DockPanel_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
           this.Dispatcher.Invoke(new Action( () => PageScroller.ScrollToTop() ));
        }
    }
}
