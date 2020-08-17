using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Remoting.Lifetime;

namespace rC
{

    class Program
    {


        static void Main(string[] args)
        {
            string readline;
            List<string> codeLines = new List<string>();
            string[] varTypes = new string[] { "number", "str", "save(this)", "Write", "WriteStr", "WriteNum" , "for"};
            string[] methods = new string[] { "Write", "WriteStr", "WriteNum" };
            string [] loops = new string[] {"for"};

            Console.Write("0 ");


            while ((readline = Console.ReadLine()).ToLower() != "rcompiler.compile")
            {


                if (varTypes.Any(readline.StartsWith))
                {
                    Console.Clear();
                    foreach (var line in codeLines)
                    {
                        if (line == "save(this)" && loops.Any(line.Contains) == false)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(codeLines.IndexOf(line) + " " + line + "\n");
                            Console.ResetColor();
                        }
                        else if (methods.Any(line.StartsWith) && line != "save(this)" && loops.Any(line.StartsWith) == false)
                        {
                            Console.ResetColor();
                            Console.Write(codeLines.IndexOf(line) + " ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(line.Split('&').First());
                            Console.ResetColor();
                            try
                            {
                                Console.Write("&>" + line.Split(new[] { "&>" }, StringSplitOptions.None).Last() + "\n");
                            }
                            catch
                            {
                                Console.Write(" <---- Invalid Syntax\n");
                            }
                        }
                        else if (varTypes.Any(line.StartsWith) && loops.Any(line.StartsWith) == false && methods.Any(line.StartsWith) == false && line != "save(this)" && loops.Any(line.StartsWith) == false)
                        {
                            Console.ResetColor();
                            Console.Write(codeLines.IndexOf(line) + " ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(line.Split(' ').First());
                            Console.ForegroundColor = ConsoleColor.White;
                            try
                            {
                                Console.Write(" " + line.Split(' ')[1] + " >>" + line.Split('>').Last() + "\n");
                            }
                            catch
                            {
                                Console.Write(" <---- Invalid Syntax\n");
                            }
                        }
                        else if (loops.Any(line.StartsWith) && methods.Any(line.StartsWith) == false && line != "save(this)")
                        {
                            Console.ResetColor();
                            Console.Write(codeLines.IndexOf(line) + " ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(line.Split(' ').First());
                            Console.ResetColor();
                            Console.Write(line.Split(new[] { line.Split(' ').First() }, StringSplitOptions.None).Last() + "\n");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(codeLines.IndexOf(line) + " " + line + "\n");
                        }

                    }
                    if (methods.Any(readline.Contains) == false && readline != "save(this)"  && loops.Any(readline.Contains) == false)
                    {
                        Console.ResetColor();
                        Console.Write(codeLines.Count + " ");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(readline.Split(' ').First());
                        Console.ForegroundColor = ConsoleColor.White;
                        try
                        {
                            Console.Write(" " + readline.Split(' ')[1] + " >>" + readline.Split('>').Last() + "\n");
                        }
                        catch
                        {
                            Console.Write(" <---- Invalid Syntax\n");
                        }


                    }
                    else if (readline == "save(this)")
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(codeLines.Count + " " + readline + "\n");
                        Console.ResetColor();
                    }else if (loops.Any(readline.StartsWith))
                    {
                        Console.ResetColor();
                        Console.Write(codeLines.Count + " ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(readline.Split(' ').First());
                        Console.ResetColor();
                        Console.Write(readline.Split(new[] { readline.Split(' ').First() }, StringSplitOptions.None).Last() + "\n");
                        Console.ResetColor();
                    }
                    else
                    {

                        Console.ResetColor();
                        Console.Write(codeLines.Count + " ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(readline.Split('&').First());
                        Console.ResetColor();
                        try
                        {
                            Console.Write("&>" + readline.Split(new[] { "&>" }, StringSplitOptions.None).Last() + "\n");
                        }
                        catch
                        {
                            Console.Write(" <---- Invalid Syntax\n");
                        }
                    }




                }

                if (readline.StartsWith("line.modify") && readline.Contains(":"))
                {
                    int modifyLineIndex = Convert.ToInt32(readline.Split(new[] { "line.modify(" }, StringSplitOptions.None).Last().Split(')').First());
                    try
                    {
                        codeLines[modifyLineIndex] = readline.Split(new[] { ": " }, StringSplitOptions.None).Last();
                    }
                    catch
                    {
                        Console.Write(" <----- Invalid Syntax (Line not Found)\n");
                    }
                    Console.Clear();
                    foreach (var modified in codeLines)
                    {
                        if (varTypes.Any(modified.StartsWith))
                        {

                            if (methods.Any(modified.StartsWith) == false && modified != "save(this)" && loops.Any(modified.StartsWith) == false)
                            {


                                Console.ResetColor();
                                Console.Write(codeLines.IndexOf(modified) + " ");
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write(modified.Split(' ').First());
                                Console.ForegroundColor = ConsoleColor.White;
                                try
                                {
                                    Console.Write(" " + modified.Split(' ')[1] + " >>" + modified.Split('>').Last() + "\n");
                                }
                                catch
                                {
                                    Console.Write(" <---- Invalid Syntax\n");
                                }
                            }
                            else if (modified == "save(this)")
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write(codeLines.IndexOf(modified) + " " + modified + "\n");
                                Console.ResetColor();
                            }
                            else if (loops.Any(modified.StartsWith))
                            {
                                Console.ResetColor();
                                Console.Write(codeLines.Count + " ");
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write(modified.Split(' ').First());
                                Console.ResetColor();
                                Console.Write(modified.Split(new[] { modified.Split(' ').First() }, StringSplitOptions.None).Last() + "\n");
                                Console.ResetColor();
                            }
                            else
                            {

                                Console.ResetColor();
                                Console.Write(codeLines.IndexOf(modified) + " ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(modified.Split('&').First());
                                Console.ResetColor();
                                try
                                {
                                    Console.Write("&>" + modified.Split(new[] { "&>" }, StringSplitOptions.None).Last() + "\n");
                                }
                                catch
                                {
                                    Console.Write(" <---- Invalid Syntax\n");
                                }
                            }
                        }

                    }
                }
                else
                {
                    codeLines.Add(readline);
                    Console.Write("" + codeLines.Count + " ");
                }
            }
            Console.Clear();
            List<string> numberNames = new List<string>();
            List<double> numberValues = new List<double>();
            List<string> strNames = new List<string>();
            List<string> strValues = new List<string>();
            rCompiler.Compile(codeLines, numberNames, numberValues, strNames, strValues);
            Console.WriteLine("\n------------------------------------------------------\nCompiled, Output is Above\nPress enter to exit...");
            Console.ReadLine();
        }
        public static void WriteId(int id)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n------------------------------------------------------\n ----> Code Saved With Id " + id + " <---- \n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
