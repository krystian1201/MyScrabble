
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using MyScrabble.Controller;


namespace MyScrabble.View
{

    public partial class BoardUC : UserControl
    {
        private Board _board;

        //otherwise the project doesn't compile
        //"no default constructor for BoardUC"
        public BoardUC()
        {
            InitializeComponent();

            DefineGridRowsAndColumns();
            AddCells();
            ColorBonusCells();

            InitializeBoardSideMarkers();                                                                                   

            BoardGrid.Drop += BoardGrid_Drop;

            _board = new Board();
        }


        private void DefineGridRowsAndColumns()
        {
            for (int i = 0; i < Board.BOARD_SIZE; i++)
            {
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                BoardGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void AddCells()
        {
            SolidColorBrush strokeColor = new SolidColorBrush(Colors.Gray);
            SolidColorBrush fillColor = new SolidColorBrush(Colors.White);
            Thickness marginThickness = new Thickness(1.0);


            for (int row = 0; row < Board.BOARD_SIZE; row++)
            {
                for (int column = 0; column < Board.BOARD_SIZE; column++)
                {
                    Rectangle rectangleToAdd =
                        CreateRectangleToAddToGrid(row, column, strokeColor, fillColor, marginThickness);

                    BoardGrid.Children.Add(rectangleToAdd);
                }
            }
        }


        private Rectangle CreateRectangleToAddToGrid(int row, int column,
            SolidColorBrush strokeColor, SolidColorBrush fillColor, Thickness marginThickness)
        {
            Rectangle rectangleToAdd = new Rectangle();

            rectangleToAdd.Stroke = strokeColor;
            rectangleToAdd.StrokeThickness = 0.5;
            rectangleToAdd.Margin = marginThickness;
            rectangleToAdd.Fill = fillColor;

            Grid.SetRow(rectangleToAdd, row);
            Grid.SetColumn(rectangleToAdd, column);

            return rectangleToAdd;
        }


        private void ColorBonusCells()
        {
            ColorRedBonusCells();
            ColorHazyRedBonusCells();
            ColorBlueBonusCells();
            ColorHazyBlueBonusCells();
        }


        private void ColorRedBonusCells()
        {
            List<Rectangle> redCells = new List<Rectangle>();

            for (int row = 0; row < Board.BOARD_SIZE; row += Board.BOARD_SIZE / 2)
            {
                for (int column = 0; column < Board.BOARD_SIZE; column += Board.BOARD_SIZE / 2)
                {
                    redCells.Add(GetCellByRowAndColumn(row, column));
                }
            }

            redCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Colors.Red));
        }


