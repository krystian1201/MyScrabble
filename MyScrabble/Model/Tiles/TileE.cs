

namespace MyScrabble.Model.Tiles
{
    sealed class TileE : Tile
    {
        private const string imageURI =
            @"\Assets\E.jpg";

        public TileE()
            : base('e', 1, imageURI)
        {
        }
    }
}
