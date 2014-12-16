
using MyScrabble.Controller.Tiles;
using MyScrabble.View;


namespace MyScrabble.Controller
{

    public class Board
    {
        public const int BOARD_SIZE = 15;

        private readonly Tile[,] _boardArray = new Tile[BOARD_SIZE, BOARD_SIZE];


        public Board()
        {
        }


        public void PlaceATile(Tile tileToPlaceOnBoard, int xPosition, int yPosition)
        {
            //TODO: check if tile can be placed in a given position

            //we assume that it is check in BoardUC whether or not a tile
            //can be placed in a given cell
            //if (canTileBePlacedHere(xPosition, yPosition))
            //{
                _boardArray[xPosition, yPosition] = tileToPlaceOnBoard;
            //}

        }

        
        public bool canTileBePlacedHere(int xPosition, int yPosition)
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
