

namespace MyScrabble.Controller.Tiles
{
    sealed class TileB : Tile
    {
        private const string imageURI =
            @"\Assets\B.jpg";

        public TileB()
            : base('b', 3, imageURI)
        {
        }
    }
}
