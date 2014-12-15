
using System.Collections.Generic;

using MyScrabble.Controller.Tiles;

namespace MyScrabble.Controller
{
    class TilesRack
    {
        //TODO: setter/getter needed?
        public List<Tile> TilesList;

        public TilesRack()
        {
            TilesList = new List<Tile>();
   
        }

        public void populateTilesRack()
        {
            TilesList.Add(new TileB());
            TilesList.Add(new TileA());
            TilesList.Add(new TileC());
            TilesList.Add(new TileA());
        }

    }
}
