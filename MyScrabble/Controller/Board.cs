﻿
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyScrabble.Model;
using MyScrabble.View;


namespace MyScrabble.Controller
{

    public class Board
    {
        private readonly BoardUC _boardUC;
        public const int BOARD_SIZE = 15;

        private readonly Tile[,] _boardArray = new Tile[BOARD_SIZE, BOARD_SIZE];


        public Board(BoardUC _boardUC)
        {
            this._boardUC = _boardUC;
        }


        public void PlaceATileOnBoard(Tile tileToPlaceOnBoard, int xPosition, int yPosition)
        {
                tileToPlaceOnBoard.PositionOnBoard = new Point(xPosition, yPosition);
                
                _boardArray[xPosition, yPosition] = tileToPlaceOnBoard;
        }


        public void RemoveTiles(List<Tile> tilesToRemoveFromBoard)
        {
            foreach (Tile tile in tilesToRemoveFromBoard)
            {
                RemoveATile(tile);
            }
        }

        public void RemoveATile(Tile tileToRemoveFromBoard)
        {
            if (tileToRemoveFromBoard.PositionOnBoard != null)
            {
                _boardArray[(int)tileToRemoveFromBoard.PositionOnBoard.Value.X, (int)tileToRemoveFromBoard.PositionOnBoard.Value.Y] = null;

                tileToRemoveFromBoard.PositionOnBoard = null;
            }
            else
            {
                throw new Exception("The tile to remove doesn't have position on board.");
            }
            
        }
        

        public void MakeAMoveHuman()
        {
            List<Tile> tilesInMove = _boardArray.Cast<Tile>().
                 Where(tile => tile != null && tile.WasMoveMade == false).
                 ToList();


            MarkTilesAfterMoveWasMade(tilesInMove);

            if (Game.IsFirstMove)
            {
                Game.SetAfterFirstMove();
            }
        }

        public void MakeAMoveAI(List<Tile> tilesInMove)
        {
            foreach (Tile tile in tilesInMove)
            {
                if (tile.PositionOnBoard != null)
                    PlaceATileOnBoard(tile, (int)tile.PositionOnBoard.Value.X, (int)tile.PositionOnBoard.Value.Y);
                else
                {
                    throw new Exception("Tile must have assigned a position on board");
                }
            }

            _boardUC.MakeAMoveAI(tilesInMove);

            MarkTilesAfterMoveWasMade(tilesInMove);

            if (Game.IsFirstMove)
            {
                Game.SetAfterFirstMove();
            }
        }

        private void MarkTilesAfterMoveWasMade(List<Tile> tilesInMove)
        {
            foreach (Tile tile in tilesInMove)
            {
                tile.WasMoveMade = true;
            }
        }


        public List<string> ValidateMove()
        {
            List<string> validationMessages = new List<string>();

            List<Tile> tilesInMove = _boardArray.Cast<Tile>().
                Where(tile => tile != null && tile.WasMoveMade == false).
                ToList();

            ScrabbleDictionary scrabbleDictionary = new ScrabbleDictionary();

            if (IsMoveEmpty(tilesInMove))
            {
                validationMessages.Add("You didn't place any tiles on board");
            }
            else if (tilesInMove.Count >= 2 && !AreTilesInSameRowOrColumn(tilesInMove))
            {
                validationMessages.Add("The tiles are not in one row or column");
            }
            else if (tilesInMove.Count >= 2 && !AreTilesNextToEachOther(tilesInMove))
            {
                validationMessages.Add("The tiles are not next to each other");
            }
            else if (Game.IsFirstMove && !WordGoesThroughTheCenterOfBoard(tilesInMove))
            {
                validationMessages.Add("The first word in the game should go through the center of board");
            }
            else if (Game.IsFirstMove && tilesInMove.Count == 1)
            {
                validationMessages.Add("Single-letter words are not allowed in Scrabble");
            }
            else if (!Game.IsFirstMove && !FormWordsInCrosswordWay(tilesInMove))
            {
                validationMessages.Add("All the words (except the first one) must be formed" +
                                        " in a crossword way");
            }
            else if (!FormsWordsFromDictionary(tilesInMove, scrabbleDictionary))
            {
                validationMessages.Add("The word is not in the official Scrabble dictionary");
            }
            else if (!FormsNoInvalidWordsInAnyDirection(tilesInMove, scrabbleDictionary))
            {
                validationMessages.Add("The word forms at least one invalid word in some direction");
            }

            return validationMessages;
        }


