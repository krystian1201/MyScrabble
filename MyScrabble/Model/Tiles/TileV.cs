
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileV : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileV()
            : base('v', 3, imageURI)
        {
        }
    }
}
