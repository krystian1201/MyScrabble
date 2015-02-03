using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

using MyScrabble.Model;
using MyScrabble.Utilities;
using MyScrabble.View;

namespace MyScrabble.Controller
{
    class AIPlayerBrute : BaseAIPlayer
    {
        private int highestScore = 0;
        private List<Tile> bestMove = null;
        private string wordOfBestMove = null;
        private Point bestMoveStartPosition = new Point(-1, -1);
        private WordOrientation bestMoveWordOrientation;
        private string bestMoveSubstringFromAnchor = null;
        private int bestWordSubstringFromAnchorIndex = -1;


        protected override List<Tile> GenerateFirstMove(TilesRack tilesRack, Board board)
        {
            List<Tile> tilesInTilesRack = tilesRack.TilesArray.ToList<Tile>();

            List<string> subsetsOfTilesInTilesRack = GetAllSubsetsOfTiles(tilesInTilesRack, 2, 7);

            List<string> alphabetizedSubsets = AlphabetizeSubsets(subsetsOfTilesInTilesRack);

            List<string> wordsFromTilesSubsets = GetWordsFromTilesSubsets(alphabetizedSubsets);

            List<Tile> tilesInMove = FindHighestScoringMoveForFirstMove(wordsFromTilesSubsets, tilesRack, board);

            string word = BuildStringFromTiles(tilesInMove);

            WordOrientation wordOrientation = board.GetWordOrientationFromTiles(tilesInMove);
            Point wordStartPosition = board.GetWordStartPositionFromTiles(tilesInMove, wordOrientation);
            

            AssignPositionsOnBoardToTilesInMove(word, tilesInMove, wordStartPosition, wordOrientation, "", null, board);

            return tilesInMove;
        }


        private List<string> GetAllSubsetsOfTiles(List<Tile> tilesInTilesRack, int minLength, int maxLength)
        {
            List<string> subsetsOfTiles = new List<string>();

            //it can happen that the tiles rack will containt less than 7 tiles
            //then we will return subsets up to the length of the number of tiles
            //in tiles rack
            int maxSubsetLength = Math.Min(tilesInTilesRack.Count, maxLength);

            for (int i = 0; i < Math.Pow(2, maxSubsetLength); i++)
            {

                BitArray bitArray = new BitArray(new int[] { i });

                StringBuilder subsetStringBuilder = new StringBuilder();

                for (int tileIndex = 0; tileIndex < maxSubsetLength; tileIndex++)
                {
                    if (bitArray[tileIndex] && tilesInTilesRack[tileIndex] != null)
                    {
                        subsetStringBuilder.Append(tilesInTilesRack[tileIndex].Letter);
                    }
                }

                subsetsOfTiles.Add(subsetStringBuilder.ToString());
            }


            return subsetsOfTiles.Where(subset => subset.Length >= minLength).ToList<string>();
        }


        private List<string> AlphabetizeSubsets(List<string> subsetsOfTilesInTilesRack)
        {
            List<string> alphabetizedSubsets = new List<string>();

            foreach (string subset in subsetsOfTilesInTilesRack)
            {
                alphabetizedSubsets.Add(_aiDictionary.AlphabetizeString(subset));

            }

            return alphabetizedSubsets;
        }

        private List<string> GetWordsFromTilesSubsets(List<string> alphabetizedSubsets)
        {
            List<string> wordsFromTilesSubsets = new List<string>();

            foreach (string subset in alphabetizedSubsets)
            {
                if (_aiDictionary.AlphabetizedWordsPermutations.ContainsKey(subset))
                {
                    List<string> wordsFromSubsetPermutations = 
                        _aiDictionary.AlphabetizedWordsPermutations[subset];

                    foreach (string word in wordsFromSubsetPermutations)
                    {
                        if (!wordsFromTilesSubsets.Contains(word))
                        {
                            wordsFromTilesSubsets.Add(word);
                        }
                        
                    }
                }
            }

            return wordsFromTilesSubsets;
        }

        private List<Tile> FindHighestScoringMoveForFirstMove(List<string> wordsFromTilesSubsets, TilesRack tilesRack, Board board)
        {
            int highestScore = 0;
            List<Tile> bestMove = null;

            foreach (string word in wordsFromTilesSubsets)
            {
                //the orientation of the first word really doesn't matter
                //because board is symmetrical
                WordOrientation wordOrientation = GetRandomWordOrientation();

                List<Point> allWordStartPositions = GetAllWordStartPositionsForFirstMove(word, wordOrientation);

                List<Tile> tilesInMove = GetTilesFromTilesRackInWord(word, tilesRack);

                foreach (Point wordStartPosition in allWordStartPositions)
                {
                    AssignPositionsOnBoardToTilesInMove(word, tilesInMove, wordStartPosition, wordOrientation, "", null, board);

                    int moveScore = board.GetScoreOfMove(tilesInMove);

                    if (moveScore > highestScore)
                    {
                        highestScore = moveScore;
                        bestMove = CopyTilesList(tilesInMove);
                    }

                    board.RemoveTiles(tilesInMove);
                }

            }

            return bestMove;
        }

       

