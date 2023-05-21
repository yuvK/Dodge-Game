using System;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;

namespace CatDodger
{
    internal class AudioManager
    {
        public MediaPlayer inGameLoop { get; set; }
        public MediaPlayer themeSong { get; set; }
        public MediaPlayer loseFX { get; set; }
        public MediaPlayer winFX { get; set; }
        public MediaPlayer enemyDownFX { get; set; }
        public MediaPlayer playerMinusOneFX { get; set; }
        public MediaElement theme { get; set; }
        public AudioManager()
        {
            inGameLoop = new MediaPlayer();
            inGameLoop.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/gameLoop.wav"));
            inGameLoop.IsLoopingEnabled = true;

            themeSong = new MediaPlayer();
            themeSong.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/ThemeSong.wav"));
            themeSong.IsLoopingEnabled = true;

            loseFX = new MediaPlayer();
            loseFX.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/win.wav"));

            winFX = new MediaPlayer();
            winFX.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/fail.wav"));

            enemyDownFX = new MediaPlayer();
            enemyDownFX.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/ting.wav"));

            playerMinusOneFX = new MediaPlayer();
            playerMinusOneFX.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Audio/tang.wav"));
        }
    }
}