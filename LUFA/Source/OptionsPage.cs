using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    [System.ComponentModel.DesignerCategory("")]
    public class OptionsPage : DialogPage
    {
        private bool _easterEgg = true;

        [DisplayName("Enable Easter Egg")]
        [Description("Make the after-work world a slightly more whimsical place.")]
        public bool EasterEgg
        {
            get { return _easterEgg; }
            set { _easterEgg = value; }
        }
    }
}
