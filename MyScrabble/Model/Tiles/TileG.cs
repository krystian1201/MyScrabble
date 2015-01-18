
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileG : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileG()
            : base('g', 3, imageURI)
        {
        }
    }
}
