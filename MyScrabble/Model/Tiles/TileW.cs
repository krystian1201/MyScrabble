﻿
using MyScrabble.Model;

namespace MyScrabble.Model.Tiles
{
    sealed class TileW : Tile
    {
        private const string imageURI =
            @"\Assets\C.jpg";

        public TileW()
            : base('w', 3, imageURI)
        {
        }
    }
}
