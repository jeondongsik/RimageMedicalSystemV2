using System;
using System.Collections.Generic;
using System.Text;

namespace RimageKorea
{
    public class RandomOrderNumber
    {
        public static string GetNewOrderNumber()
        {
            // Wait to allow the timer to advance.
            System.Threading.Thread.Sleep(1);
            Random autoRand = new Random();
            string randomNumber = getNextRandomNumber(autoRand);
            return randomNumber;
        }

        // Generate random numbers from the specified Random object.
        static string getNextRandomNumber(Random randObj)
        {
            return randObj.Next().ToString();
        }

        public static string GetNewOrderNumber2()
        {
            // Wait to allow the timer to advance.
            System.Threading.Thread.Sleep(100);
            Random autoRand = new Random();
            string randomNumber = autoRand.Next(1, 999).ToString().PadLeft(3, '0');
            return randomNumber;
        }
    }
}
