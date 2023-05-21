using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CatDodger
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Logic logic { get; set; }
        public bool paused;
        public MainPage()
        {
            this.InitializeComponent();
            logic = new Logic(masterGrid);
            PauseGameBtn.IsEnabled = false;
            SaveGameBtn.IsEnabled = false;
            GameSetUpView();
        }

        private void NewGameBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            cnvs.Children.Clear();
            msgGrid.Visibility = Visibility.Collapsed;
            cnvs.Visibility = Visibility.Visible;
            logic.StartGame();
            PauseGameBtn.IsEnabled = true;
            SaveGameBtn.IsEnabled = true;
            logic.gamePaused = false;
        }
        private void SaveBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!logic.gamePaused)
            {
                logic.timer.Stop();
                PauseGameBtn.Content = "\uE768";
                logic.PicPauseChange();
                logic.gamePaused = true;
            }
            logic.SaveGame();
        }
        private void loadBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            cnvs.Children.Clear();
            logic.LoadGame();
            msgGrid.Visibility = Visibility.Collapsed;
            cnvs.Visibility = Visibility.Visible;
        }
        public void pauseBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!logic.gamePaused)
            {
                logic.timer.Stop();
                PauseGameBtn.Content = "\uE768";
                logic.PicPauseChange();
                logic.gamePaused = true;
            }
            else
            {
                logic.timer.Start();
                PauseGameBtn.Content = "\uE769";
                logic.gamePaused = false;
            }
        }
        private void ExitBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.Exit();
        }
        public void GameSetUpView()
        {
            msgGrid.Visibility = Visibility.Visible;
            gameOverTbl.Visibility = Visibility.Collapsed;
            winLoseTbl.Text = "Welcome";
            msgImg.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/CatSit1.png"));
        }
    }
}