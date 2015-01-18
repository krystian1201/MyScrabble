
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileI : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileI()
            : base('i', 3, imageURI)
        {
        }
    }
}
