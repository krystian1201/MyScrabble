
using System;
using System.Windows.Controls;

using MyScrabble.Controller;


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

            tilesRack = new TilesRack();
        }

        public void PopulateTilesRackUC()
        {
            TilesRackGrid.Children.Clear();
           
            tilesRack.PopulateWithTiles();


            for (int column = 0; column < tilesRack.TilesArray.Length; column++)
            {
                TileUC tileUC = new TileUC(tilesRack.TilesArray[column]);
                Grid.SetColumn(tileUC, column);
                TilesRackGrid.Children.Add(tileUC);
            }
        }

        public void PlaceATileInTilesRack(TileUC tileUC, int? position)
        {
            if (position != null)
            {
                Grid.SetColumn(tileUC, (int)position);
                TilesRackGrid.Children.Add(tileUC);

                tilesRack.InsertTileIntoTilesArray(tileUC.Tile, (int)position);
            }
            else
            {
                throw new Exception("Position in the tiles rack for the tile was not filled");
            }  
        }

    }
}
