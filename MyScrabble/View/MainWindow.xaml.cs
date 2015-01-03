
using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;

using MyScrabble.Controller;
using MyScrabble.Controller.Tiles;


namespace MyScrabble.View
{
    
    public partial class MainWindow : Window
    {
        private TilesBag tilesBag;
        private readonly Player player1;

        public MainWindow()
        {
            InitializeComponent();


            //tiles bag is not visible so it
            //doesn't have a UI representation
            tilesBag = new TilesBag();
            tilesBag.PopulateWithTiles();

            player1 = new Player();

            tilesRackUC.PopulateTilesRackUC();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            player1.MakeAMove();
        }

        private void ExchangeTilesButton_Click(object sender, RoutedEventArgs e)
        {
            tilesRackUC.PopulateTilesRackUC();
        }

        private void ResetTilesButton_Click(object sender, RoutedEventArgs e)
        {
            boardUC.GetLastTilesFromBoardToTilesRack();
        }

    }
}
