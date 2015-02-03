
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileZ : Tile
    {
        private const string imageURI =
            @"\Assets\Z.jpg";

        public TileZ()
            : base('z', 10, imageURI)
        {
        }
    }
}