        private List<Point> GetAllWordStartPositionsForFirstMove(string word, WordOrientation wordOrientation)
        {
            List<Point> wordStartPositions = new List<Point>();


            if (wordOrientation == WordOrientation.Horizontal)
            {
                for (int coordinate = 7 - word.Length + 1; coordinate < 8; coordinate++)
                {
                    wordStartPositions.Add(new Point(coordinate, 7));
                }
                
            }

            if (wordOrientation == WordOrientation.Vertical)
            {
                for (int coordinate = 7 - word.Length + 1; coordinate < 8; coordinate++)
                {
                    wordStartPositions.Add(new Point(7, coordinate));
                }               
            }

            return wordStartPositions;

        }

        private List<Tile> GetTilesFromTilesRackInWord(string word, TilesRack tilesRack)
        {
            List<Tile> tilesFromTilesRackInWord = new List<Tile>();
            List<Tile> tempTilesFromTilesRack = new List<Tile>(tilesRack.TilesArray);

            char[] wordCharArray = word.ToCharArray();

            foreach (char letter in wordCharArray)
            {
                Tile tileFromTilesRack =
                    tempTilesFromTilesRack.FirstOrDefault(tile => tile.Letter == letter);

                tilesFromTilesRackInWord.Add(tileFromTilesRack);

                //so that the tiles are drawn from tiles rack without repetitions
                tempTilesFromTilesRack.Remove(tileFromTilesRack);
            }

            return tilesFromTilesRackInWord;
        }

        private List<Tile> CopyTilesList(List<Tile> listToCopy)
        {
            List<Tile> copiedList = new List<Tile>();

            foreach (Tile tile in listToCopy)
            {
                Tile newTile = new Tile(tile);
                copiedList.Add(newTile);
            }

            return copiedList;
        }


        protected override List<Tile> GenerateSecondAndAboveMove(TilesRack tilesRack, Board board)
        {
            List<Tile> tilesInTilesRack = tilesRack.TilesArray.ToList<Tile>();

            List<string> subsetsOfTilesInTilesRack = GetAllSubsetsOfTiles(tilesInTilesRack, 1, 7);

            //any tile on board can be "anchor" tile
            List<Tile> anchorTiles = board.GetTilesOnBoard();

            ResetBestMove();

            WordOrientation firstCheckedWordOrientation = GetRandomWordOrientation();

            try
            {
                if (firstCheckedWordOrientation == WordOrientation.Horizontal)
                {
                    GetBestMoveInGivenOrientation(tilesRack, board, subsetsOfTilesInTilesRack, anchorTiles,
                    WordOrientation.Horizontal);

                    GetBestMoveInGivenOrientation(tilesRack, board, subsetsOfTilesInTilesRack, anchorTiles,
                    WordOrientation.Vertical);
                }
                else
                {
                    GetBestMoveInGivenOrientation(tilesRack, board, subsetsOfTilesInTilesRack, anchorTiles,
                    WordOrientation.Vertical);

                    GetBestMoveInGivenOrientation(tilesRack, board, subsetsOfTilesInTilesRack, anchorTiles,
                    WordOrientation.Horizontal);
                }
               
                
            }
            catch (OperationCanceledException)
            {
            }
            

            AssignPositionsOnBoardToTilesInMove(wordOfBestMove, bestMove, bestMoveStartPosition, 
                bestMoveWordOrientation, bestMoveSubstringFromAnchor, bestWordSubstringFromAnchorIndex, board);

            return bestMove;
        }

        private void ResetBestMove()
        {
             highestScore = 0;
             bestMove = null;
             wordOfBestMove = null;
             bestMoveStartPosition = new Point(-1, -1);
             bestMoveSubstringFromAnchor = null;
             bestWordSubstringFromAnchorIndex = -1;
        }

        private void GetBestMoveInGivenOrientation(TilesRack tilesRack, Board board, List<string> subsetsOfTilesInTilesRack, 
            List<Tile> anchorTiles, WordOrientation wordOrientation)
        {
            foreach (string subsetFromTilesRack in subsetsOfTilesInTilesRack)
            {
                foreach (Tile anchorTile in anchorTiles)
                {
                    GetBestMoveFromAnchorTile(tilesRack, board, wordOrientation, anchorTile, subsetFromTilesRack);
                }
            }
        }

