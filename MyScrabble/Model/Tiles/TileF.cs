
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileF : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileF()
            : base('f', 3, imageURI)
        {
        }
    }
}
