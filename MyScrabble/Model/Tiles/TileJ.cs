
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileJ : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileJ()
            : base('j', 3, imageURI)
        {
        }
    }
}
