using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace rC
{
    class FileStream
    {
        public static void WriteToFile(string file, string content)
        {
            if(File.Exists(file))
            {
                StreamWriter writer = File.AppendText(file);
                writer.WriteLine(content);
                writer.Close();
            }
            else
            {
                StreamWriter writer = File.CreateText(file);
                writer.WriteLine(content);
                writer.Close();
            }
        }
        public static void CreateFile (string filename)
        {
            File.CreateText(filename);
        }
        public static void DeleteFile(string filename)
        {
            File.Delete(filename);
        }
        public static string ReadFile (string filename)
        {
            string content = File.ReadAllText(filename);
            return content;
        }

    }
}
