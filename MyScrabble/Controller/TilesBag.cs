
using System;
using System.Collections.Generic;

using MyScrabble.Model;
using MyScrabble.Model.Tiles;


namespace MyScrabble.Controller
{

    public sealed class TilesBag
    {
        private static readonly TilesBag _tilesBagInstance = new TilesBag();

        private readonly List<Tile> _tilesList;

        //just for testing
        public List<Tile> TilesList
        {
            get { return _tilesList; }
        }

        private TilesBag()
        {
            _tilesList = new List<Tile>();
            PopulateWithTiles();
        }

        public static TilesBag TilesBagInstance
        {
            get { return _tilesBagInstance; }
        }

        public void PopulateWithTiles()
        {
            for (int i = 0; i < 9; i++)
            {
                _tilesList.Add(new TileA());
            }

            for (int i = 0; i < 2; i++)
            {
                _tilesList.Add(new TileB());
            }

            for (int i = 0; i < 2; i++)
            {
                _tilesList.Add(new TileC());
            }

            for (int i = 0; i < 4; i++)
            {
                _tilesList.Add(new TileD());
            }

            for (int i = 0; i < 12; i++)
            {
                _tilesList.Add(new TileE());
            }

            for (int i = 0; i < 2; i++)
            {
                _tilesList.Add(new TileF());
            }

            for (int i = 0; i < 3; i++)
            {
                _tilesList.Add(new TileG());
            }

            for (int i = 0; i < 2; i++)
            {
                _tilesList.Add(new TileH());
            }

            for (int i = 0; i < 9; i++)
            {
                _tilesList.Add(new TileI());
            }

            _tilesList.Add(new TileJ());

            _tilesList.Add(new TileK());

            for (int i = 0; i < 4; i++)
            {
                _tilesList.Add(new TileL());
            }

            for (int i = 0; i < 2; i++)
            {
                _tilesList.Add(new TileM());
            }

            for (int i = 0; i < 6; i++)
            {
                _tilesList.Add(new TileN());
            }

            for (int i = 0; i < 8; i++)
            {
                _tilesList.Add(new TileO());
            }

            for (int i = 0; i < 2; i++)
            {
                _tilesList.Add(new TileP());
            }

            _tilesList.Add(new TileQ());

            for (int i = 0; i < 6; i++)
            {
                _tilesList.Add(new TileR());
            }

            for (int i = 0; i < 4; i++)
            {
                _tilesList.Add(new TileS());
            }

            for (int i = 0; i < 6; i++)
            {
                _tilesList.Add(new TileT());
            }

            for (int i = 0; i < 4; i++)
            {
                _tilesList.Add(new TileU());
            }

            for (int i = 0; i < 2; i++)
            {
                _tilesList.Add(new TileV());
            }

            for (int i = 0; i < 2; i++)
            {
                _tilesList.Add(new TileW());
            }

            _tilesList.Add(new TileX());

            for (int i = 0; i < 2; i++)
            {
                _tilesList.Add(new TileY());
            }

            _tilesList.Add(new TileZ());
            

            //TODO: not all tiles have been added yet
            //TODO: the amounts of tiles may be not correct - just for tests
        }

        public Tile GetRandomTile(Random random)
        {
            Tile tileToReturn = null;

            if (_tilesList.Count > 0)
            {

                int tileIndex = random.Next(0, _tilesList.Count);

                tileToReturn = _tilesList[tileIndex];

                _tilesList.RemoveAt(tileIndex);

            }

            return tileToReturn;
        }
    }
}
