using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace SilverlightApplication6
{
    public static class SimpleUniqueRandom
    {
        private static IDictionary<int, int> numbersGenerated = new Dictionary<int, int>();
        private static Random random;
        private static bool firstTime = true;
        private static int numberIndex = 0;

        public static int getInt()
        {
            int i;
            if (firstTime)
            {
                random = new Random(DateTime.Now.Millisecond);
                firstTime = false;
                i = random.Next();
            }
            else {
                i = random.Next();
                while (numbersGenerated.ContainsKey(i)) {
                    i = random.Next();
                }
            }

            numbersGenerated.Add(i, numberIndex);
            numberIndex++;
            return i;
        }
    }
}
