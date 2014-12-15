

namespace MyScrabble.Controller.Tiles
{
    public sealed class TileA : Tile
    {
        private const string imageURI =
            @"Assets\A.jpg";

        public TileA() : base('A', 1, imageURI)
        {
        }
    }
}
