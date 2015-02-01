

namespace MyScrabble.Model.Tiles
{
    sealed class TileD : Tile
    {
        private const string imageURI =
            @"\Assets\D.jpg";

        public TileD()
            : base('d', 2, imageURI)
        {
        }
    }
}
