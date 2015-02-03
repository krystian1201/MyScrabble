
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileX : Tile
    {
        private const string imageURI =
            @"\Assets\X.jpg";

        public TileX()
            : base('x', 8, imageURI)
        {
        }
    }
}
