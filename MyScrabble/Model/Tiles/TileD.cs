
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileD : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileD()
            : base('d', 3, imageURI)
        {
        }
    }
}
