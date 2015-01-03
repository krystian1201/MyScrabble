
using System;

using MyScrabble.Controller.Tiles;
using MyScrabble.View;


namespace MyScrabble.Controller
{

    public class Board
    {
        public const int BOARD_SIZE = 15;

        private Tile[,] _boardArray = new Tile[BOARD_SIZE, BOARD_SIZE];


        public Board()
        {
        }


        public void PlaceATile(Tile tileToPlaceOnBoard, int xPosition, int yPosition)
        {

            //we assume that it is check in BoardUC whether or not a tile
            //can be placed in a given cell
            //I don't know where it makes bigger sense
            //if (canTileBePlacedHere(xPosition, yPosition))
            //{
                tileToPlaceOnBoard.XPositionOnBoard = xPosition;
                tileToPlaceOnBoard.YPositionOnBoard = yPosition;
                _boardArray[xPosition, yPosition] = tileToPlaceOnBoard;
            //}

        }

        public void RemoveATile(Tile tileToRemoveFromBoard)
        {
            if (tileToRemoveFromBoard.XPositionOnBoard != null &&
                tileToRemoveFromBoard.YPositionOnBoard != null)
            {
                _boardArray[(int)tileToRemoveFromBoard.XPositionOnBoard, (int)tileToRemoveFromBoard.YPositionOnBoard] = null;

                tileToRemoveFromBoard.XPositionOnBoard = null;
                tileToRemoveFromBoard.YPositionOnBoard = null;
            }
            else
            {
                throw new Exception("The tile to remove doesn't have position on board.");
            }
            
        }
        
        public bool CanTileBePlacedHere(int xPosition, int yPosition)
        {

            //TODO for now we just check if a tile has already been placed at
            //a given spot
            //later also more complex conditions will be checked
            if (_boardArray[xPosition, yPosition] == null)
            {
                return true;
            }

            return false;
        }

    }
}
