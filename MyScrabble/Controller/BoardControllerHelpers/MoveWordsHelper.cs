using MyScrabble.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using MyScrabble.Constants;


namespace MyScrabble.Controller
{
    public static class MoveWordsHelper
    {
        //it is responsibility of user to initialize BoardArray
        public static Tile[,] BoardArray;

        public static List<List<Tile>> GetAllWordsFromMove(List<Tile> tilesInMove)
        {
            int? commonColumn = null;
            int? commonRow = null;
            TilesPositionsHelper.GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);

            //that case includes only single-letter words, right?
            if (commonRow != null && commonColumn != null)
            {
                List<List<Tile>> allWordsFromMove = GetAllWordsFromHorizontalMove(tilesInMove, (int)commonRow);

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

        private static List<List<Tile>> GetAllWordsFromHorizontalMove(List<Tile> tilesInMove, int commonRow)
        {
            List<List<Tile>> wordsFromTiles = new List<List<Tile>>();

            List<Tile> horizontalWord = GetTilesOfWordInRow(tilesInMove, commonRow);
            wordsFromTiles.Add(horizontalWord);


            foreach (Tile tileInMove in tilesInMove)
            {
                int xIndex = (int)tileInMove.PositionOnBoard.Value.X;

                if (TilesPositionsHelper.IsThereTileAdjacentAbove(xIndex, commonRow) ||
                    TilesPositionsHelper.IsThereTileAdjacentBelow(xIndex, commonRow))
                {

                    List<Tile> verticalWord = GetTilesOfWordInColumn(tilesInMove, xIndex);
                    wordsFromTiles.Add(verticalWord);
                }
            }

            return wordsFromTiles;
        }

        private static List<List<Tile>> GetAllWordsFromVerticalMove(List<Tile> tilesInMove, int commonColumn)
        {
            List<List<Tile>> wordsFromTiles = new List<List<Tile>>();

            List<Tile> verticalWord = GetTilesOfWordInColumn(tilesInMove, commonColumn);
            wordsFromTiles.Add(verticalWord);


            foreach (Tile tileInMove in tilesInMove)
            {
                int yIndex = (int)tileInMove.PositionOnBoard.Value.Y;

                if (TilesPositionsHelper.IsThereTileAdjacentToTheLeft(commonColumn, yIndex) ||
                    TilesPositionsHelper.IsThereTileAdjacentToTheRight(commonColumn, yIndex))
                {

                    List<Tile> horizontalWord = GetTilesOfWordInRow(tilesInMove, yIndex);
                    wordsFromTiles.Add(horizontalWord);
                }
            }

            return wordsFromTiles;
        }

        public static bool IsTileFromCurrentMove(Tile tileInWord, List<Tile> tilesInMove)
        {
            Point tileInWordPosition = (Point)tileInWord.PositionOnBoard;
            Tile tileInMove = tilesInMove.FirstOrDefault(t => t.PositionOnBoard == tileInWordPosition);

            if (tileInMove != null)
            {
                return true;
            }

            return false;
        }

        public static WordOrientation GetWordOrientationFromTiles(List<Tile> tilesInMove)
        {
            int? commonColumn = null;
            int? commonRow = null;

            TilesPositionsHelper.GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);

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

        public static Point GetWordStartPositionFromTiles(List<Tile> tilesInMove, WordOrientation wordOrientation)
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

        public static string GetWordInRow(int row, List<Tile> tilesInMove)
        {
            int wordLeftMostIndex = GetWordLeftMostIndex(row, tilesInMove);
            int wordRightMostIndex = GetWordRightMostIndex(row, tilesInMove);


            string word = BuildWordFromHorizontalRange(row, wordLeftMostIndex, wordRightMostIndex);

            return word;
        }

        public static List<Tile> GetTilesOfWordInRow(List<Tile> tilesInMove, int row)
        {
            int wordLeftMostIndex = GetWordLeftMostIndex(row, tilesInMove);
            int wordRightMostIndex = GetWordRightMostIndex(row, tilesInMove);


            List<Tile> tilesOfWordInRow =
                BoardArray.Cast<Tile>().
                 Where(tile => tile != null &&
                     tile.PositionOnBoard.Value.Y == row &&
                     tile.PositionOnBoard.Value.X >= wordLeftMostIndex &&
                     tile.PositionOnBoard.Value.X <= wordRightMostIndex).
                 ToList();

            return tilesOfWordInRow;
        }

        //this method (and the similar ones for right, top and bottom) 
        //consider both tiles in move and tiles on board next to them
        public static int GetWordLeftMostIndex(int row, List<Tile> tilesInMove)
        {
            //a starting index - takes into account the possibility
            //of a "gap" in word's tiles
            int wordLeftMostIndex = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.X);

            //extending range of the word including tiles placed in previous moves
            if (wordLeftMostIndex >= 1)
            {
                while (wordLeftMostIndex > 0 && BoardArray[wordLeftMostIndex - 1, row] != null)
                {
                    wordLeftMostIndex -= 1;
                }
            }
            return wordLeftMostIndex;
        }

        public static int GetWordRightMostIndex(int row, List<Tile> tilesInMove)
        {
            //a starting index - takes into account the possibility
            //of a "gap" in word's tiles
            int wordRightMostIndex = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.X);

            if (wordRightMostIndex <= BoardConstants.BOARD_SIZE - 2)
            {
                while (wordRightMostIndex < BoardConstants.BOARD_SIZE - 1 && BoardArray[wordRightMostIndex + 1, row] != null)
                {
                    wordRightMostIndex += 1;
                }
            }
            return wordRightMostIndex;
        }

