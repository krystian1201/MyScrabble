
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileM : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileM()
            : base('m', 3, imageURI)
        {
        }
    }
}
