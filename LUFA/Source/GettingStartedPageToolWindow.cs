using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

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
            ResetScrollPosition();
        }

        public void ResetScrollPosition()
        {
            _content.Dispatcher.Invoke(new Action( () => _content.PageScroller.ScrollToTop() ));
        }
    }
}
