
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileY : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileY()
            : base('y', 3, imageURI)
        {
        }
    }
}
