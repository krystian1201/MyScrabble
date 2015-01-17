
using System;
using System.Windows.Controls;
using System.Collections.Generic;

using MyScrabble.Controller;
using MyScrabble.Model;
using MyScrabble.Model.Tiles;


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

            _tilesBag = new TilesBag();
            _tilesBag.PopulateWithTiles();

            _tilesRack = new TilesRack(_tilesBag);


            for (int i = 0; i < _tilesRack.TilesArray.Length; i++)
            {
                TilesRackGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }  
        }

        public void PopulateTilesRackUC()
        {
            TilesRackGrid.Children.Clear();

            //we should use this function
            //_tilesRack.PopulateWithTiles();

            //but for testing purposes - we use this one
            _tilesRack.PopulateWithSetTiles();


            for (int position = 0; position < _tilesRack.TilesArray.Length; position++)
            {
                //it can happen that the tiles rack will
                //contain less than 7 tiles
                if (_tilesRack.TilesArray[position] != null)
                {
                    ShowATileFromTilesRackInTilesRackUC(position);
                }    
            }
        }

        private void ShowATileFromTilesRackInTilesRackUC(int position)
        {
            TileUC tileUC = new TileUC(_tilesRack.TilesArray[position]);

            Grid.SetColumn(tileUC, position);
            TilesRackGrid.Children.Add(tileUC);
        }

        public void PlaceATileFromBoardInTilesRack(TileUC tileUC, int? position)
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

        public void RefillTilesFromTilesBag()
        {
            if (_tilesBag.TilesList.Count > 0)
            {
                _tilesRack.RefillTilesFromTilesBag();

                TilesRackGrid.Children.Clear();

                for (int position = 0; position < _tilesRack.TilesArray.Length; position++)
                {
                    if (_tilesRack.TilesArray[position] != null)
                    {
                        ShowATileFromTilesRackInTilesRackUC(position);
                    }
                }
            } 
        }
    }
}
