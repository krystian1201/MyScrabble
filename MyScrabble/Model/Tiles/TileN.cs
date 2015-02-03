

namespace MyScrabble.Model.Tiles
{
    sealed class TileN : Tile
    {
        private const string imageURI =
            @"\Assets\N.jpg";

        public TileN()
            : base('n', 1, imageURI)
        {
        }
    }
}
