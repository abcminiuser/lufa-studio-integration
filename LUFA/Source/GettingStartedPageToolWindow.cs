using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    [Guid("C02047B1-FD51-456B-95D1-6FF77A2A1894")]
    class GettingStartedPageToolWindow : ToolWindowPane
    {
        private Pages.GettingStarted mContent;

        public GettingStartedPageToolWindow() : base(null)
        {
            mContent = new Pages.GettingStarted();

            this.Caption = "LUFA - Getting Started";
            base.Content = mContent;
            ResetScrollPosition();
        }

        public void ResetScrollPosition()
        {
            mContent.Dispatcher.Invoke(new Action( () => mContent.PageScroller.ScrollToTop() ));
        }
    }
}
