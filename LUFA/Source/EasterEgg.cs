using System;
using System.Media;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    class EasterEgg
    {
        private readonly DTE _DTE;
        private readonly OptionsPage _settings;
        private readonly BuildEvents _buildEvents;

        private readonly SoundPlayer _player;
        private readonly Random _random;

        private DateTime _previousPlayTime = DateTime.MinValue;

        public EasterEgg(OptionsPage settings)
        {
            _DTE = Package.GetGlobalService(typeof(DTE)) as DTE;
            _settings = settings;

            _buildEvents = _DTE.Events.BuildEvents;
            _buildEvents.OnBuildBegin += new _dispBuildEvents_OnBuildBeginEventHandler(mBuildEvents_OnBuildBegin);
            _buildEvents.OnBuildDone += new _dispBuildEvents_OnBuildDoneEventHandler(mBuildEvents_OnBuildDone);

            _random = new Random();
            _player = new SoundPlayer(Resources.ys);
            _player.LoadAsync();
        }

        void mBuildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            if (_player == null)
                return;

            _player.Stop();
        }

        void mBuildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            if (_player == null)
                return;

            if ((_settings.EasterEgg == true) && /* Must be enabled */
                (DateTime.Now.Hour >= 20) && /* Must be after 8PM */
                (_random.Next(50) == 1) && /* 2% chance of occurance */
                ((DateTime.Now - _previousPlayTime).TotalMinutes > 45)) /* Must be at least 45 minutes since last occurance */
            {
                _previousPlayTime = DateTime.Now;

                _player.PlayLooping();
                _DTE.StatusBar.Text = "Yackety Sax time, brought to you by LUFA! Turn off via Tools->Options menu, Extensions->LUFA Library->Easter Egg.";
            }
        }    
    }
}
