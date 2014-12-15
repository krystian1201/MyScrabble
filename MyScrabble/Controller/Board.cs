
using System.Collections.Generic;

using MyScrabble.Controller.Tiles;
using MyScrabble.View;


namespace MyScrabble.Controller
{

    class Board
    {
        public const int boardSize = 15;

        private BoardUC boardUC;

        private  readonly Tile[,] boardArray = new Tile[boardSize, boardSize];

        private  readonly List<int> validXPositionsForTile = new List<int>();
        public  List<int> ValidXPositionsForTile 
        {
            get { return validXPositionsForTile; }
        }

        private  readonly List<int> validYPositionsForTile = new List<int>();
        public  List<int> ValidYPositionsForTile
        {
            get { return validYPositionsForTile; }
        }


        public Board(BoardUC boardUC )
        {
            this.boardUC = boardUC;
        }

        public  void Initialize()
        {
            for (int column = 0; column < boardSize; column++)
            {
                validXPositionsForTile.Add(column);
            }

            for (int row = 0; row < boardSize; row++)
            {
                validYPositionsForTile.Add(row);
            }
        }

        public  void PlaceATile(Tile tileToPlaceOnBoard, int xPosition, int yPosition)
        {
            

            //TODO: check if tile can be placed in a given position

            MarkPositionAsNotValidForTilePlacement(xPosition, yPosition);

            boardArray[xPosition, yPosition] = tileToPlaceOnBoard;

            //player.PlaceATile(tileToPlaceOnBoard, xPositionOnBoard, yPositionOnBoard);

            
            boardUC.PlaceATile(tileToPlaceOnBoard, xPosition, yPosition);
   
        }

        private  void MarkPositionAsNotValidForTilePlacement(int xPosition, int yPosition)
        {
            validXPositionsForTile.Remove(xPosition);
            validYPositionsForTile.Remove(yPosition);

           
        }

        /*private  void checkIfTileCanBePlacedHere(int xPosition, int yPosition)
        {
            
        }*/

    }
}
