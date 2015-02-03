
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileU : Tile
    {
        private const string imageURI =
            @"\Assets\U.jpg";

        public TileU()
            : base('u', 1, imageURI)
        {
        }
    }
}
