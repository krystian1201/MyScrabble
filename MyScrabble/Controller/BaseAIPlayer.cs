
using System.Collections.Generic;


using MyScrabble.Model;

namespace MyScrabble.Controller
{
    public abstract class BaseAIPlayer
    {
        protected AIDictionary _aiDictionary;
        //protected TilesRack _tilesRack;

        protected BaseAIPlayer()
        {
            _aiDictionary = new AIDictionary();

        }

        public abstract List<Tile> GenerateFirstMove(TilesRack tilesRack);
        public abstract List<Tile> GenerateSecondAndAboveMove(TilesRack tilesRack);
    }
}
