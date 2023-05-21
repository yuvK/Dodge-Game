using Newtonsoft.Json;
using System;
using Windows.UI.Xaml.Media.Imaging;

namespace CatDodger
{
    internal class Player : GamePiece
    {
        [JsonIgnore]
        public BitmapImage catRight1 = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/CatWalkRight1.png"));
        [JsonIgnore]
        public BitmapImage catRight2 = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/CatWalkRight2.png"));
        [JsonIgnore]
        public BitmapImage catLeft1 = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/CatWalkLeft1.png"));
        [JsonIgnore]
        public BitmapImage catLeft2 = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/CatWalkLeft2.png"));     
        [JsonIgnore]
        public BitmapImage CatLeft = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/catLeft.gif"));
        [JsonIgnore]
        public BitmapImage CatRight = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/catRight.gif"));
       
        public Player()
        {
            img.Source = catLeft1;
            speed = 3;
            lives = 3;
        }
        public Player(GamePiece piece)
        {
            this.X = piece.X;
            this.Y = piece.Y;
            this.img = piece.img;
            this.speed = piece.speed;
            this.lives = piece.lives;

            img.Source = catLeft1;
        } //constractor for loading

    }
}
