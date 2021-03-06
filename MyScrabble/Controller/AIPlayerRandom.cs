﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

using MyScrabble.Model;
using MyScrabble.Utilities;


namespace MyScrabble.Controller
{
    public class AIPlayerRandom : BaseAIPlayer
    {
        readonly Random random = new Random();

        protected override List<Tile> GenerateFirstMove(TilesRack tilesRack, BoardController board)
        {

            //At this point we already know that the tiles chosen randomly
            //from tiles rack, will form a valid word. However, we don't know exactly
            //yet which word will be chosen
            List <Tile> tilesInMove = GetRandomTilesFromTilesRackThatFormValidWord(tilesRack);

            
            string stringFromTiles = BuildStringFromTiles(tilesInMove);
            string word = GetRandomWordFromRandomStringThatContainsSubstring(stringFromTiles, "");

            WordOrientation wordOrientation = GetRandomWordOrientation();

            Point startTilePosition = GetRandomStartTilePositionForFirstMove(word, wordOrientation);

            AssignPositionsOnBoardToTilesInMove(word, tilesInMove, startTilePosition, wordOrientation, "", null, board);


            return tilesInMove;
        }


        private List<Tile> GetRandomTilesFromTilesRackThatFormValidWord(TilesRack tilesRack)
        {

            string alphabetizedString;
            List<Tile> randomTilesFromTilesRack;

            //do it until the random word is in Scrabble dictionary (or alternatively the alphabetized
            //string appears as a key in the AI dictionary)
            do
            {
                int randomNumberOfTiles = GetRandomNumberOfTilesToChoseFromTilesRack(tilesRack);
                randomTilesFromTilesRack = GetRandomlyChosenNumberOfTilesFromTilesRack(tilesRack, randomNumberOfTiles);

                string stringFromTiles = BuildStringFromTiles(randomTilesFromTilesRack);
                alphabetizedString = _aiDictionary.AlphabetizeString(stringFromTiles);

            }
            while (!_aiDictionary.AlphabetizedWordsPermutations.ContainsKey(alphabetizedString));

            //TODO: can be added checking if the loop doesn't execute too long

            return randomTilesFromTilesRack;
        }


        private Point GetRandomStartTilePositionForFirstMove(string word, WordOrientation wordOrientation)
        {
            if (String.IsNullOrEmpty(word))
            {
                throw new ArgumentException("Word formed from tiles cannot be empty");
            }

            
            int randomCordinate = random.Next(7 - word.Length + 1, 8);

            if (wordOrientation == WordOrientation.Horizontal)
            {
                return new Point(randomCordinate, 7);
            }
            if (wordOrientation == WordOrientation.Vertical)
            {
                return new Point(7, randomCordinate);
            }

            return new Point(-1, -1);
        }


        protected override List<Tile> GenerateSecondAndAboveMove(TilesRack tilesRack, BoardController board)
        {
            List<Tile> tilesOnBoardFromAnchor;
            List<Tile> tilesInMove;
            WordOrientation wordOrientation;
            ScrabbleDictionary scrabbleDictionary = new ScrabbleDictionary();
            bool areInvalidWordsInSomeDirection;

            do
            {
                areInvalidWordsInSomeDirection = false;

                string word =
                    GetRandomValidWordForMoveSecondAndAbove(tilesRack, board, out wordOrientation, out tilesInMove, out tilesOnBoardFromAnchor);

                string substringFromTilesOnBoard = BuildStringFromTiles(tilesOnBoardFromAnchor);
                int substringIndex;

                Point startTilePosition =
                    GetRandomStartTilePositionForMoveSecondAndAbove(word, substringFromTilesOnBoard,
                    wordOrientation, tilesOnBoardFromAnchor, out substringIndex);


                try
                {
                    AssignPositionsOnBoardToTilesInMove(word, tilesInMove, startTilePosition,
                        wordOrientation, substringFromTilesOnBoard, substringIndex, board);
                }
                catch (Exception)
                {
                    continue;
                }


                if (!MoveValidator.FormsNoInvalidWordsInAnyDirection(tilesInMove, scrabbleDictionary))
                {
                    board.RemoveTiles(tilesInMove);
                    areInvalidWordsInSomeDirection = true;
                }

            } 
            while (areInvalidWordsInSomeDirection);


            return tilesInMove;
        }


        private string GetRandomValidWordForMoveSecondAndAbove(TilesRack tilesRack, BoardController board, out WordOrientation wordOrientation, 
                                                            out List<Tile> tilesInMove, out List<Tile> tilesOnBoardFromAnchor)
        {
            string word;

            do
            {
                wordOrientation = GetRandomWordOrientation();

                Tile anchorTile = GetRandomAnchorTile(board);

                tilesOnBoardFromAnchor = board.GetTilesOnBoardFromAnchor(anchorTile, wordOrientation);


                int randomNumberOfTiles = GetRandomNumberOfTilesToChoseFromTilesRack(tilesRack);
                tilesInMove = GetRandomlyChosenNumberOfTilesFromTilesRack(tilesRack, randomNumberOfTiles);


                word = GetRandomWordForMoveSecondAndAbove(tilesInMove, tilesOnBoardFromAnchor);

            } 
            while (word == null);

            return word;
        }


        private Tile GetRandomAnchorTile(BoardController board)
        {
            List<Tile> tilesOnBoard = board.GetTilesAlreadyOnBoard();

            if (tilesOnBoard.Count < 2)
            {
                throw new Exception("This method is called for move 2nd and above which means there should " +
                                    "be at least two tiles on board already");
            }

            Tile randomTileOnBoard;

            //if a tile is totally surrounded by other tiles from left, right, top and bottom
            //we cannot use such tile to form new words
            do
            {
                int randomTileOnBoardIndex = random.Next(0, tilesOnBoard.Count);

                randomTileOnBoard = tilesOnBoard[randomTileOnBoardIndex];
            }
            while (TilesPositionsHelper.IsTileTotallySurrounded(randomTileOnBoard));

            return randomTileOnBoard;
        }

