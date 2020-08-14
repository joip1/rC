using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace rC
{
    public class rCompiler
    {
        public static void Compile(List<string> code)
        {
            Console.Clear();
            //values and indicators
            List<string> numberNames = new List<string>();
            List<double> numberValues = new List<double>();
            List<string> strNames = new List<string>();
            List<string> strValues = new List<string>();
            int id = 0;
            //read code line by line
            foreach (var line in code)
            {
                if (line.StartsWith("number") && line.Contains(">>"))
                {
                    try
                    {
                        if(numberNames.Contains(line.Split(' ')[1].Split('>').First()))
                        {
                            numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Convert.ToDouble(line.Split('>').Last().Split(' ').Last());
                        }
                        else
                        {
                            numberNames.Add(line.Split(' ')[1].Split('>').First());
                            numberValues.Add(Convert.ToDouble(line.Split('>').Last().Split(' ').Last()));
                        }
                    }
                    catch
                    {
                        int errorLine = code.IndexOf(line);
                        Console.WriteLine($"Invalid Syntax (Line {errorLine--})");
                    }
                }

                if (line.StartsWith("str") && line.Contains(">>"))
                {
                    try
                    {
                        if (strNames.Contains(line.Split(' ')[1].Split('>').First()))
                        {
                            strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = line.Split('>').Last();
                        }
                        else
                        {
                            strNames.Add(line.Split(' ')[1].Split('>').First());
                            strValues.Add(line.Split('>').Last());
                        }
                       
                    }

                    catch
                    {
                        int errorLine = code.IndexOf(line);
                        Console.WriteLine($"Invalid Syntax (Line {errorLine--})");
                    }
                }

                if (line.StartsWith("list(str)"))
                {
                    List<List<string>> listsStr = new List<List<string>>(); 
                }






                //Write 

                if (line.ToLower() == "newline" || line.ToLower() == "newln")
                {
                    Console.WriteLine("");
                }
                if (line.StartsWith("Write") && line.Contains("&>") && line.Contains("<&") && line.Contains("WriteStr") == false && line.Contains("WriteNum") == false)
                {
                    //check if it is a number 
                    var matchesNumber = numberNames.Where(x => line.Contains(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString()));

                    Console.Write(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString() + " ");

                }
                else if (line.StartsWith("WriteStr") && line.Contains("&>"))
                {
                    foreach (var name in strNames)
                    {
                        try
                        {
                            if (line.Contains(name))
                            {
                                Console.Write(strValues[strNames.IndexOf(name)] + line.Split(new[] { name}, StringSplitOptions.None)[1].Split(new[] { "<&" }, StringSplitOptions.None).First());
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid Syntax on Line " + code.IndexOf(line));
                        }
                    }

                }
                else if (line.StartsWith("WriteNum") && line.Contains("&>"))
                {
                    foreach (var name in numberNames)
                    {
                        if (line.Contains(name))
                        {
                            try
                            {
                                Console.Write(numberValues[numberNames.IndexOf(name)] + line.Split(new[] { name }, StringSplitOptions.None)[1].Split(new[] { "<&"}, StringSplitOptions.None).First());
                            }
                            catch
                            {
                                Console.WriteLine("Invalid Syntax on Line " + code.IndexOf(line));
                            }
                        }
                    }





                    //save
                    if (line == "save(this)")
                    {
                        id = new Random().Next(1, 10000);
                        StreamWriter writer = File.CreateText("code.id(" + id + ").txt");
                        foreach (var lineToSave in code)
                        {
                            writer.WriteLine(lineToSave);
                        }
                        writer.Close();
                    }
                    else if (line == "save = false")
                    {

                    }
                }

                if (id != 0)
                {
                    Program.WriteId(id);
                }


                //for
            }
        }
    }
}
