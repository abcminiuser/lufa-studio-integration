using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    [Guid("C02047B1-FD51-456B-95D1-6FF77A2A1894")]
    class GettingStartedPageToolWindow : ToolWindowPane
    {
        public GettingStartedPageToolWindow() : base(null)
        {
            this.Caption = "LUFA - Getting Started";
            base.Content = new Pages.GettingStarted();
            ResetScrollPosition();
        }

        public void ResetScrollPosition()
        {
           (this.Content as Pages.GettingStarted).PageScroller.ScrollToTop();
        }
    }
}
