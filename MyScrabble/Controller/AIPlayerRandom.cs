using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using MyScrabble.Model;



namespace MyScrabble.Controller
{
    public class AIPlayerRandom : BaseAIPlayer
    {
        public AIPlayerRandom() : base()
        {
        }

        public override List<Tile> GenerateFirstMove(TilesRack tilesRack)
        {

            List <Tile> tilesInMove = GetRandomTilesFromTilesRackThatFormValidWord(tilesRack);

            //for tests
            //string word = GetRandomWordFromDictionary();

            //random word has to be formed from tiles in tiles rack
            string word = GetRandomWordFromTilesRackAndDictionary(tilesInMove);
            
            //for tests
            //List<Tile> tilesInMove = GenerateTilesFromWord(word);


            int wordStartPosition = GetRandomStartTilePosition(word);

            WordOrientation wordOrientation = GetRandomWordOrientation();

            AssignPositionsOnBoardToTilesInMove(word, tilesInMove, wordStartPosition, wordOrientation);


            return tilesInMove;
        }

        private List<Tile> GetRandomTilesFromTilesRackThatFormValidWord(TilesRack tilesRack)
        {
            
            string alphabetizedString;
            List<Tile> randomTilesFromTilesRack = new List<Tile>();
            
            //do it until the random word is in Scrabble dictionary (or alternatively the alphabetized
            //string appears as a key in the AI dictionary)
            do
            {
                randomTilesFromTilesRack = GetRandomTilesFromTilesRack(tilesRack);

                string stringFromTiles = BuildStringFromTiles(randomTilesFromTilesRack);

                alphabetizedString = _aiDictionary.AlphabetizeString(stringFromTiles);
            } 
            while (!_aiDictionary.AlphabetizedWordsPermutations.ContainsKey(alphabetizedString));

            return randomTilesFromTilesRack;
        }

        private List<Tile> GetRandomTilesFromTilesRack(TilesRack tilesRack)
        {
            Random random = new Random();

            //2..7
            int randomNumberOfTiles = random.Next(2, 8);

            List<Tile> randomTilesFromTilesRack = new List<Tile>();
            List<int> randomPositionsInTilesRack = new List<int>();

            for (int i = 0; i < randomNumberOfTiles; i++)
            {

                int randomPositionInTilesRack;

                //we need to make sure that the randomly chosen positions
                //don't repeat
                do
                {
                    randomPositionInTilesRack = random.Next(0, 7);
                } 
                while (randomPositionsInTilesRack.Contains(randomPositionInTilesRack));
                
                
                randomPositionsInTilesRack.Add(randomPositionInTilesRack);

                Tile randomTile = tilesRack.TilesArray[randomPositionInTilesRack];

                randomTilesFromTilesRack.Add(randomTile);

            }

            return randomTilesFromTilesRack;
        }

        private string GetRandomWordFromTilesRackAndDictionary(List<Tile> randomTiles)
        {

            string stringFromTiles = BuildStringFromTiles(randomTiles);

            string alphabetizedString = _aiDictionary.AlphabetizeString(stringFromTiles);

            List<string> wordsList = _aiDictionary.AlphabetizedWordsPermutations[alphabetizedString];

            Random random = new Random();
            string randomWord = wordsList[random.Next(0, wordsList.Count)];

            return randomWord;
        }

        private string BuildStringFromTiles(List<Tile> randomTilesFromTilesRack)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (Tile tile in randomTilesFromTilesRack)
            {
                stringBuilder.Append(tile.Letter);
            }

            return stringBuilder.ToString();
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

        //for tests
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

        private void AssignPositionsOnBoardToTilesInMove(string word, List<Tile> tilesInMove,  int startTilePosition, 
            WordOrientation wordOrientation)
        {
            List<Tile> tempTilesList = new List<Tile>(tilesInMove);


            int? row = null;
            int? column = null;

            int letterIndex = 0;

            while (tempTilesList.Count > 0)
            {
                //first tile with a given letter
                //actually, if we have two or more tiles with the same letter in tiles rack, 
                //they can be used in any place on board that requires a given letter
                Tile tileInMove = tempTilesList.FirstOrDefault(tile => tile.Letter == word[letterIndex]);


                if (wordOrientation == WordOrientation.Horizontal)
                {
                    row = 7;
                    tileInMove.PositionOnBoard = new Point(startTilePosition + letterIndex, (int)row);

                }
                else if (wordOrientation == WordOrientation.Vertical)
                {
                    column = 7;
                    tileInMove.PositionOnBoard = new Point((int)column, startTilePosition + letterIndex);
                }


                tempTilesList.Remove(tileInMove);

                letterIndex++;
            }
        }

        public override List<Tile> GenerateSecondAndAboveMove(TilesRack tilesRack)
        {
            throw new NotImplementedException();
        }
    }
}
