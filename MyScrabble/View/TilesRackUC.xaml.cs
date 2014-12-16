
using MyScrabble.Controller;

using System.Windows.Controls;

namespace MyScrabble.View
{
    public partial class TilesRackUC : UserControl
    {
        private TilesRack tilesRack;

        public TilesRack TilesRack
        {
            get { return tilesRack; }
        }

        public TilesRackUC()
        {
            InitializeComponent();

            for (int i = 0; i < 7; i++)
            {
                TilesRackGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            populateTilesRackUC();
            
        }

        private void populateTilesRackUC()
        {
            tilesRack = new TilesRack();
            tilesRack.PopulateTilesRack();


            for (int column = 0; column < tilesRack.TilesList.Count; column++)
            {
                TileUC tileUC = new TileUC(tilesRack.TilesList[column]);
                Grid.SetColumn(tileUC, column);
                TilesRackGrid.Children.Add(tileUC);
            }
        }
    }
}
