using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;

namespace FourWalledCubicle.LUFA
{
    class ModalDialogHandle : IWin32Window
    {
        private readonly DTE _DTE;

        public ModalDialogHandle()
        {
            _DTE = Package.GetGlobalService(typeof(DTE)) as DTE;
        }

        public IntPtr Handle
        {
            get
            {
                return (IntPtr)_DTE.MainWindow.HWnd;
            }
        }
    }
}
