
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

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
            
        }

        public List<string> ValidateMove()
        {
            List<string> validationMessages = new List<string>();

            if (!AreTilesInLine())
            {
                validationMessages.Add("The tiles are not in one line");
            }

            return validationMessages;
        }


        private bool AreTilesInLine()
        {
            List<Tile> tilesInMove = _boardArray.Cast<Tile>().
                 Where(tile => tile != null && tile.WasMoveMade == false).
                 ToList();

            bool areInLine = false;

            if (tilesInMove.Count > 0)
            {
                areInLine = AreTilesInSameRowOrColumn(tilesInMove);
                
            }
            else
            {
                throw new Exception("No tile was placed on board in the move");
            }

             return areInLine;
        }

        private bool AreTilesInSameRowOrColumn(List<Tile> tilesInMove)
        {
            //here we assume that tilesInMove contains at least one element
            int xPosition = (int)tilesInMove[0].PositionOnBoard.Value.X;
            int yPosition = (int)tilesInMove[0].PositionOnBoard.Value.Y;

            bool areInSameRow = true;
            bool areInSameColumn = true;

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

            return areInSameRow || areInSameColumn;
        }

        private bool AreTilesNextToEachOther(List<Tile> tilesInMove)
        {
            return false;
        }

        public bool CanTileBePlacedHere(int xPosition, int yPosition)
        {

            //TODO for now we just check if a tile has already been placed at
            //a given spot
            //later also more complex conditions will be checked
            //or not - this applies to just a single tile - not whole words
            if (_boardArray[xPosition, yPosition] == null)
            {
                return true;
            }

            return false;
        }

    }
}
