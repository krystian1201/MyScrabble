
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileQ : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileQ()
            : base('q', 3, imageURI)
        {
        }
    }
}
