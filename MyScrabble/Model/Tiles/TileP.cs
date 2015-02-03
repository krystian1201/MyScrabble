

namespace MyScrabble.Model.Tiles
{
    sealed class TileP : Tile
    {
        private const string imageURI =
            @"\Assets\P.jpg";

        public TileP()
            : base('p', 3, imageURI)
        {
        }
    }
}
