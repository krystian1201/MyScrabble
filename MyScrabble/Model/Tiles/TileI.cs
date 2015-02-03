
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileI : Tile
    {
        private const string imageURI =
            @"\Assets\I.jpg";

        public TileI()
            : base('i', 1, imageURI)
        {
        }
    }
}
