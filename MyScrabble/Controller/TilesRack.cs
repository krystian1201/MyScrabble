
using System;
using System.Collections.Generic;

using MyScrabble.Controller.Tiles;

namespace MyScrabble.Controller
{
    public class TilesRack
    {
        
        //TODO: setter/getter needed?
        public List<Tile> TilesList;

        private readonly List<char> UniqueTilesList =
            new List<char>() {'A', 'B', 'C'};

        public TilesRack()
        {
            TilesList = new List<Tile>();
   
        }

        public void PopulateWithTiles()
        {
            TilesList.Clear();

            Random random = new Random();

            for (int i = 0; i < 7; i++)
            {
                int tileIndex = random.Next(0, UniqueTilesList.Count);

                switch (UniqueTilesList[tileIndex])
                {
                    case 'A':
                        TilesList.Add(new TileA());
                        break;
                    case 'B':
                        TilesList.Add(new TileB());
                        break;
                    case 'C':
                        TilesList.Add(new TileC());
                        break;
                    default:
                        throw new Exception("Tile doesn't belong to the valid set of tiles");
                }
            }

        }

        public void RemoveTileFromTilesList(Tile tileToRemove)
        {
            TilesList.Remove(tileToRemove);
        }
    }
}
