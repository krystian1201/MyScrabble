
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileO : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileO()
            : base('o', 3, imageURI)
        {
        }
    }
}
