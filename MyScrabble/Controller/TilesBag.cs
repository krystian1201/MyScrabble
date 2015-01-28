
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
                Tile tileToAdd = new TileA();

                _tilesList.Add(tileToAdd);
            }

            for (int i = 0; i < 2; i++)
            {
                Tile tileToAdd = new TileB();

                _tilesList.Add(tileToAdd);
            }

            for (int i = 0; i < 2; i++)
            {
                Tile tileToAdd = new TileC();

                _tilesList.Add(tileToAdd);
            }

            //TODO: not all tiles have been added yet
        }

        public Tile GetRandomTile()
        {
            Tile tileToReturn = null;

            if (_tilesList.Count > 0)
            {
                Random random = new Random();

                int tileIndex = random.Next(0, _tilesList.Count);

                tileToReturn = _tilesList[tileIndex];

                _tilesList.RemoveAt(tileIndex);

            }

            return tileToReturn;
        }
    }
}
