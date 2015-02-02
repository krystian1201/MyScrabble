

using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace MyScrabble.Model
{
    public class Tile
    {

        public char Letter { get; private set; }

        public int Points { get; private set; }


        public string ImageURI { get; private set; }

        public Image TileImage { get; private set; }
        

        public bool WasMoveMade { get; set; }

        public int? PositionInTilesRack { get; set; }


        public Point? PositionOnBoard { get; set; }

        public Tile(Tile tile)
        {
            this.Letter = tile.Letter;
            this.Points = tile.Points;
            this.TileImage = tile.TileImage;
            this.WasMoveMade = tile.WasMoveMade;
            this.PositionOnBoard = tile.PositionOnBoard;
            this.PositionInTilesRack = tile.PositionInTilesRack;
            this.ImageURI = tile.ImageURI;

        }

        protected Tile(char letter, int points, string imageURI)
        {
            this.Letter = letter;
            this.Points = points;
            this.ImageURI = imageURI;
            this.TileImage = new Image();
            this.TileImage.Source = new BitmapImage(new Uri(imageURI, UriKind.RelativeOrAbsolute));
            this.WasMoveMade = false;
            this.PositionInTilesRack = null;
            this.PositionOnBoard = null;
        }

    }
}
