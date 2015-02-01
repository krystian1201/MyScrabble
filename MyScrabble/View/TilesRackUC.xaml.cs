
using System;
using System.Windows.Controls;
using System.Collections.Generic;

using MyScrabble.Controller;
using MyScrabble.Model;


namespace MyScrabble.View
{
    public partial class TilesRackUC : UserControl
    {
        //private readonly TilesBag TilesBag.TilesBagInstance;

        public TilesRack TilesRack { get; private set; }

        public TilesRackUC()
        {
            InitializeComponent();

            TilesRack = new TilesRack();


            for (int i = 0; i < TilesRack.TilesArray.Length; i++)
            {
                TilesRackGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }  
        }

        public void PopulateTilesRackUC()
        {
            //we should use this function
            TilesRack.PopulateWithTilesFromTilesBag(TilesBag.TilesBagInstance);

            //but for testing purposes - we can use this one
            //TilesRack.PopulateWithSetTiles();
           
            RefreshTilesRackUCFromTilesRack();
        }

        public void RefillTilesFromTilesBag()
        {
            //if there are tiles in tiles bag
            if (TilesBag.TilesBagInstance.TilesList.Count > 0)
            {
                TilesRack.RefillTilesFromTilesBag(TilesBag.TilesBagInstance);

                RefreshTilesRackUCFromTilesRack();
            }
        }

        private void RefreshTilesRackUCFromTilesRack()
        {
            TilesRackGrid.Children.Clear();

            //it can happen that the tiles rack will
            //contain less than 7 tiles
            for (int position = 0; position < TilesRack.TilesArray.Length; position++)
            {
                if (TilesRack.TilesArray[position] != null)
                {
                    ShowATileFromTilesRackInTilesRackUC(position);
                }
            }
        }

        private void ShowATileFromTilesRackInTilesRackUC(int position)
        {
            TileUC tileUC = new TileUC(TilesRack.TilesArray[position]);

            Grid.SetColumn(tileUC, position);
            TilesRackGrid.Children.Add(tileUC);
        }


        public void PlaceATileFromBoardInTilesRack(TileUC tileUC, int? position)
        {
            if (position != null)
            {
                Grid.SetColumn(tileUC, (int)position);
                TilesRackGrid.Children.Add(tileUC);

                TilesRack.InsertTile(tileUC.Tile, (int)position);
            }
            else
            {
                throw new Exception("Position in the tiles rack for the tile was not filled");
            }  
        }

        //just for testing
        public List<Tile> GetAllTilesFromTilesBag()
        {
            return TilesBag.TilesBagInstance.TilesList;
        }

        //just a wrapper
        //becasue MainWindow doesn't contain TilesRack but TilesRackUC
        public void GetTilesFromTilesRackToTilesBag()
        {
            TilesRack.GetTilesFromTilesRackToTilesBag();
        }

    
    }
}
