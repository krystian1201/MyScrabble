
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using MyScrabble.Controller;


namespace MyScrabble.Model
{
   
    public class AIDictionary
    {
        private Dictionary<string, List<string>> _alphabetizedWordsPermutations;

        public Dictionary<string, List<string>> AlphabetizedWordsPermutations
        {
            get { return _alphabetizedWordsPermutations; }
        }

        const string _alphabetizedWordsPermutationsFileName = "alphabetizedWordsPermutations.txt";

        public AIDictionary()
        {
            //an offline process - done just once
            //BuildAlphabetizedWordsPermutations();
            //SaveAlphabetizedWordsPermutationsToFile(_alphabetizedWordsPermutationsFileName);

            _alphabetizedWordsPermutations = ReadAlphabetizedWordsPermutationsFromFile(_alphabetizedWordsPermutationsFileName);
        }

        private void BuildAlphabetizedWordsPermutations()
        {
            _alphabetizedWordsPermutations = new Dictionary<string, List<string>>();
            ScrabbleDictionary tempScrabbleDictionary = new ScrabbleDictionary();

            //tempScrabbleDictionary.WordList.Count
            for (int i = 0; i < tempScrabbleDictionary.WordList.Count; i++)
            {
                string baseWord = tempScrabbleDictionary.WordList[i];

                string alphabetizedWord = AlphabetizeString(baseWord);

                //just for being extra-sure
                if (!_alphabetizedWordsPermutations.ContainsKey(alphabetizedWord))
                {
                    _alphabetizedWordsPermutations.Add
                        (alphabetizedWord, new List<string>() { baseWord });


                    CheckWordListForPermutations(tempScrabbleDictionary, i, baseWord, alphabetizedWord);

                }
            }
        }

        private void CheckWordListForPermutations(ScrabbleDictionary tempScrabbleDictionary, int i, 
            string baseWord, string alphabetizedWord)
        {
            //we start checking for words being permutations of a given word
            //from the word that appears in the dictionary after the given word

            for (int j = i + 1; j < tempScrabbleDictionary.WordList.Count; j++)
            {
                string innerWord = tempScrabbleDictionary.WordList[j];

                if (IsStringPermutation(baseWord, innerWord))
                {
                    _alphabetizedWordsPermutations[alphabetizedWord].Add(innerWord);
                    tempScrabbleDictionary.RemoveWordFromDictionary(innerWord);
                }
            }
        }


        private bool IsStringPermutation(String str1, String str2)
        {

            if (str1.Length != str2.Length)
                return false;

            char[] charArray1 = str1.ToCharArray();
            char[] charArray2 = str2.ToCharArray();


            //we know that data to sort are just English alphabet letters
            //so we can use sorting "by counting"(?)
            //anyway - without using comparisons - in linear time
            
            int[] letterCountsInStr1 = new int[26];
            int[] letterCountsInStr2 = new int[26];

            for (int i = 0; i < charArray1.Length; i++)
            {
                letterCountsInStr1[charArray1[i] - 97]++;
                letterCountsInStr2[charArray2[i] - 97]++;
                
            }

            for (int i = 0; i < letterCountsInStr1.Length; i++)
            {
                if (letterCountsInStr1[i] != letterCountsInStr2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public string AlphabetizeString(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                throw new ArgumentException("Cannot alphabetize an empty string");
            }

            // 1.
            // Convert to char array.
            char[] charArray = s.ToCharArray();

            // 2.
            // Sort letters.
            Array.Sort(charArray);

            // 3.
            // Return modified string.
            return new string(charArray);
        }

        private void SaveAlphabetizedWordsPermutationsToFile(string fileName)
        {

            //"clear" the file before writing to it
            FileStream fileStream = File.Open(fileName, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close();


            // Append new text to an existing file. 
            // The using statement automatically closes the stream and calls  
            // IDisposable.Dispose on the stream object. 
            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fileName, true))
            {
                foreach (KeyValuePair<string, List<string>> dictionaryEntry in _alphabetizedWordsPermutations)
                {
                    string valuesListInString = string.Join(",", dictionaryEntry.Value.ToArray());

                    streamWriter.WriteLine(dictionaryEntry.Key);
                    streamWriter.WriteLine(valuesListInString);
                }
            }

        }

        private Dictionary<string, List<string>> ReadAlphabetizedWordsPermutationsFromFile(string fileName)
        {
            var dictionary = new Dictionary<string, List<string>>();

            string[] lines = File.ReadAllLines(fileName);

            for (int i = 0; i < lines.Length; i += 2)
            {
                string alphabetizedWord = lines[i];
                List<string> wordPermutations = lines[i + 1].Split(',').ToList();

                dictionary[alphabetizedWord] = wordPermutations;
            }


            return dictionary;
        }
    }
}
