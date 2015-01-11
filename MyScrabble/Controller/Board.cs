
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyScrabble.Controller;
using MyScrabble.Controller.Tiles;
using MyScrabble.View;


namespace MyScrabble.Controller
{

    public class Board
    {
        public const int BOARD_SIZE = 15;

        private Tile[,] _boardArray = new Tile[BOARD_SIZE, BOARD_SIZE];

        private ScrabbleDictionary _scrabbleDictionary;

        public Board(ScrabbleDictionary scrabbleDictionary)
        {
            this._scrabbleDictionary = scrabbleDictionary;
        }


        public void PlaceATile(Tile tileToPlaceOnBoard, int xPosition, int yPosition)
        {

            //we assume that it is check in BoardUC whether or not a tile
            //can be placed in a given cell
            //I don't know where it makes bigger sense
            //if (canTileBePlacedHere(xPosition, yPosition))
            //{
                tileToPlaceOnBoard.PositionOnBoard = new Point(xPosition, yPosition);
                
                _boardArray[xPosition, yPosition] = tileToPlaceOnBoard;
            //}

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
        

        public void MakeAMove()
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

            List<Tile> tilesOnBoard = _boardArray.Cast<Tile>().
                Where(tile => tile != null).
                ToList();

            List<Tile> tilesInMove =
                tilesOnBoard.Where(tile => tile.WasMoveMade == false).
                ToList();


            if (IsMoveEmpty(tilesInMove))
            {
                validationMessages.Add("You didn't place any tiles on board");
            }
            else if (tilesInMove.Count >=2 )
            {
                if (!AreTilesInSameRowOrColumn(tilesInMove))
                {
                    validationMessages.Add("The tiles are not in one row or column");
                }
                else if(!AreTilesNextToEachOther(tilesOnBoard, tilesInMove))
                {
                    validationMessages.Add("The tiles are not next to each other");
                } 
            }
            if (Game.IsFirstMove)
            {
                if (!WordGoesThroughTheCenterOfBoard(tilesInMove))
                {
                    validationMessages.Add("The first word in the game should go through the center of board");
                }
            }
            else
            {
                if (!FormWordsInCrosswordWay(tilesInMove))
                {
                    validationMessages.Add("All the words (except the first one) must be formed" +
                        " in a crossword way");
                }
            }
            if (validationMessages.Count == 0 && !FormsWordsFromDictionary(tilesInMove))
            {
                validationMessages.Add("The word is not in the official Scrabble dictionary");
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
        private bool AreTilesNextToEachOther(List<Tile> tilesOnBoard, List<Tile> tilesInMove)
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

        private bool FormsWordsFromDictionary(List<Tile> tilesInMove)
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
                isWordInDictionaryInColumn = _scrabbleDictionary.IsWordInDictionary(wordInColumn);
            }
            if (commonRow != null)
            {
                string wordInRow = GetWordInRow((int)commonRow, tilesInMove);
                isWordInDictionaryInRow = _scrabbleDictionary.IsWordInDictionary(wordInRow);
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

        private string GetWordInRow(int row, List<Tile> tilesInMove)
        {
            int wordLeftMostIndex = GetWordLeftMostIndex(row, tilesInMove);
            int wordRightMostIndex = GetWordRightMostIndex(row, tilesInMove);


            string word = BuildWordFromHorizontalRange(row, wordLeftMostIndex, wordRightMostIndex);

            return word;
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

        private string GetWordInColumn(int column, List<Tile> tilesInMove)
        {
            int wordTopMostIndex = GetWordTopMostIndex(column, tilesInMove);
            int wordBottomMostIndex = GetWordBottomMostIndex(column, tilesInMove);

            string word = BuildWordFromVerticalRange(column, wordTopMostIndex, wordBottomMostIndex);

            return word;
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
            bool result = false;

            foreach (Tile tileInMove in tilesInMove)
            {
                if (tileInMove.PositionOnBoard == new Point(7, 7))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        private bool FormWordsInCrosswordWay(List<Tile> tilesInMove)
        {
            bool result = false;

            result = IsWordAdjacentToOthers() || DoesWordCrossOthers();

            return result;
        }

        private bool IsWordAdjacentToOthers()
        {
            return false;
        }

        private bool DoesWordCrossOthers()
        {
            return false;
        }

        public bool IsThePlaceOnBoardOccupied(int xPosition, int yPosition)
        {
            if (_boardArray[xPosition, yPosition] == null)
            {
                return true;
            }

            return false;
        } 

    }
}
