using System;

using MyScrabble.Model;
using MyScrabble.Model.Tiles;

namespace MyScrabble.Controller
{
    public static class TilesFactory
    {
        public static Tile CreateTileByLetter(char letter)
        {
            switch (letter)
            {
                case 'a':
                    return new TileA();
                case 'b':
                    return new TileB();
                case 'c':
                    return new TileC();
                case 'd':
                    return new TileD();
                case 'e':
                    return new TileE();
                case 'f':
                    return new TileF();
                case 'g':
                    return new TileG();
                case 'h':
                    return new TileH();
                case 'i':
                    return new TileI();
                case 'j':
                    return new TileJ();
                case 'k':
                    return new TileK();
                case 'l':
                    return new TileL();
                case 'm':
                    return new TileM();
                case 'n':
                    return new TileN();
                case 'o':
                    return new TileO();
                case 'p':
                    return new TileP();
                case 'q':
                    return new TileQ();
                case 'r':
                    return new TileR();
                case 's':
                    return new TileS();
                case 't':
                    return new TileT();
                case 'u':
                    return new TileU();
                case 'v':
                    return new TileV();
                case 'w':
                    return new TileW();
                case 'x':
                    return new TileX();
                case 'y':
                    return new TileY();
                case 'z':
                    return new TileZ();
                default:
                    throw new Exception("Given symbol is not a valid English letter");
            }

        }
    }
}
