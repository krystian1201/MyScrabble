
using System;
using System.Windows;
using System.IO;

using System.Collections.Generic;


namespace MyScrabble.Controller
{
    public class ScrabbleDictionary
    {
        private List<string> _wordList;

        public List<string> WordList 
        {
            get { return _wordList;  }  
        }

        public ScrabbleDictionary()
        {
            _wordList = new List<string>();

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
                lines = File.ReadAllLines(@"sowpods.txt");

            }
            catch (IOException e)
            {
                MessageBox.Show("The dictionary file could not be read:\n" + e.Message);
            }

            if (lines != null && lines.Length > 0)
            {
                _wordList.AddRange(lines);
            }
            else
            {
                MessageBox.Show("The dictionary file was not read correctly");
            }
           
        }


        //just for tests
        private void PopulateWordListWithSetWords()
        {
            _wordList.Add("baba");
            _wordList.Add("baca");
        }


        public void RemoveWordFromDictionary(string wordToRemove)
        {
            _wordList.Remove(wordToRemove);
        }

        public bool IsWordInDictionary(string wordToCheck)
        {
            return _wordList.Contains(wordToCheck);
        }
    }
}
