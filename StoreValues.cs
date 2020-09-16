using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rC
{
    public class StoreValues
    {
        public static List<string> numberNames;
        public static List<double> numberValues;
        public static List<string> strNames;
        public static List<string> strValues;
        public static void Store()
        {
            foreach(var item in numberValues)
            {
                Console.WriteLine(item);
            }
        }
    }
}
