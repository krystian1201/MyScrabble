
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileK : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileK()
            : base('k', 3, imageURI)
        {
        }
    }
}
