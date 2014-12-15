
using System.Windows.Controls;

using MyScrabble.Controller.Tiles;


namespace MyScrabble
{
    public partial class TileUC : UserControl
    {
        private Tile tile;

        public TileUC(Tile tile)
        {
            InitializeComponent();

            this.tile = tile;

            this.Content = tile.TileImage;
            
        }
    }
}

