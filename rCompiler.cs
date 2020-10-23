using System;
using System.Collections.Generic;
using System.IO;
using rC;
using System.Linq;
using System.Windows.Forms;

namespace rC
{
    public class rCompiler
    {
        //added with ubuntu
        //fix: 
        //TODO: MAKE TRY / CATCH STATEMENTS
        //new to docs == for // readline // color //num cursor_x >> set, cursor_y >> set;
        //write,read,delete,create file
        //numToStr(str>>num)
        public static void Compile(
            List<string> code,
            List<string> numberNames,
            List<double> numberValues,
            List<string> strNames,
            List<string> strValues,
            List<string> references)
        {

            //values and indicators

            int id = 0;
            List<int> pixelX = new List<int>();
            List<int> pixelY = new List<int>();
            List<ConsoleColor> pixelColors = new List<ConsoleColor>();
            List<string> charachtersToDraw = new List<string>();
            List<int> pixelXChar = new List<int>();
            List<int> pixelYChar = new List<int>();
            Random rand = new Random();
            List<ConsoleColor> pixelColorsChar = new List<ConsoleColor>();

            //read code line by line
            foreach (var line in code)
            {
               

                if(line.StartsWith("numToStr"))
                {

                    if (strNames.Contains(line.Split('>').First().Split('(').Last()) && numberNames.Contains(line.Split('>').Last().Split(',').Last().Split(')').First()))
                    {
                        strValues[strNames.IndexOf(line.Split('>').First().Split('(').Last())] = numberValues[numberNames.IndexOf(line.Split('>').Last().Split(')').First())].ToString();
                    }
                    else
                    {
                        strNames.Add((line.Split('>').First().Split('(').Last()));
                        try
                        {
                            strValues.Add(numberValues[numberNames.IndexOf(line.Split('>').Last().Split(')').First())].ToString());
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(line.Split('>').Last().Split(')').First() + " does not exist at Line With Index: " + code.IndexOf(line));
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    
                    }
                    
                }

                if (numberNames.Contains("screen_width"))
                {
                    numberValues[numberNames.IndexOf("screen_width")] = Console.WindowWidth;
                }
                else
                {
                    numberNames.Add("screen_width");
                    numberValues.Add(Console.WindowWidth);

                }
                if (numberNames.Contains("screen_height"))
                {
                    numberValues[numberNames.IndexOf("screen_height")] = Console.WindowHeight;
                }
                else
                {
                    numberNames.Add("screen_height");
                    numberValues.Add(Console.WindowHeight);
                }


                if (numberNames.Contains("cursor_x"))
                {
                    numberValues[numberNames.IndexOf("cursor_x")] = Console.CursorLeft;
                }
                else
                {
                    numberValues.Add(Console.CursorLeft);
                    numberNames.Add("cursor_x");
                }
                if (numberNames.Contains("cursor_y"))
                {
                    numberValues[numberNames.IndexOf("cursor_y")] = Console.CursorTop;
                }
                else
                {
                    numberValues.Add(Console.CursorTop);
                    numberNames.Add("cursor_y");
                }


                if (line.StartsWith("#") == false)
                {
                    if (line.StartsWith("cursor_x >>"))
                    {
                        try
                        {
                            numberValues[numberNames.IndexOf("cursor_x")] = Convert.ToInt32(line.Split('>').Last());
                        }
                        catch
                        {
                            numberValues[numberNames.IndexOf("cursor_x")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
                        }
                        Console.CursorLeft = Convert.ToInt32(numberValues[numberNames.IndexOf("cursor_x")]);
                    }
                    else if (line.StartsWith("screen_width >>"))
                    {
                        try
                        {
                            numberValues[numberNames.IndexOf("screen_width")] = Convert.ToInt32(line.Split('>').Last());
                        }
                        catch
                        {
                            numberValues[numberNames.IndexOf("screen_width")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
                        }
                        Console.WindowWidth = Convert.ToInt32(numberValues[numberNames.IndexOf("screen_width")]);
                    }
                    else if (line.StartsWith("screen_height >>"))
                    {
                        try
                        {
                            numberValues[numberNames.IndexOf("screen_height")] = Convert.ToInt32(line.Split('>').Last());
                        }
                        catch
                        {
                            numberValues[numberNames.IndexOf("screen_height")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
                        }
                        Console.WindowHeight = Convert.ToInt32(numberValues[numberNames.IndexOf("screen_height")]);
                    }
                    else if (line.StartsWith("cursor_y >>"))
                    {
                        try
                        {
                            numberValues[numberNames.IndexOf("cursor_y")] = Convert.ToInt32(line.Split('>').Last());
                        }
                        catch
                        {
                            numberValues[numberNames.IndexOf("cursor_y")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
                        }
                        Console.CursorTop = Convert.ToInt32(numberValues[numberNames.IndexOf("cursor_y")]);
                    }
                    if (line == "exit")
                    {
                        Environment.Exit(1);
                    }
                    else if (line.StartsWith("screen_width >>"))
                    {
                        try
                        {
                            numberValues[numberNames.IndexOf("screen_width")] = Convert.ToInt32(line.Split('>').Last());
                        }
                        catch
                        {
                            numberValues[numberNames.IndexOf("screen_width")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
                        }
                        Console.WindowWidth = Convert.ToInt32(numberValues[numberNames.IndexOf("screen_width")]);
                    }
                    else if (line.StartsWith("screen_height >>"))
                    {
                        try
                        {
                            numberValues[numberNames.IndexOf("screen_height")] = Convert.ToInt32(line.Split('>').Last());
                        }
                        catch
                        {
                            numberValues[numberNames.IndexOf("screen_height")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
                        }
                        Console.WindowHeight = Convert.ToInt32(numberValues[numberNames.IndexOf("screen_height")]);
                    }
                    else if (line.StartsWith("cursor_y >>"))
                    {
                        try
                        {
                            numberValues[numberNames.IndexOf("cursor_y")] = Convert.ToInt32(line.Split('>').Last());
                        }
                        catch
                        {
                            numberValues[numberNames.IndexOf("cursor_y")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
                        }
                        Console.CursorTop = Convert.ToInt32(numberValues[numberNames.IndexOf("cursor_y")]);
                    }
                    if (line == "exit")
                    {
                        Environment.Exit(1);
                    }


                    else if (line.ToLower().StartsWith("compile_lines_from_file"))
                    {
                        string fileToCompile = "";
                        try
                        {
                            int firstIndex = Convert.ToInt32(line.Split('(').Last().Split(',')[1]);
                            int lastIndex = Convert.ToInt32(line.Split('(').Last().Split(',').Last().Split(')').First());
                            firstIndex--;
                            fileToCompile = line.ToLower().Split(new[] { "file:" }, StringSplitOptions.None).Last().Split(',').First();

                            if (!File.Exists(fileToCompile + ".rcode"))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("ERROR: File Does Not Exist (Line " + code.IndexOf(line) + ")(" + line + ")");
                                Console.ResetColor();
                            }
                            else
                            {
                                StreamReader specificLine_Compiler = File.OpenText(fileToCompile + ".rcode");
                                string lineReading;
                                List<string> linesFromFile = new List<string>();
                                while ((lineReading = specificLine_Compiler.ReadLine()) != null)
                                {
                                    linesFromFile.Add(lineReading);
                                }
                                specificLine_Compiler.Close();
                                Compile(linesFromFile.GetRange(firstIndex, lastIndex - (firstIndex)), numberNames, numberValues, strNames, strValues, references);
                            }
                        }
                        catch
                        {
                            try
                            {
                                fileToCompile = line.ToLower().Split(new[] { "file:" }, StringSplitOptions.None).Last().Split(',').First();
                                if (line.Split('(').Last().Split(',').Last().Split(')').First().ToLower() == "all")
                                {
                                    StreamReader specificLine_Compiler = File.OpenText(fileToCompile + ".rcode");
                                    string lineReading;
                                    List<string> linesFromFile = new List<string>();
                                    while ((lineReading = specificLine_Compiler.ReadLine()) != null)
                                    {
                                        linesFromFile.Add(lineReading);
                                    }
                                    specificLine_Compiler.Close();
                                }
                            }
                            catch
                            {
                                try {

                                    StreamReader specificLine_Compiler = File.OpenText(fileToCompile + ".rcode");
                                    string lineReading;
                                    int firstIndex = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('(').Last().Split(',')[1])]);
                                    int lastIndex = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('(').Last().Split(',').Last().Split(')').First())]);
                                    firstIndex--;
                                    List<string> linesFromFile = new List<string>();
                                    while ((lineReading = specificLine_Compiler.ReadLine()) != null)
                                    {
                                        linesFromFile.Add(lineReading);
                                    }
                                    specificLine_Compiler.Close();
                                    Compile(linesFromFile.GetRange(firstIndex, lastIndex - (firstIndex)), numberNames, numberValues, strNames, strValues, references);
                                }catch{
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("INVALID SYNTAX IN LINE " + code.IndexOf(line) + "(" + line + ")");
                                Console.ResetColor();
                                }
                            }

                        }
                    }

                    else if (line.ToLower().StartsWith("compile_lines"))
                    {

                        try
                        {
                            int firstIndex = Convert.ToInt32(line.Split('(').Last().Split(',').First());
                            int lastIndex = Convert.ToInt32(line.Split('(').Last().Split(',').Last().Split(')').First());
                            firstIndex--;
                            List<string> newCompile = code;
                            Compile(newCompile.GetRange(firstIndex, (lastIndex - firstIndex)), numberNames, numberValues, strNames, strValues, references);
                        }
                        catch
                        {
                            if (line.Split('(').Last().Split(')').First().ToLower() == "all")
                            {
                                List<string> newCompile = code;
                                Compile(newCompile, numberNames, numberValues, strNames, strValues, references);
                            }
                            else
                            {
                                try {
                                    int firstIndex = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('(').Last().Split(',')[0])]);
                                    int lastIndex = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('(').Last().Split(',').Last().Split(')').First())]);
                                    firstIndex--;
                                    List<string> newCompile = code; 
                                    Compile(newCompile.GetRange(firstIndex, (lastIndex - firstIndex)), numberNames, numberValues, strNames, strValues, references);
                                }catch{
                                    
                                }
                            }
                        }
                    }

                    foreach (var num in numberNames)
                    {

                        if (line.StartsWith(num.ToString() + "++"))
                        {
                            double newNum = numberValues[numberNames.IndexOf(num)];
                            newNum++;
                            numberValues[numberNames.IndexOf(num)] = newNum;
                        }
                        else if (line.StartsWith(num.ToString() + "--"))
                        {
                            double newNum = numberValues[numberNames.IndexOf(num)];
                            newNum--;
                            numberValues[numberNames.IndexOf(num)] = newNum;
                        }
                        else if (line.StartsWith(num.ToString() + "+"))
                        {
                            try
                            {
                                double newNum = numberValues[numberNames.IndexOf(num)];
                                newNum = newNum + Convert.ToDouble(line.Split('+').Last());
                                numberValues[numberNames.IndexOf(num)] = newNum;
                            }
                            catch
                            {
                                try
                                {
                                    foreach (var getNum in numberNames)
                                    {
                                        if (line.Split('+').Last() == getNum)
                                        {
                                            double newNum = numberValues[numberNames.IndexOf(num)];
                                            newNum = newNum + numberValues[numberNames.IndexOf(getNum)];
                                            numberValues[numberNames.IndexOf(num)] = newNum;
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        if (line.StartsWith(num.ToString() + "-"))
                        {
                            try
                            {
                                numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] - Convert.ToDouble(line.Split('-').Last());
                            }
                            catch
                            {
                                try
                                {
                                    foreach (var getNum in numberNames)
                                    {
                                        if (line.Split('-').Last() == getNum)
                                        {
                                            numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] - numberValues[numberNames.IndexOf(getNum)];
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        if (line.StartsWith(num.ToString() + "/"))
                        {
                            try
                            {
                                numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] / Convert.ToDouble(line.Split('/').Last());

                            }
                            catch
                            {
                                try
                                {
                                    foreach (var getNum in numberNames)
                                    {
                                        if (line.Split('/').Last() == getNum)
                                        {
                                            numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] / numberValues[numberNames.IndexOf(getNum)];
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        if (line.StartsWith(num.ToString() + "*"))
                        {
                            try
                            {
                                numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] * Convert.ToDouble(line.Split('*').Last());
                            }
                            catch
                            {
                                try
                                {
                                    foreach (var getNum in numberNames)
                                    {
                                        if (line.Split('*').Last() == getNum)
                                        {
                                            numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] * numberValues[numberNames.IndexOf(getNum)];
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }


                    }
                    //to lower
                    /*USAGE
                    ___________________________________________________________________________________________________
                    str f >> 
                    str name >>$readline
                    toLower (f>>name)      <---- gets the value of name.toLower and assigns it to the variable f
                    toLower (name)    <---- gets the value of name.toLower and assigns it to itself
                    __________________________________________________________________________________________________*/
                    if (line.StartsWith("toLower (") && line.Contains(")") && line.Contains(">>"))
                    {
                        if (strNames.Contains(line.Split(new[] { "toLower (" }, StringSplitOptions.None).Last().Split(new[] { ">>" }, StringSplitOptions.None).First()))
                        {
                            if (strNames.Contains(line.Split(new[] { ">>" }, StringSplitOptions.None).Last().Split(')').First()))
                            {
                                strValues[strNames.IndexOf(line.Split(new[] { "toLower (" }, StringSplitOptions.None).Last().Split(new[] { ">>" }, StringSplitOptions.None).First())] = strValues[strNames.IndexOf(line.Split(new[] { ">>" }, StringSplitOptions.None).Last().Split(')').First())].ToLower();
                            }
                            else
                            {
                                strValues[strNames.IndexOf(line.Split(new[] { "toLower (" }, StringSplitOptions.None).Last().Split(new[] { ">>" }, StringSplitOptions.None).First())] = line.Split(new[] { ">>" }, StringSplitOptions.None).Last().Split(')').First().ToLower();
                            }
                        }

                    }
                    else if (line.StartsWith("toLower (") && line.Contains(")") && line.Contains(">>") == false)
                    {
                        try
                        {
                            strValues[strNames.IndexOf(line.Split(new[] { "toLower (" }, StringSplitOptions.None).Last().Split(')').First())] = strValues[strNames.IndexOf(line.Split(new[] { "toLower (" }, StringSplitOptions.None).Last().Split(')').First())].ToLower();
                        }
                        catch
                        {

                        }
                    }


                    //toUpper


                    foreach (var str in strNames)
                    {
                        if (line.StartsWith("str (" + str) && line.Contains('+') && line.Contains(')') || line.StartsWith("str(" + str) && line.Contains('+') && line.Contains(')'))
                        {
                            if (line.Split('+')[1].Split(')').First() != "$readline" || str != "$readline")
                            {
                                try
                                {
                                    strValues[strNames.IndexOf(str)] = strValues[strNames.IndexOf(str)] + strValues[strNames.IndexOf(line.Split('+')[1].Split(')').First())];
                                }
                                catch
                                {
                                    strValues[strNames.IndexOf(str)] = strValues[strNames.IndexOf(str)] + line.Split('+')[1].Split(')').First();
                                }
                            }
                            else if (str == "$readline")
                            {
                                try
                                {
                                    strValues[strNames.IndexOf(line.Split('+')[1].Split(')').First())] = Console.ReadLine() + strValues[strNames.IndexOf(line.Split('+')[1].Split(')').First())];
                                }
                                catch
                                {

                                }
                            }
                            else if (line.Split('+')[1].Split(')').First() == "$readline")
                            {
                                strValues[strNames.IndexOf(str)] = strValues[strNames.IndexOf(str)] + Console.ReadLine();
                            }
                        }
                    }

                    if (line.ToLower() == "clear")
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
                        else if (line.ToLower() == "import filestream")
                        {
                            references.Add("filestream");
                        }else if(line.ToLower() == "import keys")
                        {
                            references.Add("keys");
                        }
                    }


                    if (references.Contains("pixel"))
                    {
                        if (line.ToLower().StartsWith("pixel.draw"))
                        {
                            
                                //add to normal pixel



                                try
                                {
                                    pixelX.Add(Convert.ToInt32(line.Split(new[] { "x:" }, StringSplitOptions.None).Last().Split(' ').First()));
                                }
                                catch
                                {
                                    pixelX.Add(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new[] { "x:" }, StringSplitOptions.None).Last().Split(' ').First())]));
                                }
                                try
                                {
                                    pixelY.Add(Convert.ToInt32(line.Split(new[] { "y:" }, StringSplitOptions.None).Last().Split(' ').First()));
                                }
                                catch
                                {
                                    pixelY.Add(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new[] { "y:" }, StringSplitOptions.None).Last().Split(' ').First())]));
                                }
                                string color = line.ToLower().Split(new[] { "color:" }, StringSplitOptions.None).Last().Split(' ').First();


                                if (color == "white")
                                {
                                    pixelColors.Add(ConsoleColor.White);
                                }
                                else if (color == "red")
                                {
                                    pixelColors.Add(ConsoleColor.Red);
                                }
                                else if (color == "blue")
                                {
                                    pixelColors.Add(ConsoleColor.Blue);
                                }
                                else if (color == "magenta")
                                {
                                    pixelColors.Add(ConsoleColor.Magenta);
                                }
                                else if (color == "green")
                                {
                                    pixelColors.Add(ConsoleColor.Green);
                                }
                                else if (color == "yellow")
                                {
                                    pixelColors.Add(ConsoleColor.Yellow);
                                }

                            
                           
                        }
                        if (line.ToLower().StartsWith("pixel.drawchar"))
                        {
                            try
                            {
                                //add to char
                                try
                                {
                                    pixelXChar.Add(Convert.ToInt32(line.Split(new[] { "x:" }, StringSplitOptions.None).Last().Split(' ').First()));
                                }
                                catch
                                {
                                    pixelXChar.Add(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new[] { "x:" }, StringSplitOptions.None).Last().Split(' ').First())]));
                                }
                                try
                                {
                                    pixelYChar.Add(Convert.ToInt32(line.Split(new[] { "y:" }, StringSplitOptions.None).Last().Split(' ').First()));
                                }
                                catch
                                {
                                    pixelYChar.Add(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new[] { "y:" }, StringSplitOptions.None).Last().Split(' ').First())]));
                                }
                                string color = line.Split(new[] { "color:" }, StringSplitOptions.None).Last().Split(' ').First();
                                string characterToDraw = line.Split(new[] { "char:" }, StringSplitOptions.None).Last().Split(' ').First();
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
                                else if (color == "magenta")
                                {
                                    pixelColorsChar.Add(ConsoleColor.Magenta);
                                }
                                else if (color == "green")
                                {
                                    pixelColorsChar.Add(ConsoleColor.Green);
                                }
                                else if (color == "yellow")
                                {
                                    pixelColorsChar.Add(ConsoleColor.Yellow);
                                }


                            }
                            catch
                            {
                                Console.WriteLine("Invalid Syntax In Line " + code.IndexOf(line));
                            }
                        }
                    }
                    if (references.Contains("keys"))
                    {
                        if (line.StartsWith("SendKeys"))
                        {
                            if (strNames.Contains(line.Split(new[] { "SendKeys" }, StringSplitOptions.None).Last().Split('(').Last().Split(')').First()))
                            {
                                SendKeys.Send(strValues[strNames.IndexOf(line.Split(new[] { "SendKeys" }, StringSplitOptions.None).Last().Split('(').Last().Split(')').First())]);

                            }
                            else
                            {
                                SendKeys.Send(strValues[strNames.IndexOf(line.Split(new[] { "SendKeys" }, StringSplitOptions.None).Last().Split('(').Last().Split(')').First())]);
                            }
                        }
                    }
                    if(references.Contains("filestream"))
                    {
                        //WriteFile file:filename content: Writing 
                        //CreateFile file:filename
                        //DeleteFile file.filename
                        //todo ReadFile
                        if(line.StartsWith("WriteToFile"))
                        {
                            string filename = line.Split(new [] {"file:"},StringSplitOptions.None).Last().Split(' ').First();
                            if(strNames.Contains(filename)){
                                filename = strValues[strNames.IndexOf(filename)];
                            }
                            string content = line.Split(new [] {"content:"}, StringSplitOptions.None).Last();
                            if(strNames.Contains(content) == false)
                            {
                                FileStream.WriteToFile(filename,content);
                            }else
                            {
                                FileStream.WriteToFile(filename,strValues[strNames.IndexOf(content)]);
                            }
                        }
                        else if(line.StartsWith("DeleteFile"))
                        {
                            string filename = line.Split(new [] {"file:"},StringSplitOptions.None).Last().Split(' ').First();
                            if(strNames.Contains(filename))
                            {
                                filename = strValues[strNames.IndexOf(filename)];
                            }
                            FileStream.DeleteFile(filename);
                        }else if(line.StartsWith("CreateFile"))
                        {
                            string filename = line.Split(new [] {"file:"},StringSplitOptions.None).Last().Split(' ').First();
                            if(strNames.Contains(filename))
                            {
                                filename = strValues[strNames.IndexOf(filename)];
                            }
                            FileStream.CreateFile(filename);
                        }else if(line.StartsWith("ReadFile"))
                        {
                            string filename = line.Split(new [] {"file:"},StringSplitOptions.None).Last().Split(' ').First();
                            string strToReadTo = line.Split(new [] {"str:"},StringSplitOptions.None).Last().Split(' ').First();
                            if(strNames.Contains(filename))
                            {
                                filename = strValues[strNames.IndexOf(filename)];
                            }
                            if(strNames.Contains(strToReadTo))
                            {
                                strValues[strNames.IndexOf(strToReadTo)] =  FileStream.ReadFile(filename);;
                            }else{
                                strValues.Add(FileStream.ReadFile(filename));
                                strNames.Add(strToReadTo);
                            }
                           
                        }
                    }
                    
                    if (line.ToLower().StartsWith("sleep >>"))
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(Convert.ToInt32(line.Split('>').Last()));
                        }
                        catch
                        {
                            try
                            {
                                System.Threading.Thread.Sleep(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('>').Last())]));
                            }
                            catch
                            {
                                System.Threading.Thread.Sleep(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new[] { "> " }, StringSplitOptions.None).Last())]));
                            }
                        }
                    }
                    //color indicators
                    if (line.ToLower().StartsWith("color.reset"))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (line.ToLower().StartsWith("color.green"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (line.ToLower().StartsWith("color.blue"))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (line.ToLower().StartsWith("color.red"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (line.ToLower().StartsWith("color.magenta"))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else if (line.ToLower().StartsWith("color.yellow"))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    if (line.ToLower() == ("$readline"))
                    {
                        Console.ReadLine();
                    }

                    if (line.StartsWith("number ") || line.StartsWith("num ") && line.Contains(">>")
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
                                        if (line.Split('>').Last().Contains("rand:"))
                                        {
                                            numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = rand.Next(Convert.ToInt32(line.Split(new[] { "rand:" }, StringSplitOptions.None).Last().Split(',').First()), Convert.ToInt32(line.Split(new[] { "rand:" }, StringSplitOptions.None).Last().Split(',').Last()));
                                        }
                                        else
                                        {
                                            numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Convert.ToDouble(line.Split('>').Last());
                                        }
                                    }
                                    catch
                                    {
                                        if (line.Split('>').Last().Contains("rand:"))
                                        {
                                            numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = rand.Next(Convert.ToInt32(line.Split(new[] { "rand:" }, StringSplitOptions.None).Last().Split(',').First()), Convert.ToInt32(line.Split(new[] { "rand:" }, StringSplitOptions.None).Last().Split(',').Last()));
                                        }
                                        else
                                        {
                                            numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
                                        }
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
                                        if (line.Split('>').Last().Contains("rand:"))
                                        {
                                            numberValues.Add(rand.Next(Convert.ToInt32(line.Split(new[] { "rand:" }, StringSplitOptions.None).Last().Split(',').First()), Convert.ToInt32(line.Split(new[] { "rand:" }, StringSplitOptions.None).Last().Split(',').Last())));
                                        }
                                        else
                                        {
                                            try
                                            {
                                                numberValues.Add(Convert.ToDouble(line.Split('>').Last().Split(' ').Last()));
                                            }
                                            catch
                                            {

                                                numberValues.Add(numberValues[numberNames.IndexOf(line.Split('>').Last())]);


                                            }
                                        }
                                    }
                                    catch
                                    {
                                        if (line.Split('>').Last().Contains("rand:"))
                                        {
                                            numberValues.Add(rand.Next(Convert.ToInt32(line.Split(new[] { "rand:" }, StringSplitOptions.None).Last().Split(',').First()), Convert.ToInt32(line.Split(new[] { "rand:" }, StringSplitOptions.None).Last().Split(',').Last())));
                                        }
                                        {
                                            numberValues.Add(Convert.ToDouble(line.Split('>').Last()));
                                        }
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
                    if (line.StartsWith("str")
                        && line.Contains(">>")
                        && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false
                        && line.Contains("$>") == false)
                    {
                        try
                        {
                            if (strNames.Contains(line.Split(' ')[1].Split('>').First()))
                            {
                                if (line.Split('>').Last().ToLower().StartsWith("$read") == false)
                                {
                                    strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = line.Split('>').Last();
                                }
                                else if (line.Split('>').Last().ToLower() == "$readkey")
                                {
                                    strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Console.ReadKey().Key.ToString();
                                }
                                else if (line.Split('>').Last().ToLower() == "$readline")
                                {
                                    strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Console.ReadLine();
                                }
                            }
                            else
                            {

                                if (line.Split('>').Last().ToLower().StartsWith("$read") == false)
                                {
                                    strNames.Add(line.Split(' ')[1].Split('>').First());
                                    strValues.Add(line.Split('>').Last());
                                }
                                else if (line.Split('>').Last().ToLower() == "$readkey")
                                {
                                    strNames.Add(line.Split(' ')[1].Split('>').First());
                                    strValues.Add(Console.ReadKey().Key.ToString());
                                }
                                else if (line.Split('>').Last().ToLower() == ("$readline"))
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





                    //Write 

                    if (line.ToLower().StartsWith("newline") || line.ToLower().StartsWith("newln"))
                    {
                        Console.WriteLine("");
                    }
                    if (line.StartsWith("Write") && line.Contains("&>") && line.Contains("<&") && line.Contains("WriteStr") == false && line.Contains("WriteNum") == false && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false)
                    {
                        //check if it is a number 
                        //var matchesNumber = numberNames.Where(x => line.Contains(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString()));

                        Console.Write(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString());

                    }
                    else if (line.StartsWith("WriteStr")
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
                    else if (line.StartsWith("WriteNum")
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

                        string looper = line.ToLower().Split(new[] { "for " }, StringSplitOptions.None).Last().Split(new[] { " in range %" }, StringSplitOptions.None).First();
                        var getContent = line.Split(new[] { "$>" }, StringSplitOptions.None);
                        List<string> loopContent = new List<string>();

                        foreach (var content in getContent)
                        {
                            if (content.ToLower().StartsWith("for") == false && content.ToLower().Contains("in range %") == false && content.Contains("$>") == false)
                            {
                                loopContent.Add(content);
                            }
                        }
                        ForLoop(range, looper, loopContent);
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
                            Compile(CompileFile, numberNames, numberValues, strNames, strValues, references);
                            Console.WriteLine("");
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ERROR: Couldn't find " + fileToCompile);
                            Console.ResetColor();
                        }


                    }
                    else if (line.StartsWith("compile >>"))
                    {
                        List<string> CompileFile = new List<string>();
                        string fileToCompile = line.Split(new[] { ">> " }, StringSplitOptions.None).Last();

                        try
                        {
                            StreamReader reader = new StreamReader(fileToCompile);
                            string lineReader;
                            while ((lineReader = reader.ReadLine()) != null)
                            {
                                CompileFile.Add(lineReader);
                            }
                            Compile(CompileFile, numberNames, numberValues, strNames, strValues, references);
                            Console.Write("\n");
                            reader.Close();
                        }
                        catch
                        {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("File Not Found / Compilation Error! \n(Make Sure You Leave a Space Between the indicators (>>) ex: compile >> filename.rcode");
                            Console.ResetColor();
                        }
                    }
                    else if (line.StartsWith("if") && line.Contains("(") && line.Contains(")"))
                    {
                        string statement = line.Split(new[] { "if (" }, StringSplitOptions.None).Last().Split(')')[0];
                        var getContent = line.Split(new[] { "!>" }, StringSplitOptions.None);
                        List<string> loopContent = new List<string>();

                        foreach (var content in getContent)
                        {
                            if (content.ToLower().StartsWith("if") == false && content.ToLower().Contains("(") == false && content.Contains("!>") == false)
                            {
                                loopContent.Add(content);
                            }
                        }

                        if (line.Contains("str(") && line.Contains("=="))
                        {
                            try
                            {
                                if (strValues[strNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "str(" }, StringSplitOptions.None).Last().Split(new[] { "==" }, StringSplitOptions.None).First())]
                                    == strValues[strNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "==" }, StringSplitOptions.None).Last().Split(')').First())])
                                {
                                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references);
                                }
                                else
                                {
                                    if (line.Contains("&else"))
                                    {
                                        string newStatement = line.Split(new[] { "&else" }, StringSplitOptions.None).Last();
                                        var getElseContent = newStatement.Split(new[] { "->" }, StringSplitOptions.None);
                                        List<string> loopElseContent = new List<string>();
                                        foreach (var content in getElseContent)
                                        {
                                            if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false)
                                            {
                                                loopElseContent.Add(content);
                                            }
                                        }
                                        Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references);
                                    }
                                }
                            }
                            catch
                            {
                            }

                        }/*else if(line.Contains("str(") && line.Contains("contains("))
                        {
                            if(strValues[strNames.Contains("")])
                        }*/
                        else if (line.Contains("str(") && line.Contains("!="))
                        {
                            try
                            {
                                if (strValues[strNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "str(" }, StringSplitOptions.None).Last().Split(new[] { "!=" }, StringSplitOptions.None).First())]
                                    != strValues[strNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "!=" }, StringSplitOptions.None).Last().Split(')').First())])
                                {
                                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references);
                                }
                                else
                                {
                                    if (line.Contains("&else"))
                                    {
                                        string newStatement = line.Split(new[] { "&else" }, StringSplitOptions.None).Last();
                                        var getElseContent = newStatement.Split(new[] { "->" }, StringSplitOptions.None);
                                        List<string> loopElseContent = new List<string>();
                                        foreach (var content in getElseContent)
                                        {
                                            if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false)
                                            {
                                                loopElseContent.Add(content);
                                            }
                                        }
                                        Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references);
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        if (line.Contains("num(") && line.Contains("=="))
                        {
                            try
                            {
                                if (numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "num(" }, StringSplitOptions.None).Last().Split(new[] { "==" }, StringSplitOptions.None).First())]
                                    == numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "==" }, StringSplitOptions.None).Last().Split(')').First())])
                                {
                                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references);
                                }
                                else
                                {
                                    if (line.Contains("&else"))
                                    {
                                        string newStatement = line.Split(new[] { "&else" }, StringSplitOptions.None).Last();
                                        var getElseContent = newStatement.Split(new[] { "->" }, StringSplitOptions.None);
                                        List<string> loopElseContent = new List<string>();
                                        foreach (var content in getElseContent)
                                        {
                                            if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false)
                                            {
                                                loopElseContent.Add(content);
                                            }
                                        }
                                        Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references);
                                    }
                                }
                            }
                            catch
                            {
                            }

                        }
                        else if (line.Contains("num(") && line.Contains("!="))
                        {
                            try
                            {
                                if (numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "num(" }, StringSplitOptions.None).Last().Split(new[] { "!=" }, StringSplitOptions.None).First())]
                                    != numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "!=" }, StringSplitOptions.None).Last().Split(')').First())])
                                {
                                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references);
                                }
                                else
                                {
                                    if (line.Contains("&else"))
                                    {
                                        string newStatement = line.Split(new[] { "&else" }, StringSplitOptions.None).Last();
                                        var getElseContent = newStatement.Split(new[] { "->" }, StringSplitOptions.None);
                                        List<string> loopElseContent = new List<string>();
                                        foreach (var content in getElseContent)
                                        {
                                            if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false)
                                            {
                                                loopElseContent.Add(content);
                                            }
                                        }
                                        Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references);
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        else if (line.Contains("num(") && line.Contains(">="))
                        {
                            try
                            {
                                if (numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "num(" }, StringSplitOptions.None).Last().Split(new[] { ">=" }, StringSplitOptions.None).First())]
                                    >= numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { ">=" }, StringSplitOptions.None).Last().Split(')').First())])
                                {
                                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references);
                                }
                                else
                                {
                                    if (line.Contains("&else"))
                                    {
                                        string newStatement = line.Split(new[] { "&else" }, StringSplitOptions.None).Last();
                                        var getElseContent = newStatement.Split(new[] { "->" }, StringSplitOptions.None);
                                        List<string> loopElseContent = new List<string>();
                                        foreach (var content in getElseContent)
                                        {
                                            if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false)
                                            {
                                                loopElseContent.Add(content);
                                            }
                                        }
                                        Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references);
                                    }
                                }
                            }
                            catch
                            {
                            }

                        }
                        else if (line.Contains("num(") && line.Contains("<="))
                        {
                            try
                            {
                                if (numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "num(" }, StringSplitOptions.None).Last().Split(new[] { "<=" }, StringSplitOptions.None).First())]
                                    <= numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "<=" }, StringSplitOptions.None).Last().Split(')').First())])
                                {
                                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references);
                                }
                                else
                                {
                                    if (line.Contains("&else"))
                                    {
                                        string newStatement = line.Split(new[] { "&else" }, StringSplitOptions.None).Last();
                                        var getElseContent = newStatement.Split(new[] { "->" }, StringSplitOptions.None);
                                        List<string> loopElseContent = new List<string>();
                                        foreach (var content in getElseContent)
                                        {
                                            if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false)
                                            {
                                                loopElseContent.Add(content);
                                            }
                                        }
                                        Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references);
                                    }
                                }
                            }
                            catch
                            {
                            }

                        }
                        else if (line.Contains("num(") && line.Contains("+>"))
                        {
                            try
                            {
                                if (numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "num(" }, StringSplitOptions.None).Last().Split(new[] { "+>" }, StringSplitOptions.None).First())]
                                    > numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "+>" }, StringSplitOptions.None).Last().Split(')').First())])
                                {
                                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references);
                                }
                                else
                                {
                                    if (line.Contains("&else ->"))
                                    {
                                        string newStatement = line.Split(new[] { "&else" }, StringSplitOptions.None).Last();
                                        var getElseContent = newStatement.Split(new[] { "->" }, StringSplitOptions.None);
                                        List<string> loopElseContent = new List<string>();
                                        foreach (var content in getElseContent)
                                        {
                                            if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false)
                                            {
                                                loopElseContent.Add(content);
                                            }
                                        }

                                        Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references);


                                    }
                                }
                            }
                            catch
                            {
                            }

                        }
                        else if (line.Contains("num(") && line.Contains("<-"))
                        {
                            try
                            {
                                if (numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "num(" }, StringSplitOptions.None).Last().Split(new[] { "<-" }, StringSplitOptions.None).First())]
                                    < numberValues[numberNames.IndexOf(line.Split(new[] { "&else" }, StringSplitOptions.None).First().Split(new[] { "num(" }, StringSplitOptions.None).Last().Split(new[] { "<-" }, StringSplitOptions.None).Last().Split(')').First())])
                                {
                                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references);
                                }
                                else
                                {
                                    if (line.Contains("&else ->"))
                                    {
                                        string newStatement = line.Split(new[] { "&else" }, StringSplitOptions.None).Last();
                                        var getElseContent = newStatement.Split(new[] { "->" }, StringSplitOptions.None);
                                        List<string> loopElseContent = new List<string>();
                                        foreach (var content in getElseContent)
                                        {
                                            if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false)
                                            {
                                                loopElseContent.Add(content);
                                            }
                                        }
                                        Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references);
                                    }
                                }

                            }
                            catch
                            {
                            }

                        }


                    }


                    /* foreach (var item in numberValues)
                     {
                         Console.WriteLine(item);
                     }
                     foreach (var d in numberNames)
                     {
                         Console.WriteLine(d);
                     }*/
                    //  StoreValues.Store();

                }
                if (pixelColors.Count >= 1 && pixelX.Count >= 1 && pixelY.Count >= 1)
                {
                   for(int i=0;i<pixelColors.Count;i++){
                        Pixel.Draw(pixelX[i], pixelY[i], pixelColors[i]);
                   }
                }
                if (pixelColorsChar.Count >= 1 && pixelXChar.Count >= 1 && pixelYChar.Count >= 1)
                {
                    for(int j=0;j<pixelColorsChar.Count;j++){
                        Pixel.DrawChar(pixelXChar[j], pixelYChar[j], pixelColorsChar[j], charachtersToDraw[j]);
                   }
                }
            }
            Console.ResetColor();
            void ForLoop(int range,
            string looper, List<string> loopContent)

            {

                for (int x = 0; x < range; x++)
                {
                    if (numberNames.Contains(looper))
                    {
                        numberValues[numberNames.IndexOf(looper)] = x;
                    }
                    else
                    {
                        numberValues.Add(x);
                        numberNames.Add(looper);
                    }
                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references);
                }
            }
        }
        //receive every variable;



    }
}