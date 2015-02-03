

namespace MyScrabble.Model.Tiles
{
    sealed class TileQ : Tile
    {
        private const string imageURI =
            @"\Assets\Q.jpg";

        public TileQ()
            : base('q', 10, imageURI)
        {
        }
    }
}
