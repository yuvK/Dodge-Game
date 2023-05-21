using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;

namespace CatDodger
{
    internal class GamePiece
    {
        private double _x;
        private double _y;
        //setting x and y automaticly whenever asking for it
        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                Canvas.SetLeft(img, _x);
            }
        }
        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                Canvas.SetTop(img, _y);
            }
        }
        [JsonIgnore]
        public Image img { get; set; }
        public double speed { get; set; }
        public int lives { get; set; }
        public GamePiece()
        {
            img = new Image();
            img.Width = 70;
            img.Height = 40;
        }
    }
}
