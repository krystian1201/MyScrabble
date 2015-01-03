

using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyScrabble.Controller.Tiles
{
    public abstract class Tile
    {
        private readonly char letter;
        public char Letter
        {
            get { return letter; }
        }

        private readonly int points;
        public int Points
        {
            get { return points; }
        }

        private readonly string imageURI;

        public string ImageURI
        {
            get { return imageURI; }
        }

        private readonly Image tileImage = new Image();

        public Image TileImage 
        {
            get { return tileImage; }
            
        }

        protected Tile(char letter, int points, string imageURI)
        {
            this.letter = letter;
            this.points = points;
            this.imageURI = imageURI;

            this.tileImage.Source = new BitmapImage(new Uri(imageURI, UriKind.RelativeOrAbsolute));
        }

    }
}
