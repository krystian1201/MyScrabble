using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyScrabble.Model;
using MyScrabble.Constants;

namespace MyScrabble.Controller
{
    public static class TilesPositionsHelper
    {
        //it is responsibility of user to initialize BoardArray
        public static Tile[,] BoardArray;

        public static bool IsTileTotallySurrounded(Tile tile)
        {
            int x = (int)tile.PositionOnBoard.Value.X;
            int y = (int)tile.PositionOnBoard.Value.Y;


            if (BoardArray[x - 1, y] != null && BoardArray[x + 1, y] != null
                && BoardArray[x, y - 1] != null && BoardArray[x, y + 1] != null)
            {
                return true;
            }

            return false;
        }

        public static bool IsThereTileAdjacentToTheLeft(int xIndex, int yIndex)
        {
            if (xIndex >= 1 && BoardArray[xIndex - 1, yIndex] != null)
            {
                return true;
            }

            return false;
        }

        public static bool IsThereTileAdjacentToTheRight(int xIndex, int yIndex)
        {
            if (xIndex <= BoardConstants.BOARD_SIZE - 2 && BoardArray[xIndex + 1, yIndex] != null)
            {
                return true;
            }

            return false;
        }

        public static bool IsThereTileAdjacentAbove(int xIndex, int yIndex)
        {
            if (yIndex >= 1 && BoardArray[xIndex, yIndex - 1] != null)
            {
                return true;
            }

            return false;
        }

        public static bool IsThereTileAdjacentBelow(int xIndex, int yIndex)
        {
            if (yIndex <= BoardConstants.BOARD_SIZE - 2 && BoardArray[xIndex, yIndex + 1] != null)
            {
                return true;
            }

            return false;
        }

        public static bool AreTilesInSameRowOrColumn(List<Tile> tilesInMove)
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
        public static bool AreTilesNextToEachOther(List<Tile> tilesInMove)
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

        public static bool AreTilesNextToEachOtherInColumn(List<Tile> tilesInMove, int column)
        {
            bool result = false;

            int indexOfTopMostTile = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.Y);

            //starting from the bottom-most tile, the top-most index till gap (if any)
            int topMostIndexContinuous = MoveWordsHelper.GetWordTopMostIndex(column, tilesInMove);


            int indexOfBottomMostTile = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.Y);
            int bottomMostIndexContinuous = MoveWordsHelper.GetWordBottomMostIndex(column, tilesInMove);


            if (topMostIndexContinuous <= indexOfTopMostTile &&
                bottomMostIndexContinuous >= indexOfBottomMostTile)
            {
                result = true;
            }


            return result;
        }

        public static bool AreTilesNextToEachOtherInRow(List<Tile> tilesInMove, int row)
        {
            bool result = false;

            int indexOfLeftMostTile = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.X);

            //starting from the right-most tile, the left-most index till gap (if any)
            int leftMostIndexContinuous = MoveWordsHelper.GetWordLeftMostIndex(row, tilesInMove);


            int indexOfRightMostTile = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.X);
            int rightMostIndexContinuous = MoveWordsHelper.GetWordRightMostIndex(row, tilesInMove);


            if (leftMostIndexContinuous <= indexOfLeftMostTile &&
                rightMostIndexContinuous >= indexOfRightMostTile)
            {
                result = true;
            }

            return result;
        }

        public static void GetTilesCommonRowOrColumnOrBoth(List<Tile> tilesInMove, ref int? commonColumn, ref int? commonRow)
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
                if ((column >= 1 && BoardArray[column, row - 1] != null) ||
                (column <= BoardConstants.BOARD_SIZE - 2 && BoardArray[column, row + 1] != null))
                {
                    commonColumn = column;
                }

                //if there is a tile placed to the left or to the right, then
                //the single tile placed in the current move have a common row with them
                if ((row >= 1 && BoardArray[column - 1, row] != null) ||
                (row <= BoardConstants.BOARD_SIZE - 2 && BoardArray[column + 1, row] != null))
                {
                    commonRow = row;
                }
            }
        }

        public static bool IsThePlaceOnBoardFree(int xPosition, int yPosition)
        {
            if (BoardArray[xPosition, yPosition] == null)
            {
                return true;
            }

            return false;
        }
    }
}
