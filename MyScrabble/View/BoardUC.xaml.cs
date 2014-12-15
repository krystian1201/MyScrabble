
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;

using MyScrabble.Controller;
using MyScrabble.Controller.Tiles;



namespace MyScrabble.View
{

    public partial class BoardUC : UserControl
    {
        
        public BoardUC()
        {
            InitializeComponent();

            DefineGridRowsAndColumns();
            AddCells();
            ColorBonusCells();

            InitializeBoardSideMarkers();
        }

        private void DefineGridRowsAndColumns()
        {
            for (int i = 0; i < Board.boardSize; i++)
            {
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                BoardGrid.RowDefinitions.Add(new RowDefinition());
            }

            //TODO: not sure if this is necessary
            BoardGrid.Children.Clear();
        }

        private void AddCells()
        {
            SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Gray);
            Thickness marginThickness = new Thickness(1.0);


            for (int row = 0; row < Board.boardSize; row++)
            {
                for (int column = 0; column < Board.boardSize; column++)
                {
                    Rectangle rectangleToAdd =
                        CreateRectangleToAddToGrid(row, column, solidColorBrush, marginThickness);

                    BoardGrid.Children.Add(rectangleToAdd);
                }
            }
        }


        private Rectangle CreateRectangleToAddToGrid(int row, int column,
            SolidColorBrush solidColorBrush, Thickness marginThickness)
        {
            Rectangle rectangleToAdd = new Rectangle();

            rectangleToAdd.Stroke = solidColorBrush;
            rectangleToAdd.StrokeThickness = 0.5;
            rectangleToAdd.Margin = marginThickness;

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

            for (int row = 0; row < Board.boardSize; row += Board.boardSize / 2)
            {
                for (int column = 0; column < Board.boardSize; column += Board.boardSize / 2)
                {
                    redCells.Add(GetCellByRowAndColumn(row, column));
                }
            }

            redCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Colors.Red));
        }


        private void ColorHazyRedBonusCells()
        {
            List<Rectangle> redHazyCells = new List<Rectangle>();

            for (int rowColumn = 1; rowColumn < Board.boardSize / 2 - 2; rowColumn++)
            {
                redHazyCells.Add(GetCellByRowAndColumn(rowColumn, rowColumn));
                redHazyCells.Add(GetCellByRowAndColumn(rowColumn, Board.boardSize - rowColumn - 1));
                redHazyCells.Add(GetCellByRowAndColumn(Board.boardSize - rowColumn - 1, rowColumn));
                redHazyCells.Add(GetCellByRowAndColumn(Board.boardSize - rowColumn - 1, Board.boardSize - rowColumn - 1));
            }

            redHazyCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)));
        }


        private void ColorBlueBonusCells()
        {
            List<Rectangle> blueCells = new List<Rectangle>();

            for (int row = 1; row < Board.boardSize; row += Board.boardSize / 4 + 1)
            {
                blueCells.Add(GetCellByRowAndColumn(row, 5));
                blueCells.Add(GetCellByRowAndColumn(row, Board.boardSize - 1 - 5));
            }

            blueCells.Add(GetCellByRowAndColumn(5, 1));
            blueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - 5, 1));

            blueCells.Add(GetCellByRowAndColumn(5, Board.boardSize - 1 - 1));
            blueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - 5, Board.boardSize - 1 - 1));


            blueCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Colors.Blue));
        }


        private void ColorHazyBlueBonusCells()
        {
            List<Rectangle> hazyBlueCells = new List<Rectangle>();

            for (int column = 0; column < Board.boardSize; column += Board.boardSize / 2)
            {
                hazyBlueCells.Add(GetCellByRowAndColumn(3, column));
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - 3, column));
            }

            for (int row = 0; row < Board.boardSize; row += Board.boardSize / 2)
            {
                hazyBlueCells.Add(GetCellByRowAndColumn(row, 3));
                hazyBlueCells.Add(GetCellByRowAndColumn(row, Board.boardSize - 1 - 3));
            }

            for (int column = 2; column < Board.boardSize / 2; column += 4)
            {
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize / 2 - 1, column));
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize / 2 + 1, column));

                hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize / 2 - 1, Board.boardSize - 1 - column));
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize / 2 + 1, Board.boardSize - 1 - column));
            }

            for (int row = 2; row < Board.boardSize / 2; row += 4)
            {
                hazyBlueCells.Add(GetCellByRowAndColumn(row, Board.boardSize / 2 - 1));
                hazyBlueCells.Add(GetCellByRowAndColumn(row, Board.boardSize / 2 + 1));

                hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - row, Board.boardSize / 2 - 1));
                hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - row, Board.boardSize / 2 + 1));
            }

            hazyBlueCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255)));
        }


        private Rectangle GetCellByRowAndColumn(int row, int column)
        {
            const string rowOutOfRangeExceptionMsg =
                "row cannot be less than 0 or greater than board size";

            const string columnOutOfRangeExceptionMsg =
                "column cannot be less than 0 or greater than board size";

            if (row >= Board.boardSize || row < 0)
            {
                throw new
                    ArgumentOutOfRangeException("row", row, rowOutOfRangeExceptionMsg);
            }

            if (column >= Board.boardSize || column < 0)
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

        public void PlaceATile(Tile tileToPlaceOnBoard, int xPosition, int yPosition)
        {
            //TODO: a given tile/letter image should be placed on board in a given position

            Grid.SetRow(tileToPlaceOnBoard.TileImage, yPosition);
            Grid.SetColumn(tileToPlaceOnBoard.TileImage, xPosition);

            BoardGrid.Children.Add(tileToPlaceOnBoard.TileImage);
        }


        public void InitializeBoardSideMarkers()
        {

            for (int row = 0; row < Board.boardSize; row++)
            {
                BoardLeftSideMarksGrid.RowDefinitions.Add(new RowDefinition());
                Label boardSideMarkLabel = CreateBoardSideLabel(row, BoardSide.Left);
                BoardLeftSideMarksGrid.Children.Add(boardSideMarkLabel);


                BoardRightSideMarksGrid.RowDefinitions.Add(new RowDefinition());
                boardSideMarkLabel = CreateBoardSideLabel(row, BoardSide.Right);
                BoardRightSideMarksGrid.Children.Add(boardSideMarkLabel);
            }

            for (int column = 0; column < Board.boardSize; column++)
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
