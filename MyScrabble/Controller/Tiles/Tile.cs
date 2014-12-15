

using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyScrabble.Controller.Tiles
{
    public abstract class Tile
    {
        private readonly char letter;

        private readonly int points;

        private readonly string imageURI;

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

            this.tileImage.Source = new BitmapImage(new Uri(imageURI, UriKind.Relative));
        }
    }
}
