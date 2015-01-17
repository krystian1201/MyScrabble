using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyScrabble.Model;

namespace MyScrabble.Controller
{
    public class AIPlayerRandom : BaseAIPlayer
    {
        public AIPlayerRandom() : base()
        {

        }

        public override List<Tile> GenerateMove()
        {
            Random random = new Random();

            //random integer - 0 or 1
            int randomValue = random.Next(0, 2);

            WordOrientation wordOrientation = GetRandomWordOrientation();

            List<Tile> tilesInMove = new List<Tile>();

            return tilesInMove;
        }

        private WordOrientation GetRandomWordOrientation()
        {
            //WordOrientation wordOrientation;

            if (randomValue == 0)
            {
                return WordOrientation.Horizontal;
            }
            if (randomValue == 1)
            {
                return WordOrientation.Vertical;
            }
           
            throw new Exception("Word orientation should be either horizontal or vertical");
            

           // return wordOrientation;
        }
    }
}
