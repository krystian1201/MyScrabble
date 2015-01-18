
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileH : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileH()
            : base('h', 3, imageURI)
        {
        }
    }
}
