
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;

using MyScrabble.Controller;
using MyScrabble.Controller.Tiles;
using MyScrabble.Model;


namespace MyScrabble.View
{
    
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

            tilesRackUC.PopulateTilesRackUC();
            UpdateTilesBagListBox();

            Game.Start();

            AIDictionary aiDictionary = new AIDictionary();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            boardUC.MakeAMove();
            tilesRackUC.RefillTilesFromTilesBag();

            UpdateTilesBagListBox();
        }

        private void ExchangeTilesButton_Click(object sender, RoutedEventArgs e)
        {
            tilesRackUC.PopulateTilesRackUC();
            tilesRackUC.GetTilesFromTilesRackToTilesBag();
            UpdateTilesBagListBox();
        }

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
