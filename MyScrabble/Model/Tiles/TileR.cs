
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileR : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileR()
            : base('r', 3, imageURI)
        {
        }
    }
}
