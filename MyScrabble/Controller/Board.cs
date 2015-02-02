
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
        private Dictionary<Point, ScoringBonus> bonusScoringMatrix;

        public Board(BoardUC _boardUC)
        {
            this._boardUC = _boardUC;
            CreateBonusScoringMatrix();
        }

        #region BonusScoringMatrix

        private void CreateBonusScoringMatrix()
        {
            bonusScoringMatrix = new Dictionary<Point, ScoringBonus>();

            AddTrippleWordBonusSpotsToBonusMatrix();

            AddDoubleWordBonusSpotsToBonusMatrix();

            AddTrippleLetterBonusSpotsToBonusMatrix();

            AddDoubleLetterBonusSpotsToBonusMatrix();
        }

        private void AddTrippleWordBonusSpotsToBonusMatrix()
        {
            for (int row = 0; row < BOARD_SIZE; row += BOARD_SIZE / 2)
            {
                for (int column = 0; column < BOARD_SIZE; column += BOARD_SIZE / 2)
                {
                    bonusScoringMatrix.Add(new Point(column, row), ScoringBonus.TrippleWord);
                }
            }

            //the center cell has actually a double-word bonus
            bonusScoringMatrix.Remove(new Point(7, 7));
        }

        private void AddDoubleWordBonusSpotsToBonusMatrix()
        {
            for (int rowColumn = 1; rowColumn < BOARD_SIZE/2 - 2; rowColumn++)
            {
                bonusScoringMatrix.Add(new Point(rowColumn, rowColumn), ScoringBonus.DoubleWord);
                bonusScoringMatrix.Add(new Point(rowColumn, BOARD_SIZE - rowColumn - 1), ScoringBonus.DoubleWord);
                bonusScoringMatrix.Add(new Point(BOARD_SIZE - rowColumn - 1, rowColumn), ScoringBonus.DoubleWord);
                bonusScoringMatrix.Add(new Point(BOARD_SIZE - rowColumn - 1, BOARD_SIZE - rowColumn - 1),
                    ScoringBonus.DoubleWord);
            }

            bonusScoringMatrix.Add(new Point(7, 7), ScoringBonus.DoubleWord);
        }

        
        private void AddTrippleLetterBonusSpotsToBonusMatrix()
        {
            for (int row = 1; row < BOARD_SIZE; row += BOARD_SIZE / 4 + 1)
            {
                bonusScoringMatrix.Add(new Point(row, 5), ScoringBonus.TrippleLetter);
                bonusScoringMatrix.Add(new Point(row, BOARD_SIZE - 1 - 5), ScoringBonus.TrippleLetter);
            }

            bonusScoringMatrix.Add(new Point(5, 1), ScoringBonus.TrippleLetter);
            bonusScoringMatrix.Add(new Point(BOARD_SIZE - 1 - 5, 1), ScoringBonus.TrippleLetter);
            bonusScoringMatrix.Add(new Point(5, BOARD_SIZE - 1 - 1), ScoringBonus.TrippleLetter);
            bonusScoringMatrix.Add(new Point(BOARD_SIZE - 1 - 5, BOARD_SIZE - 1 - 1), ScoringBonus.TrippleLetter);
        }


        private void AddDoubleLetterBonusSpotsToBonusMatrix()
        {
            for (int column = 0; column < BOARD_SIZE; column += BOARD_SIZE / 2)
            {
                bonusScoringMatrix.Add(new Point(column, 3), ScoringBonus.DoubleLetter);
                bonusScoringMatrix.Add(new Point(column, BOARD_SIZE - 1 - 3), ScoringBonus.DoubleLetter);
            }

            for (int row = 0; row < BOARD_SIZE; row += BOARD_SIZE / 2)
            {
                bonusScoringMatrix.Add(new Point(3, row), ScoringBonus.DoubleLetter);
                bonusScoringMatrix.Add(new Point(BOARD_SIZE - 1 - 3, row), ScoringBonus.DoubleLetter);
            }

            for (int column = 2; column < BOARD_SIZE / 2; column += 4)
            {
                bonusScoringMatrix.Add(new Point(column, BOARD_SIZE / 2 - 1), ScoringBonus.DoubleLetter);
                bonusScoringMatrix.Add(new Point(column, BOARD_SIZE / 2 + 1), ScoringBonus.DoubleLetter);

                bonusScoringMatrix.Add(new Point(BOARD_SIZE - 1 - column, BOARD_SIZE / 2 - 1), ScoringBonus.DoubleLetter);
                bonusScoringMatrix.Add(new Point(BOARD_SIZE - 1 - column, BOARD_SIZE / 2 + 1), ScoringBonus.DoubleLetter);
            }

            
            bonusScoringMatrix.Add(new Point(BOARD_SIZE / 2 - 1, 2), ScoringBonus.DoubleLetter);
            bonusScoringMatrix.Add(new Point(BOARD_SIZE / 2 + 1, 2), ScoringBonus.DoubleLetter);

            bonusScoringMatrix.Add(new Point(BOARD_SIZE / 2 - 1, BOARD_SIZE - 1 - 2), ScoringBonus.DoubleLetter);
            bonusScoringMatrix.Add(new Point(BOARD_SIZE / 2 + 1, BOARD_SIZE - 1 - 2), ScoringBonus.DoubleLetter);
            
        }

        #endregion

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
        

        public List<Tile> MakeAMoveHuman()
        {
            List<Tile> tilesInMove = _boardArray.Cast<Tile>().
                 Where(tile => tile != null && tile.WasMoveMade == false).
                 ToList();


            MarkTilesAfterMoveWasMade(tilesInMove);

            if (Game.IsFirstMove)
            {
                Game.SetAfterFirstMove();
            }

            return tilesInMove;
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

        #region MoveValidation

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

        public List<Tile> GetTilesOfWordInRow(List<Tile> tilesInMove, int row)
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

        public List<Tile> GetTilesOfWordInColumn(List<Tile> tilesInMove, int column)
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
                return CheckInvalidWordsForHorizontalTiles(tilesInMove, (int)commonRow, scrabbleDictionary) &&
                       CheckInvalidWordsForVerticalTiles(tilesInMove, (int)commonColumn, scrabbleDictionary);
            }
            if (commonRow != null)
            {
                return CheckInvalidWordsForHorizontalTiles(tilesInMove, (int)commonRow, scrabbleDictionary);
            }
            if (commonColumn != null)
            {
                return CheckInvalidWordsForVerticalTiles(tilesInMove, (int)commonColumn, scrabbleDictionary);
            }
            
            return true;
        }

        private bool CheckInvalidWordsForHorizontalTiles(List<Tile> tilesInMove, int commonRow, ScrabbleDictionary scrabbleDictionary)
        {

            string horizontalWord = GetWordInRow(commonRow, tilesInMove);

            if (!scrabbleDictionary.WordList.Contains(horizontalWord))
            {
                return false;
            }
            

            foreach (Tile tileInMove in tilesInMove)
            {
                int xIndex = (int)tileInMove.PositionOnBoard.Value.X;

                if (IsThereTileAdjacentAbove(xIndex, commonRow) ||
                    IsThereTileAdjacentBelow(xIndex, commonRow))
                {

                    string verticalWord = GetWordInColumn(xIndex, tilesInMove);

                    if (!scrabbleDictionary.WordList.Contains(verticalWord))
                    {
                        return false;
                    }
                }
            }
            

            return true;
        }

        private bool CheckInvalidWordsForVerticalTiles(List<Tile> tilesInMove, int commonColumn, ScrabbleDictionary scrabbleDictionary)
        {

            string verticalWord = GetWordInColumn(commonColumn, tilesInMove);

            if (!scrabbleDictionary.WordList.Contains(verticalWord))
            {
                return false;
            }


            foreach(Tile tileInMove in tilesInMove)
            {
                int yIndex = (int)tileInMove.PositionOnBoard.Value.Y;

                if (IsThereTileAdjacentToTheLeft((int) commonColumn, yIndex) ||
                    IsThereTileAdjacentToTheRight((int) commonColumn, yIndex))
                {
                    string horizontalWord = GetWordInRow(yIndex, tilesInMove);
                    

                    if (!scrabbleDictionary.WordList.Contains(horizontalWord))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

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

        public int GetScoreOfMove(List<Tile> tilesInMove)
        {
            if (tilesInMove == null)
            {
                return 0;
            }

            List<List<Tile>> wordsFromMove = GetAllWordsFromMove(tilesInMove);
            int score = 0;

            foreach (List<Tile> word in wordsFromMove)
            {
                score += GetScoreOfWord(word, tilesInMove);
            }

            //a 50-points bonus for putting all 7 tiles in one move ("bingo")
            if (tilesInMove.Count == 7)
            {
                score += 50;
            }

            return score;
        }

       

        private int GetScoreOfWord(List<Tile> tilesInWord, List<Tile> tilesInMove)
        {
            int score = 0;
            int wordBonusFactor = 1;

            foreach (Tile tileInWord in tilesInWord)
            {
                int letterBonusFactor = 1;

                bool isTileFromCurrentMove = IsTileFromCurrentMove(tileInWord, tilesInMove);

                Point tileInWordPosition = (Point)tileInWord.PositionOnBoard;

                if (isTileFromCurrentMove && bonusScoringMatrix.ContainsKey(tileInWordPosition))
                {
                    if (bonusScoringMatrix[tileInWordPosition] == ScoringBonus.TrippleWord)
                    {
                        wordBonusFactor *= 3;
                    }
                    else if (bonusScoringMatrix[tileInWordPosition] == ScoringBonus.DoubleWord)
                    {
                        wordBonusFactor *= 2;
                    }
                    else if (bonusScoringMatrix[tileInWordPosition] == ScoringBonus.TrippleLetter)
                    {
                        letterBonusFactor = 3;
                    }
                    else if (bonusScoringMatrix[tileInWordPosition] == ScoringBonus.DoubleLetter)
                    {
                        letterBonusFactor = 2;
                    }
                }

                score += tileInWord.Points*letterBonusFactor;
            }

            score *= wordBonusFactor;
            return score;
        }

        private List<List<Tile>> GetAllWordsFromMove(List<Tile> tilesInMove)
        {
            int? commonColumn = null;
            int? commonRow = null;

            GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);


            //that case includes only single-letter words, right?
            if (commonRow != null && commonColumn != null)
            {
                List<List<Tile>> allWordsFromMove = GetAllWordsFromHorizontalMove(tilesInMove, (int) commonRow);

                return allWordsFromMove;
            }
            if (commonRow != null)
            {
                return GetAllWordsFromHorizontalMove(tilesInMove, (int)commonRow);
            }
            if (commonColumn != null)
            {
                return GetAllWordsFromVerticalMove(tilesInMove, (int)commonColumn);
            }

            return null;
        }

        private List<List<Tile>> GetAllWordsFromHorizontalMove(List<Tile> tilesInMove, int commonRow)
        {
            List<List<Tile>> wordsFromTiles = new List<List<Tile>>();

            List<Tile> horizontalWord = GetTilesOfWordInRow(tilesInMove, commonRow);
            wordsFromTiles.Add(horizontalWord);


            foreach (Tile tileInMove in tilesInMove)
            {
                int xIndex = (int)tileInMove.PositionOnBoard.Value.X;

                if (IsThereTileAdjacentAbove(xIndex, commonRow) ||
                    IsThereTileAdjacentBelow(xIndex, commonRow))
                {

                    List<Tile> verticalWord = GetTilesOfWordInColumn(tilesInMove, xIndex);
                    wordsFromTiles.Add(verticalWord);
                }
            }

            return wordsFromTiles;
        }

        private List<List<Tile>> GetAllWordsFromVerticalMove(List<Tile> tilesInMove, int commonColumn)
        {
            List<List<Tile>> wordsFromTiles = new List<List<Tile>>();

            List<Tile> verticalWord = GetTilesOfWordInColumn(tilesInMove, commonColumn);
            wordsFromTiles.Add(verticalWord);


            foreach (Tile tileInMove in tilesInMove)
            {
                int yIndex = (int)tileInMove.PositionOnBoard.Value.Y;

                if (IsThereTileAdjacentToTheLeft(commonColumn, yIndex) ||
                    IsThereTileAdjacentToTheRight(commonColumn, yIndex))
                {

                    List<Tile> horizontalWord = GetTilesOfWordInRow(tilesInMove, yIndex);
                    wordsFromTiles.Add(horizontalWord);
                }
            }

            return wordsFromTiles;
        }

        private bool IsTileFromCurrentMove(Tile tileInWord, List<Tile> tilesInMove)
        {
            Point tileInWordPosition = (Point)tileInWord.PositionOnBoard;
            Tile tileInMove = tilesInMove.FirstOrDefault(t => t.PositionOnBoard == tileInWordPosition);

            if (tileInMove != null)
            {
                return true;
            }

            return false;
        }

        public WordOrientation GetWordOrientationFromTiles(List<Tile> tilesInMove)
        {
            int? commonRow = null;
            int? commonColumn = null;

            GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);

            if (commonRow != null)
            {
                return WordOrientation.Horizontal;
            }

            if (commonColumn != null)
            {
                return WordOrientation.Vertical;
            }

            throw new Exception("Cannot assign orientation to word");
        }

        public Point GetWordStartPositionFromTiles(List<Tile> tilesInMove, WordOrientation wordOrientation)
        {
            if (wordOrientation == WordOrientation.Horizontal)
            {
                int minX = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.X);
                int commonRow = (int)tilesInMove.First().PositionOnBoard.Value.Y;

                return new Point(minX, commonRow);
            }

            if (wordOrientation == WordOrientation.Vertical)
            {
                int minY = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.Y);
                int commonColumn = (int)tilesInMove.First().PositionOnBoard.Value.X;

                return new Point(commonColumn, minY);
            }
            
            return new Point(-1, -1);
        }
    }

    public enum ScoringBonus
    {
        DoubleLetter, TrippleLetter, DoubleWord, TrippleWord
    }
}
