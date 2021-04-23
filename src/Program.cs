using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace rC
{

    class Program
    {

        //NEW STUFF:
        //create_project directory\projectName
        //upgrade_project directory\projectName
        //run >> name
        //compile_lines / compile_lines_from_file (file:file,0,1)
        //create_file filename
        //run_project : runs the project
        //rC filename.rcode 
        //arg1, arg2, arg3

        static void Main(string[] args)
        {
            string readline;
            List<string> codeLines = new List<string>();
            string[] varTypes = new string[] { "number", "str", "save(this)", "Write", "#", "WriteStr", "WriteNum", "sleep", "for", "color", "compile_lines", "if", "pixel", "import", "CreateFile", "toLower", "toUpper" };
            string[] methods = new string[] { "Write", "WriteStr", "WriteNum" };
            string[] loops = new string[] { "for", "compile_lines", "setcursorpos:", "sleep", "pixel", "color", "if", "CreateFile", "#", "import", "str", "number", "toLower", "toUpper" };
            bool isCompiling = true;
            List<string> numberNames = new List<string>();
            List<double> numberValues = new List<double>();
            List<string> strNames = new List<string>();
            List<string> strValues = new List<string>();
            List<string> references = new List<string>();
            List<string> strListNames = new List<string>();
            List<List<string>> strListValues = new List<List<string>>();
            List<string> numListNames = new List<string>();
            List<List<double>> numListValues = new List<List<double>>();

            foreach (var arg in args)
            {
                int x = Array.IndexOf(args,arg);
                if (x != 0)
                {
                    strNames.Add("arg" + x);
                    strValues.Add(arg);
                }
            }

            if (args.Length >= 1)
            {
                StreamReader reader_file;
                List<string> run_code = new List<string>();
                try
                {
                    reader_file = File.OpenText(args[0]);
                
                string line_init;
                while ((line_init = reader_file.ReadLine()) != null)
                {
                    run_code.Add(line_init);
                }
                reader_file.Close();
                }catch
                {
                    Console.WriteLine("Exception: Could not find file: "+args[0]);
                }
                rCompiler.Compile(run_code, numberNames, numberValues, strNames, strValues, references,strListNames,strListValues,numListNames,numListValues);

            }
            else
            {
                try{
                    StreamReader reader_file;
                    List<string> run_code = new List<string>();
                        reader_file = File.OpenText("Main.rcode");
                    
                    string line_init;
                    while ((line_init = reader_file.ReadLine()) != null)
                    {
                        run_code.Add(line_init);
                    }
                    reader_file.Close();
                   
                    rCompiler.Compile(run_code, numberNames, numberValues, strNames, strValues, references,strListNames,strListValues,numListNames,numListValues);
                } catch{

                    }
                if (File.Exists("run_config.rconfig"))
                {
                    StreamReader readConfig = File.OpenText("run_config.rconfig");
                    string readline_config;
                    while ((readline_config = readConfig.ReadLine()) != null)
                    {
                        if (readline_config.StartsWith("init:"))
                        {
                            if (File.Exists(readline_config.Split(':').Last()))
                            {
                                StreamReader reader_init = File.OpenText(readline_config.Split(':').Last());
                                List<string> run_code = new List<string>();
                                string line_init;
                                while ((line_init = reader_init.ReadLine()) != null)
                                {
                                    run_code.Add(line_init);
                                }
                                reader_init.Close();
                                rCompiler.Compile(run_code, numberNames, numberValues, strNames, strValues, references,strListNames,strListValues,numListNames,numListValues);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Init File Does Not Exist, Change it in run_config.rconfig");
                                Console.ResetColor();
                            }
                        }
                    }
                    readConfig.Close();

                }
                else
                {
                    if (File.Exists("Main.rcode"))
                    {
                        Console.ResetColor();
                        //do nothing
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("#rC Started Successfully");
                        Console.ResetColor();
                        Console.Write("rC-shell ~# ");

                        Console.ResetColor();
                    }
                }

                while ((readline = Console.ReadLine()).ToLower() != "rcompiler.compile" && isCompiling == true)
                {
                    Console.Write("rC-shell ~# ");
                    if (readline.StartsWith("create_file"))
                    {
                        var file = File.CreateText(readline.Split(new[] { "create_file " }, StringSplitOptions.None).Last() + ".rcode");
                        file.Close();
                    }
                    else if (readline == "run_project")
                    {
                        if (File.Exists("Main.rcode"))
                        {
                            string read;
                            StreamReader reader = File.OpenText("Main.rcode");
                            List<string> entryPoint_Code = new List<string>();
                            while ((read = reader.ReadLine()) != null)
                            {
                                entryPoint_Code.Add(read);
                            }
                            reader.Close();
                            rCompiler.Compile(entryPoint_Code, numberNames, numberValues, strNames, strValues, references,strListNames,strListValues,numListNames,numListValues);
                            Console.Write("\n");
                        }
                        else if (!File.Exists("run_config.rconfig"))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Main Run Config: run_config not found, Please Run the Command: restore_project");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Entry Point Main.rcode Not Found, Please Run the Command: restore_project");
                            Console.ResetColor();
                        }
                    }
                    //RESTORING PROJECT
                    else if (readline.ToLower() == "restore_project")
                    {
                        if (!File.Exists("Main.rcode"))
                        {
                            var file = File.CreateText("Main.rcode");
                            file.Close();
                        }
                        else if (!File.Exists("run_config.rconfig"))
                        {
                            StreamWriter ConfigWriter = File.CreateText("run_config.rconfig");
                            ConfigWriter.WriteLine("init:Main.rcode");
                            ConfigWriter.Close();
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Project Restored Successfully!");
                        Console.ResetColor();
                    }
                    else if (readline == "clrscr")
                    {
                        Console.Clear();
                    }
                    else if (readline.ToLower().StartsWith("upgrade_project"))
                    {
                        if (File.Exists(readline.Split(new[] { "upgrade_project " }, StringSplitOptions.None).Last() + @"\Main.rcode"))
                        {
                            File.Delete(readline.Split(new[] { "upgrade_project " }, StringSplitOptions.None).Last() + @"\Run_Program.exe");
                            File.Copy("rC.exe", readline.Split(new[] { "upgrade_project " }, StringSplitOptions.None).Last() + @"\Run_Program.exe");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Project Upgraded Successfully!");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Project not Found, Please Create a Project Using create_project or Restore the Current Project Using: restore_project");
                            Console.ResetColor();
                        }
                    }
                    else if (readline.ToLower().StartsWith("create_project "))
                    {
                        var dirToCopy = Directory.CreateDirectory(readline.Split(new[] { "create_project " }, StringSplitOptions.None).Last());
                        File.CreateText(dirToCopy.FullName + @"/Main.rcode");
                        StreamWriter ConfigWriter = File.CreateText(dirToCopy.FullName + @"/run_config.rconfig");
                        ConfigWriter.WriteLine("init:Main.rcode");
                        ConfigWriter.Close();
                        File.Copy("rC.exe", dirToCopy.FullName + @"/Run_Program.exe");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Project Created Successfully!");
                        Console.ResetColor();
                    }
                    else if (readline.StartsWith("compile >>") || readline.StartsWith("run >>"))
                    {
                        codeLines.Add(readline);
                        rCompiler.Compile(codeLines, numberNames, numberValues, strNames, strValues, references,strListNames,strListValues,numListNames,numListValues);
                        Console.ReadLine();
                        //isCompiling = false;
                    }
                    else if (readline.ToLower() == "quit")
                    {
                        Console.WriteLine("Exiting...\nCode Will Be Saved as a Temporary File");
                        StreamWriter temp = File.CreateText("tempSave.rcode");
                        foreach (var lineTemp in codeLines)
                        {
                            temp.WriteLine(lineTemp);
                        }
                        temp.Close();
                        System.Threading.Thread.Sleep(1000);
                        Environment.Exit(1);
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
                            if (modified == "newline")
                            {
                                Console.WriteLine("newline");
                            }
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
                        //codeLines.Add(readline);
                        //Console.Write("" + codeLines.Count + " ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        if (readline!=""){
                        Console.Write("Command not Recognized!");
                        }
                        Console.ResetColor();
                    }
                }
                Console.Clear();
                List<string> compileAfter = codeLines;
                rCompiler.Compile(codeLines, numberNames, numberValues, strNames, strValues, references,strListNames,strListValues,numListNames,numListValues);

                Console.ReadLine();
            }
        }
        public static void WriteId(int id)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n------------------------------------------------------\n ----> Code Saved With Id " + id + " <---- \n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
