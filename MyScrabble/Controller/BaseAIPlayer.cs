
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MyScrabble.Model;

namespace MyScrabble.Controller
{
    public abstract class BaseAIPlayer : Player
    {
        protected AIDictionary _aiDictionary;
        

        protected BaseAIPlayer()
        {
            _aiDictionary = new AIDictionary();

        }

        public List<Tile> GenerateMove(TilesRack tilesRack, Board board)
        {
            List<Tile> tilesInMove = null;

            if (Game.IsFirstMove)
            {
                tilesInMove = GenerateFirstMove(tilesRack, board);
            }
            else if (!Game.IsFirstMove)
            {
                tilesInMove = GenerateSecondAndAboveMove(tilesRack, board);
            }

            if (tilesInMove == null || tilesInMove.Count == 0)
            {
                throw new Exception("No tiles in move");
            }

            return tilesInMove;
        }

        protected abstract List<Tile> GenerateFirstMove(TilesRack tilesRack, Board board);

        protected abstract List<Tile> GenerateSecondAndAboveMove(TilesRack tilesRack, Board board);


        protected WordOrientation GetRandomWordOrientation()
        {
            Random random = new Random();

            //I don't know why but when I call random.Next(0, 2), I always get 0
            int randomValue = random.Next(0, 10);

            if (randomValue >= 0 && randomValue <= 4)
            {
                return WordOrientation.Horizontal;
            }
            if (randomValue >= 5 && randomValue <= 9)
            {
                return WordOrientation.Vertical;
            }

            throw new Exception("Word orientation should be either horizontal or vertical");
        }

        protected void AssignPositionsOnBoardToTilesInMove(string word, List<Tile> tilesInMove,
           Point startTilePosition, WordOrientation wordOrientation,
           string substring, int? substringIndex, Board board)
        {

            if (!word.Contains(substring))
            {
                throw new ArgumentException("Word to be formed does not contain the substring (from tiles already on board)");
            }

            //TODO: there can be more than one substring
            //TODO: -> more than one substringIndex

            //it is used so that we don't choose the same tile in tiles rack more than once
            List<Tile> tempTilesInMove = new List<Tile>(tilesInMove);

            int letterIndex = 0;

            //checking both of the conditions is probably unnecessary
            //however, I added it for extra safety
            while (tempTilesInMove.Count > 0 && letterIndex < word.Length)
            {
                if (letterIndex == substringIndex)
                {
                    letterIndex += substring.Length;
                }

                //actually, if we have two or more tiles with the same letter in tiles rack, 
                //they can be used in any place on board that requires a given letter
                Tile tileInMove =
                    tempTilesInMove.FirstOrDefault(tile => tile.Letter == word[letterIndex]);


                if (wordOrientation == WordOrientation.Horizontal)
                {
                    //tileInMove.PositionOnBoard = new Point(startTilePosition.X + letterIndex, startTilePosition.Y);
                    board.PlaceATileOnBoard(tileInMove, (int)(startTilePosition.X + letterIndex), (int)startTilePosition.Y);
                }
                else if (wordOrientation == WordOrientation.Vertical)
                {
                    //tileInMove.PositionOnBoard = new Point(startTilePosition.X, startTilePosition.Y + letterIndex);
                    board.PlaceATileOnBoard(tileInMove, (int)startTilePosition.X, (int)(startTilePosition.Y + letterIndex));
                }


                tempTilesInMove.Remove(tileInMove);

                letterIndex++;
            }
        }

        protected string BuildStringFromTiles(List<Tile> tiles)
        {
            if (tiles == null || tiles.Count == 0)
            {
                throw new ArgumentOutOfRangeException("tiles", "String cannot be built when there are no tiles");
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (Tile tile in tiles)
            {
                stringBuilder.Append(tile.Letter);
            }

            return stringBuilder.ToString();
        }
    }
}
