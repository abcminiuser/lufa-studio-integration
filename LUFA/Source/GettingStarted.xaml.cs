using System.Windows.Controls;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

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

        private void AuthorBlog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mDTE.ItemOperations.Navigate(@"http://www.fourwalledcubicle.com/blog", vsNavigateOptions.vsNavigateOptionsNewWindow);
        }

        private void AuthorWebsite_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mDTE.ItemOperations.Navigate(@"http://www.fourwalledcubicle.com/", vsNavigateOptions.vsNavigateOptionsNewWindow);
        }

        private void LUFAMailingList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            mDTE.ItemOperations.Navigate(@"http://www.lufa-lib.org/support", vsNavigateOptions.vsNavigateOptionsNewWindow);
        }
    }
}
