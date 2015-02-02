using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MyScrabble.Model;
using MyScrabble.Model.Tiles;

namespace MyScrabble.Controller
{
    class AIPlayerBrute : BaseAIPlayer
    {
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
                    if (bitArray[tileIndex])
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
            throw new NotImplementedException();
        }
    }
}
