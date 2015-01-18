
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileN : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileN()
            : base('n', 3, imageURI)
        {
        }
    }
}
