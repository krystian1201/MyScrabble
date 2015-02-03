
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileV : Tile
    {
        private const string imageURI =
            @"\Assets\V.jpg";

        public TileV()
            : base('v', 4, imageURI)
        {
        }
    }
}
