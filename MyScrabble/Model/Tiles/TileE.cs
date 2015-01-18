
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileE : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileE()
            : base('e', 3, imageURI)
        {
        }
    }
}