        private static string BuildWordFromHorizontalRange(int row, int wordLeftMostIndex, int wordRightMostIndex)
        {
            StringBuilder sbWord = new StringBuilder();

            for (int xIndex = wordLeftMostIndex; xIndex <= wordRightMostIndex; xIndex++)
            {
                char tileLetter = BoardArray[xIndex, row].Letter;
                sbWord.Append(tileLetter);
            }

            return sbWord.ToString();
        }

        public static string GetWordInColumn(int column, List<Tile> tilesInMove)
        {
            int wordTopMostIndex = GetWordTopMostIndex(column, tilesInMove);
            int wordBottomMostIndex = GetWordBottomMostIndex(column, tilesInMove);

            string word = BuildWordFromVerticalRange(column, wordTopMostIndex, wordBottomMostIndex);

            return word;
        }

        public static List<Tile> GetTilesOfWordInColumn(List<Tile> tilesInMove, int column)
        {
            int wordTopMostIndex = GetWordTopMostIndex(column, tilesInMove);
            int wordBottomMostIndex = GetWordBottomMostIndex(column, tilesInMove);


            List<Tile> tilesOfWordInColumn =
                BoardArray.Cast<Tile>().
                 Where(tile => tile != null &&
                     tile.PositionOnBoard.Value.X == column &&
                     tile.PositionOnBoard.Value.Y >= wordTopMostIndex &&
                     tile.PositionOnBoard.Value.Y <= wordBottomMostIndex).
                 ToList();

            return tilesOfWordInColumn;
        }

        public static int GetWordTopMostIndex(int column, List<Tile> tilesInMove)
        {
            //a starting index - takes into account the possibility
            //of a "gap" in word's tiles
            int wordTopMostIndex = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.Y);

            //extending range of the word including tiles placed in previous moves
            if (wordTopMostIndex >= 1)
            {
                while (wordTopMostIndex > 0 && BoardArray[column, wordTopMostIndex - 1] != null)
                {
                    wordTopMostIndex -= 1;
                }
            }

            return wordTopMostIndex;
        }

        public static int GetWordBottomMostIndex(int column, List<Tile> tilesInMove)
        {
            //a starting index - takes into account the possibility
            //of a "gap" in word's tiles
            int wordBottomMostIndex = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.Y);

            //extending range of the word including tiles placed in previous moves
            if (wordBottomMostIndex <= BoardConstants.BOARD_SIZE - 2)
            {
                while (wordBottomMostIndex < BoardConstants.BOARD_SIZE - 1 && BoardArray[column, wordBottomMostIndex + 1] != null)
                {
                    wordBottomMostIndex += 1;
                }
            }

            return wordBottomMostIndex;
        }

        private static string BuildWordFromVerticalRange(int column, int wordTopMostIndex, int wordBottomMostIndex)
        {
            StringBuilder sbWord = new StringBuilder();

            for (int yIndex = wordTopMostIndex; yIndex <= wordBottomMostIndex; yIndex++)
            {
                char tileLetter = BoardArray[column, yIndex].Letter;
                sbWord.Append(tileLetter);
            }

            return sbWord.ToString();
        }

        public static bool CheckAdjacentAndCrossingTilesForHorizontalWord(List<Tile> tilesInMove, int commonRow)
        {

            int wordLeftMostIndex = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.X);
            int wordRightMostIndex = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.X);


            //for the left-most tile in word
            //-> check tiles adjacent to the left

            //for the right-most tile in word
            //-> check tiles adjacent to the right

            if (TilesPositionsHelper.IsThereTileAdjacentToTheLeft(wordLeftMostIndex, (int)commonRow) ||
               TilesPositionsHelper.IsThereTileAdjacentToTheRight(wordRightMostIndex, (int)commonRow))
            {
                return true;
            }


            //for all tiles
            //-> check tiles adjacent above and below
            for (int xIndex = wordLeftMostIndex; xIndex <= wordRightMostIndex; xIndex++)
            {
                if (TilesPositionsHelper.IsThereTileAdjacentAbove(xIndex, (int)commonRow) ||
                    TilesPositionsHelper.IsThereTileAdjacentBelow(xIndex, (int)commonRow))
                {
                    return true;
                }
            }

            return false; ;
        }

        public static bool CheckAdjacentAndCrossingTilesForVerticalWord(List<Tile> tilesInMove, int commonColumn)
        {

            int wordTopMostIndex = (int)tilesInMove.Min(tile => tile.PositionOnBoard.Value.Y);
            int wordBottomMostIndex = (int)tilesInMove.Max(tile => tile.PositionOnBoard.Value.Y);


            //for the top-most tile in word
            //-> check tiles adjacent above

            //for the bottom-most tile in word
            //-> check tiles adjacent below

            if (TilesPositionsHelper.IsThereTileAdjacentAbove((int)commonColumn, wordTopMostIndex) ||
               TilesPositionsHelper.IsThereTileAdjacentBelow((int)commonColumn, wordBottomMostIndex))
            {
                return true;
            }


            //for all tiles
            //-> check tiles adjacent to the left and to the right
            for (int yIndex = wordTopMostIndex; yIndex <= wordBottomMostIndex; yIndex++)
            {
                if (TilesPositionsHelper.IsThereTileAdjacentToTheLeft((int)commonColumn, yIndex) ||
                    TilesPositionsHelper.IsThereTileAdjacentToTheRight((int)commonColumn, yIndex))
                {
                    return true;
                }
            }

            return false; ;
        }

    }
}
