

namespace MyScrabble.Model.Tiles
{
    sealed class TileM : Tile
    {
        private const string imageURI =
            @"\Assets\M.jpg";

        public TileM()
            : base('m', 3, imageURI)
        {
        }
    }
}
