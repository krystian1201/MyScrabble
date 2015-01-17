
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyScrabble;
using MyScrabble.Controller.Tiles;

namespace MyScrabbleTest
{
    [TestClass]
    public class MainWindowTest
    {
        [TestMethod]
        public void Test_GetTileToPlaceOnBoard_CorrectLetters()
        {
            char rawSelectedLetter = 'A';
            Tile tileToPlaceOnBoard = MainWindow.GetTileToPlaceOnBoard(rawSelectedLetter);
            Assert.AreEqual(tileToPlaceOnBoard.GetType().Name, "TileA");


            rawSelectedLetter = 'B';
            tileToPlaceOnBoard = MainWindow.GetTileToPlaceOnBoard(rawSelectedLetter);
            Assert.AreEqual(tileToPlaceOnBoard.GetType().Name, "TileB");

            rawSelectedLetter = 'C';
            tileToPlaceOnBoard = MainWindow.GetTileToPlaceOnBoard(rawSelectedLetter);
            Assert.AreEqual(tileToPlaceOnBoard.GetType().Name, "TileC");
        }


        [TestMethod]
        public void Test_GetTileToPlaceOnBoard_IncorrectCharacters_ThrowsException()
        {
            char character = '<';

            try
            {
                MainWindow.GetTileToPlaceOnBoard(character);
            }
            catch (Exception exception)
            {
                Assert.AreEqual(exception.Message, "Tile to be placed on the board is incorrect");
                return;
            }
            
            //if the exception was not thrown
            Assert.Fail();
        }

        [TestMethod]
        public void Test_GetTileToPlaceOnBoard_Null_ThrowsException()
        {
            try
            {
                MainWindow.GetTileToPlaceOnBoard(null);
            }
            catch (Exception exception)
            {
                Assert.AreEqual(exception.Message, "Tile to be placed on the board was not selected");
                return;
            }

            //if the exception was not thrown
            Assert.Fail();
        }
    }
}
