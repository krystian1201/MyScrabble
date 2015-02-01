using System;
using System.Collections.Generic;


namespace MyScrabble.Utilities
{
    public static class AllIndexesOfSubstringInsideString
    {
        public static IEnumerable<int> AllIndexesOf(this string str, string substring)
        {
            if (String.IsNullOrEmpty(substring))
                throw new ArgumentException("the string to find may not be empty", "substring");

            for (int index = 0; ; index += substring.Length)
            {
                index = str.IndexOf(substring, index);

                if (index == -1)
                    break;
                yield return index;
            }
        }
    }
}
