using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

using MyScrabble.Model;
using MyScrabble.Model.Tiles;



namespace MyScrabble.Controller
{
    public class AIPlayerRandom : BaseAIPlayer
    {
        public AIPlayerRandom() : base()
        {
        }

        public override List<Tile> GenerateMove()
        {

            string word = GetRandomWordFromDictionary();

            List<Tile> tilesInMove = GenerateTilesFromWord(word);


            int startTilePosition = GetRandomStartTilePosition(word);

            WordOrientation wordOrientation = GetRandomWordOrientation();

            AssignPositionsOnBoardToTilesInMove(tilesInMove, startTilePosition, wordOrientation);

            Game.SetAfterFirstMove();

            return tilesInMove;
        }

        

        private WordOrientation GetRandomWordOrientation()
        {
            Random random = new Random();

            //random integer - 0 or 1
            int randomValue = random.Next(0, 2);

            if (randomValue == 0)
            {
                return WordOrientation.Horizontal;
            }
            if (randomValue == 1)
            {
                return WordOrientation.Vertical;
            }
           
            throw new Exception("Word orientation should be either horizontal or vertical");
        }

        private string GetRandomWordFromDictionary()
        {
            Random random = new Random();


            int randomWordKeyIndex;
            string alphabetizedString = null;


            //in the first move we can play at most 7-letter word
            if (Game.IsFirstMove)
            {
                do 
                {
                    randomWordKeyIndex = random.Next(0, _aiDictionary.AlphabetizedWordsPermutations.Count);

                    alphabetizedString = 
                        _aiDictionary.AlphabetizedWordsPermutations.Keys.ElementAt(randomWordKeyIndex);
                }
                while (alphabetizedString.Length > 7);
            }
            else
            {
                randomWordKeyIndex = random.Next(0, _aiDictionary.AlphabetizedWordsPermutations.Count);

                alphabetizedString =
                    _aiDictionary.AlphabetizedWordsPermutations.Keys.ElementAt(randomWordKeyIndex);
            }
            

            int randomWordPermutationIndex =
                random.Next(0, _aiDictionary.AlphabetizedWordsPermutations[alphabetizedString].Count);

            string word = _aiDictionary.AlphabetizedWordsPermutations[alphabetizedString][randomWordPermutationIndex];

            return word;
        }

        //the position of the "start"/"first" tile in word can be described by just one
        //number because either row or column must be equal to 7 so that the word
        //goes through the center of board

        //the position here means either starting column - for horizontal word, or
        //starting row - for vertical word
        private int GetRandomStartTilePosition(string word)
        {
            Random random = new Random();

            int randomStartTilePosition = random.Next(7 - word.Length + 1, 8);

            return randomStartTilePosition;
        }

        private List<Tile> GenerateTilesFromWord(string word)
        {
            List<Tile> tilesInMove = new List<Tile>();

            char[] charArrayWord = word.ToCharArray();

            foreach (char letter in charArrayWord)
	        {
                Tile tileToAdd = TilesFactory.CreateTileByLetter(letter);
                tilesInMove.Add(tileToAdd);
	        }

            return tilesInMove;
        }

        private void AssignPositionsOnBoardToTilesInMove(List<Tile> tilesInMove,  int startTilePosition, 
            WordOrientation wordOrientation)
        {
            int? row = null;
            int? column = null;

            if (wordOrientation == WordOrientation.Horizontal)
            {
                row = 7;

                for (int i = 0; i < tilesInMove.Count; i++)
                {
                    tilesInMove[i].PositionOnBoard = new Point(startTilePosition + i, (int)row);
                }
            }
            else if (wordOrientation == WordOrientation.Vertical)
            {
                column = 7;

                for (int i = 0; i < tilesInMove.Count; i++)
                {
                    tilesInMove[i].PositionOnBoard = new Point((int)column, startTilePosition + i);
                }
            } 
        }
    }
}
