﻿
using System;
using System.Collections.Generic;
using System.Linq;

using MyScrabble.Model;
using MyScrabble.Model.Tiles;

namespace MyScrabble.Controller
{
    public class TilesRack
    {
        
        private const int TILES_RACK_SIZE = 7;

        //TODO: setter/getter needed?
        public Tile[] TilesArray;


        //private TilesBag tilesBag;

        public TilesRack()
        {
            TilesArray = new Tile[TILES_RACK_SIZE];
        }

        //the proper method
        public void PopulateWithTilesFromTilesBag(TilesBag tilesBag)
        {
            Random random = new Random();

            for (int i = 0; i < TILES_RACK_SIZE; i++)
            {
                Tile randomTileFromTilesBag = tilesBag.GetRandomTile(random);

                if (randomTileFromTilesBag != null)
                {
                    InsertTile(randomTileFromTilesBag, i);
                } 
                //there are no more tiles in tiles bag
                else
                {
                    SetTilesArrayPositionAsEmpty(i);
                }
            }

        }

        //for tests
        public void PopulateWithRandomTiles()
        {
             List<char> UniqueTilesList = new List<char>() {'A', 'B', 'C'};

            Random random = new Random();

            for (int i = 0; i < TILES_RACK_SIZE; i++)
            {
                int tileIndex = random.Next(0, UniqueTilesList.Count);

                switch (UniqueTilesList[tileIndex])
                {
                    case 'A':
                        InsertTile(new TileA(), i);
                        break;
                    case 'B':
                        InsertTile(new TileB(), i);
                        break;
                    case 'C':
                        InsertTile(new TileC(), i);
                        break;
                    default:
                        throw new Exception("Tile doesn't belong to the valid set of tiles");
                }
            }
        }

        //for testing purposes
        public void PopulateWithSetTiles()
        {
            InsertTile(new TileA(), 0);
            InsertTile(new TileA(), 1);
            InsertTile(new TileB(), 2);
            InsertTile(new TileB(), 3);
            InsertTile(new TileB(), 4);
            InsertTile(new TileC(), 5);
            InsertTile(new TileC(), 6);
        }

        public void RefillTilesFromTilesBag(TilesBag tilesBag)
        {
            Random random = new Random();

            for (int i = 0; i < TILES_RACK_SIZE; i++)
            {
                if (TilesArray[i] == null)
                {
                    Tile randomTileFromTilesBag = tilesBag.GetRandomTile(random);

                    if (randomTileFromTilesBag != null)
                    {
                        InsertTile(randomTileFromTilesBag, i);
                    } 
                }
            }
        }

        public void GetTilesFromTilesRackToTilesBag()
        {
            throw new NotImplementedException();
        }

        public void RemoveTiles(List<Tile> tilesToRemove)
        {
            foreach (Tile tile in tilesToRemove)
            {
                RemoveTile(tile);
            }
        }

        public void RemoveTile(Tile tileToRemove)
        {
            TilesArray[(int)tileToRemove.PositionInTilesRack] = null;

            //it is useful to keep the tile's position in tiles rack
            //in case the tile will come back from board to tiles rack
            //because, for example, the word is invalid
            //tileToRemove.PositionInTilesRack = null;
        }

        public void InsertTile(Tile tileToInsert, int position)
        {
            tileToInsert.PositionInTilesRack = position;
            TilesArray[position] = tileToInsert;
        }

        private void SetTilesArrayPositionAsEmpty(int position)
        {
            TilesArray[position] = null;
        }

        public int GetNumberOfTilesInTilesRack()
        {
            List<Tile> tilesInTilesRack = TilesArray.Where(tile => tile != null).ToList();

            return tilesInTilesRack.Count;
        }
    }
}
