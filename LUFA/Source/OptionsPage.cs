using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    public class OptionsPage : DialogPage
    {
        private bool mEasterEgg = true;

        [DisplayName("Enable Easter Egg")]
        [Description("Make the after-work world a slightly more whimsical place.")]
        public bool EasterEgg
        {
            get { return mEasterEgg; }
            set { mEasterEgg = value; }
        }
    }
}
