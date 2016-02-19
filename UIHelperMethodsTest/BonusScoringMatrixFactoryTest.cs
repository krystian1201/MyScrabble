using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyScrabble.Controller;
using MyScrabble.Constants;

namespace MyScrabbleTest
{
    [TestClass]
    public class BonusScoringMatrixFactoryTest
    {
        [TestMethod]
        public void BonusScoringMatrix_Should_have_correct_tripple_word_bonus_places()
        {
            var matrix = BonusScoringMatrixFactory.Create();

            Assert.IsTrue(matrix[new System.Windows.Point(0, 0)] == ScoringBonus.TrippleWord);
            Assert.IsTrue(matrix[new System.Windows.Point(0, BoardConstants.BOARD_SIZE - 1)] == ScoringBonus.TrippleWord);
            Assert.IsTrue(matrix[new System.Windows.Point(BoardConstants.BOARD_SIZE - 1, 0)] == ScoringBonus.TrippleWord);
            Assert.IsTrue(matrix[new System.Windows.Point(BoardConstants.BOARD_SIZE - 1, BoardConstants.BOARD_SIZE - 1)] 
                == ScoringBonus.TrippleWord);
        }

        [TestMethod]
        public void BonusScoringMatrix_Should_have_correct_double_word_bonus_places()
        {
            var matrix = BonusScoringMatrixFactory.Create();

            Assert.IsTrue(matrix[new System.Windows.Point(1, 1)] == ScoringBonus.DoubleWord);
            Assert.IsFalse(matrix[new System.Windows.Point(6, 6)] == ScoringBonus.DoubleWord);

            Assert.IsTrue(matrix[new System.Windows.Point(BoardConstants.BOARD_SIZE - 2, BoardConstants.BOARD_SIZE - 2)] 
                == ScoringBonus.DoubleWord);
        }

        [TestMethod]
        public void BonusScoringMatrix_Should_have_correct_tripple_letter_bonus_places()
        {
            var matrix = BonusScoringMatrixFactory.Create();

            Assert.IsTrue(matrix[new System.Windows.Point(5, 1)] == ScoringBonus.TrippleLetter);
            
            Assert.IsTrue(matrix[new System.Windows.Point(13, 5)] == ScoringBonus.TrippleLetter);
        }

        [TestMethod]
        public void BonusScoringMatrix_Should_have_correct_double_letter_bonus_places()
        {
            var matrix = BonusScoringMatrixFactory.Create();

            Assert.IsTrue(matrix[new System.Windows.Point(3, 0)] == ScoringBonus.DoubleLetter);

            Assert.IsTrue(matrix[new System.Windows.Point(BoardConstants.BOARD_SIZE - 4, BoardConstants.BOARD_SIZE - 1)] == ScoringBonus.DoubleLetter);
        }
    }
}
