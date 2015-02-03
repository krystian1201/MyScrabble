
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileW : Tile
    {
        private const string imageURI =
            @"\Assets\W.jpg";

        public TileW()
            : base('w', 4, imageURI)
        {
        }
    }
}
