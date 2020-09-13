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
        //added with ubuntu
        //fix: 
        //new to docs == for // readline // color // ;
        //receive every variable for further changes;
        public static void Compile(
            List<string> code,
            List<string> numberNames,
            List<double> numberValues,
            List<string> strNames,
            List<string> strValues)
        {

            //values and indicators

            int id = 0;
            List<int> pixelX = new List<int>();
            List<int> pixelY = new List<int>();
            List<ConsoleColor> pixelColors = new List<ConsoleColor>();
            List<string> charachtersToDraw = new List<string>();
            List<int> pixelXChar = new List<int>();
            List<int> pixelYChar = new List<int>();
            List<ConsoleColor> pixelColorsChar = new List<ConsoleColor>();
            List<string> references = new List<string>();

            //read code line by line
            foreach (var line in code)
            {

             //to lower
             /*USAGE
             ___________________________________________________________________________________________________
             str f >> 
             str name >>$readline
             toLower (f>>name)      <---- gets the value of name.toLower and assigns it to the variable f
             toLower (name)    <---- gets the value of name.toLower and assigns it to itself
             __________________________________________________________________________________________________*/
                if(line.StartsWith("toLower (")&&line.Contains(")")&&line.Contains(">>")){
                    if(strNames.Contains(line.Split(new [] {"toLower ("}, StringSplitOptions.None).Last().Split(new [] {">>"}, StringSplitOptions.None).First())){
                        if(strNames.Contains(line.Split(new [] {">>"}, StringSplitOptions.None).Last().Split(')').First())){
                            strValues[strNames.IndexOf(line.Split(new [] {"toLower ("}, StringSplitOptions.None).Last().Split(new [] {">>"}, StringSplitOptions.None).First())]=strValues[strNames.IndexOf(line.Split(new [] {">>"}, StringSplitOptions.None).Last().Split(')').First())].ToLower();
                        }else
                        {
                            strValues[strNames.IndexOf(line.Split(new [] {"toLower ("}, StringSplitOptions.None).Last().Split(new [] {">>"}, StringSplitOptions.None).First())]=line.Split(new [] {">>"}, StringSplitOptions.None).Last().Split(')').First().ToLower(); 
                        }
                    }

                }else if(line.StartsWith("toLower (")&&line.Contains(")")&&line.Contains(">>")==false)
                {
                    try
                    {
                        strValues[strNames.IndexOf(line.Split(new [] {"toLower ("}, StringSplitOptions.None).Last().Split(')').First())]=strValues[strNames.IndexOf(line.Split(new [] {"toLower ("}, StringSplitOptions.None).Last().Split(')').First())].ToLower();
                    }
                    catch
                    {
                        
                    }
                }


            //toUpper


                foreach (var str in strNames)
                {
                    if(line.StartsWith("str (" + str)&&line.Contains('+')&&line.Contains(')') || line.StartsWith("str(" + str)&&line.Contains('+')&&line.Contains(')'))
                    {
                        if(line.Split('+')[1].Split(')').First()!="$readline" || str !="$readline"){
                        try
                        {
                            strValues[strNames.IndexOf(str)]= strValues[strNames.IndexOf(str)]+strValues[strNames.IndexOf(line.Split('+')[1].Split(')').First())];               
                        }catch
                        {
                            strValues[strNames.IndexOf(str)]= strValues[strNames.IndexOf(str)]+line.Split('+')[1].Split(')').First();
                        } 
                        }else if(str=="$readline"){
                            try {
                                strValues[strNames.IndexOf(line.Split('+')[1].Split(')').First())] = Console.ReadLine() + strValues[strNames.IndexOf(line.Split('+')[1].Split(')').First())];
                            }catch
                            {

                            }
                        }else if(line.Split('+')[1].Split(')').First()=="$readline"){
                             strValues[strNames.IndexOf(str)]=strValues[strNames.IndexOf(str)]+Console.ReadLine();
                        }
                    }
                }

                if(line.ToLower() == "clear")
                {
                    Console.Clear();  
                }
                else if (line.StartsWith("import"))
                {
                    if (line.ToLower() == "import pixel")
                    {
                        references.Add("pixel");
                    }
                    else if (line.ToLower() == "import split")
                    {
                        references.Add("split");
                    }
                }
               
                if (references.Contains("pixel"))
                {
                    if (line.ToLower().StartsWith("pixel.draw"))
                    {
                        try
                        {
                            //add to normal pixel
                            int x = Convert.ToInt32(line.ToLower().Split(new[] { "x:" }, StringSplitOptions.None).Last().Split(' ').First());
                            int y = Convert.ToInt32(line.ToLower().Split(new[] { "y:" }, StringSplitOptions.None).Last().Split(' ').First());
                            pixelX.Add(Convert.ToInt32(line.ToLower().Split(new[] { "x:" }, StringSplitOptions.None).Last().Split(' ').First()));
                            pixelY.Add(Convert.ToInt32(line.ToLower().Split(new[] { "y:" }, StringSplitOptions.None).Last().Split(' ').First()));
                            string color = line.ToLower().Split(new[] { "color:" }, StringSplitOptions.None).Last().Split(' ').First();

                               
                            if(color == "white")
                            {
                                pixelColors.Add(ConsoleColor.White);
                            }else if(color == "red")
                            {
                                pixelColors.Add(ConsoleColor.Red);
                            }else if(color == "blue")
                            {
                                pixelColors.Add(ConsoleColor.Blue);
                            }

                        }
                        catch
                        {
                            Console.WriteLine("Invalid Syntax In Line " + code.IndexOf(line));
                        }
                    }
                    if (line.ToLower().StartsWith("pixel.drawchar"))
                    {
                        try
                        {
                            //add to char
                            int x = Convert.ToInt32(line.ToLower().Split(new[] { "x:" }, StringSplitOptions.None).Last().Split(' ').First());
                            int y = Convert.ToInt32(line.ToLower().Split(new[] { "y:" }, StringSplitOptions.None).Last().Split(' ').First());
                            pixelXChar.Add(Convert.ToInt32(line.ToLower().Split(new[] { "x:" }, StringSplitOptions.None).Last().Split(' ').First()));
                            pixelYChar.Add(Convert.ToInt32(line.ToLower().Split(new[] { "y:" }, StringSplitOptions.None).Last().Split(' ').First()));
                            string color = line.ToLower().Split(new[] { "color:" }, StringSplitOptions.None).Last().Split(' ').First();
                            string characterToDraw = line.ToLower().Split(new[] { "char:" }, StringSplitOptions.None).Last().Split(' ').First();
                            charachtersToDraw.Add(characterToDraw);

                            if (color == "white")
                            {
                                pixelColorsChar.Add(ConsoleColor.White);
                            }
                            else if (color == "red")
                            {
                                pixelColorsChar.Add(ConsoleColor.Red);
                            }
                            else if (color == "blue")
                            {
                                pixelColorsChar.Add(ConsoleColor.Blue);
                            }
                        


                        }
                        catch
                        {
                            Console.WriteLine("Invalid Syntax In Line " + code.IndexOf(line));
                        }
                    }
                }
                if (line.ToLower().StartsWith("sleep >>"))
                {
                    System.Threading.Thread.Sleep(Convert.ToInt32(line.Split('>').Last()));
                }
                //color indicators
                if (line.ToLower().Contains("color.reset"))
                {
                    Console.ResetColor();
                }
                else if (line.ToLower().Contains("color.green"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (line.ToLower().Contains("color.blue"))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (line.ToLower().Contains("color.red"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (line.ToLower().Contains("color.magenta"))
                {
                Console.ForegroundColor = ConsoleColor.Magenta;
                }
                 else if (line.ToLower().Contains("color.yellow"))
                {
                Console.ForegroundColor = ConsoleColor.Yellow;
                }

            if (line.ToLower() == ("$readline"))
            {
                Console.ReadLine();
            }

            if (line.Contains("number") && line.Contains(">>")
                && line.ToLower().StartsWith("for") == false
                && line.ToLower().Contains("in range %") == false
                && line.Contains("$>") == false)
            {
                try
                {
                    if (numberNames.Contains(line.Split(' ')[1].Split('>').First()))
                    {
                        if (line.ToLower().Contains("$readline") == false)
                        {
                            try
                            {
                                numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Convert.ToDouble(line.Split('>').Last().Split(' ').Last());
                            }
                            catch
                            {
                                numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Convert.ToDouble(line.Split('>').Last());
                            }
                        }
                        else if (line.ToLower().Contains("$readline") == true)
                        {
                            numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Convert.ToDouble(Console.ReadLine());
                        }
                    }
                    else
                    {
                        if (line.ToLower().Contains("$readline") == false)
                        {
                            numberNames.Add(line.Split(' ')[1].Split('>').First());
                            try
                            {
                                numberValues.Add(Convert.ToDouble(line.Split('>').Last().Split(' ').Last()));
                            }
                            catch
                            {
                                numberValues.Add(Convert.ToDouble(line.Split('>').Last()));
                            }
                        }
                        else if (line.ToLower().Contains("$readline") == true)
                        {
                            numberNames.Add(line.Split(' ')[1].Split('>').First());
                            numberValues.Add(Convert.ToDouble(Console.ReadLine()));
                        }

                    }
                }
                catch
                {
                    int errorLine = code.IndexOf(line);
                    Console.WriteLine($"Invalid Syntax (Line {errorLine--})");
                }
            }

            //string definer
            if (line.Contains("str")
                && line.Contains(">>")
                && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false
                && line.Contains("$>") == false)
            {
                try
                {
                    if (strNames.Contains(line.Split(' ')[1].Split('>').First()))
                    {
                        if (line.ToLower().Contains("$readline") == false)
                        {
                            strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = line.Split('>').Last();
                        }
                        else if (line.ToLower().Contains("$readline") == true)
                        {
                            strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Console.ReadLine();
                        }
                    }
                    else
                    {

                        if (line.ToLower().Contains("$readline") == false)
                        {
                            strNames.Add(line.Split(' ')[1].Split('>').First());
                            strValues.Add(line.Split('>').Last());
                        }
                        else if (line.ToLower().Contains("$readline") == true)
                        {
                            strNames.Add(line.Split(' ')[1].Split('>').First());
                            strValues.Add(Console.ReadLine());
                        }
                    }

                }

                catch
                {
                    int errorLine = code.IndexOf(line);
                    Console.WriteLine($"Invalid Syntax (Line {errorLine--})");
                }
            }

            if (line.Contains("list(str)") && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false)
            {
                List<List<string>> listsStr = new List<List<string>>();
            }






            //Write 

            if (line.ToLower().Contains("newline") || line.ToLower().Contains("newln"))
            {
                Console.WriteLine("");
            }
                if (line.Contains("Write") && line.Contains("&>") && line.Contains("<&") && line.Contains("WriteStr") == false && line.Contains("WriteNum") == false && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false)
                {
                    //check if it is a number 
                    var matchesNumber = numberNames.Where(x => line.Contains(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString()));

                    Console.Write(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString() + " ");

                }
                else if (line.Contains("WriteStr")
                    && line.Contains("&>")
                    && line.ToLower().StartsWith("for") == false
                    && line.ToLower().Contains("in range %") == false
                    && line.Contains("$>") == false)
                {
                    foreach (var name in strNames)
                    {
                        var namesToCheck = line.Split(' ');

                        foreach (var nametoCheck in namesToCheck)
                        {
                            try
                            {
                                if (nametoCheck == name || nametoCheck == "&>" + name || nametoCheck == name + "<&" || nametoCheck == "&>" + name + "<&")
                                {
                                    Console.Write(strValues[strNames.IndexOf(name)] + line.Split(new[] { name }, StringSplitOptions.None)[1].Split(new[] { "<&" }, StringSplitOptions.None).First());
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Invalid Syntax on Line " + code.IndexOf(line));
                            }
                        }
                    }

                }
                else if (line.Contains("WriteNum")
                    && line.Contains("&>")
                    && line.ToLower().StartsWith("for") == false
                    && line.ToLower().Contains("in range %") == false
                    && line.Contains("$>") == false)
                
                {
                    foreach (var name in numberNames)
                    {
                        var namesToCheck = line.Split(' ');

                        foreach (var nametoCheck in namesToCheck)
                        {
                            try
                            {
                                if (nametoCheck == name || nametoCheck == "&>" + name || nametoCheck == name + "<&" || nametoCheck == "&>" + name + "<&")
                                {
                                    Console.Write(numberValues[numberNames.IndexOf(name)] + line.Split(new[] { name }, StringSplitOptions.None)[1].Split(new[] { "<&" }, StringSplitOptions.None).First());
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Invalid Syntax on Line " + code.IndexOf(line));
                            }
                        
                    }
                
                }





                //save

            }
            else if (line.StartsWith("save(this)") && line.Contains("as") == false)
            {
                id = new Random().Next(1, 10000);
                StreamWriter writer = File.CreateText("code.id(" + id + ").rcode");
                foreach (var lineToSave in code)
                {
                        if (lineToSave.StartsWith("save"))
                        {
                            writer.WriteLine(lineToSave);
                        }
                }
                if (id != 0)
                {
                    Program.WriteId(id);
                }

                writer.Close();
            }
            else if (line.StartsWith("save(this)") && line.Contains("as") == true)
            {
                StreamWriter writerAs = File.CreateText(line.Split(new[] { "save(this) as " }, StringSplitOptions.None).Last() + ".rcode");
                    foreach (var lineToSave in code)
                    {
                        if (lineToSave.StartsWith("save") == false)
                        {
                            writerAs.WriteLine(lineToSave);
                        }
                    }
                    writerAs.Close();
                    Console.ResetColor();
                Console.Write("\nFile Saved as ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(line.Split(new[] { "save(this) as " }, StringSplitOptions.None).Last() + ".rcode");
                Console.ResetColor();
                Console.Write(" in order to edit it, please do so using a text editor or visual studio.\nIn order to compile it, use the command load >> " + (line.Split(new[] { "save(this) as" }, StringSplitOptions.None).Last() + ".rcode"));

            }
            else if (line == "save = false")
            {
                id = 0;
            }
            else if (line.ToLower().StartsWith("for") && line.ToLower().Contains("in range %") && line.Contains("$>"))
            {
                int range = 0;
                try
                {
                    range = Convert.ToInt32(line.ToLower().Split(new[] { "in range %" }, StringSplitOptions.None).Last().Split(new[] { " $>" }, StringSplitOptions.None).First());
                }
                catch
                {
                    try
                    {
                        range = Convert.ToInt32(numberValues[numberNames.IndexOf(line.ToLower().Split(new[] { "in range % " }, StringSplitOptions.None).Last().Split(new[] { " $>" }, StringSplitOptions.None).First())]);
                    }
                    catch
                    {
                        range = Convert.ToInt32(numberValues[numberNames.IndexOf(line.ToLower().Split(new[] { "in range %" }, StringSplitOptions.None).Last().Split(new[] { " $>" }, StringSplitOptions.None).First())]);
                    }
                }

                string looper = line.ToLower().Split(new[] { "for" }, StringSplitOptions.None).Last().Split(new[] { "in range %" }, StringSplitOptions.None).First();
                var getContent = line.Split(new[] { "$>" }, StringSplitOptions.None);
                List<string> loopContent = new List<string>();

                foreach (var content in getContent)
                {
                    if (content.ToLower().StartsWith("for") == false && content.ToLower().Contains("in range %") == false && content.Contains("$>") == false)
                    {
                        loopContent.Add(content);
                    }
                }
                ForLoop(range, looper, loopContent, numberNames, numberValues, strNames, strValues);
            }
            else if (line.StartsWith("load >>") || line.StartsWith("compiler.load"))
            {
                string fileToCompile = line.Split(new[] { "load >> " }, StringSplitOptions.None).Last();
                Console.WriteLine(fileToCompile);
                List<string> CompileFile = new List<string>();
                try
                {
                    StreamReader reader = new StreamReader(fileToCompile);
                    string lineReader;
                    while ((lineReader = reader.ReadLine()) != null)
                    {
                       CompileFile.Add(lineReader);
                    }
                    string isVisualizing;

                    Console.WriteLine("Do you want to Visualize it's code (Y/n): ");
                    isVisualizing = Console.ReadLine();
                    if (isVisualizing.ToLower() != "n")
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n " + fileToCompile + " (Code): \n");
                        Console.ResetColor();
                        foreach (var lineToShow in CompileFile)
                        {
                            Console.WriteLine(CompileFile.IndexOf(lineToShow) + " " + lineToShow);
                        }
                        Console.WriteLine("\n");
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n \n \n Output:");
                    Console.ResetColor();
                    Compile(CompileFile, numberNames, numberValues, strNames, strValues);
                    Console.WriteLine("");
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Couldn't find " + fileToCompile);
                    Console.ResetColor();
                }


            }else if(line.StartsWith("compile >> "))
                {
                    List<string> CompileFile = new List<string>();
                    string fileToCompile = line.Split(new [] { "compile >> "}, StringSplitOptions.None).Last();

                    try
                    {
                        StreamReader reader = new StreamReader(fileToCompile);
                        string lineReader;
                        while ((lineReader = reader.ReadLine()) != null)
                        {
                            CompileFile.Add(lineReader);
                        }
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("File Not Found / Compilation Error!");
                        Console.ResetColor();
                    }
                 }
            else if (line.StartsWith("if") && line.Contains("(") && line.Contains(")"))
            {
                string statement = line.Split(new[] { "if (" }, StringSplitOptions.None).Last().Split(')')[0];
                Console.WriteLine(statement);
            }

        }
            if(pixelColors.Count >= 1 && pixelX.Count >=1 && pixelY.Count >= 1)
            {
                foreach (var pixel in pixelColors)
                {
                    Pixel.Draw(pixelX[pixelColors.IndexOf(pixel)], pixelY[pixelColors.IndexOf(pixel)], pixel);
                }
            }
            if(pixelColorsChar.Count >=1 && pixelXChar.Count>=1 && pixelYChar.Count >= 1)
            {
                foreach (var pixel in pixelColorsChar)
                {
                    Pixel.DrawChar(pixelXChar[pixelColors.IndexOf(pixel)], pixelYChar[pixelColors.IndexOf(pixel)], pixel, charachtersToDraw[pixelColorsChar.IndexOf(pixel)]);
                }
            }
        Console.ResetColor();
            }
    //receive every variable;
    public static void ForLoop(int range,
        string looper,
        List<string> loopContent,
        List<string> numberNames,
        List<double> numberValues,
        List<string> strNames,
        List<string> strValues)
    {
        for (int x = 0; x < range; x++)
        {
            Compile(loopContent, numberNames, numberValues, strNames, strValues);
        }
    }

    }
} 

