
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MyScrabble.Model;
using MyScrabble.View;
using MyScrabble.Constants;


namespace MyScrabble.Controller
{

    public class BoardController
    {
        private readonly BoardUC _boardUC;

        //TODO: to model?
        private Tile[,] _boardArray;

        public Tile[,] BoardArray
        {
            get
            {
                if (_boardArray == null)
                {
                    _boardArray = new Tile[BoardConstants.BOARD_SIZE, BoardConstants.BOARD_SIZE];
                }

                return _boardArray;
            }
        }

        //private Dictionary<Point, ScoringBonus> _bonusScoringMatrix;

        public BoardController(BoardUC _boardUC)
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

        #region makeAMove

        //todo: to players?
        public List<Tile> MakeAMoveHuman()
        {
            List<Tile> tilesInMove = _boardArray.Cast<Tile>().
                 Where(tile => tile != null && tile.WasMoveMade == false).
                 ToList();


            MarkTilesAfterMoveWasMade(tilesInMove);

            if (GameController.IsFirstMove)
            {
                GameController.SetAfterFirstMove();
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

            if (GameController.IsFirstMove)
            {
                GameController.SetAfterFirstMove();
            }
        }

        private void MarkTilesAfterMoveWasMade(List<Tile> tilesInMove)
        {
            foreach (Tile tile in tilesInMove)
            {
                tile.WasMoveMade = true;
            }
        }

        #endregion makeAMove

        public List<Tile> GetTilesAlreadyOnBoard()
        {
            List<Tile> tilesOnBoard = _boardArray.Cast<Tile>().
                 Where(tile => tile != null && tile.WasMoveMade == true).
                 ToList();

            return tilesOnBoard;
        }

        public List<Tile> GetTilesOnBoardFromAnchor(Tile anchorTile, WordOrientation wordOrientation)
        {
            List<Tile> tilesOnBoardFromAnchor = null;

            if (wordOrientation == WordOrientation.Horizontal)
            {
                tilesOnBoardFromAnchor = MoveWordsHelper.GetTilesOfWordInRow(new List<Tile>() { anchorTile }, (int)anchorTile.PositionOnBoard.Value.Y);
            }
            else if (wordOrientation == WordOrientation.Vertical)
            {
                tilesOnBoardFromAnchor = MoveWordsHelper.GetTilesOfWordInColumn(new List<Tile>() { anchorTile }, (int)anchorTile.PositionOnBoard.Value.X);
            }

            if (tilesOnBoardFromAnchor == null || tilesOnBoardFromAnchor.Count == 0)
            {
                throw new Exception("The anchor position seems to be invalid");
            }

            return tilesOnBoardFromAnchor;
        }

        public List<Tile> GetTilesOnBoardFromCurrentMove()
        {
            List<Tile> tilesFromCurrentMoveOnBoard =
                _boardArray.Cast<Tile>().Where(tile => tile != null && tile.WasMoveMade == false).ToList();

            return tilesFromCurrentMoveOnBoard;
        }
    }

    public enum ScoringBonus
    {
        DoubleLetter, TrippleLetter, DoubleWord, TrippleWord
    }
}
