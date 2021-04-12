using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rC
{
    public static class Split
    {
        public static string split(string ToSplit,string spliter,int index)
        {
            string Splited = "";
            Splited = ToSplit.Split(new [] {spliter},StringSplitOptions.None)[index];
            return Splited;
        }        
    }
}
