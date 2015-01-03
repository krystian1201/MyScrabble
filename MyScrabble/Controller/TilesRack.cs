
using System;
using System.Collections.Generic;

using MyScrabble.Controller.Tiles;

namespace MyScrabble.Controller
{
    public class TilesRack
    {
        
        //TODO: setter/getter needed?
        public Tile[] TilesArray;

        private readonly List<char> UniqueTilesList =
            new List<char>() {'A', 'B', 'C'};


        public TilesRack()
        {
            TilesArray = new Tile[7];
        }

        public void PopulateWithTiles()
        {
            Array.ForEach(TilesArray, tile => tile = null);

            Random random = new Random();

            for (int i = 0; i < 7; i++)
            {
                int tileIndex = random.Next(0, UniqueTilesList.Count);

                switch (UniqueTilesList[tileIndex])
                {
                    case 'A':
                        InsertTileIntoTilesArray(new TileA() { PositionInTilesRack = i }, i);
                        break;
                    case 'B':
                        InsertTileIntoTilesArray(new TileB() { PositionInTilesRack = i }, i);
                        break;
                    case 'C':
                        InsertTileIntoTilesArray(new TileC() { PositionInTilesRack = i }, i);
                        break;
                    default:
                        throw new Exception("Tile doesn't belong to the valid set of tiles");
                }
            }

        }

        public void RemoveTileFromTilesArray(Tile tileToRemove)
        {
            TilesArray[(int)tileToRemove.PositionInTilesRack] = null; 
        }

        public void InsertTileIntoTilesArray(Tile tileToInsert, int position)
        {
            TilesArray[position] = tileToInsert;
        }
    }
}
