using System;
using System.Linq;
using System.Windows.Forms;
using Atmel.Studio.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        private class IconMap
        {
            public LUFADialogIcon Abstract;
            public DialogIcon Atmel;
            public MessageBoxIcon WinForms;
        };

        private static readonly List<IconMap> _buttonMap = new List<IconMap>
            (new[] {
             new IconMap{ Abstract = LUFADialogIcon.Information, Atmel = DialogIcon.Information, WinForms = MessageBoxIcon.Information },
             new IconMap{ Abstract = LUFADialogIcon.Warning,     Atmel = DialogIcon.Warning    , WinForms = MessageBoxIcon.Warning     },
             new IconMap{ Abstract = LUFADialogIcon.None,        Atmel = DialogIcon.None       , WinForms = MessageBoxIcon.None        },
            });

        /* Older shell versions' dialog service has bugs - no icons and causes window closing issues */
        private static readonly string[] _incompatibleShellVersions = { "6.0", "6.1" };

        public enum LUFADialogIcon
        {
            Information,
            Warning,
            None
        };

        public static void ShowDialog(string Message, LUFADialogIcon Icon)
        {
            IconMap iconInfo = null;

            try
            {
                iconInfo = _buttonMap.First((i) => { return i.Abstract == Icon; });
            }
            catch (InvalidOperationException)
            {
                iconInfo = _buttonMap.First((i) => { return i.Abstract == LUFADialogIcon.None; });
            }

            if (_incompatibleShellVersions.Contains(ExtensionInformation.Shell.Version))
                MessageBox.Show(new ModalDialogHandle(), Message, @"LUFA Library", MessageBoxButtons.OK, iconInfo.WinForms);
            else
                ATServiceProvider.DialogService.ShowDialog(null, Message, @"LUFA Library", DialogButtonSet.Ok, iconInfo.Atmel);
        }
    }
}
