
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Shapes;
//using MyScrabble.Controller;
//using MyScrabble.Controller.Tiles;


//namespace MyScrabble.View
//{
//    static class BoardView
//    {
        
//        //private static Grid _boardGrid;
//        private static Grid _boardLeftSideMarksGrid;
//        private static Grid _boardRightSideMarksGrid;
//        private static Grid _boardTopSideMarksGrid;
//        private static Grid _boardBottomSideMarksGrid;

//        public static void Initialize(Grid boardLeftSideMarksGrid,
//            Grid boardRightSideMarksGrid, Grid boardTopSideMarksGrid,
//            Grid boardBottomSideMarksGrid)
//        {
//            //_boardGrid = boardGrid;
//            _boardLeftSideMarksGrid = boardLeftSideMarksGrid;
//            _boardRightSideMarksGrid = boardRightSideMarksGrid;
//            _boardTopSideMarksGrid = boardTopSideMarksGrid;
//            _boardBottomSideMarksGrid = boardBottomSideMarksGrid;
//        }


//        public static void InitializeBoardSideMarkers()
//        {
//            for (int row = 0; row < Board.boardSize; row++)
//            {
//                _boardLeftSideMarksGrid.RowDefinitions.Add(new RowDefinition());
//                Label boardSideMarkLabel = CreateBoardSideLabel(row, BoardSide.Left);
//                _boardLeftSideMarksGrid.Children.Add(boardSideMarkLabel);


//                _boardRightSideMarksGrid.RowDefinitions.Add(new RowDefinition());
//                boardSideMarkLabel = CreateBoardSideLabel(row, BoardSide.Right);
//                _boardRightSideMarksGrid.Children.Add(boardSideMarkLabel);
//            }

//            for (int column = 0; column < Board.boardSize; column++)
//            {
//                _boardTopSideMarksGrid.ColumnDefinitions.Add(new ColumnDefinition());
//                Label boardSideMarkLabel = CreateBoardSideLabel(column, BoardSide.Top);
//                _boardTopSideMarksGrid.Children.Add(boardSideMarkLabel);

//                _boardBottomSideMarksGrid.ColumnDefinitions.Add(new ColumnDefinition());
//                boardSideMarkLabel = CreateBoardSideLabel(column, BoardSide.Bottom);
//                _boardBottomSideMarksGrid.Children.Add(boardSideMarkLabel);
//            }
//        }

//        private static Label CreateBoardSideLabel(int rowColumn, BoardSide boardSide)
//        {
//            Label labelToAdd = new Label();
//            labelToAdd.FontSize = 15;
//            labelToAdd.Foreground = new SolidColorBrush(Colors.White);

//            if (boardSide == BoardSide.Left)
//            {
//                SetVerticalBoardSideLabel(labelToAdd, rowColumn);

//                labelToAdd.HorizontalAlignment = HorizontalAlignment.Right;
//            }
//            else if (boardSide == BoardSide.Right)
//            {
//                SetVerticalBoardSideLabel(labelToAdd, rowColumn);

//                labelToAdd.HorizontalAlignment = HorizontalAlignment.Left;
//            }
//            else if (boardSide == BoardSide.Top)
//            {
//                SetHorizontalBoardSideLabel(labelToAdd, rowColumn);

//                labelToAdd.VerticalAlignment = VerticalAlignment.Bottom;
//            }
//            else if (boardSide == BoardSide.Bottom)
//            {
//                SetHorizontalBoardSideLabel(labelToAdd, rowColumn);

//                labelToAdd.VerticalAlignment = VerticalAlignment.Top;
//            }


//            return labelToAdd;
//        }

//        private static void SetHorizontalBoardSideLabel(Label labelToAdd, int column)
//        {
//            labelToAdd.Content = (char)('A' + column);
//            Grid.SetColumn(labelToAdd, column);
//        }

//        private static void SetVerticalBoardSideLabel(Label labelToAdd, int row)
//        {
//            labelToAdd.Content = (row + 1).ToString();
//            Grid.SetRow(labelToAdd, row);
//        }

//    }

    
//}