        private void GetBestMoveFromAnchorTile(TilesRack tilesRack, Board board, WordOrientation wordOrientation, 
            Tile anchorTile, string subsetFromTilesRack)
        {
            List<Tile> tilesOnBoardFromAnchor;

            try
            {
                tilesOnBoardFromAnchor = board.GetTilesOnBoardFromAnchor(anchorTile, wordOrientation);
            }
            catch (Exception)
            {
                
                return;
            }

            string stringFromTilesFromAnchor = BuildStringFromTiles(tilesOnBoardFromAnchor);

            string stringFromTilesRackAndAnchorTiles =
                subsetFromTilesRack + stringFromTilesFromAnchor;

            string alphabetizedString = _aiDictionary.AlphabetizeString(stringFromTilesRackAndAnchorTiles);



            if (_aiDictionary.AlphabetizedWordsPermutations.ContainsKey(alphabetizedString))
            {
                List<string> words = _aiDictionary.AlphabetizedWordsPermutations[alphabetizedString];

                foreach (string word in words)
                {
                   
                     GetBestMoveFromWord(tilesRack, board, wordOrientation, word, subsetFromTilesRack, stringFromTilesFromAnchor,
                        tilesOnBoardFromAnchor);
                      
                }
            }
        }

        private void GetBestMoveFromWord(TilesRack tilesRack, Board board, WordOrientation wordOrientation, string word, 
            string subsetFromTilesRack, string stringFromTilesFromAnchor, List<Tile> tilesOnBoardFromAnchor)
        {

            if (word.Contains(stringFromTilesFromAnchor))
            {
                List<int> substringStartPositions;

                List<Point> wordStartPositions =
                    GetStartTilePositionsForMoveSecondAndAbove(word, stringFromTilesFromAnchor,
                        wordOrientation, tilesOnBoardFromAnchor, out substringStartPositions);

                List<Tile> tilesInMove = GetTilesFromTilesRackInWord(subsetFromTilesRack, tilesRack);

                int startPositionIndex = 0;

                foreach (Point wordStartPosition in wordStartPositions)
                {
                    try
                    {
                        AssignPositionsOnBoardToTilesInMove(word, tilesInMove, wordStartPosition, wordOrientation,
                        stringFromTilesFromAnchor, substringStartPositions[startPositionIndex], board);
                    }
                    catch (Exception)
                    {
                        return;
                    }

                    if (board.FormsNoInvalidWordsInAnyDirection(tilesInMove, new ScrabbleDictionary()))
                    {
                        int moveScore = board.GetScoreOfMove(tilesInMove);

                        if (moveScore > highestScore)
                        {
                            highestScore = moveScore;
                            bestMove = CopyTilesList(tilesInMove);
                            wordOfBestMove = word;
                            bestMoveStartPosition = wordStartPosition;
                            bestMoveWordOrientation = wordOrientation;
                            bestMoveSubstringFromAnchor = stringFromTilesFromAnchor;
                            bestWordSubstringFromAnchorIndex = substringStartPositions[startPositionIndex];

                            if (MainWindow.backgroundWorker.CancellationPending)
                            {
                                throw new OperationCanceledException();
                            }
                        }


                        startPositionIndex++;
                    }

                    board.RemoveTiles(tilesInMove);
                }
            }

        }

        private List<Point> GetStartTilePositionsForMoveSecondAndAbove(string word, string substring,
            WordOrientation wordOrientation, List<Tile> tilesOnBoardFromAnchor, out List<int> indexesOfSubstring)
        {
            if (!word.Contains(substring))
            {
                throw new ArgumentException("Word does not contain the substring");
            }

            if (String.IsNullOrEmpty(substring))
            {
                throw new ArgumentException("Substring cannot be empty - the word must be formed in a crossword fashion");
            }

            indexesOfSubstring = word.AllIndexesOf(substring).ToList();
            

            if (wordOrientation == WordOrientation.Horizontal)
            {
                List<Point> startTilePositions = new List<Point>();

                foreach (int indexOfSubstring in indexesOfSubstring)
                {
                    Point startTilePosition =
                        new Point(tilesOnBoardFromAnchor.Min(tile => tile.PositionOnBoard.Value.X) - indexOfSubstring,
                              tilesOnBoardFromAnchor.First().PositionOnBoard.Value.Y);

                    startTilePositions.Add(startTilePosition);
                }

                return startTilePositions;

            }

            if (wordOrientation == WordOrientation.Vertical)
            {
                List<Point> startTilePositions = new List<Point>();

                foreach (int indexOfSubstring in indexesOfSubstring)
                {
                    Point startTilePosition =
                        new Point(tilesOnBoardFromAnchor.First().PositionOnBoard.Value.X,
                              tilesOnBoardFromAnchor.Min(tile => tile.PositionOnBoard.Value.Y) - indexOfSubstring);

                    startTilePositions.Add(startTilePosition);
                }

                return startTilePositions;
            }

            throw new Exception("Incorrect word orientation");
        }
    }
}
