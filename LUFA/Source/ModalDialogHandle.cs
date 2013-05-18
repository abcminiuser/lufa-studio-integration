using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    class ModalDialogHandle : IWin32Window
    {
        private readonly DTE mDTE;

        public ModalDialogHandle()
        {
            this.mDTE = Package.GetGlobalService(typeof(DTE)) as DTE;
        }

        public System.IntPtr Handle
        {
            get
            {
                return new System.IntPtr(mDTE.MainWindow.HWnd);
            }
        }
    }
}
