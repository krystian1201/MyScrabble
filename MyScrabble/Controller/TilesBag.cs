
using System.Collections.Generic;


using MyScrabble.Controller.Tiles;


namespace MyScrabble.Controller
{
    class TilesBag
    {
        private List<Tile> tiles;

        public TilesBag()
        {
            tiles = new List<Tile>();
  
        }

        public void PopulateWithTiles()
        {
            for (int i = 0; i < 9; i++)
            {
                Tile tileToAdd = new TileA();

                tiles.Add(tileToAdd);
            }

            for (int i = 0; i < 2; i++)
            {
                Tile tileToAdd = new TileB();

                tiles.Add(tileToAdd);
            }

            for (int i = 0; i < 2; i++)
            {
                Tile tileToAdd = new TileC();

                tiles.Add(tileToAdd);
            }

            //TODO: not all tiles have been added yet
        }
    }
}
