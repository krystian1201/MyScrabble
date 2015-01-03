
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
    
    public partial class MainWindow : Window
    {
        private TilesBag tilesBag;
        private readonly Board board;
        //private readonly Player player;
        private readonly TilesRackUC tilesRackUC;
        private readonly BoardUC boardUC;
        

        public MainWindow()
        {
            InitializeComponent();

            //control stuff

            //board = new Board();

            tilesBag = new TilesBag();
            tilesBag.PopulateWithTiles();

            //view stuff

            boardUC = new BoardUC();
            

            tilesRackUC = new TilesRackUC();

        }

    }
}
