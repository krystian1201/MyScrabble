

namespace MyScrabble.Model.Tiles
{
    sealed class TileF : Tile
    {
        private const string imageURI =
            @"\Assets\F.jpg";

        public TileF()
            : base('f', 4, imageURI)
        {
        }
    }
}
