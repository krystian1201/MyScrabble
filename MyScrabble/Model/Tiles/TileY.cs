
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileY : Tile
    {
        private const string imageURI =
            @"\Assets\Y.jpg";

        public TileY()
            : base('y', 4, imageURI)
        {
        }
    }
}