        private bool IsMoveEmpty(List<Tile> tilesInMove)
        {
            if(tilesInMove != null)
            {
                if (tilesInMove.Count > 0)
                {
                    return false;
                }

                return true;
                
            }

            return true;
        }

        private bool AreTilesInSameRowOrColumn(List<Tile> tilesInMove)
        {
            bool areInSameRow = false;
            bool areInSameColumn = false;

            //here we assume that there were at least two tiles in a move
            //if there is just one tile in move, checking if it is in the same
            //row or column with others just doesn't make sense
            if (tilesInMove == null || tilesInMove.Count <= 1)
            {
                throw new 
                    Exception("There should be at least two tiles placed in a move to check" +
                                "if tiles are in the same row or column");
            }
            else if (tilesInMove.Count >= 2)
            {
                int xPosition = (int)tilesInMove[0].PositionOnBoard.Value.X;
                int yPosition = (int)tilesInMove[0].PositionOnBoard.Value.Y;


                areInSameRow = true;
                areInSameColumn = true;

                foreach (Tile tile in tilesInMove)
                {
                    if (areInSameColumn && (int)tile.PositionOnBoard.Value.X != xPosition)
                    {
                        areInSameColumn = false;
                    }

                    if (areInSameRow && (int)tile.PositionOnBoard.Value.Y != yPosition)
                    {
                        areInSameRow = false;
                    }

                    if (!areInSameColumn && !areInSameRow)
                    {
                        break;
                    }
                }
            }


            return areInSameRow ^ areInSameColumn;
        }


        //this method takes into account both tiles in the move and
        //tiles on board next to them
        private bool AreTilesNextToEachOther(List<Tile> tilesInMove)
        {
            if (tilesInMove == null || tilesInMove.Count <= 1)
            {
                throw new
                    Exception("There should be at least two tiles in move to check" +
                                "if tiles are next to each other");
            }

            int? commonColumn = null;
            int? commonRow = null;
            
            GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);


            bool areTilesNextToEachOtherInColumn = false;
            bool areTilesNextToEachOtherInRow = false;

            if (commonColumn != null)
            {
                areTilesNextToEachOtherInColumn = AreTilesNextToEachOtherInColumn(tilesInMove, (int)commonColumn);
            }
            if (commonRow != null)
            {
                areTilesNextToEachOtherInRow = AreTilesNextToEachOtherInRow(tilesInMove, (int)commonRow);
            }
            if (commonColumn == null && commonRow == null)
            {
                throw new 
                    Exception("Cannot check if tiles are next to each other because" +
                                "they are not in the same row or column");
            }

            return areTilesNextToEachOtherInColumn || areTilesNextToEachOtherInRow;
        }
 

        private bool AreTilesNextToEachOtherInColumn(List<Tile> tilesInMove, int column)
        {
            bool result = false;

            int indexOfTopMostTile = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.Y);

            //starting from the bottom-most tile, the top-most index till gap (if any)
            int topMostIndexContinuous = GetWordTopMostIndex(column, tilesInMove);


