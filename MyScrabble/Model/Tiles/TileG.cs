
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileG : Tile
    {
        private const string imageURI =
            @"\Assets\G.jpg";

        public TileG()
            : base('g', 2, imageURI)
        {
        }
    }
}