        private void ColorHazyRedBonusCells()
        {
            List<Rectangle> redHazyCells = new List<Rectangle>();

            for (int rowColumn = 1; rowColumn < Board.BOARD_SIZE / 2 - 2; rowColumn++)
            {
                redHazyCells.Add(GetCellByRowAndColumn(rowColumn, rowColumn));
                redHazyCells.Add(GetCellByRowAndColumn(rowColumn, Board.BOARD_SIZE - rowColumn - 1));
                redHazyCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE - rowColumn - 1, rowColumn));
                redHazyCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE - rowColumn - 1, Board.BOARD_SIZE - rowColumn - 1));
            }

            redHazyCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)));
        }


        private void ColorBlueBonusCells()
        {
            List<Rectangle> blueCells = new List<Rectangle>();

            for (int row = 1; row < Board.BOARD_SIZE; row += Board.BOARD_SIZE / 4 + 1)
            {
                blueCells.Add(GetCellByRowAndColumn(row, 5));
                blueCells.Add(GetCellByRowAndColumn(row, Board.BOARD_SIZE - 1 - 5));
            }

            blueCells.Add(GetCellByRowAndColumn(5, 1));
            blueCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE - 1 - 5, 1));

            blueCells.Add(GetCellByRowAndColumn(5, Board.BOARD_SIZE - 1 - 1));
            blueCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE - 1 - 5, Board.BOARD_SIZE - 1 - 1));


            blueCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Colors.Blue));
        }


        private void ColorHazyBlueBonusCells()
        {
            List<Rectangle> hazyBlueCells = new List<Rectangle>();

            for (int column = 0; column < Board.BOARD_SIZE; column += Board.BOARD_SIZE / 2)
            {
                hazyBlueCells.Add(GetCellByRowAndColumn(3, column));
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE - 1 - 3, column));
            }

            for (int row = 0; row < Board.BOARD_SIZE; row += Board.BOARD_SIZE / 2)
            {
                hazyBlueCells.Add(GetCellByRowAndColumn(row, 3));
                hazyBlueCells.Add(GetCellByRowAndColumn(row, Board.BOARD_SIZE - 1 - 3));
            }

            for (int column = 2; column < Board.BOARD_SIZE / 2; column += 4)
            {
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE / 2 - 1, column));
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE / 2 + 1, column));

                hazyBlueCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE / 2 - 1, Board.BOARD_SIZE - 1 - column));
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE / 2 + 1, Board.BOARD_SIZE - 1 - column));
            }

            for (int row = 2; row < Board.BOARD_SIZE / 2; row += 4)
            {
                hazyBlueCells.Add(GetCellByRowAndColumn(row, Board.BOARD_SIZE / 2 - 1));
                hazyBlueCells.Add(GetCellByRowAndColumn(row, Board.BOARD_SIZE / 2 + 1));

                hazyBlueCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE - 1 - row, Board.BOARD_SIZE / 2 - 1));
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.BOARD_SIZE - 1 - row, Board.BOARD_SIZE / 2 + 1));
            }

            hazyBlueCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255)));
        }


        private Rectangle GetCellByRowAndColumn(int row, int column)
        {
            const string rowOutOfRangeExceptionMsg =
                "row cannot be less than 0 or greater than board size";

            const string columnOutOfRangeExceptionMsg =
                "column cannot be less than 0 or greater than board size";

            if (row >= Board.BOARD_SIZE || row < 0)
            {
                throw new
                    ArgumentOutOfRangeException("row", row, rowOutOfRangeExceptionMsg);
            }

            if (column >= Board.BOARD_SIZE || column < 0)
            {
                throw new
                    ArgumentOutOfRangeException("column", column, columnOutOfRangeExceptionMsg);
            }

            return BoardGrid.Children
                .Cast<Rectangle>()
                .Where(rectangle => Grid.GetRow(rectangle) == row).
                Where(rectangle => Grid.GetColumn(rectangle) == column).
                FirstOrDefault();

        }


        private void BoardGrid_Drop(object sender, DragEventArgs dragEventArgs)
        {
            base.OnDrop(dragEventArgs);

            // If the DataObject contains TileUC object, extract it. 
            if (dragEventArgs.Data.GetDataPresent("TileUC"))
            {
               
                //cannot cast to Rectangle or any other specific control
                //because we don't know where a tile will be dropped
                UIElement uiElement = (UIElement)dragEventArgs.Source;
                int column = Grid.GetColumn(uiElement);
                int row = Grid.GetRow(uiElement);


                if (_board.CanTileBePlacedHere(column, row))
                {
                    TileUC tileUC = (TileUC)dragEventArgs.Data.GetData("TileUC");

                    char letter = tileUC.Tile.Letter;

                    TestLabel.Content = letter + " " + column + "," + row;

                    RemoveTileFromItsSource(tileUC);

                    PlaceATileOnBoard(tileUC, column, row);
                }
            }

            dragEventArgs.Handled = true;
        }

        private void RemoveTileFromItsSource(TileUC tileUC)
        {
            //if a tile was dragged from tiles rack
            if (tileUC.Tile.XPositionOnBoard == null &&
                tileUC.Tile.YPositionOnBoard == null)
            {
                RemoveTileFromTilesRack(tileUC);
            }
            //if a tile was dragged from one place on the board to another
            else if (tileUC.Tile.XPositionOnBoard != null &&
                tileUC.Tile.YPositionOnBoard != null)
            {
                RemoveTileFromBoard(tileUC);
            }
        }


        private void PlaceATileOnBoard(TileUC tileUC, int xPosition, int yPosition)
        {
            //UI side
            Grid.SetRow(tileUC, yPosition);
            Grid.SetColumn(tileUC, xPosition);

            BoardGrid.Children.Add(tileUC);

            //Controller-logic side
            _board.PlaceATile(tileUC.Tile, xPosition, yPosition);
        }

        private void RemoveTileFromBoard(TileUC tileUC)
        {
            //UI side
            BoardGrid.Children.Remove(tileUC); 

            //Controller-logic side
            _board.RemoveATile(tileUC.Tile);
        }

        private void RemoveTileFromTilesRack(TileUC tileUC)
        {
            //UI side
            Grid tilesRackGrid = (Grid)tileUC.Parent;
            tilesRackGrid.Children.Remove(tileUC);

            //Controller-logic side
            TilesRackUC tilesRackUC = (TilesRackUC) tilesRackGrid.Parent;
            tilesRackUC.TilesRack.RemoveTileFromTilesArray(tileUC.Tile);
        }


        public void GetLastTilesFromBoardToTilesRack()
        {
            List<TileUC> lastTilesOnBoard =
                BoardGrid.Children.OfType<TileUC>().Where(TileUC => TileUC.Tile.MoveMade == false).ToList<TileUC>();

            Grid MainGrid = (Grid)this.Parent;

            TilesRackUC tilesRackUC = MainGrid.Children.OfType<TilesRackUC>().FirstOrDefault();

            foreach (TileUC tileUC in lastTilesOnBoard)
            {
                RemoveTileFromBoard(tileUC);
                tilesRackUC.PlaceATileInTilesRack(tileUC, tileUC.Tile.PositionInTilesRack);  
            }

        }

        public void InitializeBoardSideMarkers()
        {

            for (int row = 0; row < Board.BOARD_SIZE; row++)
            {
                BoardLeftSideMarksGrid.RowDefinitions.Add(new RowDefinition());
                Label boardSideMarkLabel = CreateBoardSideLabel(row, BoardSide.Left);
                BoardLeftSideMarksGrid.Children.Add(boardSideMarkLabel);


                BoardRightSideMarksGrid.RowDefinitions.Add(new RowDefinition());
                boardSideMarkLabel = CreateBoardSideLabel(row, BoardSide.Right);
                BoardRightSideMarksGrid.Children.Add(boardSideMarkLabel);
            }

            for (int column = 0; column < Board.BOARD_SIZE; column++)
            {
                BoardTopSideMarksGrid.ColumnDefinitions.Add(new ColumnDefinition());
                Label boardSideMarkLabel = CreateBoardSideLabel(column, BoardSide.Top);
                BoardTopSideMarksGrid.Children.Add(boardSideMarkLabel);

                BoardBottomSideMarksGrid.ColumnDefinitions.Add(new ColumnDefinition());
                boardSideMarkLabel = CreateBoardSideLabel(column, BoardSide.Bottom);
                BoardBottomSideMarksGrid.Children.Add(boardSideMarkLabel);
            }
        }

        private Label CreateBoardSideLabel(int rowColumn, BoardSide boardSide)
        {
            Label labelToAdd = new Label();
            labelToAdd.FontSize = 15;
            labelToAdd.Foreground = new SolidColorBrush(Colors.White);

            if (boardSide == BoardSide.Left)
            {
                SetVerticalBoardSideLabel(labelToAdd, rowColumn);

                labelToAdd.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else if (boardSide == BoardSide.Right)
            {
                SetVerticalBoardSideLabel(labelToAdd, rowColumn);

                labelToAdd.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (boardSide == BoardSide.Top)
            {
                SetHorizontalBoardSideLabel(labelToAdd, rowColumn);

                labelToAdd.VerticalAlignment = VerticalAlignment.Bottom;
            }
            else if (boardSide == BoardSide.Bottom)
            {
                SetHorizontalBoardSideLabel(labelToAdd, rowColumn);

                labelToAdd.VerticalAlignment = VerticalAlignment.Top;
            }


            return labelToAdd;
        }

        private void SetHorizontalBoardSideLabel(Label labelToAdd, int column)
        {
            labelToAdd.Content = (char)('A' + column);
            Grid.SetColumn(labelToAdd, column);
        }

        private void SetVerticalBoardSideLabel(Label labelToAdd, int row)
        {
            labelToAdd.Content = (row + 1).ToString();
            Grid.SetRow(labelToAdd, row);
        }

        
    }


    enum BoardSide
    {
        Left, Right, Top, Bottom
    }
}
