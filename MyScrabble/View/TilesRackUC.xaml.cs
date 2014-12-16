
using MyScrabble.Controller;

using System.Windows.Controls;

namespace MyScrabble.View
{
    public partial class TilesRackUC : UserControl
    {
        private TilesRack tilesRack;

        public TilesRackUC()
        {
            InitializeComponent();

            for (int i = 0; i < 7; i++)
            {
                TilesRackGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            //TilesRackGrid.Children.Clear(); //necessary?

            tilesRack = new TilesRack();
            tilesRack.populateTilesRack();


            for (int column = 0; column < tilesRack.TilesList.Count; column++)
            {
                TileUC tileUC = new TileUC(tilesRack.TilesList[column]);
                Grid.SetColumn(tileUC, column);
                TilesRackGrid.Children.Add(tileUC);
            }
        }
    }
}
