
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileL : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileL()
            : base('l', 3, imageURI)
        {
        }
    }
}
