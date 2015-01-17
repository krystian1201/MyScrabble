
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

        const string _alphabetizedWordsPermutationsFileName = "alphabetizedWordsPermutations.txt";

        public AIDictionary()
        {
            //just for tests
            //listOfKeys = CreateAlphabetizedStringsManually();

            //gave up for now
            //listOfKeys = GenerateAlphabetizedStrings();

            //AssignWordListsToAlphabetizedStrings(listOfKeys);



            _alphabetizedWordsPermutations = new Dictionary<string, List<string>>();
            ScrabbleDictionary tempScrabbleDictionary = new ScrabbleDictionary();

            //tempScrabbleDictionary.WordList.Count
            for (int i = 0; i < 270; i++)
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

            SaveAlphabetizedWordsPermutationsToFile(_alphabetizedWordsPermutationsFileName);
            ReadAlphabetizedWordsPermutationsFromFile(_alphabetizedWordsPermutationsFileName);
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
            
            for (int i = 0; i < lines.Length; i += 2 )
			{
                string alphabetizedWord = lines[i];
                List<string> wordPermutations = lines[i + 1].Split(',').ToList();

                dictionary[alphabetizedWord] = wordPermutations;
			}
            

            return dictionary;
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


        //private void AddPermutationsIfValidWords(ScrabbleDictionary tempScrabbleDictionary, 
        //    string alphabetizedWord, List<string> permutations)
        //{
        //    foreach (string permutation in permutations)
        //    {
        //        if (tempScrabbleDictionary.IsWordInDictionary(permutation) &&
        //           !_alphabetizedStringsPermutations[alphabetizedWord].Contains(permutation))
        //        {
        //            _alphabetizedStringsPermutations[alphabetizedWord].Add(permutation);
        //            tempScrabbleDictionary.RemoveWordFromDictionary(permutation);
        //        }

        //    }
        //}


        //TODO - the creation of keys should be done automatically
        //TODO - it should be done for all letters and all string
        //lengths from 2 up to 7
        //private List<string> CreateAlphabetizedStringsManually()
        //{
        //    List<string> list = new List<string>();

        //    list.Add("aa");
        //    list.Add("ab");
        //    list.Add("ac");
        //    list.Add("bb");
        //    list.Add("bc");
        //    list.Add("cc");

        //    list.Add("aaa");
        //    list.Add("aab");
        //    list.Add("aac");

        //    list.Add("abb");
        //    list.Add("abc");
        //    list.Add("acc");

        //    list.Add("bbb");
        //    list.Add("bbc");
        //    list.Add("bcc");

        //    list.Add("ccc");

        //    return list;
        //}

        //give up this method for now
        //private List<string> GenerateAlphabetizedStrings()
        //{
        //    int startLength = 2;
        //    int endLength = 3;

        //    char startLetter = 'a';
        //    char endLetter = 'c';

        //    List<string> alphabetizedStrings = new List<string>();

            
        //    //GenerateAlphabetizedStringsOfGivenLenght(4, startLetter, endLetter);

        //    return alphabetizedStrings;
        //}

        //private List<string> GenerateAlphabetizedStringsOfGivenLenght(int stringLength, char startLetter, char endLetter)
        //{

        //    if (stringLength <= 1)
        //    {
        //        throw new Exception("Scrabble words must be at least 2-letters long");
        //    }

        //    char[] charArray = new char[stringLength];
        //    List<string> alphabetizedStrings = new List<string>();

        //    //for (char outerLetter = startLetter; outerLetter <= endLetter; outerLetter++)
        //    //{
        //    //    //int i = 0;

        //    //    for (int i = 0; i < stringLength; i++)
        //    //    {
        //    //        charArray[i] = outerLetter;
        //    //    }

        //    //    alphabetizedStrings.Add(new string(charArray));


        //    //        for (char innerLetter = outerLetter; innerLetter <= endLetter; innerLetter++)
        //    //        {
        //    //            //charArray[0] = outerLetter;

        //    //            for (int i = 0; i < stringLength; i++)
        //    //            {
        //    //                charArray[i] = (char)(innerLetter + i);// Convert.ToChar(i);
                            
        //    //            }

        //    //            alphabetizedStrings.Add(new string(charArray));
        //    //        } 
        //    //}


        //    //next attempt
        //    for (char startLetterChanged = (char)(startLetter + 1); startLetterChanged <= endLetter; startLetterChanged++)
        //    {

        //        for (int letterChangeStartPosition = stringLength - 1; letterChangeStartPosition > 0; letterChangeStartPosition--)
        //        {
        //            //char startLetterChanged = (char)(startLetter + (stringLength - (letterChangePosition + 1)));


        //            for (char letterChanged = startLetterChanged; letterChanged <= endLetter; letterChanged++)
        //            {
        //                for (int letterChangePosition = letterChangeStartPosition; letterChangePosition < stringLength; letterChangePosition++)
        //                {

        //                }
        //            }

        //        }
        //    }

             

        //    return alphabetizedStrings;
        //}

        //private void AssignWordListsToAlphabetizedStrings(List<string> listOfKeys)
        //{
        //    _alphabetizedStringsPermutations = new Dictionary<string, List<string>>();

        //    _scrabbleDictionary = new ScrabbleDictionary();


        //    foreach (string alphabetizedString in listOfKeys)
        //    {
                
        //        char[] alphabetizedStringArray = alphabetizedString.ToCharArray();

        //        List<string> permutationsList = new List<string>();

        //        GeneratePermutations(permutationsList, alphabetizedStringArray, 0);

        //        List<string> validWordsInPermutationsList =
        //            CheckIfPermutationsAreValidWords(permutationsList);

        //        if (validWordsInPermutationsList.Count >= 1)
        //        {
        //            _alphabetizedStringsPermutations[alphabetizedString] = validWordsInPermutationsList;
        //        }
                
        //    }
        //}


        //algorithm from this site:
        //http://stackoverflow.com/questions/11208446/generating-permutations-of-a-set-most-efficiently
        //private void GeneratePermutations(List<string> permutationsList, char[] charArray, int i)
        //{

        //    if (i >= charArray.Length - 1)
        //    {
        //        if (!permutationsList.Contains(new string(charArray)))
        //        {
        //            permutationsList.Add(new string(charArray));
        //        }    
        //    }
        //    else 
        //    {
        //        GeneratePermutations(permutationsList, charArray, i + 1);

        //        for (int j = i + 1; j < charArray.Length; j++)
        //        {
        //            SwapElementsInCharArray(charArray, i, j);
        //            GeneratePermutations(permutationsList, charArray, i + 1);
        //            SwapElementsInCharArray(charArray, i, j);
        //        }
        //    }
        //}

        //private void SwapElementsInCharArray(char[] charArray, int i, int j)
        //{
        //    char temp = charArray[i];
        //    charArray[i] = charArray[j];
        //    charArray[j] = temp;
        //}

        //private List<string> CheckIfPermutationsAreValidWords(List<string> permutationsList)
        //{
        //    List<string> validWords = new List<string>();

        //    foreach (string permutation in permutationsList)
        //    {
        //        if (_scrabbleDictionary.IsWordInDictionary(permutation))
        //        {
        //            validWords.Add(permutation);
        //        }
        //    }

        //    return validWords;
        //}

        private string AlphabetizeString(string s)
        {
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
    }
}
