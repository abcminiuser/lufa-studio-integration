using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace FourWalledCubicle.LUFA
{
    [Guid("C02047B1-FD51-456B-95D1-6FF77A2A1894")]
    class GettingStartedPageToolWindow : ToolWindowPane
    {
        private Pages.GettingStarted _content;

        public GettingStartedPageToolWindow() : base(null)
        {
            _content = new Pages.GettingStarted();

            this.Caption = "LUFA - Getting Started";
            base.Content = _content;
        }

        public void ResetScrollPosition()
        {
            _content.Dispatcher.Invoke(new Action( () => _content.PageScroller.ScrollToTop() ));
        }

        public void ForceMDIDock()
        {
            IVsWindowFrame gettingStartedWindowFrame = (IVsWindowFrame)this.Frame;

            try
            {
                gettingStartedWindowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_MdiChild);
            }
            catch { }
        }
    }
}
