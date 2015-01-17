using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScrabble.Controller
{
    static class Game
    {
        private static bool isFirstMove = true;

        public static bool IsFirstMove
        {
            get { return isFirstMove;  }
        }

        public static void Start()
        {
            isFirstMove = true;
        }

        public static void SetAfterFirstMove()
        {
            isFirstMove = false;
        }
    }
}
