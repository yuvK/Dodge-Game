using Newtonsoft.Json;
using System;
using Windows.UI.Xaml.Media.Imaging;

namespace CatDodger
{
    internal class Enemy : GamePiece
    {
        [JsonIgnore]
        public BitmapImage DogRunRight1 = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/DogRunRight1.png"));
        [JsonIgnore]
        public BitmapImage DogRunRight2 = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/DogRunRight2.png"));
        [JsonIgnore]
        public BitmapImage DogRunLeft1 = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/DogRunLeft1.png"));
        [JsonIgnore]
        public BitmapImage DogRunLeft2 = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/DogRunLeft2.png"));   
        [JsonIgnore]
        public BitmapImage DogLeft = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/dogLeft.gif"));   
        [JsonIgnore]
        public BitmapImage DogRight = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/dogRight.gif"));
        [JsonIgnore]
        public BitmapImage DogDead = new BitmapImage(new Uri(@"ms-appx:///Assets/Images/DogSleep.png"));
        public Enemy()
        {
            img.Source = DogRunRight1;
            speed = 0.8;
        }
        public Enemy(GamePiece piece)  //constractor for loading
        {
            this.img = piece.img;
            this.speed = piece.speed;
            this.X = piece.X;
            this.Y = piece.Y;
            img.Source = DogRunRight1;
        }

    }
}
