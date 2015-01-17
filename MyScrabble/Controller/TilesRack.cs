
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

        private readonly List<char> UniqueTilesList =
            new List<char>() {'A', 'B', 'C'};

        private TilesBag _tilesBag;

        public TilesRack(TilesBag tilesBag)
        {
            TilesArray = new Tile[TILES_RACK_SIZE];

            _tilesBag = tilesBag;
        }

        public void PopulateWithTilesFromTilesBag()
        {

            for (int i = 0; i < TILES_RACK_SIZE; i++)
            {
                Tile randomTileFromTilesBag = _tilesBag.GetRandomTile();

                if (randomTileFromTilesBag != null)
                {
                    InsertTileIntoTilesArray(randomTileFromTilesBag, i);
                } 
                //there are no more tiles in tiles rack
                else
                {
                    SetTilesArrayPositionAsEmpty(i);
                }
            }

        }

        public void PopulateWithRandomTiles()
        {

            Random random = new Random();

            for (int i = 0; i < TILES_RACK_SIZE; i++)
            {
                int tileIndex = random.Next(0, UniqueTilesList.Count);

                switch (UniqueTilesList[tileIndex])
                {
                    case 'A':
                        InsertTileIntoTilesArray(new TileA(), i);
                        break;
                    case 'B':
                        InsertTileIntoTilesArray(new TileB(), i);
                        break;
                    case 'C':
                        InsertTileIntoTilesArray(new TileC(), i);
                        break;
                    default:
                        throw new Exception("Tile doesn't belong to the valid set of tiles");
                }
            }
        }

        //for testing purposes
        public void PopulateWithSetTiles()
        {
            InsertTileIntoTilesArray(new TileA(), 0);
            InsertTileIntoTilesArray(new TileA(), 1);
            InsertTileIntoTilesArray(new TileB(), 2);
            InsertTileIntoTilesArray(new TileB(), 3);
            InsertTileIntoTilesArray(new TileB(), 4);
            InsertTileIntoTilesArray(new TileC(), 5);
            InsertTileIntoTilesArray(new TileC(), 6);
        }

        public void RefillTilesFromTilesBag()
        {
            //Tile[] emptyPositionsInTilesRack = 
            //    TilesArray.Where(tile => tile == null).ToArray();

            for (int i = 0; i < TILES_RACK_SIZE; i++)
            {
                if (TilesArray[i] == null)
                {
                    Tile randomTileFromTilesBag = _tilesBag.GetRandomTile();

                    if (randomTileFromTilesBag != null)
                    {
                        InsertTileIntoTilesArray(randomTileFromTilesBag, i);
                    } 
                }
            }
        }

        public void GetTilesFromTilesRackToTilesBag()
        {
            throw new NotImplementedException();
        }

        public void RemoveTileFromTilesArray(Tile tileToRemove)
        {
            TilesArray[(int)tileToRemove.PositionInTilesRack] = null; 
        }

        public void InsertTileIntoTilesArray(Tile tileToInsert, int position)
        {
            tileToInsert.PositionInTilesRack = position;
            TilesArray[position] = tileToInsert;
        }

        private void SetTilesArrayPositionAsEmpty(int position)
        {
            TilesArray[position] = null;
        }
    }
}
