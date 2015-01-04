
using System;
using System.Windows.Controls;
using System.Collections.Generic;

using MyScrabble.Controller;
using MyScrabble.Controller.Tiles;


namespace MyScrabble.View
{
    public partial class TilesRackUC : UserControl
    {
        private TilesRack _tilesRack;
        private TilesBag _tilesBag;

        public TilesRack TilesRack
        {
            get { return _tilesRack; }
        }

        public TilesRackUC()
        {
            InitializeComponent();

            for (int i = 0; i < 7; i++)
            {
                TilesRackGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            _tilesBag = new TilesBag();
            _tilesBag.PopulateWithTiles();

            _tilesRack = new TilesRack(_tilesBag);
        }

        public void PopulateTilesRackUC()
        {
            TilesRackGrid.Children.Clear();

            _tilesRack.PopulateWithTiles();


            for (int column = 0; column < _tilesRack.TilesArray.Length; column++)
            {
                //it can happen that the tiles rack will
                //contain less than 7 tiles
                if (_tilesRack.TilesArray[column] != null)
                {
                    TileUC tileUC = new TileUC(_tilesRack.TilesArray[column]);
                    Grid.SetColumn(tileUC, column);
                    TilesRackGrid.Children.Add(tileUC);
                }    
            }
        }


        public void PlaceATileInTilesRack(TileUC tileUC, int? position)
        {
            if (position != null)
            {
                Grid.SetColumn(tileUC, (int)position);
                TilesRackGrid.Children.Add(tileUC);

                _tilesRack.InsertTileIntoTilesArray(tileUC.Tile, (int)position);
            }
            else
            {
                throw new Exception("Position in the tiles rack for the tile was not filled");
            }  
        }

        //just for testing
        public List<Tile> GetAllTilesFromTilesBag()
        {
            return _tilesBag.TilesList;
        }

        //just a wrapper
        //becasue MainWindow doesn't contain TilesRack but TilesRackUC
        public void GetTilesFromTilesRackToTilesBag()
        {
            _tilesRack.GetTilesFromTilesRackToTilesBag();
        }
    }
}
