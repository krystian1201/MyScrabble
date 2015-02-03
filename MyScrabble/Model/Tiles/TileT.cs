
namespace MyScrabble.Model.Tiles
{
    sealed class TileT : Tile
    {
        private const string imageURI =
            @"\Assets\T.jpg";

        public TileT()
            : base('t', 1, imageURI)
        {
        }
    }
}
