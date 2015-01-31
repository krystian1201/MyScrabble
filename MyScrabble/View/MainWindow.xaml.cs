
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;

using MyScrabble.Controller;
using MyScrabble.Model;
using MyScrabble.Model.Tiles;


namespace MyScrabble.View
{
    
    public partial class MainWindow : Window
    {
        readonly AIPlayerRandom _aiPlayerRandom;

        public MainWindow()
        {
            InitializeComponent();

            Player1TilesRackUC.PopulateTilesRackUC();
            Player2TilesRackUC.PopulateTilesRackUC();

            UpdateTilesBagListBox();

            Game.Start();


            _aiPlayerRandom = new AIPlayerRandom();
           
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            boardUC.MakeAMoveHuman();

            Player1TilesRackUC.RefillTilesFromTilesBag();
            
            UpdateTilesBagListBox();
        }

        private void ExchangeTilesButton_Click(object sender, RoutedEventArgs e)
        {
            Player1TilesRackUC.PopulateTilesRackUC();
            Player1TilesRackUC.GetTilesFromTilesRackToTilesBag();
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

            List<Tile> tilesInBag = Player1TilesRackUC.GetAllTilesFromTilesBag();

            foreach (Tile tile in tilesInBag)
            {
                TilesBagListBox.Items.Add(tile.Letter);
            }
        }

        private void AIPlayerMakeMoveButton_Click(object sender, RoutedEventArgs e)
        {

            List<Tile> tilesInMove = 
                _aiPlayerRandom.GenerateMove(Player2TilesRackUC.TilesRack, boardUC.Board);

            if (tilesInMove == null || tilesInMove.Count == 0)
            {
                throw new Exception("No tiles in move");
            }

            boardUC.Board.MakeAMoveAI(tilesInMove);

            Player2TilesRackUC.TilesRack.RemoveTiles(tilesInMove);
            Player2TilesRackUC.RefillTilesFromTilesBag();

            UpdateTilesBagListBox();
        }
    }
}
