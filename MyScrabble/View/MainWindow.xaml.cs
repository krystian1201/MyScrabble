
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
        private readonly AIPlayerRandom _aiRandomPlayer;
        private readonly Player _humanPlayer;

        public MainWindow()
        {
            InitializeComponent();

            Player1TilesRackUC.PopulateTilesRackUC();
            Player2TilesRackUC.PopulateTilesRackUC();

            UpdateTilesBagListBox();

            Game.Start();


            _aiRandomPlayer = new AIPlayerRandom();
            _humanPlayer = new Player();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            List<Tile> tilesInMove = boardUC.MakeAMoveHuman();

            int moveScore = boardUC.Board.GetScoreOfMove(tilesInMove);
            _humanPlayer.UpdateTotalScoreWithLastMoveScore(moveScore);
            UpdatePlayer1ScoreLabels(moveScore, _humanPlayer.TotalScore);

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
        

        private void AIPlayerMakeMoveButton_Click(object sender, RoutedEventArgs e)
        {

            List<Tile> tilesInMove = 
                _aiRandomPlayer.GenerateMove(Player2TilesRackUC.TilesRack, boardUC.Board);

            if (tilesInMove == null || tilesInMove.Count == 0)
            {
                throw new Exception("No tiles in move");
            }

            int moveScore = boardUC.Board.GetScoreOfMove(tilesInMove);
            _aiRandomPlayer.UpdateTotalScoreWithLastMoveScore(moveScore);
            UpdatePlayer2ScoreLabels(moveScore, _aiRandomPlayer.TotalScore);

            boardUC.Board.MakeAMoveAI(tilesInMove);

            Player2TilesRackUC.TilesRack.RemoveTiles(tilesInMove);
            Player2TilesRackUC.RefillTilesFromTilesBag();

            UpdateTilesBagListBox();
        }

        private void UpdatePlayer1ScoreLabels(int lastMoveScore, int totalScore)
        {
            Player1LastMoveScoreLabel.Content = "Last Move Score " + lastMoveScore;
            Player1TotalScoreLabel.Content = "Total Score: " + totalScore;
        }

        private void UpdatePlayer2ScoreLabels(int lastMoveScore, int totalScore)
        {
            Player2LastMoveScoreLabel.Content = "Last Move Score " + lastMoveScore;
            Player2TotalScoreLabel.Content = "Total Score: " + totalScore;
        }

        private void UpdateTilesBagListBox()
        {
            TilesBagListBox.Items.Clear();

            List<Tile> tilesInBag = Player1TilesRackUC.GetAllTilesFromTilesBag();

            foreach (Tile tile in tilesInBag)
            {
                TilesBagListBox.Items.Add(tile.Letter);
            }
        }
    }
}
