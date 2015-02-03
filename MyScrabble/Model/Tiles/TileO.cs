
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileO : Tile
    {
        private const string imageURI =
            @"\Assets\O.jpg";

        public TileO()
            : base('o', 1, imageURI)
        {
        }
    }
}
