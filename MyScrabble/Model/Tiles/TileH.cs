


namespace MyScrabble.Model.Tiles
{
    sealed class TileH : Tile
    {
        private const string imageURI =
            @"\Assets\H.jpg";

        public TileH()
            : base('h', 4, imageURI)
        {
        }
    }
}
