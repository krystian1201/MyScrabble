

namespace MyScrabble.Controller.Tiles
{
    sealed class TileC : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileC()
            : base('C', 3, imageURI)
        {
        }
    }
}
