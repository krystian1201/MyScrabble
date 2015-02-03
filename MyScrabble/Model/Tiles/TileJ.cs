
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileJ : Tile
    {
        private const string imageURI =
            @"\Assets\J.jpg";

        public TileJ()
            : base('j', 8, imageURI)
        {
        }
    }
}
