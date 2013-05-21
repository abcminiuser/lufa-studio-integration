using System;
using System.Media;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace FourWalledCubicle.LUFA
{
    class EasterEgg
    {
        private readonly DTE mDTE;
        private readonly OptionsPage mSettings;
        private readonly BuildEvents mBuildEvents;

        private readonly SoundPlayer mPlayer;
        private readonly Random mRandom;

        private DateTime mPreviousPlayTime = DateTime.MinValue;

        public EasterEgg(OptionsPage settings)
        {
            mDTE = Package.GetGlobalService(typeof(DTE)) as DTE;
            mSettings = settings;

            mBuildEvents = mDTE.Events.BuildEvents;
            mBuildEvents.OnBuildBegin += new _dispBuildEvents_OnBuildBeginEventHandler(mBuildEvents_OnBuildBegin);
            mBuildEvents.OnBuildDone += new _dispBuildEvents_OnBuildDoneEventHandler(mBuildEvents_OnBuildDone);

            mRandom = new Random();
            mPlayer = new SoundPlayer(Resources.ys);
            mPlayer.LoadAsync();
        }

        void mBuildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            if (mPlayer == null)
                return;

            mPlayer.Stop();
        }

        void mBuildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            if (mPlayer == null)
                return;

            if ((mSettings.EasterEgg == true) && /* Must be enabled */
                (DateTime.Now.Hour >= 20) && /* Must be after 8PM */
                (mRandom.Next(20) == 2) && /* 5% chance of occurance */
                ((DateTime.Now - mPreviousPlayTime).TotalMinutes > 45)) /* Must be at least 45 minutes since last occurance */
            {
                mPreviousPlayTime = DateTime.Now;

                mPlayer.PlayLooping();
                mDTE.StatusBar.Text = "Yackety Sax time, brought to you by LUFA! Turn off via Tools->Options menu, Extensions->LUFA Library->Easter Egg.";
            }
        }    
    }
}
