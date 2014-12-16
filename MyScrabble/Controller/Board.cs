
using MyScrabble.Controller.Tiles;
using MyScrabble.View;


namespace MyScrabble.Controller
{

    class Board
    {
        public const int BOARD_SIZE = 15;

        private readonly BoardUC _boardUC;

        private readonly Tile[,] _boardArray = new Tile[BOARD_SIZE, BOARD_SIZE];


        public Board(BoardUC boardUC )
        {
            this._boardUC = boardUC;
        }


        public  void PlaceATile(Tile tileToPlaceOnBoard, int xPosition, int yPosition)
        {
            //TODO: check if tile can be placed in a given position

            if (canTileBePlacedHere(xPosition, yPosition))
            {
                _boardArray[xPosition, yPosition] = tileToPlaceOnBoard;
            }

        }

        
        private  bool canTileBePlacedHere(int xPosition, int yPosition)
        {

            //TODO for now we just check if a tile has already been placed at
            //a given spot
            //later also more complex conditions will be checked
            if (_boardArray[yPosition, xPosition] == null)
            {
                return true;
            }

            return false;
        }

        

    }
}
