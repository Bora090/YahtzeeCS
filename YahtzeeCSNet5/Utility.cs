using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace YahtzeeCSNet5
{
    public class Utility //a unsorted mess because I AM LAZY
    {
        public static Random rnd = new Random();
        public static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;


        public static string beautifyName(string input)
        {
            return textInfo.ToTitleCase(input.ToString().ToLower().Replace("-", " "));
        }
        public static bool hasValues(List<int> list, List<int> valuesToCheck) => hasValues(list, valuesToCheck.ToArray()); //wrapper 
        public static bool hasValues(List<int> list, params int[] valuesToCheck)
        {
            List<int> _list = list.ToList(); //Copy of list
            List<int> _valuesToCheck = valuesToCheck.ToList(); //Copy of valuesToCheck

            if (_list.Count == 0) //Return false if there are no values in list, otherwise this func would always return true if a empty list is passed
            {
                return false;
            }

            foreach (int value in _valuesToCheck)
            {
                if (_list.Contains(value))
                {
                    _list.Remove(value);
                    continue;
                }
                return false;
            }
            return true;
        }
    }
}
