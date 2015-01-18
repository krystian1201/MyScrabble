
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileX : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileX()
            : base('x', 3, imageURI)
        {
        }
    }
}
