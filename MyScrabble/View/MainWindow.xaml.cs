
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

            player1 = new Player();

            tilesRackUC.PopulateTilesRackUC();

            UpdateTilesBagListBox();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            boardUC.MakeAMove();
            UpdateTilesBagListBox();
        }

        //Echange tiles functionality was suspended for the moment
        //private void ExchangeTilesButton_Click(object sender, RoutedEventArgs e)
        //{
        //    tilesRackUC.PopulateTilesRackUC();
        //    tilesRackUC.GetTilesFromTilesRackToTilesBag();
        //    UpdateTilesBagListBox();
        //}

        private void ResetTilesButton_Click(object sender, RoutedEventArgs e)
        {
            boardUC.GetLastTilesFromBoardToTilesRack();
        }

        //just for testing/debugging
        private void UpdateTilesBagListBox()
        {
            TilesBagListBox.Items.Clear();

            List<Tile> tilesInBag = tilesRackUC.GetAllTilesFromTilesBag();

            foreach (Tile tile in tilesInBag)
            {
                TilesBagListBox.Items.Add(tile.Letter);
            }
        }
    }
}
