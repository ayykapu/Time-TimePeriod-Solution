using System;
using System.Collections.Generic;

namespace Time
{
    class Program
    {
        static void Main(string[] args)
        {
            
            TimePeriod tp2 = new TimePeriod(t1Input: "10:00:00",t2Input: "20:30:00"); //10:30
            TimePeriod tp1 = new TimePeriod(t1Input: "15:00:00", t2Input: "15:35:00"); //00:35

            TimePeriod tp3 = new TimePeriod();
            tp3 = tp1.Minus(tp2);
   

            Console.WriteLine();
            Console.WriteLine(tp3.ToString());






        }
    }
}