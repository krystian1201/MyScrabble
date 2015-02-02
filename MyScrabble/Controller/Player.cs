


namespace MyScrabble.Controller
{
    public class Player
    {
        public int TotalScore { get; private set; }
        //public String Name { get; set; }


        public Player()
        {
            TotalScore = 0;
            //Name = "Player With No Name";
        }

        public void UpdateTotalScoreWithLastMoveScore(int lastMoveScore)
        {
            TotalScore += lastMoveScore;
        }

    }
}
