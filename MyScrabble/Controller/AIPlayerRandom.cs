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


        public override List<Tile> GenerateMove(TilesRack tilesRack, Board board)
        {
            List<Tile> tilesInMove = null;

            if (Game.IsFirstMove)
            {
                tilesInMove = GenerateFirstMove(tilesRack);
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

        

        private List<Tile> GenerateFirstMove(TilesRack tilesRack)
        {

            List <Tile> tilesInMove = GetRandomTilesFromTilesRackThatFormValidWord(tilesRack);

            
            string stringFromTiles = BuildStringFromTiles(tilesInMove);

            //random word has to be formed from tiles in tiles rack
            string word = GetRandomWordFromRandomString(randomString: stringFromTiles, substring: "");
            
            //for tests
            //List<Tile> tilesInMove = GenerateTilesFromWord(word);


            int wordStartPosition = GetRandomStartTilePositionForFirstMove(word);

            WordOrientation wordOrientation = GetRandomWordOrientation();

            AssignPositionsOnBoardToTilesInMove(word, tilesInMove, new List<Tile>(), wordStartPosition, new Point(7, 7), wordOrientation, "", -1);


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

            
            int randomNumberOfTiles;

            //first move -> 2..7 letters
            //second move and above -> 1..7 letters
            if (Game.IsFirstMove)
            {
                randomNumberOfTiles = random.Next(2, 8);
            }
            else
            {
                randomNumberOfTiles = random.Next(1, 8);
            }
           
            List<Tile> randomTilesFromTilesRack = new List<Tile>();
            List<int> randomPositionsInTilesRack = new List<int>();

            for (int i = 0; i < randomNumberOfTiles; i++)
            {

                int randomPositionInTilesRack;
                Tile randomTile;

                //we need to make sure that the randomly chosen positions
                //don't repeat
                do
                {
                    randomPositionInTilesRack = random.Next(0, 7);

                    //it can happen that the tiles rack will be empty at the
                    //chosen position (near the end of game)
                    randomTile = tilesRack.TilesArray[randomPositionInTilesRack];
                } 
                while (randomTile == null || randomPositionsInTilesRack.Contains(randomPositionInTilesRack));
                
                
                randomPositionsInTilesRack.Add(randomPositionInTilesRack);

                randomTilesFromTilesRack.Add(randomTile);

            }

            return randomTilesFromTilesRack;
        }

        private string GetRandomWordFromRandomString(string randomString, string substring)
        {
            string alphabetizedString = _aiDictionary.AlphabetizeString(randomString);
            string randomWord = null;

            if (_aiDictionary.AlphabetizedWordsPermutations.ContainsKey(alphabetizedString))
            {
                List<string> wordsList = _aiDictionary.AlphabetizedWordsPermutations[alphabetizedString];
                List<string> tempWordsList = new List<string>(wordsList);

                Random random = new Random();
                
                do
                {
                    randomWord = tempWordsList[random.Next(0, tempWordsList.Count)];
                    tempWordsList.Remove(randomWord);
                }
                while (!randomWord.Contains(substring) && tempWordsList.Count >= 1);
 
            }

            if (randomWord != null && randomWord.Contains(substring))
            {
                return randomWord;
            }
            return null;

        }

        private string BuildStringFromTiles(List<Tile> tiles)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (Tile tile in tiles)
            {
                stringBuilder.Append(tile.Letter);
            }

            return stringBuilder.ToString();
        }

        private WordOrientation GetRandomWordOrientation()
        {
            Random random = new Random();

            //I don't know why but when I call random.Next(0, 2), I always get 0
            int randomValue = random.Next(0, 10);

            if (randomValue >= 0 && randomValue <= 4)
            {
                return WordOrientation.Horizontal;
            }
            if (randomValue >= 5 && randomValue <= 9)
            {
                return WordOrientation.Vertical;
            }
           
            throw new Exception("Word orientation should be either horizontal or vertical");
        }


        //the position of the "start"/"first" tile in word can be described by just one
        //number because either row or column must be equal to 7 so that the word
        //goes through the center of board

        //the position here means either starting column - for horizontal word, or
        //starting row - for vertical word
        private int GetRandomStartTilePositionForFirstMove(string word)
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

        private void AssignPositionsOnBoardToTilesInMove(string word, List<Tile> tilesInMove,
            List<Tile> tilesOnBoardFromAnchor, int startTilePosition, Point anchorPosition, WordOrientation wordOrientation, 
            string substring, int substringIndex)
        {
            
            //List<Tile> tilesInWordNewPlusOnBoard = new List<Tile>();
            //tilesInWordNewPlusOnBoard.AddRange(tilesOnBoardFromAnchor);
            //tilesInWordNewPlusOnBoard.AddRange(tilesInMove);

            //List<Tile> tempTilesList = new List<Tile>(tilesInWordNewPlusOnBoard);
            List<Tile> tempTilesList = new List<Tile>(tilesInMove);

            int letterIndex = 0;

            while (tempTilesList.Count >= 1 && letterIndex <= word.Length - 1)
            {
                if (letterIndex == substringIndex)
                {
                    letterIndex += substring.Length;
                }

                //first tile with a given letter
                //actually, if we have two or more tiles with the same letter in tiles rack, 
                //they can be used in any place on board that requires a given letter
                Tile tileInMove = 
                    tempTilesList.FirstOrDefault(tile => tile.Letter == word[letterIndex] );

                //we assign position on board only to tiles that were actually added in the current move
                //if (!tilesOnBoardFromAnchor.Contains(tileInMove))
                {
                    if (wordOrientation == WordOrientation.Horizontal)
                    {
                        tileInMove.PositionOnBoard = new Point(startTilePosition + letterIndex, (int)anchorPosition.Y);

                    }
                    else if (wordOrientation == WordOrientation.Vertical)
                    {
                        tileInMove.PositionOnBoard = new Point((int)anchorPosition.X, startTilePosition + letterIndex);
                    }
                }

                tempTilesList.Remove(tileInMove);

                letterIndex++;

                
            }
        }

        private List<Tile> GenerateSecondAndAboveMove(TilesRack tilesRack, Board board)
        {
            string word = null;
            List<Tile> tilesOnBoardFromAnchor = null;
            List<Tile> randomTilesFromTilesRack = null;
            WordOrientation wordOrientation;
            Tile anchorTile;

            do
            {
                wordOrientation = GetRandomWordOrientation();

                anchorTile = GetRandomAnchorTile(board);

                tilesOnBoardFromAnchor = GetTilesOnBoardFromAnchor(anchorTile, board, wordOrientation);

                randomTilesFromTilesRack = GetRandomTilesFromTilesRack(tilesRack);


                word = GetRandomWordFromTilesRackAndBoard(randomTiles: randomTilesFromTilesRack, tilesOnBoardFromAnchor: tilesOnBoardFromAnchor, 
                    anchorTile: anchorTile, board: board, wordOrientation: wordOrientation);
            } 
            while (word == null);

            AssignPositionsToTilesInMoveSecondAndAbove(randomTilesFromTilesRack, tilesOnBoardFromAnchor, 
                anchorTile, wordOrientation, word);

            return randomTilesFromTilesRack;
        }

        private Tile GetRandomAnchorTile(Board board)
        {
            List<Tile> tilesOnBoard = board.GetTilesOnBoard();

            Random random = new Random();
            Tile randomTileOnBoard;

            //if a tile is totally surrounded by other tiles from left, right, top and bottom
            //we cannot use such tile to form new words
            do
            {
                int randomTileOnBoardIndex = random.Next(0, tilesOnBoard.Count);

                randomTileOnBoard = tilesOnBoard[randomTileOnBoardIndex];
            }
            while (board.IsTileTotallySurrounded(randomTileOnBoard));

            return randomTileOnBoard;
        }

        private List<Tile> GetTilesOnBoardFromAnchor(Tile anchorTile, Board board, WordOrientation wordOrientation)
        {
  
            List<Tile> tilesOnBoardFromAnchor = null;

            if (wordOrientation == WordOrientation.Horizontal)
            {
                tilesOnBoardFromAnchor = board.GetTilesOfWordInRow((int)anchorTile.PositionOnBoard.Value.Y, new List<Tile>() { anchorTile });
            }
            else if (wordOrientation == WordOrientation.Vertical)
            {
                tilesOnBoardFromAnchor = board.GetTilesOfWordInColumn((int)anchorTile.PositionOnBoard.Value.X, new List<Tile>() { anchorTile });
            }

            return tilesOnBoardFromAnchor;
        }

        private string GetRandomWordFromTilesRackAndBoard(List<Tile> randomTiles, List<Tile> tilesOnBoardFromAnchor, 
            Tile anchorTile, Board board, WordOrientation wordOrientation)
        {
            string wordFromTilesOnBoard = BuildStringFromTiles(tilesOnBoardFromAnchor);

            string stringFromRandomTiles = BuildStringFromTiles(randomTiles);

            string randomWord = 
                GetRandomWordFromRandomString(randomString: wordFromTilesOnBoard + stringFromRandomTiles, substring: wordFromTilesOnBoard);

            return randomWord;
        }

        private void AssignPositionsToTilesInMoveSecondAndAbove(List<Tile> randomTilesFromTilesRack, List<Tile> tilesOnBoardFromAnchor,
            Tile anchorTile, WordOrientation wordOrientation, string word)
        {
            

            string wordFromTilesOnBoard = BuildStringFromTiles(tilesOnBoardFromAnchor);

            //int wordStartPosition = 
            //    GetStartTilePositionForMoveSecondAndAbove(word: word, substring: wordFromTilesOnBoard, 
            //    wordOrientation: wordOrientation, tilesOnBoardFromAnchor: tilesOnBoardFromAnchor);


            List<int> indexesOfSubstring = word.AllIndexesOf(wordFromTilesOnBoard).ToList();

            Random random = new Random();
            int randomIndexOfSubstring = indexesOfSubstring[random.Next(0, indexesOfSubstring.Count)];

            int wordStartPosition = -1;

            if (wordOrientation == WordOrientation.Horizontal)
            {
                wordStartPosition = (int) tilesOnBoardFromAnchor.Min(tile => tile.PositionOnBoard.Value.X) -
                                    randomIndexOfSubstring;
            }
            else if (wordOrientation == WordOrientation.Vertical)
            {
                wordStartPosition = (int)tilesOnBoardFromAnchor.Min(tile => tile.PositionOnBoard.Value.Y) -
                                    randomIndexOfSubstring;
            }


            AssignPositionsOnBoardToTilesInMove(word: word, tilesInMove: randomTilesFromTilesRack,
                tilesOnBoardFromAnchor: tilesOnBoardFromAnchor, startTilePosition: wordStartPosition, 
                anchorPosition: (Point)anchorTile.PositionOnBoard, wordOrientation: wordOrientation,
                substring: wordFromTilesOnBoard, substringIndex: randomIndexOfSubstring);
        }

        //private int GetStartTilePositionForMoveSecondAndAbove(string word, string substring, 
        //    WordOrientation wordOrientation, List<Tile> tilesOnBoardFromAnchor)
        //{

        //    List<int> indexesOfSubstring = word.AllIndexesOf(substring).ToList();

        //    Random random = new Random();
        //    int randomIndexOfSubstring = indexesOfSubstring[random.Next(0, indexesOfSubstring.Count)];

        //    int wordStartPosition = -1;

        //    if (wordOrientation == WordOrientation.Horizontal)
        //    {
        //        wordStartPosition = (int) tilesOnBoardFromAnchor.Min(tile => tile.PositionOnBoard.Value.X) -
        //                            randomIndexOfSubstring;
        //    }
        //    else if (wordOrientation == WordOrientation.Vertical)
        //    {
        //        wordStartPosition = (int)tilesOnBoardFromAnchor.Min(tile => tile.PositionOnBoard.Value.Y) -
        //                            randomIndexOfSubstring;
        //    }

        //    return wordStartPosition;
        //}

       
    }

    public static class AllIndexesOfSubstringInsideString
    {
        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    break;
                yield return index;
            }
        }
    }
}
