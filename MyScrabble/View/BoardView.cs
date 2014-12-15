﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MyScrabble.Controller;
using MyScrabble.Controller.Tiles;


namespace MyScrabble.View
{
    static class BoardView
    {
        
        //private static Grid _boardGrid;
        private static Grid _boardLeftSideMarksGrid;
        private static Grid _boardRightSideMarksGrid;
        private static Grid _boardTopSideMarksGrid;
        private static Grid _boardBottomSideMarksGrid;

        public static void Initialize(Grid boardLeftSideMarksGrid,
            Grid boardRightSideMarksGrid, Grid boardTopSideMarksGrid,
            Grid boardBottomSideMarksGrid)
        {
            //_boardGrid = boardGrid;
            _boardLeftSideMarksGrid = boardLeftSideMarksGrid;
            _boardRightSideMarksGrid = boardRightSideMarksGrid;
            _boardTopSideMarksGrid = boardTopSideMarksGrid;
            _boardBottomSideMarksGrid = boardBottomSideMarksGrid;
        }

        //public static void DefineGridRowsAndColumns()
        //{
        //    for (int i = 0; i < Board.boardSize; i++)
        //    {
        //        _boardGrid.ColumnDefinitions.Add(new ColumnDefinition());
        //        _boardGrid.RowDefinitions.Add(new RowDefinition());
        //    }

        //}

        //public static void AddCells()
        //{
        //    SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Gray);
        //    Thickness marginThickness = new Thickness(1.0);


        //    for (int row = 0; row < Board.boardSize; row++)
        //    {
        //        for (int column = 0; column < Board.boardSize; column++)
        //        {
        //            Rectangle rectangleToAdd =
        //                CreateRectangleToAddToGrid(row, column, solidColorBrush, marginThickness);

        //            _boardGrid.Children.Add(rectangleToAdd);
        //        }
        //    }
        //}


        //private static Rectangle CreateRectangleToAddToGrid(int row, int column,
        //    SolidColorBrush solidColorBrush, Thickness marginThickness)
        //{
        //    Rectangle rectangleToAdd = new Rectangle();

        //    rectangleToAdd.Stroke = solidColorBrush;
        //    rectangleToAdd.StrokeThickness = 0.5;
        //    rectangleToAdd.Margin = marginThickness;

        //    Grid.SetRow(rectangleToAdd, row);
        //    Grid.SetColumn(rectangleToAdd, column);

        //    return rectangleToAdd;
        //}


        //public static void ColorBonusCells()
        //{
        //    ColorRedBonusCells();
        //    ColorHazyRedBonusCells();
        //    ColorBlueBonusCells();
        //    ColorHazyBlueBonusCells();
        //}


        //private static void ColorRedBonusCells()
        //{
        //    List<Rectangle> redCells = new List<Rectangle>();

        //    for (int row = 0; row < Board.boardSize; row += Board.boardSize / 2)
        //    {
        //        for (int column = 0; column < Board.boardSize; column += Board.boardSize / 2)
        //        {
        //            redCells.Add(GetCellByRowAndColumn(row, column));
        //        }
        //    }

        //    redCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Colors.Red));
        //}


        //private static void ColorHazyRedBonusCells()
        //{
        //    List<Rectangle> redHazyCells = new List<Rectangle>();

        //    for (int rowColumn = 1; rowColumn < Board.boardSize / 2 - 2; rowColumn++)
        //    {
        //        redHazyCells.Add(GetCellByRowAndColumn(rowColumn, rowColumn));
        //        redHazyCells.Add(GetCellByRowAndColumn(rowColumn, Board.boardSize - rowColumn - 1));
        //        redHazyCells.Add(GetCellByRowAndColumn(Board.boardSize - rowColumn - 1, rowColumn));
        //        redHazyCells.Add(GetCellByRowAndColumn(Board.boardSize - rowColumn - 1, Board.boardSize - rowColumn - 1));
        //    }

        //    redHazyCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Color.FromArgb(100, 255, 0, 0)));
        //}


        //private static void ColorBlueBonusCells()
        //{
        //    List<Rectangle> blueCells = new List<Rectangle>();

        //    for (int row = 1; row < Board.boardSize; row += Board.boardSize / 4 + 1)
        //    {
        //        blueCells.Add(GetCellByRowAndColumn(row, 5));
        //        blueCells.Add(GetCellByRowAndColumn(row, Board.boardSize - 1 - 5));
        //    }

        //    blueCells.Add(GetCellByRowAndColumn(5, 1));
        //    blueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - 5, 1));

        //    blueCells.Add(GetCellByRowAndColumn(5, Board.boardSize - 1 - 1));
        //    blueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - 5, Board.boardSize - 1 - 1));


        //    blueCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Colors.Blue));
        //}


        //private static void ColorHazyBlueBonusCells()
        //{
        //    List<Rectangle> hazyBlueCells = new List<Rectangle>();

