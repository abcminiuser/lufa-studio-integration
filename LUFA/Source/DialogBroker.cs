using System;
using System.Linq;
using System.Windows.Forms;
using Atmel.Studio.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    abstract class DialogBroker
    {
        private class ModalDialogHandle : IWin32Window
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
        
        public enum Icon
        {
            Information,
            Warning
        };

        private static DialogIcon IconToAtmel(Icon Icon)
        {
            if (Icon == DialogBroker.Icon.Information)
                return DialogIcon.Information;
            else if (Icon == DialogBroker.Icon.Warning)
                return DialogIcon.Warning;
            else
                return DialogIcon.None;            
        }

        private static MessageBoxIcon IconToWinForms(Icon Icon)
        {
            if (Icon == DialogBroker.Icon.Information)
                return MessageBoxIcon.Information;
            else if (Icon == DialogBroker.Icon.Warning)
                return MessageBoxIcon.Warning;
            else
                return MessageBoxIcon.None;
        }

        /* Older shell versions' dialog service has bugs - no icons and causes window closing issues */
        private static readonly string[] _incompatibleShellVersions = { "6.0", "6.1" };

        public static void ShowDialog(string Message, Icon Icon)
        {
            if (_incompatibleShellVersions.Contains(ExtensionInformation.Shell.Version))
                MessageBox.Show(new ModalDialogHandle(), Message, @"LUFA Library", MessageBoxButtons.OK, IconToWinForms(Icon));
            else
                ATServiceProvider.DialogService.ShowDialog(null, Message, @"LUFA Library", DialogButtonSet.Ok, IconToAtmel(Icon));
        }
    }
}
