
using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using MyScrabble.Controller;
using MyScrabble.Model;



namespace MyScrabble.View
{
    
    public partial class MainWindow : Window
    {
        private readonly AIPlayerRandom _aiRandomPlayer;
        private readonly AIPlayerBrute _aiBrutePlayer;
        private readonly Player _humanPlayer;
        public static BackgroundWorker backgroundWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            Player1TilesRackUC.PopulateTilesRackUC();
            Player2TilesRackUC.PopulateTilesRackUC();

            UpdateTilesBagListBox();

            Game.Start();


            _aiRandomPlayer = new AIPlayerRandom();
            _aiBrutePlayer = new AIPlayerBrute();
            _humanPlayer = new Player();

            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(bw_DoWork_GenerateMoveAIBrutePlayer);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_AIBrutePlayer);
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


        private void AIRAndomPlayerMakeMoveButton_Click(object sender, RoutedEventArgs e)
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

        private void AIBruteForcePlayerMakeMoveButton_Click(object sender, RoutedEventArgs e)
        {

            List<Object> backGroundWorkerArgs = new List<object>() { Player2TilesRackUC.TilesRack, boardUC.Board };

            if (backgroundWorker.IsBusy != true)
            {
                backgroundWorker.RunWorkerAsync(backGroundWorkerArgs);
            }
            
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

        private void ButtonStopAIBrutePlayerSearch_Click(object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.WorkerSupportsCancellation == true)
            {
                backgroundWorker.CancelAsync();
            }
        }

        private void bw_DoWork_GenerateMoveAIBrutePlayer(object sender, DoWorkEventArgs e)
        {
            List<object> bwArguments = e.Argument as List<object>;
            TilesRack tilesRack = (TilesRack)bwArguments[0];
            Board board = (Board)bwArguments[1];

            List<Tile> tilesInMove =
                _aiBrutePlayer.GenerateMove(tilesRack, board);

            e.Result = tilesInMove;
 
        }

        private void bw_RunWorkerCompleted_AIBrutePlayer(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Tile> tilesInMove = (List<Tile>)e.Result;
            
            
            if (tilesInMove == null || tilesInMove.Count == 0)
            {
                throw new Exception("No tiles in move");
            }

            int moveScore = boardUC.Board.GetScoreOfMove(tilesInMove);
            _aiBrutePlayer.UpdateTotalScoreWithLastMoveScore(moveScore);
            UpdatePlayer2ScoreLabels(moveScore, _aiBrutePlayer.TotalScore);

            boardUC.Board.MakeAMoveAI(tilesInMove);

            Player2TilesRackUC.TilesRack.RemoveTiles(tilesInMove);
            Player2TilesRackUC.RefillTilesFromTilesBag();

            UpdateTilesBagListBox();
        }
    }
}
