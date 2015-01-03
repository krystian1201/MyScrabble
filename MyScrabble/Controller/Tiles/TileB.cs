

namespace MyScrabble.Controller.Tiles
{
    sealed class TileB : Tile
    {
        private const string imageURI =
            @"\Assets\B.jpg";

        public TileB()
            : base('B', 3, imageURI)
        {
        }
    }
}