        //    for (int column = 0; column < Board.boardSize; column += Board.boardSize / 2)
        //    {
        //        hazyBlueCells.Add(GetCellByRowAndColumn(3, column));
        //        hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - 3, column));
        //    }

        //    for (int row = 0; row < Board.boardSize; row += Board.boardSize / 2)
        //    {
        //        hazyBlueCells.Add(GetCellByRowAndColumn(row, 3));
        //        hazyBlueCells.Add(GetCellByRowAndColumn(row, Board.boardSize - 1 - 3));
        //    }

        //    for (int column = 2; column < Board.boardSize / 2; column += 4)
        //    {
        //        hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize / 2 - 1, column));
        //        hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize / 2 + 1, column));

        //        hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize / 2 - 1, Board.boardSize - 1 - column));
        //        hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize / 2 + 1, Board.boardSize - 1 - column));
        //    }

        //    for (int row = 2; row < Board.boardSize / 2; row += 4)
        //    {
        //        hazyBlueCells.Add(GetCellByRowAndColumn(row, Board.boardSize / 2 - 1));
        //        hazyBlueCells.Add(GetCellByRowAndColumn(row, Board.boardSize / 2 + 1));

        //        hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - row, Board.boardSize / 2 - 1));
        //        hazyBlueCells.Add(GetCellByRowAndColumn(Board.boardSize - 1 - row, Board.boardSize / 2 + 1));
        //    }

        //    hazyBlueCells.ForEach(rectangle => rectangle.Fill = new SolidColorBrush(Color.FromArgb(100, 0, 0, 255)));
        //}


        //private static Rectangle GetCellByRowAndColumn(int row, int column)
        //{
        //    const string rowOutOfRangeExceptionMsg =
        //        "row cannot be less than 0 or greater than board size";

        //    const string columnOutOfRangeExceptionMsg =
        //        "column cannot be less than 0 or greater than board size";

        //    if (row >= Board.boardSize || row < 0)
        //    {
        //        throw new
        //            ArgumentOutOfRangeException("row", row, rowOutOfRangeExceptionMsg);
        //    }

        //    if (column >= Board.boardSize || column < 0)
        //    {
        //        throw new
        //            ArgumentOutOfRangeException("column", column, columnOutOfRangeExceptionMsg);
        //    }

        //    return _boardGrid.Children
        //        .Cast<Rectangle>()
        //        .Where(rectangle => Grid.GetRow(rectangle) == row).
        //        Where(rectangle => Grid.GetColumn(rectangle) == column).
        //        FirstOrDefault();

        //}


        public static void InitializeBoardSideMarkers()
        {
            for (int row = 0; row < Board.boardSize; row++)
            {
                _boardLeftSideMarksGrid.RowDefinitions.Add(new RowDefinition());
                Label boardSideMarkLabel = CreateBoardSideLabel(row, BoardSide.Left);
                _boardLeftSideMarksGrid.Children.Add(boardSideMarkLabel);


                _boardRightSideMarksGrid.RowDefinitions.Add(new RowDefinition());
                boardSideMarkLabel = CreateBoardSideLabel(row, BoardSide.Right);
                _boardRightSideMarksGrid.Children.Add(boardSideMarkLabel);
            }

            for (int column = 0; column < Board.boardSize; column++)
            {
                _boardTopSideMarksGrid.ColumnDefinitions.Add(new ColumnDefinition());
                Label boardSideMarkLabel = CreateBoardSideLabel(column, BoardSide.Top);
                _boardTopSideMarksGrid.Children.Add(boardSideMarkLabel);

                _boardBottomSideMarksGrid.ColumnDefinitions.Add(new ColumnDefinition());
                boardSideMarkLabel = CreateBoardSideLabel(column, BoardSide.Bottom);
                _boardBottomSideMarksGrid.Children.Add(boardSideMarkLabel);
            }
        }

        private static Label CreateBoardSideLabel(int rowColumn, BoardSide boardSide)
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

        private static void SetHorizontalBoardSideLabel(Label labelToAdd, int column)
        {
            labelToAdd.Content = (char)('A' + column);
            Grid.SetColumn(labelToAdd, column);
        }

        private static void SetVerticalBoardSideLabel(Label labelToAdd, int row)
        {
            labelToAdd.Content = (row + 1).ToString();
            Grid.SetRow(labelToAdd, row);
        }

        //public static void PlaceATile(Tile tileToPlaceOnBoard, int xPosition, int yPosition)
        //{
        //    //TODO: a given tile/letter image should be placed on board in a given position

        //    Grid.SetRow(tileToPlaceOnBoard.TileImage, yPosition);
        //    Grid.SetColumn(tileToPlaceOnBoard.TileImage, xPosition);

        //    _boardGrid.Children.Add(tileToPlaceOnBoard.TileImage);
            
        //}
    }

    enum BoardSide
    {
        Left, Right, Top, Bottom
    }
}
