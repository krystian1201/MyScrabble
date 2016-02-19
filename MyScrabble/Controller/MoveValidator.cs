using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyScrabble.Model;
using System.Windows;

namespace MyScrabble.Controller
{
    public static class MoveValidator
    {
        public static List<string> ValidateMove(List<Tile> tilesInMove)
        {
            List<string> validationMessages = new List<string>();

            ScrabbleDictionary scrabbleDictionary = new ScrabbleDictionary();

            if (IsMoveEmpty(tilesInMove))
            {
                validationMessages.Add("You didn't place any tiles on board");
            }
            else if (tilesInMove.Count >= 2 && !TilesPositionsHelper.AreTilesInSameRowOrColumn(tilesInMove))
            {
                validationMessages.Add("The tiles are not in one row or column");
            }
            else if (tilesInMove.Count >= 2 && !TilesPositionsHelper.AreTilesNextToEachOther(tilesInMove))
            {
                validationMessages.Add("The tiles are not next to each other");
            }
            else if (GameController.IsFirstMove && !WordGoesThroughTheCenterOfBoard(tilesInMove))
            {
                validationMessages.Add("The first word in the game should go through the center of board");
            }
            else if (GameController.IsFirstMove && tilesInMove.Count == 1)
            {
                validationMessages.Add("Single-letter words are not allowed in Scrabble");
            }
            else if (!GameController.IsFirstMove && !FormWordsInCrosswordWay(tilesInMove))
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

        private static bool IsMoveEmpty(List<Tile> tilesInMove)
        {
            if (tilesInMove != null)
            {
                if (tilesInMove.Count > 0)
                {
                    return false;
                }

                return true;
            }

            return true;
        }

        private static bool FormsWordsFromDictionary(List<Tile> tilesInMove, ScrabbleDictionary scrabbleDictionary)
        {
            //here we assume the tiles in move are next to each other in one row or column

            bool isWordInDictionaryInColumn = false;
            bool isWordInDictionaryInRow = false;


            int? commonColumn = null;
            int? commonRow = null;

            TilesPositionsHelper.GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);

            if (commonColumn != null)
            {
                string wordInColumn = MoveWordsHelper.GetWordInColumn((int)commonColumn, tilesInMove);
                isWordInDictionaryInColumn = scrabbleDictionary.IsWordInDictionary(wordInColumn);
            }
            if (commonRow != null)
            {
                string wordInRow = MoveWordsHelper.GetWordInRow((int)commonRow, tilesInMove);
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

        private static bool WordGoesThroughTheCenterOfBoard(List<Tile> tilesInMove)
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

        private static bool FormWordsInCrosswordWay(List<Tile> tilesInMove)
        {

            int? commonColumn = null;
            int? commonRow = null;

            TilesPositionsHelper.GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);

            bool result = false;

            //that should include only single-letter words?
            if (commonRow != null && commonColumn != null)
            {
                result = MoveWordsHelper.CheckAdjacentAndCrossingTilesForHorizontalWord(tilesInMove, (int)commonRow) &&
                    MoveWordsHelper.CheckAdjacentAndCrossingTilesForVerticalWord(tilesInMove, (int)commonColumn);
            }
            else
            {
                if (commonRow != null)
                {
                    result = MoveWordsHelper.CheckAdjacentAndCrossingTilesForHorizontalWord(tilesInMove, (int)commonRow);
                }
                //I'm not sure whether row and column should be exclusive here
                //Should the word be either vertical or horizontal?
                //What about single-letter words?
                else if (commonColumn != null)
                {
                    result = MoveWordsHelper.CheckAdjacentAndCrossingTilesForVerticalWord(tilesInMove, (int)commonColumn);
                }
            }


            return result;
        }

        public static bool FormsNoInvalidWordsInAnyDirection(List<Tile> tilesInMove, ScrabbleDictionary scrabbleDictionary)
        {
            int? commonColumn = null;
            int? commonRow = null;

            TilesPositionsHelper.GetTilesCommonRowOrColumnOrBoth(tilesInMove, ref commonColumn, ref commonRow);


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

        private static bool CheckInvalidWordsForHorizontalTiles(List<Tile> tilesInMove, int commonRow, ScrabbleDictionary scrabbleDictionary)
        {

            string horizontalWord = MoveWordsHelper.GetWordInRow(commonRow, tilesInMove);

            if (!scrabbleDictionary.WordList.Contains(horizontalWord))
            {
                return false;
            }


            foreach (Tile tileInMove in tilesInMove)
            {
                int xIndex = (int)tileInMove.PositionOnBoard.Value.X;

                if (TilesPositionsHelper.IsThereTileAdjacentAbove(xIndex, commonRow) ||
                    TilesPositionsHelper.IsThereTileAdjacentBelow(xIndex, commonRow))
                {

                    string verticalWord = MoveWordsHelper.GetWordInColumn(xIndex, tilesInMove);

                    if (!scrabbleDictionary.WordList.Contains(verticalWord))
                    {
                        return false;
                    }
                }
            }


            return true;
        }

        private static bool CheckInvalidWordsForVerticalTiles(List<Tile> tilesInMove, int commonColumn, ScrabbleDictionary scrabbleDictionary)
        {

            string verticalWord = MoveWordsHelper.GetWordInColumn(commonColumn, tilesInMove);

            if (!scrabbleDictionary.WordList.Contains(verticalWord))
            {
                return false;
            }

            foreach (Tile tileInMove in tilesInMove)
            {
                int yIndex = (int)tileInMove.PositionOnBoard.Value.Y;

                if (TilesPositionsHelper.IsThereTileAdjacentToTheLeft((int)commonColumn, yIndex) ||
                    TilesPositionsHelper.IsThereTileAdjacentToTheRight((int)commonColumn, yIndex))
                {
                    string horizontalWord = MoveWordsHelper.GetWordInRow(yIndex, tilesInMove);

                    if (!scrabbleDictionary.WordList.Contains(horizontalWord))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
