﻿
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
            List<Tile> tilesInMove = _boardArray.Cast<Tile>().
                 Where(tile => tile != null && tile.WasMoveMade == false).
                 ToList();


            MarkTilesAfterMoveWasMade(tilesInMove);
        }

        private void MarkTilesAfterMoveWasMade(List<Tile> tilesInMove)
        {
            foreach (Tile tile in tilesInMove)
            {
                tile.WasMoveMade = true;
            }
        }

        public List<string> ValidateMove()
        {
            List<string> validationMessages = new List<string>();

            List<Tile> tilesInMove = _boardArray.Cast<Tile>().
                 Where(tile => tile != null && tile.WasMoveMade == false).
                 ToList();

            if (IsMoveEmpty(tilesInMove))
            {
                validationMessages.Add("You didn't place any tiles on board");
            }
            else if (!AreTilesInSameRowOrColumn(tilesInMove))
            {
                validationMessages.Add("The tiles are not in one row or column");
            }
            else if (!AreTilesNextToEachOther(tilesInMove))
            {
                validationMessages.Add("The tiles are not next to each other");
            }

            return validationMessages;
        }



        private bool AreTilesInSameRowOrColumn(List<Tile> tilesInMove)
        {
            //here we assume that there was at least one tile in a move
            if (tilesInMove == null || tilesInMove.Count == 0)
            {
                throw new 
                    Exception("There should be at least one tile placed in a move to check" +
                                "if tiles are in the same row or column");
            }

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
            //here we assume that there was at least one tile in a move and that
            //tiles are in one row or column
            if (tilesInMove == null || tilesInMove.Count == 0)
            {
                throw new
                    Exception("There should be at least one tile placed in a move to check" +
                                "if tiles are next to each other");
            }

            int? commonColumn = null;
            int? commonRow = null;

            GetTilesCommonRowOrColumn(tilesInMove, ref commonColumn, ref commonRow);

            bool areTilesNextToEachOtherInColumn = false;
            bool areTilesNextToEachOtherInRow = false;

            if (commonColumn != null)
            {
                areTilesNextToEachOtherInColumn = AreTilesNextToEachOtherInColumn(tilesInMove);
            }
            else if (commonRow != null)
            {
                areTilesNextToEachOtherInRow = AreTilesNextToEachOtherInRow(tilesInMove);
            }
            else
            {
                throw new 
                    Exception("Cannot check if tiles are next to each other because" +
                                "they are not in the same row or column");
            }

            return areTilesNextToEachOtherInColumn ^ areTilesNextToEachOtherInRow;
        }

        private void GetTilesCommonRowOrColumn(List<Tile> tilesInMove, ref int? commonColumn, ref int? commonRow)
        {
            if (tilesInMove.Count > 1)
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
                commonColumn = (int)tilesInMove[0].PositionOnBoard.Value.X;
            }
        }

        private bool AreTilesNextToEachOtherInColumn(List<Tile> tilesInMove)
        {
            bool result = true;

            List<Tile> sortedTilesInMove =
                    tilesInMove.OrderBy(tile => tile.PositionOnBoard.Value.Y).ToList();

            for (int i = 0; i < tilesInMove.Count-1; i++)
            {
                if (sortedTilesInMove[i + 1].PositionOnBoard.Value.Y !=
                    sortedTilesInMove[i].PositionOnBoard.Value.Y + 1)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private bool AreTilesNextToEachOtherInRow(List<Tile> tilesInMove)
        {
            bool result = true;

            List<Tile> sortedTilesInMove =
                    tilesInMove.OrderBy(tile => tile.PositionOnBoard.Value.X).ToList();

            for (int i = 0; i < tilesInMove.Count - 1; i++)
            {
                if (sortedTilesInMove[i + 1].PositionOnBoard.Value.X !=
                    sortedTilesInMove[i].PositionOnBoard.Value.X + 1)
                {
                    result = false;
                    break;
                }
            }

            return result;
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

        public bool IsThePlaceOnBoardOccupied(int xPosition, int yPosition)
        {
            if (_boardArray[xPosition, yPosition] == null)
            {
                return true;
            }

            return false;
        }

    }
}
