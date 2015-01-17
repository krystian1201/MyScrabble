
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileC : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileC()
            : base('c', 3, imageURI)
        {
        }
    }
}
