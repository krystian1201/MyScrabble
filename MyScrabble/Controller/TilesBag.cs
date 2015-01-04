
using System;
using System.Collections.Generic;

using MyScrabble.Controller.Tiles;


namespace MyScrabble.Controller
{
    public class TilesBag
    {
        private List<Tile> tilesList;

        //just for testing
        public List<Tile> TilesList
        {
            get { return tilesList; }
        }

        public TilesBag()
        {
            tilesList = new List<Tile>();
  
        }

        public void PopulateWithTiles()
        {
            for (int i = 0; i < 9; i++)
            {
                Tile tileToAdd = new TileA();

                tilesList.Add(tileToAdd);
            }

            for (int i = 0; i < 2; i++)
            {
                Tile tileToAdd = new TileB();

                tilesList.Add(tileToAdd);
            }

            for (int i = 0; i < 2; i++)
            {
                Tile tileToAdd = new TileC();

                tilesList.Add(tileToAdd);
            }

            //TODO: not all tiles have been added yet
        }

        public Tile GetRandomTile()
        {
            Tile tileToReturn = null;

            if (tilesList.Count > 0)
            {
                Random random = new Random();

                int tileIndex = random.Next(0, tilesList.Count);

                tileToReturn = tilesList[tileIndex];

                tilesList.RemoveAt(tileIndex);

            }

            return tileToReturn;
        }
    }
}
