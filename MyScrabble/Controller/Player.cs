
using System;

using MyScrabble.Model.Tiles;

namespace MyScrabble.Controller
{
    public class Player
    {
        public int Score { get; set; }
        public String Name { get; set; }


        public Player()
        {
            Score = 0;
            Name = "Player With No Name";
        }

        public void MakeAMove()
        {

        }
    }
}
