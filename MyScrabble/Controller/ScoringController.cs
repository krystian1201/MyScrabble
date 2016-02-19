using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyScrabble.Model;
using System.Windows;

namespace MyScrabble.Controller
{
    public class ScoringController
    {
        private static Dictionary<Point, ScoringBonus> _bonusScoringMatrixField;
        
        private static Dictionary<Point, ScoringBonus> _bonusScoringMatrix
        {
            get
            {
                if (_bonusScoringMatrixField == null)
                {
                    _bonusScoringMatrixField = BonusScoringMatrixFactory.Create();
                }

                return _bonusScoringMatrixField;
            }
        }

        public static int GetScoreOfMove(List<Tile> tilesInMove)
        {
            if (tilesInMove == null)
            {
                return 0;
            }

            List<List<Tile>> wordsFromMove = MoveWordsHelper.GetAllWordsFromMove(tilesInMove);
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

        private static int GetScoreOfWord(List<Tile> tilesInWord, List<Tile> tilesInMove)
        {
            int score = 0;
            int wordBonusFactor = 1;

            foreach (Tile tileInWord in tilesInWord)
            {
                int letterBonusFactor = 1;

                bool isTileFromCurrentMove = MoveWordsHelper.IsTileFromCurrentMove(tileInWord, tilesInMove);

                Point tileInWordPosition = (Point)tileInWord.PositionOnBoard;

                if (isTileFromCurrentMove && _bonusScoringMatrix.ContainsKey(tileInWordPosition))
                {
                    if (_bonusScoringMatrix[tileInWordPosition] == ScoringBonus.TrippleWord)
                    {
                        wordBonusFactor *= 3;
                    }
                    else if (_bonusScoringMatrix[tileInWordPosition] == ScoringBonus.DoubleWord)
                    {
                        wordBonusFactor *= 2;
                    }
                    else if (_bonusScoringMatrix[tileInWordPosition] == ScoringBonus.TrippleLetter)
                    {
                        letterBonusFactor = 3;
                    }
                    else if (_bonusScoringMatrix[tileInWordPosition] == ScoringBonus.DoubleLetter)
                    {
                        letterBonusFactor = 2;
                    }
                }

                score += tileInWord.Points * letterBonusFactor;
            }

            score *= wordBonusFactor;
            return score;
        }
    }
}