        private int GetRandomNumberOfTilesToChoseFromTilesRack(TilesRack tilesRack)
        {
            int randomNumberOfTiles = -1;

            //it can happen that the tiles rack will contain less than 7 tiles
            //(at the end of game)

            int numberOfTilesInTilesRack = tilesRack.GetNumberOfTilesInTilesRack();

            //first move -> 2..7 letters
            //second move and above -> 1..7 letters
            if (GameController.IsFirstMove)
            {
                randomNumberOfTiles = random.Next(2, numberOfTilesInTilesRack + 1);
            }
            else if (!GameController.IsFirstMove)
            {
                randomNumberOfTiles = random.Next(1, numberOfTilesInTilesRack + 1);
            }

            if (randomNumberOfTiles < 1)
            {
                throw new Exception("The number of tiles to get randomly cannot be less than 1");
            }

            return randomNumberOfTiles;
        }


        private List<Tile> GetRandomlyChosenNumberOfTilesFromTilesRack(TilesRack tilesRack, int randomNumberOfTiles)
        {
            if (randomNumberOfTiles < 1)
            {
                throw new
                    ArgumentOutOfRangeException("randomNumberOfTiles", "The number of tiles to get randomly cannot be less than 1");
            }

            List<Tile> randomTilesFromTilesRack = new List<Tile>();

            for (int i = 0; i < randomNumberOfTiles; i++)
            {
                Tile tileFromTilesRack;

                //we need to make sure that the randomly chosen positions
                //don't repeat
                //it can also happen that the tiles rack will be empty at the
                //chosen position (near the end of game)
                do
                {
                    tileFromTilesRack = tilesRack.TilesArray[random.Next(0, 7)];
                }
                while (tileFromTilesRack == null ||
                         randomTilesFromTilesRack.Contains(tileFromTilesRack));


                randomTilesFromTilesRack.Add(tileFromTilesRack);
            }

            if (randomTilesFromTilesRack.Count != randomNumberOfTiles)
            {
                throw new Exception("The number of tiles returned is different from the number set");
            }

            return randomTilesFromTilesRack;
        }


        private string GetRandomWordForMoveSecondAndAbove(List<Tile> randomTiles, List<Tile> tilesOnBoardFromAnchor)
        {
            string wordFromTilesOnBoard = BuildStringFromTiles(tilesOnBoardFromAnchor);

            string stringFromRandomTiles = BuildStringFromTiles(randomTiles);


            try
            {
                string randomWord =
                    GetRandomWordFromRandomStringThatContainsSubstring(wordFromTilesOnBoard + stringFromRandomTiles,
                        wordFromTilesOnBoard);

                return randomWord;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
            catch (InvalidDataException)
            {
                return null;
            }
            
        }


        private string GetRandomWordFromRandomStringThatContainsSubstring(string randomString, string substring)
        {
            if (String.IsNullOrEmpty(randomString) || substring == null)
            {
                throw 
                    new ArgumentException("The string cannot be empty and the substring it is supposed" + 
                                          "to contain cannot be null.");
            }

            string alphabetizedString = _aiDictionary.AlphabetizeString(randomString);
            

            if (!_aiDictionary.AlphabetizedWordsPermutations.ContainsKey(alphabetizedString))
            {
                throw new KeyNotFoundException("The alphabetized string is not a key in the AI dictionary\n" +
                                               "(No valid word can be formed using this string)");
            }

            
            List<string> wordsFromKeyContainingSubstring = 
                _aiDictionary.AlphabetizedWordsPermutations[alphabetizedString].
                Where(word => word.Contains(substring)).ToList<string>();


            if (wordsFromKeyContainingSubstring.Count == 0)
            {
                throw new InvalidDataException("There can be no valid word formed from the string: " + randomString +
                                                " that contains substring: " + substring);
            }


            string randomWordFromKey = wordsFromKeyContainingSubstring[random.Next(0, wordsFromKeyContainingSubstring.Count)];
            

            return randomWordFromKey;

        }


        private Point GetRandomStartTilePositionForMoveSecondAndAbove(string word, string substring,
            WordOrientation wordOrientation, List<Tile> tilesOnBoardFromAnchor, out int substringIndex)
        {
            if (!word.Contains(substring))
            {
                throw new ArgumentException("Word does not contain the substring");
            }

            if (String.IsNullOrEmpty(substring))
            {
                throw new ArgumentException("Substring cannot be empty - the word must be formed in a crossword fashion");
            }

            List<int> indexesOfSubstring = word.AllIndexesOf(substring).ToList();

            substringIndex = indexesOfSubstring[random.Next(0, indexesOfSubstring.Count)];

            Point startTilePosition = new Point(-1, -1);

            if (wordOrientation == WordOrientation.Horizontal)
            {
                startTilePosition =
                    new Point(tilesOnBoardFromAnchor.Min(tile => tile.PositionOnBoard.Value.X) - substringIndex,
                              tilesOnBoardFromAnchor.First().PositionOnBoard.Value.Y);
            }
            else if (wordOrientation == WordOrientation.Vertical)
            {
                startTilePosition =
                    new Point(tilesOnBoardFromAnchor.First().PositionOnBoard.Value.X,
                              tilesOnBoardFromAnchor.Min(tile => tile.PositionOnBoard.Value.Y) - substringIndex);
            }

            return startTilePosition;
        }
    }
}
