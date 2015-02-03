
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileS : Tile
    {
        private const string imageURI =
            @"\Assets\S.jpg";

        public TileS()
            : base('s', 1, imageURI)
        {
        }
    }
}
