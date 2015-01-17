

using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    public sealed class TileA : Tile
    {
        private const string imageURI =
            @"\Assets\A.jpg";

        public TileA() : base('a', 1, imageURI)
        {
        }
    }
}
