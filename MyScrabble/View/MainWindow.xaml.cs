
using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;

using MyScrabble.Controller;
using MyScrabble.Controller.Tiles;
using MyScrabble.View;


namespace MyScrabble
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TilesBag tilesBag;
        private readonly Player player;
        private readonly TilesRackUC tilesRackUC;
        private readonly BoardUC boardUC;
        private readonly Board board;

        public MainWindow()
        {
            InitializeComponent();

            //view stuff

            boardUC = new BoardUC();

            tilesRackUC = new TilesRackUC();

            InitializeComboBoxes();


            //control stuff

            board = new Board(boardUC);
            board.Initialize();

            tilesBag = new TilesBag();
            tilesBag.PopulateWithTiles();

            player = new Player();    

        }

        private void InitializeComboBoxes()
        {
            ComboBoxLetters.Items.Add('B');
            ComboBoxLetters.Items.Add('A');
            ComboBoxLetters.Items.Add('C');


            for (int i = 1; i <= Board.boardSize; i++)
            {
                ComboBoxX.Items.Add((char)('A'+i-1));
                ComboBoxY.Items.Add(i);
            }
        }

        private void UpdatePositionComboBoxes()
        {

            //TODO: the logic here is not exactly right

            //ComboBoxX.Items.Clear();

            //for (int column = 0; column < Board.ValidXPositionsForTile.Count; column++)
            //{
            //    char charToAdd = (char) ('A' + Board.ValidXPositionsForTile[column]);
            //    ComboBoxX.Items.Add(charToAdd);
            //}


            //ComboBoxY.Items.Clear();

            //for (int row = 0; row < Board.ValidYPositionsForTile.Count; row++)
            //{
            //    ComboBoxY.Items.Add(Board.ValidYPositionsForTile[row] + 1);
            //}
        }

        private void ButtonPlaceATile_Click(object sender, RoutedEventArgs e)
        {

            List<string> validationMessages = ValidateUsersTileMove();

            if (validationMessages.Count > 0)
            {
                 //I don't exactly get this construct but it looks cool :-)
                string finalValidationMessage = 
                    validationMessages.Aggregate("", (current, validationMessage) => current + (validationMessage + "\n"));

                MessageBox.Show(finalValidationMessage);
            }
            else
            {
                Tile tileToPlaceOnBoard = GetTileToPlaceOnBoard(ComboBoxLetters.SelectedItem);
                int xPositionOnBoard = ComboBoxX.SelectedIndex;
                int yPositionOnBoard = ComboBoxY.SelectedIndex;


                board.PlaceATile(tileToPlaceOnBoard, xPositionOnBoard, yPositionOnBoard);
                UpdatePositionComboBoxes();
            }
   
        }

        private List<string> ValidateUsersTileMove()
        {

            List<string> validationMessages = new List<string>();

            try
            {
                GetTileToPlaceOnBoard(ComboBoxLetters.SelectedItem);
            }
            catch (Exception exception)
            {
                validationMessages.Add(exception.Message);
            }


            int xPositionOnBoard = ComboBoxX.SelectedIndex;

            if (xPositionOnBoard == -1)
            {
                validationMessages.Add("X position for the tile was not selected");
            }


            int yPositionOnBoard = ComboBoxY.SelectedIndex;

            if (yPositionOnBoard == -1)
            {
                validationMessages.Add("Y position for the tile was not selected");
            }

            return validationMessages;
        }

        public static Tile GetTileToPlaceOnBoard(object rawSelectedLetter)
        {
            if (rawSelectedLetter == null)
            {
                throw new Exception("Tile to be placed on the board was not selected");
            }

            char tileLetter = (char)rawSelectedLetter;

            switch (tileLetter)
            {
                case 'A':
                    return new TileA();
                case 'B':
                    return new TileB();
                case 'C':
                    return new TileC();
                default:
                    throw new Exception("Tile to be placed on the board is incorrect");

                //TODO: include all letters
            }
        }

        private void TileAImage_MouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            base.OnMouseMove(mouseEventArgs);

            if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                // Package the data.
                DataObject data = new DataObject();
                data.SetData(DataFormats.StringFormat, "A");

                //data.SetData("Double", circleUI.Height);
                //data.SetData("Object", this);

                // Inititate the drag-and-drop operation.
                DragDrop.DoDragDrop(this, data, DragDropEffects.Move);
            }
        }

        private void BoardGrid_Drop(object sender, DragEventArgs dragEventArgs)
        {
            base.OnDrop(dragEventArgs);

            // If the DataObject contains string data, extract it. 
            if (dragEventArgs.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string letterString = (string)dragEventArgs.Data.GetData(DataFormats.StringFormat);

                UIElement uiElement = (UIElement)dragEventArgs.Source;
                int column = Grid.GetColumn(uiElement);
                int row = Grid.GetRow(uiElement); 

                textBox1.Text = column + ", " + row;

            }

            dragEventArgs.Handled = true;
        }

    }
}
