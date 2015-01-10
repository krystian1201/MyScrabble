
using System.Collections.Generic;


namespace MyScrabble.Controller
{
    public class ScrabbleDictionary
    {
        private List<string> wordList;

        public ScrabbleDictionary()
        {
            wordList = new List<string>();

            PopulateWordList();
        }

        private void PopulateWordList()
        {
            wordList.Add("baba");
            wordList.Add("baca");
        }


        public bool IsWordInDictionary(string wordToCheck)
        {

            if (wordList.Contains(wordToCheck))
            {
                return true;
            }


            return false;
        }
    }
}
