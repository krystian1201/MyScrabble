

namespace MyScrabble.Model.Tiles
{
    sealed class TileL : Tile
    {
        private const string imageURI =
            @"\Assets\L.jpg";

        public TileL()
            : base('l', 1, imageURI)
        {
        }
    }
}
