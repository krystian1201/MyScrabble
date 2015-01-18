
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileZ : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileZ()
            : base('z', 3, imageURI)
        {
        }
    }
}