            int indexOfBottomMostTile = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.Y);
            int bottomMostIndexContinuous = GetWordBottomMostIndex(column, tilesInMove);

            
            if (topMostIndexContinuous <= indexOfTopMostTile &&
                bottomMostIndexContinuous >= indexOfBottomMostTile)
            {
                result = true;
            }
            

            return result;
        }

        private bool AreTilesNextToEachOtherInRow(List<Tile> tilesInMove, int row)
        {
            bool result = false;

            int indexOfLeftMostTile = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.X);

            //starting from the right-most tile, the left-most index till gap (if any)
            int leftMostIndexContinuous = GetWordLeftMostIndex(row, tilesInMove);


            int indexOfRightMostTile = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.X);
            int rightMostIndexContinuous = GetWordRightMostIndex(row, tilesInMove);


            if (leftMostIndexContinuous <= indexOfLeftMostTile &&
                rightMostIndexContinuous >= indexOfRightMostTile)
            {
                result = true;
            }

            return result;
        }

        private bool FormsWordsFromDictionary(List<Tile> tilesInMove, ScrabbleDictionary scrabbleDictionary)
        {
            //here we assume the tiles in move are next to each other in one row or column

            bool isWordInDictionaryInColumn = false;
            bool isWordInDictionaryInRow = false;


            int? commonColumn = null;
            int? commonRow = null;

            GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);

            if (commonColumn != null)
            {
                string wordInColumn = GetWordInColumn((int)commonColumn, tilesInMove);
                isWordInDictionaryInColumn = scrabbleDictionary.IsWordInDictionary(wordInColumn);
            }
            if (commonRow != null)
            {
                string wordInRow = GetWordInRow((int)commonRow, tilesInMove);
                isWordInDictionaryInRow = scrabbleDictionary.IsWordInDictionary(wordInRow);
            }
            if (commonColumn == null && commonRow == null)
            {
                throw new
                    Exception("Cannot check if the word is in dictionary because" +
                                "tiles are not in the same row or column");
            }

            //if a given tile form words both in column and row,
            //then both of these words must be in dictionary
            if (commonColumn != null && commonRow != null)
            {
                return isWordInDictionaryInColumn && isWordInDictionaryInRow;
            }

            return isWordInDictionaryInColumn ^ isWordInDictionaryInRow;
        }

        private void GetTilesCommonRowOrColumnOrBoth(List<Tile> tilesInMove, ref int? commonColumn, ref int? commonRow)
        {
            if (tilesInMove.Count >= 2)
            {
                if (tilesInMove[0].PositionOnBoard.Value.X == tilesInMove[1].PositionOnBoard.Value.X)
                {
                    commonColumn = (int)tilesInMove[0].PositionOnBoard.Value.X;
                }
                else if (tilesInMove[0].PositionOnBoard.Value.Y == tilesInMove[1].PositionOnBoard.Value.Y)
                {
                    commonRow = (int)tilesInMove[0].PositionOnBoard.Value.Y;
                }
            }
            else if (tilesInMove.Count == 1)
            {
                int column = (int)tilesInMove[0].PositionOnBoard.Value.X;
                int row = (int)tilesInMove[0].PositionOnBoard.Value.Y;

                //if there is a tile placed above or below, then
                //the single tile placed in the current move have a common column with them
                if ((column >= 1 && _boardArray[column, row - 1] != null) ||
                (column <= BOARD_SIZE - 2 && _boardArray[column, row + 1] != null))
                {
                    commonColumn = column;
                }

                //if there is a tile placed to the left or to the right, then
                //the single tile placed in the current move have a common row with them
                if ((row >= 1 && _boardArray[column - 1, row] != null) ||
                (row <= BOARD_SIZE - 2 && _boardArray[column + 1, row] != null))
                {
                    commonRow = row;
                }
            }
        }

        public string GetWordInRow(int row, List<Tile> tilesInMove)
        {
            int wordLeftMostIndex = GetWordLeftMostIndex(row, tilesInMove);
            int wordRightMostIndex = GetWordRightMostIndex(row, tilesInMove);


            string word = BuildWordFromHorizontalRange(row, wordLeftMostIndex, wordRightMostIndex);

            return word;
        }

        public List<Tile> GetTilesOfWordInRow(int row, List<Tile> tilesInMove)
        {
            int wordLeftMostIndex = GetWordLeftMostIndex(row, tilesInMove);
            int wordRightMostIndex = GetWordRightMostIndex(row, tilesInMove);


            List<Tile> tilesOfWordInRow =
                _boardArray.Cast<Tile>().
                 Where(tile => tile != null && 
                     tile.PositionOnBoard.Value.Y == row &&
                     tile.PositionOnBoard.Value.X >= wordLeftMostIndex &&
                     tile.PositionOnBoard.Value.X <= wordRightMostIndex).
                 ToList();

            return tilesOfWordInRow;
        }

        //this method (and the similar ones for right, top and bottom) 
        //consider both tiles in move and tiles on board next to them
        private int GetWordLeftMostIndex(int row, List<Tile> tilesInMove)
        {
            //a starting index - takes into account the possibility
            //of a "gap" in word's tiles
            int wordLeftMostIndex = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.X);

            //extending range of the word including tiles placed in previous moves
            if (wordLeftMostIndex >= 1)
            {
                while (_boardArray[wordLeftMostIndex - 1, row] != null)
                {
                    wordLeftMostIndex -= 1;
                }
            }
            return wordLeftMostIndex;
        }

        private int GetWordRightMostIndex(int row, List<Tile> tilesInMove)
        {
            //a starting index - takes into account the possibility
            //of a "gap" in word's tiles
            int wordRightMostIndex = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.X);

            if (wordRightMostIndex <= BOARD_SIZE - 2)
            {
                while (_boardArray[wordRightMostIndex + 1, row] != null)
                {
                    wordRightMostIndex += 1;
                }
            }
            return wordRightMostIndex;
        }

        private string BuildWordFromHorizontalRange(int row, int wordLeftMostIndex, int wordRightMostIndex)
        {
            StringBuilder sbWord = new StringBuilder();

            for (int xIndex = wordLeftMostIndex; xIndex <= wordRightMostIndex; xIndex++)
            {
                char tileLetter = _boardArray[xIndex, row].Letter;
                sbWord.Append(tileLetter);
            }

            return sbWord.ToString();
        }

        public string GetWordInColumn(int column, List<Tile> tilesInMove)
        {
            int wordTopMostIndex = GetWordTopMostIndex(column, tilesInMove);
            int wordBottomMostIndex = GetWordBottomMostIndex(column, tilesInMove);

            string word = BuildWordFromVerticalRange(column, wordTopMostIndex, wordBottomMostIndex);

            return word;
        }

        public List<Tile> GetTilesOfWordInColumn(int column, List<Tile> tilesInMove)
        {
            int wordTopMostIndex = GetWordTopMostIndex(column, tilesInMove);
            int wordBottomMostIndex = GetWordBottomMostIndex(column, tilesInMove);


            List<Tile> tilesOfWordInColumn =
                _boardArray.Cast<Tile>().
                 Where(tile => tile != null &&
                     tile.PositionOnBoard.Value.X == column &&
                     tile.PositionOnBoard.Value.Y >= wordTopMostIndex &&
                     tile.PositionOnBoard.Value.Y <= wordBottomMostIndex).
                 ToList();

            return tilesOfWordInColumn;
        }

        private int GetWordTopMostIndex(int column, List<Tile> tilesInMove)
        {
            //a starting index - takes into account the possibility
            //of a "gap" in word's tiles
            int wordTopMostIndex = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.Y);

            //extending range of the word including tiles placed in previous moves
            if(wordTopMostIndex >= 1)
            {
                while (_boardArray[column, wordTopMostIndex - 1] != null)
                {
                    wordTopMostIndex -= 1;
                }
            }

            return wordTopMostIndex;
        }

        private int GetWordBottomMostIndex(int column, List<Tile> tilesInMove)
        {
            //a starting index - takes into account the possibility
            //of a "gap" in word's tiles
            int wordBottomMostIndex = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.Y);

            //extending range of the word including tiles placed in previous moves
            if (wordBottomMostIndex <= BOARD_SIZE - 2)
            {
                while (_boardArray[column, wordBottomMostIndex + 1] != null)
                {
                    wordBottomMostIndex += 1;
                }
            }

            return wordBottomMostIndex;
        }

        private string BuildWordFromVerticalRange(int column, int wordTopMostIndex, int wordBottomMostIndex)
        {
            StringBuilder sbWord = new StringBuilder();

            for (int yIndex = wordTopMostIndex; yIndex <= wordBottomMostIndex; yIndex++)
            {
                char tileLetter = _boardArray[column, yIndex].Letter;
                sbWord.Append(tileLetter);
            }

            return sbWord.ToString();
        }

        private bool WordGoesThroughTheCenterOfBoard(List<Tile> tilesInMove)
        {
            foreach (Tile tileInMove in tilesInMove)
            {
                if (tileInMove.PositionOnBoard == new Point(7, 7))
                {
                    return true;
                }
            }

            return false;
        }

        private bool FormWordsInCrosswordWay(List<Tile> tilesInMove)
        {

            int? commonColumn = null;
            int? commonRow = null;

            GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);

            bool result = false;

            //that should include only single-letter words?
            if (commonRow != null && commonColumn != null)
            {
                result = CheckAdjacentAndCrossingTilesForHorizontalWord(tilesInMove, (int)commonRow) &&
                    CheckAdjacentAndCrossingTilesForVerticalWord(tilesInMove, (int)commonColumn);
            }
            else
            {
                if (commonRow != null)
                {
                    result = CheckAdjacentAndCrossingTilesForHorizontalWord(tilesInMove, (int)commonRow);
                }
                //I'm not sure whether row and column should be exclusive here
                //Should the word be either vertical or horizontal?
                //What about single-letter words?
                else if (commonColumn != null)
                {
                    result = CheckAdjacentAndCrossingTilesForVerticalWord(tilesInMove, (int)commonColumn);
                }
            }
            

            return result;
        }

        private bool CheckAdjacentAndCrossingTilesForHorizontalWord(List<Tile> tilesInMove, int commonRow)
        {

            int wordLeftMostIndex = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.X);
            int wordRightMostIndex = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.X);


            //for the left-most tile in word
            //-> check tiles adjacent to the left

             //for the right-most tile in word
            //-> check tiles adjacent to the right

            if (IsThereTileAdjacentToTheLeft(wordLeftMostIndex, (int)commonRow) ||
               IsThereTileAdjacentToTheRight(wordRightMostIndex, (int)commonRow) )
            {
                return true;
            } 


            //for all tiles
            //-> check tiles adjacent above and below
            for (int xIndex = wordLeftMostIndex; xIndex <= wordRightMostIndex; xIndex++)
            {
                if(IsThereTileAdjacentAbove(xIndex, (int)commonRow) ||
                    IsThereTileAdjacentBelow(xIndex, (int)commonRow))
                {
                    return true;
                }
            }

            return false; ;
        }

        private bool CheckAdjacentAndCrossingTilesForVerticalWord(List<Tile> tilesInMove, int commonColumn)
        {

            int wordTopMostIndex = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.Y);
            int wordBottomMostIndex = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.Y);


            //for the top-most tile in word
            //-> check tiles adjacent above

            //for the bottom-most tile in word
            //-> check tiles adjacent below

            if (IsThereTileAdjacentAbove((int)commonColumn, wordTopMostIndex) ||
               IsThereTileAdjacentBelow((int)commonColumn, wordBottomMostIndex))
            {
                return true;
            }


            //for all tiles
            //-> check tiles adjacent to the left and to the right
            for (int yIndex = wordTopMostIndex; yIndex <= wordBottomMostIndex; yIndex++)
            {
                if (IsThereTileAdjacentToTheLeft((int)commonColumn, yIndex) ||
                    IsThereTileAdjacentToTheRight((int)commonColumn, yIndex))
                {
                    return true;
                }
            }

            return false; ;
        }

        private bool IsThereTileAdjacentToTheLeft(int xIndex, int yIndex)
        {
            if (xIndex >= 1 && _boardArray[xIndex - 1, yIndex] != null)
            {
                return true;
            }

            return false;
        }

        private bool IsThereTileAdjacentToTheRight(int xIndex, int yIndex)
        {
            if (xIndex <= BOARD_SIZE - 2 && _boardArray[xIndex + 1, yIndex] != null)
            {
                return true;
            }

            return false;
        }

        private bool IsThereTileAdjacentAbove(int xIndex, int yIndex)
        {
            if (yIndex >= 1 && _boardArray[xIndex, yIndex - 1] != null)
            {
                return true;
            }

            return false;
        }

        private bool IsThereTileAdjacentBelow(int xIndex, int yIndex)
        {
            if (yIndex <= BOARD_SIZE - 2 && _boardArray[xIndex, yIndex + 1] != null)
            {
                return true;
            }

            return false;
        }

        public bool IsThePlaceOnBoardFree(int xPosition, int yPosition)
        {
            if (_boardArray[xPosition, yPosition] == null)
            {
                return true;
            }

            return false;
        }

        public bool FormsNoInvalidWordsInAnyDirection(List<Tile> tilesInMove, ScrabbleDictionary scrabbleDictionary)
        {
            int? commonColumn = null;
            int? commonRow = null;

            GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);


            //that case includes only single-letter words, right?
            if (commonRow != null && commonColumn != null)
            {
                return CheckInvalidWordsForHorizontalTiles(tilesInMove, commonRow, scrabbleDictionary) &&
                       CheckInvalidWordsForVerticalTiles(tilesInMove, commonColumn, scrabbleDictionary);
            }
            if (commonRow != null)
            {
                return CheckInvalidWordsForHorizontalTiles(tilesInMove, commonRow, scrabbleDictionary);
            }
            if (commonColumn != null)
            {
                return CheckInvalidWordsForVerticalTiles(tilesInMove, commonColumn, scrabbleDictionary);
            }
            
            return true;
        }

        private bool CheckInvalidWordsForHorizontalTiles(List<Tile> tilesInMove, int? commonRow, ScrabbleDictionary scrabbleDictionary)
        {

            int wordLeftMostIndex = GetWordLeftMostIndex((int) commonRow, tilesInMove);
            int wordRightMostIndex = GetWordRightMostIndex((int) commonRow, tilesInMove);

            //if (IsThereTileAdjacentToTheLeft(wordLeftMostIndex, (int) commonRow) ||
            //    IsThereTileAdjacentToTheRight(wordRightMostIndex, (int) commonRow))
            {
                string horizontalWord = 
                    BuildWordFromHorizontalRange((int)commonRow, wordLeftMostIndex, wordRightMostIndex);

                if (!scrabbleDictionary.WordList.Contains(horizontalWord))
                {
                    return false;
                }
            }


            foreach (Tile tileInMove in tilesInMove)
            {
                int xIndex = (int)tileInMove.PositionOnBoard.Value.X;

                if (IsThereTileAdjacentAbove(xIndex, (int)commonRow) ||
                    IsThereTileAdjacentBelow(xIndex, (int)commonRow))
                {

                    int wordBottomMostIndex = GetWordBottomMostIndex(xIndex, new List<Tile>() { tileInMove });
                    int wordTopMostIndex = GetWordTopMostIndex(xIndex, new List<Tile>() { tileInMove });

                    string verticalWord = BuildWordFromVerticalRange(xIndex, wordTopMostIndex, wordBottomMostIndex);

                    if (!scrabbleDictionary.WordList.Contains(verticalWord))
                    {
                        return false;
                    }
                }
            }
            

            return true;
        }

        private bool CheckInvalidWordsForVerticalTiles(List<Tile> tilesInMove, int? commonColumn, ScrabbleDictionary scrabbleDictionary)
        {

            int wordTopMostIndex = GetWordTopMostIndex((int)commonColumn, tilesInMove);
            int wordBottomMostIndex = GetWordBottomMostIndex((int)commonColumn, tilesInMove);


            //if (IsThereTileAdjacentAbove(wordTopMostIndex, (int)commonColumn) ||
            //    IsThereTileAdjacentBelow(wordBottomMostIndex, (int)commonColumn))
            {
                string verticalWord =
                    BuildWordFromVerticalRange((int)commonColumn, wordTopMostIndex, wordBottomMostIndex);

                if (!scrabbleDictionary.WordList.Contains(verticalWord))
                {
                    return false;
                }
            }


            foreach(Tile tileInMove in tilesInMove)
            {
                int yIndex = (int)tileInMove.PositionOnBoard.Value.Y;

                if (IsThereTileAdjacentToTheLeft((int) commonColumn, yIndex) ||
                    IsThereTileAdjacentToTheRight((int) commonColumn, yIndex))
                {

                    int wordLeftMostIndex = GetWordLeftMostIndex(yIndex, new List<Tile>() { tileInMove});
                    int wordRightMostIndex = GetWordRightMostIndex(yIndex, new List<Tile>() { tileInMove });

                    string horizontalWord = BuildWordFromHorizontalRange(yIndex, wordLeftMostIndex, wordRightMostIndex);

                    if (!scrabbleDictionary.WordList.Contains(horizontalWord))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public List<Tile> GetTilesOnBoard()
        {
            List<Tile> tilesOnBoard = _boardArray.Cast<Tile>().
                 Where(tile => tile != null && tile.WasMoveMade == true).
                 ToList();

            return tilesOnBoard;
        }

        public bool IsTileTotallySurrounded(Tile tile)
        {
            int x = (int) tile.PositionOnBoard.Value.X;
            int y = (int) tile.PositionOnBoard.Value.Y;

            if (_boardArray[x - 1, y] != null && _boardArray[x + 1, y] != null
                && _boardArray[x, y - 1] != null && _boardArray[x, y + 1] != null)
            {
                return true;
            }

            return false;
        }

    }
}
