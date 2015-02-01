
using System;
using System.Collections.Generic;


using MyScrabble.Model;

namespace MyScrabble.Controller
{
    public abstract class BaseAIPlayer
    {
        protected AIDictionary _aiDictionary;
        

        protected BaseAIPlayer()
        {
            _aiDictionary = new AIDictionary();

        }

        public List<Tile> GenerateMove(TilesRack tilesRack, Board board)
        {
            List<Tile> tilesInMove = null;

            if (Game.IsFirstMove)
            {
                tilesInMove = GenerateFirstMove(tilesRack, board);
            }
            else if (!Game.IsFirstMove)
            {
                tilesInMove = GenerateSecondAndAboveMove(tilesRack, board);
            }

            if (tilesInMove == null || tilesInMove.Count == 0)
            {
                throw new Exception("No tiles in move");
            }

            return tilesInMove;
        }

        protected abstract List<Tile> GenerateFirstMove(TilesRack tilesRack, Board board);

        protected abstract List<Tile> GenerateSecondAndAboveMove(TilesRack tilesRack, Board board);

    }
}
