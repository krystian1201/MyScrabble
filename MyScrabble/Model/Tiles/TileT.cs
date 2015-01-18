
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileT : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileT()
            : base('t', 3, imageURI)
        {
        }
    }
}
