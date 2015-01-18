
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileU : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileU()
            : base('u', 3, imageURI)
        {
        }
    }
}
