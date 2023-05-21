using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CatDodger
{
    internal class Board
    {
        public ComboBox enemiesNumCbx { get; set; }
        public Grid masterGrid { get; set; }
        public Canvas cnvs { get; set; }
        public TextBlock enemiesLeftTbl { get; set; }
        public TextBlock LivesLeftTbl { get; set; }
        public Player player { get; set; }
        public List<Enemy> enemies { get; set; }
        public List<Enemy> deadEnemies { get; set; }
        public int enemiesNumber {get; set; }
        public Board(Grid masterGrid)
        {
            this.masterGrid = masterGrid;
            cnvs = (Canvas)masterGrid.FindName("cnvs");
            enemiesLeftTbl = (TextBlock)masterGrid.FindName("enemiesLeftTbl");
            LivesLeftTbl = (TextBlock)masterGrid.FindName("LivesLeftTbl");
            enemiesNumCbx = (ComboBox)masterGrid.FindName("enemiesNumCbx");
        }
        public void InitBoard(List<GamePiece> pieces = null)
        {
            enemiesNumber = SetEnemyNumber();
            bool isLoad = pieces != null;
            player = new Player();
            enemies = new List<Enemy>();
            //init board from new game or load game
            if (pieces == null)
            {
                for (int i = 0; i < enemiesNumber; i++)
                {
                    Enemy enemy = new Enemy();
                    enemies.Add(enemy);
                    CreatePiece(enemy, isLoad);
                }
            }
            else
            {
                player = new Player(pieces[0]); //get the player from save list
                pieces.RemoveAt(0); 
                foreach (GamePiece piece in pieces) //getting the saved enemies
                {
                    Enemy enemy = new Enemy(piece);
                    enemies.Add(enemy);
                    CreatePiece(enemy, isLoad);
                }
            }
            CreatePiece(player, isLoad);
            
            enemiesLeftTbl.Text = "Enemies : " + enemies.Count;
            enemiesLeftTbl.Visibility = Visibility.Visible;
            LivesLeftTbl.Text = " | Lives left : " + player.lives ;
            LivesLeftTbl.Visibility = Visibility.Visible;


        }
        private void CreatePiece(GamePiece piece, bool isLoad)
        {
            if (!isLoad)  //check if pieces loaded already
                SetLocation(piece);
            if (SameLocation(piece))
            {
                CreatePiece(piece, isLoad);
            }
            else
            {
                cnvs.Children.Add(piece.img);
            }
        }
        public void SetLocation(GamePiece piece)
        {
            Random rnd = new Random();
            piece.X = rnd.Next((int)(cnvs.ActualWidth - piece.img.Width));
            piece.Y = rnd.Next((int)(cnvs.ActualHeight - piece.img.Height));
        }
        public bool SameLocation(GamePiece piece)
        {
            foreach (Enemy enemy in enemies)
            {
                if (piece == enemy)
                {
                    continue;
                }
                if (Math.Abs(enemy.X - piece.X) < piece.img.Width * 0.65 &&
                   (Math.Abs(enemy.Y - piece.Y) < piece.img.Height * 0.65))
                {
                    return true;
                }
            }
            return false;
        }
        public void PlayerOutOfCanvas()
        {
            if (player.X < 0.0)
                player.X = 0.0;
            if (player.X > cnvs.ActualWidth - player.img.Width)
                player.X = cnvs.ActualWidth - player.img.Width;
            if (player.Y < 0.0)
                player.Y = 0.0;
            if (player.Y > cnvs.ActualHeight - player.img.Height)
                player.Y = cnvs.ActualHeight - player.img.Height;
        }
        public void PlayerPicSwapUpDown()
        {  //making sure player has the correct pic when moving up and down
            if (player.img.Source == player.catLeft1 || player.img.Source == player.catLeft2) player.img.Source = player.CatLeft;
            if (player.img.Source == player.catRight1 || player.img.Source == player.catRight2) player.img.Source = player.CatRight;
        }
        public int SetEnemyNumber()
        {
            return enemiesNumCbx.SelectedIndex + 2;
        }
    }
}
