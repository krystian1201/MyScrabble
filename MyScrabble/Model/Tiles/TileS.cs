
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileS : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileS()
            : base('s', 3, imageURI)
        {
        }
    }
}
