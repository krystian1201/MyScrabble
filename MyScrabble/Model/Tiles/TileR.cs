

namespace MyScrabble.Model.Tiles
{
    sealed class TileR : Tile
    {
        private const string imageURI =
            @"\Assets\R.jpg";

        public TileR()
            : base('r', 1, imageURI)
        {
        }
    }
}
