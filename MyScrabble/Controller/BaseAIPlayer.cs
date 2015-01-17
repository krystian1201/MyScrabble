using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public abstract List<Tile> GenerateMove();

    }
}
