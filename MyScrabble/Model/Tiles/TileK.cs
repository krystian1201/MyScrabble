
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileK : Tile
    {
        private const string imageURI =
            @"\Assets\K.jpg";

        public TileK()
            : base('k', 5, imageURI)
        {
        }
    }
}
