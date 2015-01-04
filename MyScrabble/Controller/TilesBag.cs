
using System.Collections.Generic;


using MyScrabble.Controller.Tiles;


namespace MyScrabble.Controller
{
    class TilesBag
    {
        private List<Tile> tilesList;

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
    }
}
