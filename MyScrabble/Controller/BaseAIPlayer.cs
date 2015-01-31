
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


        public abstract List<Tile> GenerateMove(TilesRack tilesRack, Board board);
    }
}
