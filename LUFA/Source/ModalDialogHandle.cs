using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;

namespace FourWalledCubicle.LUFA
{
    class ModalDialogHandle : IWin32Window
    {
        private readonly DTE mDTE;

        public ModalDialogHandle()
        {
            mDTE = Package.GetGlobalService(typeof(DTE)) as DTE;
        }

        public System.IntPtr Handle
        {
            get
            {
                return (IntPtr)mDTE.MainWindow.HWnd;
            }
        }
    }
}
