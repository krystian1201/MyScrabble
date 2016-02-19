using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MyScrabble.Constants;

namespace MyScrabble.Controller
{
    public static class BonusScoringMatrixFactory
    {
        public static Dictionary<Point, ScoringBonus> Create()
        {
            var bonusScoringMatrix = new Dictionary<Point, ScoringBonus>();

            AddTrippleWordBonusSpotsToBonusMatrix(bonusScoringMatrix);

            AddDoubleWordBonusSpotsToBonusMatrix(bonusScoringMatrix);

            AddTrippleLetterBonusSpotsToBonusMatrix(bonusScoringMatrix);

            AddDoubleLetterBonusSpotsToBonusMatrix(bonusScoringMatrix);

            return bonusScoringMatrix;
        }

        private static void AddTrippleWordBonusSpotsToBonusMatrix(Dictionary<Point, ScoringBonus> bonusScoringMatrix)
        {
            for (int row = 0; row < BoardConstants.BOARD_SIZE; row += BoardConstants.BOARD_SIZE / 2)
            {
                for (int column = 0; column < BoardConstants.BOARD_SIZE; column += BoardConstants.BOARD_SIZE / 2)
                {
                    bonusScoringMatrix.Add(new Point(column, row), ScoringBonus.TrippleWord);
                }
            }

            //the center cell has actually a double-word bonus
            bonusScoringMatrix.Remove(new Point(7, 7));
        }

        private static void AddDoubleWordBonusSpotsToBonusMatrix(Dictionary<Point, ScoringBonus> bonusScoringMatrix)
        {
            for (int rowColumn = 1; rowColumn < BoardConstants.BOARD_SIZE / 2 - 2; rowColumn++)
            {
                bonusScoringMatrix.Add(new Point(rowColumn, rowColumn), ScoringBonus.DoubleWord);
                bonusScoringMatrix.Add(new Point(rowColumn, BoardConstants.BOARD_SIZE - rowColumn - 1), ScoringBonus.DoubleWord);
                bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE - rowColumn - 1, rowColumn), ScoringBonus.DoubleWord);
                bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE - rowColumn - 1, BoardConstants.BOARD_SIZE - rowColumn - 1),
                    ScoringBonus.DoubleWord);
            }

            bonusScoringMatrix.Add(new Point(7, 7), ScoringBonus.DoubleWord);
        }


        private static void AddTrippleLetterBonusSpotsToBonusMatrix(Dictionary<Point, ScoringBonus> bonusScoringMatrix)
        {
            for (int row = 1; row < BoardConstants.BOARD_SIZE; row += BoardConstants.BOARD_SIZE / 4 + 1)
            {
                bonusScoringMatrix.Add(new Point(row, 5), ScoringBonus.TrippleLetter);
                bonusScoringMatrix.Add(new Point(row, BoardConstants.BOARD_SIZE - 1 - 5), ScoringBonus.TrippleLetter);
            }

            bonusScoringMatrix.Add(new Point(5, 1), ScoringBonus.TrippleLetter);
            bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE - 1 - 5, 1), ScoringBonus.TrippleLetter);
            bonusScoringMatrix.Add(new Point(5, BoardConstants.BOARD_SIZE - 1 - 1), ScoringBonus.TrippleLetter);
            bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE - 1 - 5, BoardConstants.BOARD_SIZE - 1 - 1), ScoringBonus.TrippleLetter);
        }


        private static void AddDoubleLetterBonusSpotsToBonusMatrix(Dictionary<Point, ScoringBonus> bonusScoringMatrix)
        {
            for (int column = 0; column < BoardConstants.BOARD_SIZE; column += BoardConstants.BOARD_SIZE / 2)
            {
                bonusScoringMatrix.Add(new Point(column, 3), ScoringBonus.DoubleLetter);
                bonusScoringMatrix.Add(new Point(column, BoardConstants.BOARD_SIZE - 1 - 3), ScoringBonus.DoubleLetter);
            }

            for (int row = 0; row < BoardConstants.BOARD_SIZE; row += BoardConstants.BOARD_SIZE / 2)
            {
                bonusScoringMatrix.Add(new Point(3, row), ScoringBonus.DoubleLetter);
                bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE - 1 - 3, row), ScoringBonus.DoubleLetter);
            }

            for (int column = 2; column < BoardConstants.BOARD_SIZE / 2; column += 4)
            {
                bonusScoringMatrix.Add(new Point(column, BoardConstants.BOARD_SIZE / 2 - 1), ScoringBonus.DoubleLetter);
                bonusScoringMatrix.Add(new Point(column, BoardConstants.BOARD_SIZE / 2 + 1), ScoringBonus.DoubleLetter);

                bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE - 1 - column, BoardConstants.BOARD_SIZE / 2 - 1), ScoringBonus.DoubleLetter);
                bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE - 1 - column, BoardConstants.BOARD_SIZE / 2 + 1), ScoringBonus.DoubleLetter);
            }


            bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE / 2 - 1, 2), ScoringBonus.DoubleLetter);
            bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE / 2 + 1, 2), ScoringBonus.DoubleLetter);

            bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE / 2 - 1, BoardConstants.BOARD_SIZE - 1 - 2), ScoringBonus.DoubleLetter);
            bonusScoringMatrix.Add(new Point(BoardConstants.BOARD_SIZE / 2 + 1, BoardConstants.BOARD_SIZE - 1 - 2), ScoringBonus.DoubleLetter);

        }
    }
}
