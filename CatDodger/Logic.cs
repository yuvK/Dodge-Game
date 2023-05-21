using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Color = Windows.UI.Color;

namespace CatDodger
{
    internal class Logic
    {
        public Board brd { get; set; }
        public Grid msgGrid { get; set; }
        public TextBlock winLoseTbl { get; set; }
        public TextBlock enemiesLeftTbl { get; set; }
        public TextBlock gameOverTbl { get; set; }
        public Image msgImg { get; set; }
        public Button PauseGameBtn { get; set; }
        public DispatcherTimer timer { get; set; }
        public DispatcherTimer picFadeTimer { get; set; }
        bool isUP, isDown, isLeft, isRight;
        public bool gamePaused { get; set; }

        AudioManager audioManager = new AudioManager();
        public Logic(Grid masterGrid)
        {
            msgGrid = (Grid)masterGrid.FindName("msgGrid");
            msgImg = (Image)masterGrid.FindName("msgImg");
            enemiesLeftTbl = (TextBlock)masterGrid.FindName("enemiesLeftTbl");
            winLoseTbl = (TextBlock)masterGrid.FindName("winLoseTbl");
            gameOverTbl = (TextBlock)masterGrid.FindName("gameOverTbl");
            PauseGameBtn = (Button)masterGrid.FindName("PauseGameBtn");

            gamePaused = true;

            brd = new Board(masterGrid);
            isUP = isDown = isLeft = isRight = false;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            //game main timer
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += Timer_Tick;
            //dead dogs pic timer
            picFadeTimer = new DispatcherTimer();
            picFadeTimer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            picFadeTimer.Tick += PicFadeTimer_Tick;
            
            while (gamePaused)
            {
                audioManager.inGameLoop.Pause();
                audioManager.themeSong.Play();
                break;
            }
            while (!gamePaused)
            {
                audioManager.themeSong.Pause();
                audioManager.inGameLoop.Play();
                break;
            }

            brd.deadEnemies = new List<Enemy>(); // list for dead dogs pics fade
        }
        public void StartGame()
        {
            brd.InitBoard();
            timer.Start();
            picFadeTimer.Start();
            audioManager.themeSong.Pause();
            audioManager.inGameLoop.Play();
            gamePaused = false;
        }
        public void Timer_Tick(object sender, object e)
        {
            PlayerMovement();
            EnemyMovement();
            Collide();
            brd.PlayerOutOfCanvas();
        }
        private void PicFadeTimer_Tick(object sender, object e)
        {
            foreach (Enemy enemy in brd.deadEnemies)
            {
                enemy.img.Opacity -= 0.1;
            }
        }
        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    isUP = false;
                    break;
                case Windows.System.VirtualKey.Down:
                    isDown = false;
                    break;
                case Windows.System.VirtualKey.Left:
                    isLeft = false;
                    break;
                case Windows.System.VirtualKey.Right:
                    isRight = false;
                    break;

            }
        }
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    isUP = true;
                    break;
                case Windows.System.VirtualKey.Down:
                    isDown = true;
                    break;
                case Windows.System.VirtualKey.Left:
                    isLeft = true;
                    break;
                case Windows.System.VirtualKey.Right:
                    isRight = true;
                    break;
                case Windows.System.VirtualKey.Space:
                    if (!gamePaused)
                    {
                        timer.Stop();
                        PauseGameBtn.Content = "\uE768";
                        PicPauseChange();
                        audioManager.themeSong.Play();
                        audioManager.inGameLoop.Pause();
                        gamePaused = true;
                    }
                    else
                    {
                        audioManager.themeSong.Pause();
                        audioManager.inGameLoop.Play();
                        timer.Start();
                        PauseGameBtn.Content = "\uE769";
                        gamePaused = false;
                    }
                    break;
            }
        }
        public void PlayerMovement()
        {
            if (isUP)
            {
                brd.player.Y -= brd.player.speed;
                brd.PlayerPicSwapUpDown();
            }
            if (isDown)
            {
                brd.player.Y += brd.player.speed;
                brd.PlayerPicSwapUpDown();
            }
            if (isLeft)
            {
                brd.player.X -= brd.player.speed;
                brd.player.img.Source = brd.player.CatLeft;

            }
            if (isRight)
            {
                brd.player.X += brd.player.speed;
                brd.player.img.Source = brd.player.CatRight;
            }
        }
        public void EnemyMovement()
        {
            foreach (Enemy e in brd.enemies)
            {
                double spaceX = brd.player.X - e.X;
                double spaceY = brd.player.Y - e.Y;
                if (spaceX > 0)
                {
                    e.img.Source = e.DogRight;
                    e.X += e.speed;
                }
                else
                {
                    e.img.Source = e.DogLeft;
                    e.X -= e.speed;
                }
                if (spaceY > 0)
                {
                    e.Y += e.speed;

                }
                else
                {
                    e.Y -= e.speed;

                }
            }
        }
        public void Collide()
        {
            if (brd.SameLocation(brd.player))
            {
                for (int i = 0; i < brd.enemies.Count; i++)
                {
                    if (Math.Abs(brd.enemies[i].X - brd.player.X) < brd.player.img.Width * 0.65 &&
                       (Math.Abs(brd.enemies[i].Y - brd.player.Y) < brd.player.img.Height * 0.65))
                    {
                        brd.enemies[i].img.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/DogSleep.png"));
                        brd.deadEnemies.Add(brd.enemies[i]);
                        brd.enemies.Remove(brd.enemies[i]);
                        enemiesLeftTbl.Text = "Enemies : " + brd.enemies.Count;
                        audioManager.playerMinusOneFX.Play();
                        brd.player.lives--;
                        brd.LivesLeftTbl.Text = " | Lives left : " + brd.player.lives.ToString();
                    }
                    if (brd.player.lives == 0)
                    {
                        GameLost();
                        brd.LivesLeftTbl.Visibility = Visibility.Collapsed;
                    }
                    if (brd.enemies.Count <= 1)
                    {
                        GameWon();
                        enemiesLeftTbl.Visibility = Visibility.Collapsed;
                    }
                    if (brd.player.lives == 0 && brd.enemies.Count == 0)
                    {
                        GameDraw();
                    }
                }
            }
            else
            {
                for (int i = 0; i < brd.enemies.Count; i++)
                {
                    if (brd.SameLocation(brd.enemies[i]))
                    {
                        brd.enemies[i].img.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/DogSleep.png"));
                        brd.deadEnemies.Add(brd.enemies[i]);
                        brd.enemies.Remove(brd.enemies[i]);
                        audioManager.enemyDownFX.Play();
                        enemiesLeftTbl.Text = "Enemies : " + brd.enemies.Count;
                        foreach (Enemy enemy in brd.enemies)
                        {
                            enemy.speed += 0.25;
                        }
                        if (brd.enemies.Count <= 1)
                        {
                            GameWon();
                            enemiesLeftTbl.Visibility = Visibility.Collapsed;
                        }
                        if (brd.player.lives == 0 && brd.enemies.Count == 0)
                        {
                            GameDraw();
                        }
                    }
                }
            }
        }
        public void PicPauseChange()
        {
            while (!gamePaused)
            {
                foreach (Enemy enemy in brd.enemies)
                {
                    if (enemy.img.Source == enemy.DogLeft)
                        enemy.img.Source = enemy.DogRunLeft1;
                    if (enemy.img.Source == enemy.DogRight)
                        enemy.img.Source = enemy.DogRunRight1;
                }
                if (brd.player.img.Source == brd.player.CatLeft)
                    brd.player.img.Source = brd.player.catLeft1;
                if (brd.player.img.Source == brd.player.CatRight)
                    brd.player.img.Source = brd.player.catRight1;
                break;
            }
        }
        private void GameWon()
        {
            timer.Stop();
            gamePaused = true;
            audioManager.inGameLoop.Pause();
            audioManager.winFX.Play();
            audioManager.themeSong.Play();
            brd.cnvs.Visibility = Visibility.Collapsed;
            gameOverTbl.Visibility = Visibility.Visible;
            msgGrid.Visibility = Visibility.Visible;
            winLoseTbl.Text = "YOU WIN";
        }
        private void GameLost()
        {
            timer.Stop();
            gamePaused = true;
            audioManager.inGameLoop.Pause();
            audioManager.loseFX.Play();
            audioManager.themeSong.Play();
            brd.cnvs.Visibility = Visibility.Collapsed;
            msgGrid.Visibility = Visibility.Visible;
            gameOverTbl.Visibility = Visibility.Visible;
            winLoseTbl.Text = "YOU LOST";
            msgImg.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/dogs-got-me.png"));
        }
        private void GameDraw()
        {
            gamePaused = true;
            audioManager.inGameLoop.Pause();
            audioManager.themeSong.Play();
            brd.cnvs.Visibility = Visibility.Collapsed;
            msgGrid.Visibility = Visibility.Visible;
            gameOverTbl.Visibility = Visibility.Visible;
            winLoseTbl.Text = "It's a Draw";
            msgImg.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/CatSit2.png"));
        }
        public async void SaveGame()
        {
            //creating file and location for it
            StorageFolder sFolder = ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.CreateFileAsync("Dodge_Save.txt", CreationCollisionOption.ReplaceExisting);
            //assign all board items to list
            List<GamePiece> gamePieces = new List<GamePiece>(brd.enemies);
            gamePieces.Insert(0, brd.player);
            //jason conver to array and assign to file
            var jsonString = JsonConvert.SerializeObject(gamePieces.ToArray()); 
            await FileIO.AppendTextAsync(sFile, jsonString);

            audioManager.inGameLoop.Pause();
            audioManager.themeSong.Play();
            SaveMessage();
        }
        public async void LoadGame()
        {
            StorageFolder sFolder = ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.GetFileAsync("Dodge_Save.txt");
            //getting the save file text
            string fullText = await FileIO.ReadTextAsync(sFile);
            //json to convert array to list
            List<GamePiece> gamePiecesList = JsonConvert.DeserializeObject<GamePiece[]>(fullText).ToList();

            brd.InitBoard(gamePiecesList);
            timer.Start();
            gamePaused = false;
        }
        async void SaveMessage()
        {
            ContentDialog saveMsg = new ContentDialog();
            saveMsg.Title = "Game Saved";
            saveMsg.Content = "OK to close";
            saveMsg.CloseButtonText = "OK";
            saveMsg.Background = new SolidColorBrush(Color.FromArgb(0, 124, 0, 124));
            await saveMsg.ShowAsync();
        }
    }
}