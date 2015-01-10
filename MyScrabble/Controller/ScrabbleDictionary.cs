
using System;
using System.Windows;
using System.IO;

using System.Collections.Generic;


namespace MyScrabble.Controller
{
    public class ScrabbleDictionary
    {
        private List<string> wordList;

        public ScrabbleDictionary()
        {
            wordList = new List<string>();

            //for tests
            //PopulateWordListWithSetWords();

            //proper method
            PopulateWordListWithWordsFromDictionary();
        }


        private void PopulateWordListWithWordsFromDictionary()
        {
            string[] lines = null;

            try
            {
                lines = System.IO.File.ReadAllLines(@"sowpods.txt");

            }
            catch (Exception e)
            {
                MessageBox.Show("The dictionary file could not be read:\n" + e.Message);
            }

            if (lines != null && lines.Length > 0)
            {
                wordList.AddRange(lines);
            }
            else
            {
                MessageBox.Show("The dictionary file was not read correctly");
            }
           
        }


        //just for tests
        private void PopulateWordListWithSetWords()
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
