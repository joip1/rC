using System;
using System.Collections.Generic;
using System.IO;
using rC;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace rC {
  public class rCompiler {
    //added with ubuntu
    //fix: 
    //TODO: MAKE TRY / CATCH STATEMENTS
    //new to docs == for // readline // color //num cursor_x >> set, cursor_y >> set;
    //write,read,delete,create file
    //numToStr(str>>num)
    //TODO - add import files
    public static void Compile(
      List < string > code,
      List < string > numberNames,
      List < double > numberValues,
      List < string > strNames,
      List < string > strValues,
      List < string > references,
      List < string > strListNames,
      List < List < string >> strListValues,
      List < string > numListNames,
      List < List < double >> numListValues,
      List < List < string >> lines_for_functions,
      List < string > names_for_functions) {

      //values and indicators
      int id = 0;
      List < int > pixelX = new List < int > ();
      List < int > pixelY = new List < int > ();
      List < ConsoleColor > pixelColors = new List < ConsoleColor > ();
      List < string > charachtersToDraw = new List < string > ();
      List < int > pixelXChar = new List < int > ();
      List < int > pixelYChar = new List < int > ();
      Random rand = new Random();
      List < ConsoleColor > pixelColorsChar = new List < ConsoleColor > ();
      Stopwatch execTime = new Stopwatch();
      //read code line by line
      execTime.Start();

      try {
        //list(str) "Name"
        //str Name.Add >> "Value"
        //getStrListValue(Name[1])
        //updateStrListValue(Name[1])

        foreach(var line in code) {
        /*  foreach(var func_name in names_for_functions) {
            int ocorrences = 0;
            foreach(var func_name_2 in names_for_functions){
              if(func_name_2 == func_name){
                ocorrences++;
              }
            }
            if(ocorrences>=2){
                Console.WriteLine("There is more than 1 function named: "+func_name);
            }*/
            if (line.StartsWith(func_name + "(") || line.StartsWith(func_name + " (")) {
              // foreach(var lineofcode in lines_for_functions[names_for_functions.IndexOf(func_name)]){
              //     Console.WriteLine(lineofcode);
              // }
              Compile(lines_for_functions[names_for_functions.IndexOf(func_name)], numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
            }
          }
          if (references.Contains("threading") && line.StartsWith("newThread(")) {
            try {
              string nameof_func = line.Split('(')[1].Split('(')[0];
              void newThreadStart() {
                rCompiler.Compile(lines_for_functions[names_for_functions.IndexOf(nameof_func)], numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                Console.WriteLine("");
              }

              ThreadStart thread_start = new ThreadStart(newThreadStart);
              Thread newThread = new Thread(thread_start);
              System.Threading.Thread.Sleep(15);
              newThread.Start();
              while (newThread.IsAlive != true) {
                newThread.Start();
              }
            } catch {
              Console.WriteLine($"{line} : Error Starting Thread");
            }

          }

          if (line.StartsWith("function ")) {
            //function main(str test);{
            //    Write "Im da best"
            //}main;
            string nameFunc = line.Split(new [] {
              "function "
            }, StringSplitOptions.None).Last().Split('(').First();
            names_for_functions.Add(nameFunc);
            List < string > func_content = code;
            List < string > compileAfter = new List < string > ();
            int current_index = code.IndexOf(line);
            current_index++;
            func_content = code.GetRange(current_index, (code.IndexOf("}" + nameFunc + ";") - current_index));

            string indent = "    ";

            for (int i = 0; i < func_content.Count; i++) {
              if (func_content[i].StartsWith(indent) /*&&func_content[i].StartsWith(indent+indent)==false*/ ) {
                try {
                  func_content[func_content.IndexOf(func_content[i])] = func_content[i].Split(new [] {
                    indent
                  }, StringSplitOptions.None)[1];
                } catch {}
              }
            }

            // foreach(var codeline in func_content){
            //     Console.WriteLine(codeline);
            // }
            lines_for_functions.Add(func_content);
            Compile(compileAfter, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);

          }
          if (line.ToLower().StartsWith("for") && line.ToLower().Contains("in range:")) {
            int range = 0;
            /*
                                     for x in range:

                                 */
            string looper = "";
            string indent = "";
            string name = "";
            List < string > loopContent = new List < string > ();
            List < string > compileAfter = new List < string > ();
            try {
              looper = line.Split(new [] {
                "for "
              }, StringSplitOptions.None).Last().Split(' ').First();

              try {
                range = Convert.ToInt32(line.Split(new [] {
                  "in range:"
                }, StringSplitOptions.None).Last().Split(';')[0]);
              } catch {
                try {
                  range = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                    "in range:"
                  }, StringSplitOptions.None).Last().Split(';')[0])]);
                } catch {
                  range = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                    "in range:"
                  }, StringSplitOptions.None).Last().Split(';')[0])]);
                }
              }
              name = line.Split(new [] {
                "name:"
              }, StringSplitOptions.None).Last().Split(';').First();
              indent = line.Split(new [] {
                "indent:"
              }, StringSplitOptions.None).Last().Split('"')[1].Split('"').First().Split(';').First();
            } catch {
              Console.WriteLine("Invalid Syntax Line: " + code.IndexOf(line));
            }
            List < string > newCode1 = code;

            if (newCode1.Contains("}" + name + ";")) {
              newCode1.RemoveRange(0, newCode1.IndexOf(line) + 1);
              foreach(var lineofCode in newCode1) {
                if (newCode1.IndexOf(lineofCode) > newCode1.IndexOf("}" + name + ";")) {
                  compileAfter.Add(lineofCode);
                }
              }

              newCode1.RemoveRange(newCode1.IndexOf("}" + name + ";"), newCode1.Count - newCode1.IndexOf("}" + name + ";"));
              loopContent = newCode1;
            } else {
              Console.WriteLine("\nNo End Was Found for For Loop with name: " + name);
            }
            for (int i = 0; i < newCode1.Count; i++) {
              if (newCode1[i].StartsWith(indent)) {
                try {
                  newCode1[newCode1.IndexOf(newCode1[i])] = newCode1[i].Split(new [] {
                    indent
                  }, StringSplitOptions.None)[1];
                } catch {}
              }
            }

            ForLoop(range, looper, loopContent);
            Compile(compileAfter, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
          }
          if (line.StartsWith("strip(")) {
            string toStrip = strValues[strNames.IndexOf(line.Split('(').Last().Split(')').First())];
            string cleaned = toStrip.Replace("\r", "").Replace("\n", "");
            strValues[strNames.IndexOf(line.Split('(').Last().Split(')').First())] = cleaned;
          }

          if (line.StartsWith("strToNum(")) {
            string toConvert = strValues[strNames.IndexOf(line.Split('(').Last().Split(')').First())];
            if (numberNames.Contains(line.Split('(').Last().Split(')').First())) {
              numberValues[numberNames.IndexOf(line.Split('(').Last().Split(')').First())] = Convert.ToDouble(toConvert);
            } else {
              numberNames.Add(line.Split('(').Last().Split(')').First());
              numberValues.Add(Convert.ToDouble(toConvert));
            }
          }

          if (line.StartsWith("list(str).ToCharArray:")) {
            try {
              string stringFrom = strValues[strNames.IndexOf(line.Split(new [] {
                "ToCharArray:"
              }, StringSplitOptions.None).Last().Split(';').First())];
              var ListToAssign = strListValues[strListNames.IndexOf(line.Split(new [] {
                "to:"
              }, StringSplitOptions.None).Last().Split(';').First())];
              foreach(var character in stringFrom) {
                string characterStr = character.ToString();
                ListToAssign.Add(characterStr);
              }
            } catch {
              Console.WriteLine("Fatal error, line: " + code.IndexOf(line));
            }
          }
          if (line.ToLower().StartsWith("updatestrlistvalue")) {
            //updateStrListValue(MyList[x])
            try {
              string stringToChange = strValues[strNames.IndexOf(line.Split('(')[1].Split(')')[0])];
              var listToChange = strListValues[strListNames.IndexOf(line.Split('(')[1].Split('[')[0])];
              if (numberNames.Contains(line.Split('[')[1].Split(']')[0]) == false) {
                listToChange[Convert.ToInt32(line.Split('[')[1].Split(']')[0])] = stringToChange;
              } else {
                listToChange[Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('[')[1].Split(']')[0])])] = stringToChange;
              }
            } catch {
              Console.WriteLine("Fatal error on line: " + code.IndexOf(line));
            }
          }
          if (line.ToLower().StartsWith("updatenumlistvalue")) {
            //updateStrListValue(MyList[x])
            try {
              double stringToChange = numberValues[numberNames.IndexOf(line.Split('(')[1].Split(')')[0])];
              var listToChange = numListValues[numListNames.IndexOf(line.Split('(')[1].Split('[')[0])];
              if (numberNames.Contains(line.Split('[')[1].Split(']')[0]) == false) {
                listToChange[Convert.ToInt32(line.Split('[')[1].Split(']')[0])] = stringToChange;
              } else {
                listToChange[Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('[')[1].Split(']')[0])])] = stringToChange;
              }
            } catch {
              Console.WriteLine("Fatal error on line: " + code.IndexOf(line));
            }
          }

          if (line.StartsWith("list(str) ")) {
            strListNames.Add(line.Split(')').Last().Split('\"')[1].Split('\"').First());
            strListValues.Add(new List < string > ());
          }
          if (line.StartsWith("list(num)")) {
            numListNames.Add(line.Split(')').Last().Split('\"')[1].Split('\"').First());
            numListValues.Add(new List < double > ());
          }
          foreach(var listName in strListNames) {

            if (line.StartsWith(listName + ".IndexOf:")) {
              try {
                string stringToGetIndex = "";
                //MyList.IndexOf:"Something"; to:num
                if (line.Contains('"')) {
                  stringToGetIndex = line.Split(new [] {
                    "IndexOf:"
                  }, StringSplitOptions.None).Last().Split('"')[1].Split('"').First().Split(';').First();
                } else {
                  stringToGetIndex = strValues[strNames.IndexOf(line.Split(new [] {
                    "IndexOf:"
                  }, StringSplitOptions.None).Last().Split(';').First())];
                }
                numberValues[numberNames.IndexOf(line.Split(new [] {
                  "to:"
                }, StringSplitOptions.None).Last().Split(';').First())] = strListValues[strListNames.IndexOf(listName)].IndexOf(stringToGetIndex);
              } catch {
                Console.WriteLine("Fatal Error, Line: " + code.IndexOf(line));
              }

            }

            foreach(var name in strNames) {
              if (name.ToLower().StartsWith(listName.ToLower() + ".add")) {
                if (strListValues[strListNames.IndexOf(listName)].Contains(strValues[strNames.IndexOf(name)]) == false) {
                  strListValues[strListNames.IndexOf(listName)].Add(strValues[strNames.IndexOf(name)]);
                }
              }
            }
          }
          foreach(var listName in numListNames) {
            if (line.StartsWith(listName + ".IndexOf:")) {
              try {
                string stringToGetIndex = "";
                //MyList.IndexOf:"Something"; to:num
                if (line.Contains('"')) {
                  stringToGetIndex = line.Split(new [] {
                    "IndexOf:"
                  }, StringSplitOptions.None).Last().Split('"')[1].Split('"').First().Split(';').First();
                } else {
                  stringToGetIndex = strValues[strNames.IndexOf(line.Split(new [] {
                    "IndexOf:"
                  }, StringSplitOptions.None).Last().Split(';').First())];
                }
                numberValues[numberNames.IndexOf(line.Split(new [] {
                  "to"
                }, StringSplitOptions.None).Last().Split(';').First())] = numListValues[numListNames.IndexOf(listName)].IndexOf(Convert.ToDouble(stringToGetIndex));
              } catch {
                throw new Exception("Fatal error on line: " + code.IndexOf(line));
              }

            }
            foreach(var name in numberNames) {
              if (name.ToLower().StartsWith(listName.ToLower() + ".add")) {
                if (numListValues[numListNames.IndexOf(listName)].Contains(numberValues[numberNames.IndexOf(name)]) == false) {
                  numListValues[numListNames.IndexOf(listName)].Add(numberValues[numberNames.IndexOf(name)]);
                }
              }
            }
          }
          if (line.ToLower().StartsWith("getnumlistvalue(") || line.StartsWith("getnumlistvalue (")) {
            int index = 0;
            try {
              if (numberNames.Contains(line.Split('[')[1].Split(']')[0])) {
                index = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('[')[1].Split(']')[0])]);
                if (numberNames.Contains(line.Split('(')[1].Split(')')[0])) {
                  numberValues[numberNames.IndexOf(line.Split('(')[1].Split(')')[0])] = numListValues[numListNames.IndexOf(line.Split('(')[1].Split('[')[0])][index];
                } else {
                  numberNames.Add(line.Split('(')[1].Split(')')[0]);
                  numberValues.Add(numListValues[numListNames.IndexOf(line.Split('(')[1].Split('[')[0])][index]);
                }
              } else {
                index = Convert.ToInt32(line.Split('[')[1].Split(']')[0]);
              }
            } catch {
              Console.WriteLine("\nFatal Error (Syntax) On Line: " + code.IndexOf(line));
            }
          }
          if (line.ToLower().StartsWith("getstrlistvalue(") || line.StartsWith("getstrlistvalue (")) {
            int index = 0;
            try {
              if (numberNames.Contains(line.Split('[')[1].Split(']')[0])) {
                index = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('[')[1].Split(']')[0])]);
                if (strNames.Contains(line.Split('(')[1].Split(')')[0])) {
                  strValues[strNames.IndexOf(line.Split('(')[1].Split(')')[0])] = strListValues[strListNames.IndexOf(line.Split('(')[1].Split('[')[0])][index];
                } else {
                  strNames.Add(line.Split('(')[1].Split(')')[0]);
                  strValues.Add(strListValues[strListNames.IndexOf(line.Split('(')[1].Split('[')[0])][index]);
                }
              } else {
                index = Convert.ToInt32(line.Split('[')[1].Split(']')[0]);
                if (strNames.Contains(line.Split('(')[1].Split(')')[0])) {
                  strValues[strNames.IndexOf(line.Split('(')[1].Split(')')[0])] = strListValues[strListNames.IndexOf(line.Split('(')[1].Split('[')[0])][index];
                } else {
                  strNames.Add(line.Split('(')[1].Split(')')[0]);
                  strValues.Add(strListValues[strListNames.IndexOf(line.Split('(')[1].Split('[')[0])][index]);
                }
              }
            } catch {
              Console.WriteLine("Invalid Syntax on Line: " + code.IndexOf(line));
            }

          }

          foreach(var stringList in strListNames) {
            if (numberNames.Contains(stringList + ".length")) {
              numberValues[numberNames.IndexOf(stringList + ".length")] = strListValues[strListNames.IndexOf(stringList)].Count;
            } else {
              numberNames.Add(stringList + ".length");
              numberValues.Add(strListValues[strListNames.IndexOf(stringList)].Count);
            }
          }
          foreach(var numList in numListNames) {
            if (numberNames.Contains(numList + ".length")) {
              numberValues[numberNames.IndexOf(numList + ".length")] = numListValues[numListNames.IndexOf(numList)].Count;
            } else {
              numberNames.Add(numList + ".length");
              numberValues.Add(numListValues[numListNames.IndexOf(numList)].Count);
            }
          }

          if (line.ToLower() == "exectime(secs)") {
            execTime.Stop();
            Console.Write(execTime.Elapsed);
            execTime.Start();
          }
          if (line.ToLower() == "exectime(ms)") {
            execTime.Stop();
            Console.Write(execTime.ElapsedMilliseconds);
            execTime.Start();
          }
          if (line.ToLower() == "exectime()") {
            Console.WriteLine("\nexectime() takes one argument");
          }

          if (line.StartsWith("numToStr")) {

            if (strNames.Contains(line.Split('>').First().Split('(').Last()) && numberNames.Contains(line.Split('>').Last().Split(',').Last().Split(')').First())) {
              strValues[strNames.IndexOf(line.Split('>').First().Split('(').Last())] = numberValues[numberNames.IndexOf(line.Split('>').Last().Split(')').First())].ToString();
            } else {
              strNames.Add((line.Split('>').First().Split('(').Last()));
              try {
                strValues.Add(numberValues[numberNames.IndexOf(line.Split('>').Last().Split(')').First())].ToString());
              } catch {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(line.Split('>').Last().Split(')').First() + " does not exist at Line With Index: " + code.IndexOf(line));
                Console.ForegroundColor = ConsoleColor.White;
              }

            }

          }

          if (numberNames.Contains("screen_width")) {
            numberValues[numberNames.IndexOf("screen_width")] = Console.WindowWidth;
          } else {
            numberNames.Add("screen_width");
            numberValues.Add(Console.WindowWidth);

          }
          if (numberNames.Contains("screen_height")) {
            numberValues[numberNames.IndexOf("screen_height")] = Console.WindowHeight;
          } else {
            numberNames.Add("screen_height");
            numberValues.Add(Console.WindowHeight);
          }

          if (numberNames.Contains("cursor_x")) {
            numberValues[numberNames.IndexOf("cursor_x")] = Console.CursorLeft;
          } else {
            numberValues.Add(Console.CursorLeft);
            numberNames.Add("cursor_x");
          }
          if (numberNames.Contains("cursor_y")) {
            numberValues[numberNames.IndexOf("cursor_y")] = Console.CursorTop;
          } else {
            numberValues.Add(Console.CursorTop);
            numberNames.Add("cursor_y");
          }

          if (line.StartsWith("#") == false) {
            if (line.StartsWith("cursor_x >>")) {
              try {
                numberValues[numberNames.IndexOf("cursor_x")] = Convert.ToInt32(line.Split('>').Last());
              } catch {
                numberValues[numberNames.IndexOf("cursor_x")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
              }
              Console.CursorLeft = Convert.ToInt32(numberValues[numberNames.IndexOf("cursor_x")]);
            } else if (line.StartsWith("screen_width >>")) {
              try {
                numberValues[numberNames.IndexOf("screen_width")] = Convert.ToInt32(line.Split('>').Last());
              } catch {
                numberValues[numberNames.IndexOf("screen_width")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
              }
              Console.WindowWidth = Convert.ToInt32(numberValues[numberNames.IndexOf("screen_width")]);
            } else if (line.StartsWith("screen_height >>")) {
              try {
                numberValues[numberNames.IndexOf("screen_height")] = Convert.ToInt32(line.Split('>').Last());
              } catch {
                numberValues[numberNames.IndexOf("screen_height")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
              }
              Console.WindowHeight = Convert.ToInt32(numberValues[numberNames.IndexOf("screen_height")]);
            } else if (line.StartsWith("cursor_y >>")) {
              try {
                numberValues[numberNames.IndexOf("cursor_y")] = Convert.ToInt32(line.Split('>').Last());
              } catch {
                numberValues[numberNames.IndexOf("cursor_y")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
              }
              Console.CursorTop = Convert.ToInt32(numberValues[numberNames.IndexOf("cursor_y")]);
            }
            if (line == "exit()") {
              Environment.Exit(0);
            } else if (line.StartsWith("screen_width >>")) {
              try {
                numberValues[numberNames.IndexOf("screen_width")] = Convert.ToInt32(line.Split('>').Last());
              } catch {
                numberValues[numberNames.IndexOf("screen_width")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
              }
              Console.WindowWidth = Convert.ToInt32(numberValues[numberNames.IndexOf("screen_width")]);
            } else if (line.StartsWith("screen_height >>")) {
              try {
                numberValues[numberNames.IndexOf("screen_height")] = Convert.ToInt32(line.Split('>').Last());
              } catch {
                numberValues[numberNames.IndexOf("screen_height")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
              }
              Console.WindowHeight = Convert.ToInt32(numberValues[numberNames.IndexOf("screen_height")]);
            } else if (line.StartsWith("cursor_y >>")) {
              try {
                numberValues[numberNames.IndexOf("cursor_y")] = Convert.ToInt32(line.Split('>').Last());
              } catch {
                numberValues[numberNames.IndexOf("cursor_y")] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
              }
              Console.CursorTop = Convert.ToInt32(numberValues[numberNames.IndexOf("cursor_y")]);
            }
            if (line == "exit") {
              Environment.Exit(1);
            }
            foreach(var importation in references) {

              if (line.StartsWith(importation)) {
                try {
                  string fileToCompile = line.Split('(')[0];
                  fileToCompile = fileToCompile.Replace('.', '/');
                  fileToCompile = fileToCompile + ".rcode";
                  StreamReader specificLine_Compiler = File.OpenText(fileToCompile);
                  string lineReading;
                  List < string > linesFromFile = new List < string > ();
                  while ((lineReading = specificLine_Compiler.ReadLine()) != null) {
                    linesFromFile.Add(lineReading);
                  }
                  specificLine_Compiler.Close();
                  Compile(linesFromFile, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  //Environment.Exit(1);
                } catch {
                  Console.WriteLine("Method does not exist at line of index: " + code.IndexOf(line));
                }
              }
            }
            if (line.StartsWith("method")) {
              string fileToCompile = "";
              try {
                int firstIndex = Convert.ToInt32(line.Split(new [] {
                  "first:"
                }, StringSplitOptions.None).Last().Split(';').First());
                int lastIndex = Convert.ToInt32(line.Split(new [] {
                  "last:"
                }, StringSplitOptions.None).Last().Split(';').First());
                firstIndex--;
                fileToCompile = line.Split(new [] {
                  "file:"
                }, StringSplitOptions.None).Last().Split(';').First();

                if (!File.Exists(fileToCompile + ".rcode")) {
                  Console.ForegroundColor = ConsoleColor.Red;
                  Console.WriteLine("ERROR: File Does Not Exist (Line Index: " + code.IndexOf(line) + ")(" + line + ")");
                  Console.ResetColor();
                } else {
                  StreamReader specificLine_Compiler = File.OpenText(fileToCompile + ".rcode");
                  string lineReading;
                  List < string > linesFromFile = new List < string > ();
                  while ((lineReading = specificLine_Compiler.ReadLine()) != null) {
                    linesFromFile.Add(lineReading);
                  }
                  specificLine_Compiler.Close();
                  if (line.Split(new [] {
                      "content:"
                    }, StringSplitOptions.None).Last().Split(';').First() == "all") {
                    Compile(linesFromFile, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } else {
                    Compile(linesFromFile.GetRange(firstIndex, lastIndex - (firstIndex)), numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  }
                }
              } catch {
                try {
                  fileToCompile = line.ToLower().Split(new [] {
                    "file:"
                  }, StringSplitOptions.None).Last().Split(';').First();
                  if (line.Split(new [] {
                      "content:"
                    }, StringSplitOptions.None).Last().Split(';').First() == "all") {
                    StreamReader specificLine_Compiler = File.OpenText(fileToCompile + ".rcode");
                    string lineReading;
                    List < string > linesFromFile = new List < string > ();
                    while ((lineReading = specificLine_Compiler.ReadLine()) != null) {
                      linesFromFile.Add(lineReading);
                    }
                    specificLine_Compiler.Close();
                    Compile(linesFromFile, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  }
                } catch {
                  try {

                    StreamReader specificLine_Compiler = File.OpenText(fileToCompile + ".rcode");
                    string lineReading;
                    int firstIndex = Convert.ToInt32(line.Split(new [] {
                      "first:"
                    }, StringSplitOptions.None).Last().Split(';').First());
                    int lastIndex = Convert.ToInt32(line.Split(new [] {
                      "last:"
                    }, StringSplitOptions.None).Last().Split(';').First());
                    firstIndex--;
                    fileToCompile = line.ToLower().Split(new [] {
                      "file:"
                    }, StringSplitOptions.None).Last().Split(';').First();
                    List < string > linesFromFile = new List < string > ();
                    while ((lineReading = specificLine_Compiler.ReadLine()) != null) {
                      linesFromFile.Add(lineReading);
                    }
                    specificLine_Compiler.Close();
                    if (line.Split(new [] {
                        "content:"
                      }, StringSplitOptions.None).Last().Split(';').First() == "all") {
                      Compile(linesFromFile, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                    } else {
                      Compile(linesFromFile.GetRange(firstIndex, lastIndex - (firstIndex)), numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                    }
                  } catch {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("INVALID SYNTAX IN LINE " + code.IndexOf(line) + "(" + line + ")");
                    Console.ResetColor();
                  }
                }

              }
            }

            if (line.ToLower().StartsWith("compile_lines")) {

              try {
                int firstIndex = Convert.ToInt32(line.Split(new [] {
                  "first:"
                }, StringSplitOptions.None).Last().Split(';').First());
                int lastIndex = Convert.ToInt32(line.Split(new [] {
                  "last:"
                }, StringSplitOptions.None).Last().Split(';').First());
                firstIndex--;
                List < string > newCompile = code;
                Compile(newCompile.GetRange(firstIndex, (lastIndex - firstIndex)), numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
              } catch {
                if (line.Split(new [] {
                    "content:"
                  }, StringSplitOptions.None).Last().Split(';').First() == "all") {
                  List < string > newCompile = code;
                  Compile(newCompile, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                } else {
                  try {
                    int firstIndex = Convert.ToInt32(line.Split(new [] {
                      "first:"
                    }, StringSplitOptions.None).Last().Split(';').First());
                    int lastIndex = Convert.ToInt32(line.Split(new [] {
                      "last:"
                    }, StringSplitOptions.None).Last().Split(';').First());
                    firstIndex--;
                    List < string > newCompile = code;
                    Compile(newCompile.GetRange(firstIndex, (lastIndex - firstIndex)), numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } catch {

                  }
                }
              }
            }

            foreach(var num in numberNames) {

              if (line.StartsWith(num.ToString() + "++")) {
                double newNum = numberValues[numberNames.IndexOf(num)];
                newNum++;
                numberValues[numberNames.IndexOf(num)] = newNum;
              } else if (line.StartsWith(num.ToString() + "--")) {
                double newNum = numberValues[numberNames.IndexOf(num)];
                newNum--;
                numberValues[numberNames.IndexOf(num)] = newNum;
              } else if (line.StartsWith(num.ToString() + "+")) {
                try {
                  double newNum = numberValues[numberNames.IndexOf(num)];
                  newNum = newNum + Convert.ToDouble(line.Split('+').Last());
                  numberValues[numberNames.IndexOf(num)] = newNum;
                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      if (line.Split('+').Last() == getNum) {
                        double newNum = numberValues[numberNames.IndexOf(num)];
                        newNum = newNum + numberValues[numberNames.IndexOf(getNum)];
                        numberValues[numberNames.IndexOf(num)] = newNum;
                      }
                    }
                  } catch {

                  }
                }
              }
              if (line.StartsWith(num.ToString() + "-")) {
                try {
                  numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] - Convert.ToDouble(line.Split('-').Last());
                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      if (line.Split('-').Last() == getNum) {
                        numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] - numberValues[numberNames.IndexOf(getNum)];
                      }
                    }
                  } catch {

                  }
                }
              }
              if (line.StartsWith(num.ToString() + "/")) {
                try {
                  numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] / Convert.ToDouble(line.Split('/').Last());

                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      if (line.Split('/').Last() == getNum) {
                        numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] / numberValues[numberNames.IndexOf(getNum)];
                      }
                    }
                  } catch {

                  }
                }
              }
              if (line.StartsWith(num.ToString() + "*")) {
                try {
                  numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] * Convert.ToDouble(line.Split('*').Last());
                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      if (line.Split('*').Last() == getNum) {
                        numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] * numberValues[numberNames.IndexOf(getNum)];
                      }
                    }
                  } catch {

                  }
                }
              }
              if (line.StartsWith(num.ToString() + "%")) {
                try {
                  numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] % Convert.ToDouble(line.Split('%').Last());
                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      if (line.Split('%').Last() == getNum) {
                        numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] % numberValues[numberNames.IndexOf(getNum)];
                      }
                    }
                  } catch {

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
            if (line.StartsWith("toLower (") && line.Contains(")") && line.Contains(">>")) {
              if (strNames.Contains(line.Split(new [] {
                  "toLower ("
                }, StringSplitOptions.None).Last().Split(new [] {
                  ">>"
                }, StringSplitOptions.None).First())) {
                if (strNames.Contains(line.Split(new [] {
                    ">>"
                  }, StringSplitOptions.None).Last().Split(')').First())) {
                  strValues[strNames.IndexOf(line.Split(new [] {
                    "toLower ("
                  }, StringSplitOptions.None).Last().Split(new [] {
                    ">>"
                  }, StringSplitOptions.None).First())] = strValues[strNames.IndexOf(line.Split(new [] {
                    ">>"
                  }, StringSplitOptions.None).Last().Split(')').First())].ToLower();
                } else {
                  strValues[strNames.IndexOf(line.Split(new [] {
                    "toLower ("
                  }, StringSplitOptions.None).Last().Split(new [] {
                    ">>"
                  }, StringSplitOptions.None).First())] = line.Split(new [] {
                    ">>"
                  }, StringSplitOptions.None).Last().Split(')').First().ToLower();
                }
              }

            } else if (line.StartsWith("toLower (") && line.Contains(")") && line.Contains(">>") == false) {
              try {
                strValues[strNames.IndexOf(line.Split(new [] {
                  "toLower ("
                }, StringSplitOptions.None).Last().Split(')').First())] = strValues[strNames.IndexOf(line.Split(new [] {
                  "toLower ("
                }, StringSplitOptions.None).Last().Split(')').First())].ToLower();
              } catch {

              }
            }

            //toUpper

            foreach(var str in strNames) {
              if (line.StartsWith("str (" + str) && line.Contains('+') && line.Contains(')') || line.StartsWith("str(" + str) && line.Contains('+') && line.Contains(')')) {
                if (line.Split('+')[1].Split(')').First() != "$readline" || str != "$readline") {
                  try {
                    strValues[strNames.IndexOf(str)] = strValues[strNames.IndexOf(str)] + strValues[strNames.IndexOf(line.Split('+')[1].Split(')').First())];
                  } catch {
                    try {
                      strValues[strNames.IndexOf(str)] = strValues[strNames.IndexOf(str)] + line.Split('+')[1].Split(')').First();
                    } catch {
                      Console.WriteLine($"One of the Following Strings Does Not Exist: {line.Split('+')[0].Split(')').First()} or {line.Split('+')[0].Split(')').First()}");
                    }
                  }
                } else if (str == "$readline") {
                  try {
                    strValues[strNames.IndexOf(line.Split('+')[1].Split(')').First())] = Console.ReadLine() + strValues[strNames.IndexOf(line.Split('+')[1].Split(')').First())];
                  } catch {

                  }
                } else if (line.Split('+')[1].Split(')').First() == "$readline") {
                  strValues[strNames.IndexOf(str)] = strValues[strNames.IndexOf(str)] + Console.ReadLine();
                }
              }
            }

            if (line.ToLower() == "clear") {
              Console.Clear();
            } else if (line.StartsWith("import")) {
              if (line.ToLower() == "import pixel") {
                references.Add("pixel");
              } else if (line.ToLower() == "import process") {
                references.Add("process");
              } else if (line.ToLower() == "import split") {
                references.Add("split");
              } else if (line.ToLower() == "import filestream") {
                references.Add("filestream");
              } else if (line.ToLower() == "import keys") {
                references.Add("keys");
              } else if (line.ToLower() == "import split") {
                references.Add("split");
              } else if (line.ToLower().StartsWith("import ")) {
                references.Add(line.Split(' ')[1]);
              }
            }

            if (references.Contains("pixel")) {
              if (line.ToLower().StartsWith("pixel.draw")) {

                //add to normal pixel

                try {
                  pixelX.Add(Convert.ToInt32(line.Split(new [] {
                    "x:"
                  }, StringSplitOptions.None).Last().Split(';').First()));
                } catch {
                  pixelX.Add(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                    "x:"
                  }, StringSplitOptions.None).Last().Split(';').First())]));
                }
                try {
                  pixelY.Add(Convert.ToInt32(line.Split(new [] {
                    "y:"
                  }, StringSplitOptions.None).Last().Split(';').First()));
                } catch {
                  pixelY.Add(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                    "y:"
                  }, StringSplitOptions.None).Last().Split(';').First())]));
                }
                string color = line.ToLower().Split(new [] {
                  "color:"
                }, StringSplitOptions.None).Last().Split(';').First();

                if (color == "white") {
                  pixelColors.Add(ConsoleColor.White);
                } else if (color == "red") {
                  pixelColors.Add(ConsoleColor.Red);
                } else if (color == "blue") {
                  pixelColors.Add(ConsoleColor.Blue);
                } else if (color == "magenta") {
                  pixelColors.Add(ConsoleColor.Magenta);
                } else if (color == "green") {
                  pixelColors.Add(ConsoleColor.Green);
                } else if (color == "yellow") {
                  pixelColors.Add(ConsoleColor.Yellow);
                }

              }
              if (line.ToLower().StartsWith("pixel.drawchar")) {
                try {
                  //add to char
                  try {
                    pixelXChar.Add(Convert.ToInt32(line.Split(new [] {
                      "x:"
                    }, StringSplitOptions.None).Last().Split(';').First()));
                  } catch {
                    pixelXChar.Add(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                      "x:"
                    }, StringSplitOptions.None).Last().Split(';').First())]));
                  }
                  try {
                    pixelYChar.Add(Convert.ToInt32(line.Split(new [] {
                      "y:"
                    }, StringSplitOptions.None).Last().Split(';').First()));
                  } catch {
                    pixelYChar.Add(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                      "y:"
                    }, StringSplitOptions.None).Last().Split(';').First())]));
                  }
                  string color = line.Split(new [] {
                    "color:"
                  }, StringSplitOptions.None).Last().Split(';').First();
                  string characterToDraw = line.Split(new [] {
                    "char:"
                  }, StringSplitOptions.None).Last().Split(';').First();
                  charachtersToDraw.Add(characterToDraw);

                  if (color == "white") {
                    pixelColorsChar.Add(ConsoleColor.White);
                  } else if (color == "red") {
                    pixelColorsChar.Add(ConsoleColor.Red);
                  } else if (color == "blue") {
                    pixelColorsChar.Add(ConsoleColor.Blue);
                  } else if (color == "magenta") {
                    pixelColorsChar.Add(ConsoleColor.Magenta);
                  } else if (color == "green") {
                    pixelColorsChar.Add(ConsoleColor.Green);
                  } else if (color == "yellow") {
                    pixelColorsChar.Add(ConsoleColor.Yellow);
                  }

                } catch {
                  Console.WriteLine("Invalid Syntax In Line " + code.IndexOf(line));
                }
              }
            }
            if (references.Contains("process")) {
              if (line.StartsWith("StartProcess")) {
                string[] arguments = line.Split(new [] {
                  "args:"
                }, StringSplitOptions.None).Last().Split(';').First().Split(',');
                string filename = line.Split(new [] {
                  "file:"
                }, StringSplitOptions.None).Last().Split(';').First();
                string argument = "";
                foreach(var arg in arguments) {
                  if (strNames.Contains(arg)) {
                    argument = argument + " " + strValues[strNames.IndexOf(arg)];
                  } else {
                    argument = argument + " " + arg;
                  }
                }
                Process.Start(filename, argument);
              }
            }
            if (references.Contains("split")) {
              if (line.StartsWith("getSplit")) {
                try {
                  //getSplit from:x to:y index
                  string from = line.Split(new [] {
                    "from:"
                  }, StringSplitOptions.None).Last().Split(';').First();
                  string to = line.Split(new [] {
                    "to:"
                  }, StringSplitOptions.None).Last().Split(';').First();
                  string index = line.Split(new [] {
                    "index:"
                  }, StringSplitOptions.None).Last().Split(';').First();
                  int f = 0;
                  try {
                    f = Convert.ToInt32(index);
                  } catch {
                    f = Convert.ToInt32(numberValues[numberNames.IndexOf(index)]);
                  }
                  string split = line.Split(new [] {
                    "separator:"
                  }, StringSplitOptions.None).Last().Split(';').First();
                  if (split.Contains('"')) {
                    split = line.Split(new [] {
                      "separator:"
                    }, StringSplitOptions.None).Last().Split('\"')[1].Split('\"')[0];
                  } else {
                    split = strValues[strNames.IndexOf(line.Split(new [] {
                      "separator:"
                    }, StringSplitOptions.None).Last().Split(';').First())];
                  }
                  if (strNames.Contains(from)) {
                    strValues[strNames.IndexOf(to)] = Split.split(strValues[strNames.IndexOf(from)], split, f);
                  } else {
                    strValues[strNames.IndexOf(to)] = Split.split(from, split, f);
                  }
                } catch {}
              }
            }
            if (references.Contains("keys")) {
              if (line.StartsWith("SendKeys") && line != "SendKeys.Enter") {

                try {
                  SendKeys.SendWait(strValues[strNames.IndexOf(line.Split(new [] {
                    "SendKeys"
                  }, StringSplitOptions.None).Last().Split('(').Last().Split(')').First())]);

                } catch {
                  SendKeys.SendWait(line.Split(new [] {
                    "SendKeys"
                  }, StringSplitOptions.None).Last().Split('(').Last().Split(')').First());
                }
              } else if (line == "SendKeys.Enter") {
                SendKeys.SendWait("{Enter}");
              }

            }
            if (references.Contains("filestream")) {
              //WriteFile file:filename content: Writing 
              //CreateFile file:filename
              //DeleteFile file.filename
              //todo ReadFile
              if (line.StartsWith("WriteToFile")) {
                string filename = line.Split(new [] {
                  "file:"
                }, StringSplitOptions.None).Last().Split(';').First();
                if (strNames.Contains(filename)) {
                  filename = strValues[strNames.IndexOf(filename)];
                }
                string content = line.Split(new [] {
                  "content:"
                }, StringSplitOptions.None).Last().Split(';').First();
                if (strNames.Contains(content) == false) {
                  FileStream.WriteToFile(filename, content);
                } else {
                  FileStream.WriteToFile(filename, strValues[strNames.IndexOf(content)]);
                }
              } else if (line.StartsWith("DeleteFile")) {
                string filename = line.Split(new [] {
                  "file:"
                }, StringSplitOptions.None).Last().Split(';').First();
                if (strNames.Contains(filename)) {
                  filename = strValues[strNames.IndexOf(filename)];
                }
                FileStream.DeleteFile(filename);
              } else if (line.StartsWith("CreateFile")) {
                string filename = line.Split(new [] {
                  "file:"
                }, StringSplitOptions.None).Last().Split(';').First();
                if (strNames.Contains(filename)) {
                  filename = strValues[strNames.IndexOf(filename)];
                }
                FileStream.CreateFile(filename);
              } else if (line.StartsWith("ReadFile")) {
                string filename = line.Split(new [] {
                  "file:"
                }, StringSplitOptions.None).Last().Split(';').First();
                string strToReadTo = line.Split(new [] {
                  "str:"
                }, StringSplitOptions.None).Last().Split(';').First();
                if (strNames.Contains(filename)) {
                  filename = strValues[strNames.IndexOf(filename)];
                }
                if (strNames.Contains(strToReadTo)) {
                  strValues[strNames.IndexOf(strToReadTo)] = FileStream.ReadFile(filename);;
                } else {
                  strValues.Add(FileStream.ReadFile(filename));
                  strNames.Add(strToReadTo);
                }

              }
            }

            if (line.ToLower().StartsWith("sleep >>")) {
              try {
                System.Threading.Thread.Sleep(Convert.ToInt32(line.Split('>').Last()));
              } catch {
                try {
                  System.Threading.Thread.Sleep(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('>').Last())]));
                } catch {
                  System.Threading.Thread.Sleep(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                    "> "
                  }, StringSplitOptions.None).Last())]));
                }
              }
            }
            //color indicators
            if (line.ToLower().StartsWith("color.reset")) {
              Console.ResetColor();
            } else if (line.ToLower().StartsWith("color.green")) {
              Console.ForegroundColor = ConsoleColor.Green;
            } else if (line.ToLower().StartsWith("color.blue")) {
              Console.ForegroundColor = ConsoleColor.Blue;
            } else if (line.ToLower().StartsWith("color.red")) {
              Console.ForegroundColor = ConsoleColor.Red;
            } else if (line.ToLower().StartsWith("color.magenta")) {
              Console.ForegroundColor = ConsoleColor.Magenta;
            } else if (line.ToLower().StartsWith("color.yellow")) {
              Console.ForegroundColor = ConsoleColor.Yellow;
            } else if (line.ToLower().StartsWith("color.white")) {
              Console.ForegroundColor = ConsoleColor.White;
            }

            if (line.ToLower() == ("$readline")) {
              Console.ReadLine();
            }

            //usage: replace:f; with:d; str:string;
            if (line.ToLower().StartsWith("replace")) {
              string toReplace = line.Split(new [] {
                "replace:"
              }, StringSplitOptions.None).Last().Split(';').First();
              string with = line.Split(new [] {
                "with:"
              }, StringSplitOptions.None).Last().Split(';').First();
              string strToReplace = line.Split(new [] {
                "str:"
              }, StringSplitOptions.None).Last().Split(';').First();
              string toReplaceChecked = "";
              string strChecked = "";
              string withChecked = "";
              if (toReplace.Contains('"')) {
                toReplaceChecked = toReplace.Split('"')[1].Split('"')[0];

              } else {
                try {
                  toReplaceChecked = strValues[strNames.IndexOf(toReplace)];
                } catch {
                  Console.WriteLine("Error: " + toReplace + " doesn't exist on line " + code.IndexOf(line).ToString());
                }
              }
              if (with.Contains('"')) {
                withChecked = with.Split('"')[1].Split('"')[0];

              } else {
                try {
                  withChecked = strValues[strNames.IndexOf(with)];
                } catch {
                  Console.WriteLine("Error: " + with + " doesn't exist on line " + code.IndexOf(line).ToString());
                }
              }

              if (strToReplace.Contains('"')) {
                strChecked = strToReplace.Split('"')[1].Split('"')[0];

              } else {
                try {
                  strChecked = strValues[strNames.IndexOf(strToReplace)];
                } catch {
                  Console.WriteLine("Error: " + strToReplace + " doesn't exist on line " + code.IndexOf(line).ToString());
                }
              }

              strValues[strNames.IndexOf(strToReplace)] = strChecked.Replace(toReplaceChecked, withChecked);
            }

            if (line.StartsWith("number ") || line.StartsWith("num ") && line.Contains(">>") &&
              line.ToLower().StartsWith("for") == false &&
              line.ToLower().Contains("in range %") == false &&
              line.Contains("$>") == false) {
              try {
                rand = new Random();
                if (numberNames.Contains(line.Split(' ')[1].Split('>').First())) {
                  if (line.ToLower().Contains("$readline") == false) {
                    try {
                      if (line.Split('>').Last().Contains("rand:")) {
                        numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = rand.Next(Convert.ToInt32(line.Split(new [] {
                          "rand:"
                        }, StringSplitOptions.None).Last().Split(',').First()), Convert.ToInt32(line.Split(new [] {
                          "rand:"
                        }, StringSplitOptions.None).Last().Split(',').Last()));
                      } else {
                        numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Convert.ToDouble(line.Split('>').Last());
                      }
                    } catch {
                      rand = new Random();
                      if (line.Split('>').Last().Contains("rand:")) {
                        numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = rand.Next(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                          "rand:"
                        }, StringSplitOptions.None).Last().Split(',').First())]), Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                          "rand:"
                        }, StringSplitOptions.None).Last().Split(',').Last())]));
                      } else {
                        numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = numberValues[numberNames.IndexOf(line.Split('>').Last())];
                      }
                    }
                  } else if (line.ToLower().Contains("$readline") == true) {
                    numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Convert.ToDouble(Console.ReadLine());
                  }
                } else {
                  rand = new Random();
                  if (line.ToLower().Contains("$readline") == false) {
                    numberNames.Add(line.Split(' ')[1].Split('>').First());
                    try {
                      if (line.Split('>').Last().Contains("rand:")) {
                        numberValues.Add(rand.Next(Convert.ToInt32(line.Split(new [] {
                          "rand:"
                        }, StringSplitOptions.None).Last().Split(',').First()), Convert.ToInt32(line.Split(new [] {
                          "rand:"
                        }, StringSplitOptions.None).Last().Split(',').Last())));
                      } else {
                        try {
                          numberValues.Add(Convert.ToDouble(line.Split('>').Last().Split(' ').Last()));
                        } catch {

                          numberValues.Add(numberValues[numberNames.IndexOf(line.Split('>').Last())]);

                        }
                      }
                    } catch {
                      if (line.Split('>').Last().Contains("rand:")) {
                        numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = rand.Next(Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                          "rand:"
                        }, StringSplitOptions.None).Last().Split(',').First())]), Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                          "rand:"
                        }, StringSplitOptions.None).Last().Split(',').Last())]));
                      } {
                        numberValues.Add(Convert.ToDouble(line.Split('>').Last()));
                      }
                    }
                  } else if (line.ToLower().Contains("$readline") == true) {
                    numberNames.Add(line.Split(' ')[1].Split('>').First());
                    numberValues.Add(Convert.ToDouble(Console.ReadLine()));
                  }

                }
              } catch {
                int errorLine = code.IndexOf(line);
                Console.WriteLine($"Invalid Syntax (Line {errorLine--})");
              }
            }

            //string definer
            if (line.StartsWith("str") &&
              line.Contains(">>") &&
              line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false &&
              line.Contains("$>") == false) {
              try {
                if (strNames.Contains(line.Split(' ')[1].Split('>').First())) {
                  if (line.Split('>').Last().ToLower().StartsWith("$read") == false) {
                    strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = line.Split('>')[2].Split('\"')[1].Split('\"').First();

                  } else if (line.Split('>').Last().ToLower() == "$readkey") {
                    strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Console.ReadKey().Key.ToString();
                  } else if (line.Split('>').Last().ToLower() == "$readline") {
                    strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Console.ReadLine();
                  }
                } else {

                  if (line.Split('>').Last().ToLower().StartsWith("$read") == false) {
                    strNames.Add(line.Split(' ')[1].Split('>').First());
                    if (line.Split('>')[2].Contains('"')) {
                      strValues.Add(line.Split('>')[2].Split('\"')[1].Split('\"').First());
                    } else {
                      try {
                        strValues.Add(strValues[strNames.IndexOf(line.Split('>')[2])]);
                      } catch {
                        Console.WriteLine("Error: " + line);
                      }
                    }
                  } else if (line.Split('>').Last().ToLower() == "$readkey") {
                    strNames.Add(line.Split(' ')[1].Split('>').First());
                    strValues.Add(Console.ReadKey().Key.ToString());
                  } else if (line.Split('>').Last().ToLower() == ("$readline")) {
                    strNames.Add(line.Split(' ')[1].Split('>').First());
                    strValues.Add(Console.ReadLine());
                  }
                }

              } catch {
                int errorLine = code.IndexOf(line);
                Console.WriteLine($"Invalid Syntax (Line {errorLine++})");
              }
            }

            //Write 

            if (line.ToLower().StartsWith("newline") || line.ToLower().StartsWith("newln")) {
              Console.WriteLine("");
            }
            if (line.StartsWith("Write") && line.Contains(" \"") && line.Contains("WriteStr") == false && line.Contains("WriteNum") == false && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false) {
              //check if it is a number 
              //var matchesNumber = numberNames.Where(x => line.Contains(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString()));

              Console.Write(line.Split(new [] {
                "Write \""
              }, StringSplitOptions.None).Last().ToString().Split(new [] {
                "\""
              }, StringSplitOptions.None).First().ToString());

            } else if (line.StartsWith("WriteStr") &&
              line.Contains("{") &&
              line.Contains("}") &&
              line.ToLower().StartsWith("for") == false &&
              line.ToLower().Contains("in range %") == false &&
              line.Contains("$>") == false) {
              foreach(var name in strNames) {
                var namesToCheck = line.Split('{')[1].Split('}')[0].Split(',');

                foreach(var nametoCheck in namesToCheck) {
                  try {
                    if (nametoCheck == name || nametoCheck == "{" + name || nametoCheck == name + "}" || nametoCheck == "{" + name + "}") {
                      Console.Write(strValues[strNames.IndexOf(name)]);
                    }
                  } catch {
                    Console.WriteLine("Invalid Syntax on Line " + code.IndexOf(line));
                  }
                }
              }

            } else if (line.StartsWith("WriteNum") &&
              line.Contains(" {") &&
              line.Contains("}") &&
              line.ToLower().StartsWith("for") == false &&
              line.ToLower().Contains("in range %") == false &&
              line.Contains("$>") == false)

            {
              foreach(var name in numberNames) {
                var namesToCheck = line.Split('{')[1].Split('}')[0].Split(',');

                foreach(var nametoCheck in namesToCheck) {
                  try {
                    if (nametoCheck == name || nametoCheck == "{" + name || nametoCheck == name + "}" || nametoCheck == "{" + name + "}") {
                      Console.Write(numberValues[numberNames.IndexOf(name)]);
                    }
                  } catch {
                    Console.WriteLine("Invalid Syntax on Line " + code.IndexOf(line));
                  }

                }

              }

              //save

            } else if (line.StartsWith("save(this)") && line.Contains("as") == false) {
              id = new Random().Next(1, 10000);
              StreamWriter writer = File.CreateText("code.id(" + id + ").rcode");
              foreach(var lineToSave in code) {
                if (lineToSave.StartsWith("save")) {
                  writer.WriteLine(lineToSave);
                }
              }
              if (id != 0) {
                Program.WriteId(id);
              }

              writer.Close();
            } else if (line.StartsWith("save(this)") && line.Contains("as") == true) {
              StreamWriter writerAs = File.CreateText(line.Split(new [] {
                "save(this) as "
              }, StringSplitOptions.None).Last() + ".rcode");
              foreach(var lineToSave in code) {
                if (lineToSave.StartsWith("save") == false) {
                  writerAs.WriteLine(lineToSave);
                }
              }
              writerAs.Close();
              Console.ResetColor();
              Console.Write("\nFile Saved as ");
              Console.ForegroundColor = ConsoleColor.Green;
              Console.Write(line.Split(new [] {
                "save(this) as "
              }, StringSplitOptions.None).Last() + ".rcode");
              Console.ResetColor();
              Console.Write(" in order to edit it, please do so using a text editor or visual studio.\nIn order to compile it, use the command load >> " + (line.Split(new [] {
                "save(this) as"
              }, StringSplitOptions.None).Last() + ".rcode"));

            } else if (line == "save = false") {
              id = 0;
            } else if (line.StartsWith("load >>") || line.StartsWith("compiler.load")) {
              string fileToCompile = line.Split(new [] {
                "load >> "
              }, StringSplitOptions.None).Last();
              Console.WriteLine(fileToCompile);
              List < string > CompileFile = new List < string > ();
              try {
                StreamReader reader = new StreamReader(fileToCompile);
                string lineReader;
                while ((lineReader = reader.ReadLine()) != null) {
                  CompileFile.Add(lineReader);
                }
                string isVisualizing;

                Console.WriteLine("Do you want to Visualize it's code (Y/n): ");
                isVisualizing = Console.ReadLine();
                if (isVisualizing.ToLower() != "n") {
                  Console.ForegroundColor = ConsoleColor.Cyan;
                  Console.WriteLine("\n " + fileToCompile + " (Code): \n");
                  Console.ResetColor();
                  foreach(var lineToShow in CompileFile) {
                    Console.WriteLine(CompileFile.IndexOf(lineToShow) + " " + lineToShow);
                  }
                  Console.WriteLine("\n");
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n \n \n Output:");
                Console.ResetColor();
                Compile(CompileFile, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                Console.WriteLine("");
              } catch {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Couldn't find " + fileToCompile);
                Console.ResetColor();
              }

            } else if (line.StartsWith("compile >>")) {
              List < string > CompileFile = new List < string > ();
              string fileToCompile = line.Split(new [] {
                ">> "
              }, StringSplitOptions.None).Last();

              try {
                StreamReader reader = new StreamReader(fileToCompile);
                string lineReader;
                while ((lineReader = reader.ReadLine()) != null) {
                  CompileFile.Add(lineReader);
                }
                Compile(CompileFile, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                Console.Write("\n");
                reader.Close();
              } catch {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File Not Found / Compilation Error! \n(Make Sure You Leave a Space Between the indicators (>>) ex: compile >> filename.rcode");
                Console.ResetColor();
              }
            } else if (line.StartsWith("if")) {

              /*
                  if name:1; statement:str(x==y); indent:" ";
                      Write &>Hi<&
                  endIf(1)

              */

              string name = "";
              string indent = "";
              string statement = "";
              List < string > loopContent = new List < string > ();
              List < string > compileAfter = new List < string > ();
              try {
                name = line.Split(new [] {
                  "name:"
                }, StringSplitOptions.None).Last().Split(';').First();
                statement = line.Split(new [] {
                  "statement:"
                }, StringSplitOptions.None).Last().Split(';')[0];
                indent = line.Split(new [] {
                  "indent:"
                }, StringSplitOptions.None).Last().Split('"')[1].Split('"').First().Split(';').First();
              } catch {
                Console.WriteLine("Invalid Syntax Line: " + code.IndexOf(line));
              }
              List < string > newCode1 = code;
              if (newCode1.Contains("}" + name + ";")) {
                newCode1.RemoveRange(0, newCode1.IndexOf(line) + 1);
                foreach(var lineofCode in newCode1) {
                  if (newCode1.IndexOf(lineofCode) > newCode1.IndexOf("}" + name + ";")) {
                    compileAfter.Add(lineofCode);
                  }
                }
                newCode1.RemoveRange(newCode1.IndexOf("}" + name + ";"), newCode1.Count - newCode1.IndexOf("}" + name + ";"));
                loopContent = newCode1;

              } else {
                Console.WriteLine("\nNo End Was Found for If Statement with name: " + name);
              }
              for (int i = 0; i < newCode1.Count; i++) {
                if (newCode1[i].StartsWith(indent)) {
                  try {
                    newCode1[newCode1.IndexOf(newCode1[i])] = newCode1[i].Split(new [] {
                      indent
                    }, StringSplitOptions.None)[1];
                  } catch {}
                }
              }

              if (line.Contains("str(") && line.Contains("==")) {
                try {
                  if (strValues[strNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "str("
                    }, StringSplitOptions.None).Last().Split(new [] {
                      "=="
                    }, StringSplitOptions.None).First())] ==
                    strValues[strNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "=="
                    }, StringSplitOptions.None).Last().Split(')').First())]) {
                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } else {
                    if (line.Contains("&else")) {
                      string newStatement = line.Split(new [] {
                        "&else"
                      }, StringSplitOptions.None).Last();
                      var getElseContent = newStatement.Split(new [] {
                        "->"
                      }, StringSplitOptions.None);
                      List < string > loopElseContent = new List < string > ();
                      foreach(var content in getElseContent) {
                        if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false) {
                          loopElseContent.Add(content);
                        }
                      }
                      Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                    }
                  }
                } catch {}

              }
              /*else if(line.Contains("str(") && line.Contains("contains("))
                                      {
                                          if(strValues[strNames.Contains("")])
                                      }*/
              else if (line.Contains("str(") && line.Contains("!=")) {
                try {
                  if (strValues[strNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "str("
                    }, StringSplitOptions.None).Last().Split(new [] {
                      "!="
                    }, StringSplitOptions.None).First())] !=
                    strValues[strNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "!="
                    }, StringSplitOptions.None).Last().Split(')').First())]) {
                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } else {
                    if (line.Contains("&else")) {
                      string newStatement = line.Split(new [] {
                        "&else"
                      }, StringSplitOptions.None).Last();
                      var getElseContent = newStatement.Split(new [] {
                        "->"
                      }, StringSplitOptions.None);
                      List < string > loopElseContent = new List < string > ();
                      foreach(var content in getElseContent) {
                        if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false) {
                          loopElseContent.Add(content);
                        }
                      }
                      Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                    }
                  }
                } catch {}
              }
              if (statement.Contains("num(") && statement.Contains("==")) {
                try {
                  if (numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "num("
                    }, StringSplitOptions.None).Last().Split(new [] {
                      "=="
                    }, StringSplitOptions.None).First())] ==
                    numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "=="
                    }, StringSplitOptions.None).Last().Split(')').First())]) {
                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } else {
                    if (line.Contains("&else")) {
                      string newStatement = line.Split(new [] {
                        "&else"
                      }, StringSplitOptions.None).Last();
                      var getElseContent = newStatement.Split(new [] {
                        "->"
                      }, StringSplitOptions.None);
                      List < string > loopElseContent = new List < string > ();
                      foreach(var content in getElseContent) {
                        if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false) {
                          loopElseContent.Add(content);
                        }
                      }
                      Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                    }
                  }
                } catch {}

              } else if (statement.Contains("num(") && statement.Contains("!=")) {
                try {
                  if (numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "num("
                    }, StringSplitOptions.None).Last().Split(new [] {
                      "!="
                    }, StringSplitOptions.None).First())] !=
                    numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "!="
                    }, StringSplitOptions.None).Last().Split(')').First())]) {
                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } else {
                    if (line.Contains("&else")) {
                      string newStatement = line.Split(new [] {
                        "&else"
                      }, StringSplitOptions.None).Last();
                      var getElseContent = newStatement.Split(new [] {
                        "->"
                      }, StringSplitOptions.None);
                      List < string > loopElseContent = new List < string > ();
                      foreach(var content in getElseContent) {
                        if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false) {
                          loopElseContent.Add(content);
                        }
                      }
                      Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                    }
                  }
                } catch {}
              } else if (statement.Contains("num(") && statement.Contains(">=")) {
                try {
                  if (numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "num("
                    }, StringSplitOptions.None).Last().Split(new [] {
                      ">="
                    }, StringSplitOptions.None).First())] >=
                    numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      ">="
                    }, StringSplitOptions.None).Last().Split(')').First())]) {
                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } else {
                    if (line.Contains("&else")) {
                      string newStatement = line.Split(new [] {
                        "&else"
                      }, StringSplitOptions.None).Last();
                      var getElseContent = newStatement.Split(new [] {
                        "->"
                      }, StringSplitOptions.None);
                      List < string > loopElseContent = new List < string > ();
                      foreach(var content in getElseContent) {
                        if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false) {
                          loopElseContent.Add(content);
                        }
                      }
                      Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                    }
                  }
                } catch {}

              } else if (statement.Contains("num(") && statement.Contains("<=")) {
                try {
                  if (numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "num("
                    }, StringSplitOptions.None).Last().Split(new [] {
                      "<="
                    }, StringSplitOptions.None).First())] <=
                    numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "<="
                    }, StringSplitOptions.None).Last().Split(')').First())]) {
                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } else {
                    if (line.Contains("&else")) {
                      string newStatement = line.Split(new [] {
                        "&else"
                      }, StringSplitOptions.None).Last();
                      var getElseContent = newStatement.Split(new [] {
                        "->"
                      }, StringSplitOptions.None);
                      List < string > loopElseContent = new List < string > ();
                      foreach(var content in getElseContent) {
                        if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false) {
                          loopElseContent.Add(content);
                        }
                      }
                      Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                    }
                  }
                } catch {}

              } else if (statement.Contains("num(") && statement.Contains("+>")) {
                try {
                  if (numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "num("
                    }, StringSplitOptions.None).Last().Split(new [] {
                      "+>"
                    }, StringSplitOptions.None).First())] >
                    numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "+>"
                    }, StringSplitOptions.None).Last().Split(')').First())]) {
                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } else {
                    if (line.Contains("&else ->")) {
                      string newStatement = line.Split(new [] {
                        "&else"
                      }, StringSplitOptions.None).Last();
                      var getElseContent = newStatement.Split(new [] {
                        "->"
                      }, StringSplitOptions.None);
                      List < string > loopElseContent = new List < string > ();
                      foreach(var content in getElseContent) {
                        if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false) {
                          loopElseContent.Add(content);
                        }
                      }

                      Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);

                    }
                  }
                } catch {}

              } else if (statement.Contains("num(") && statement.Contains("<-")) {
                try {
                  if (numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "num("
                    }, StringSplitOptions.None).Last().Split(new [] {
                      "<-"
                    }, StringSplitOptions.None).First())] <
                    numberValues[numberNames.IndexOf(statement.Split(new [] {
                      "&else"
                    }, StringSplitOptions.None).First().Split(new [] {
                      "num("
                    }, StringSplitOptions.None).Last().Split(new [] {
                      "<-"
                    }, StringSplitOptions.None).Last().Split(')').First())]) {
                    Compile(loopContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                  } else {
                    if (line.Contains("&else ->")) {
                      string newStatement = line.Split(new [] {
                        "&else"
                      }, StringSplitOptions.None).Last();
                      var getElseContent = newStatement.Split(new [] {
                        "->"
                      }, StringSplitOptions.None);
                      List < string > loopElseContent = new List < string > ();
                      foreach(var content in getElseContent) {
                        if (content.ToLower().StartsWith("&else") == false && content.Contains("->") == false) {
                          loopElseContent.Add(content);
                        }
                      }
                      Compile(loopElseContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
                    }
                  }

                } catch {}

              }

              Compile(compileAfter, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
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
          if (pixelColors.Count >= 1 && pixelX.Count >= 1 && pixelY.Count >= 1) {
            for (int i = 0; i < pixelColors.Count; i++) {
              Pixel.Draw(pixelX[i], pixelY[i], pixelColors[i]);
            }
          }
          if (pixelColorsChar.Count >= 1 && pixelXChar.Count >= 1 && pixelYChar.Count >= 1) {
            for (int j = 0; j < pixelColorsChar.Count; j++) {
              Pixel.DrawChar(pixelXChar[j], pixelYChar[j], pixelColorsChar[j], charachtersToDraw[j]);
            }
          }
          execTime.Stop();
        }
      } catch (System.InvalidOperationException) {

      }
      Console.ResetColor();
      void ForLoop(int range,
        string looper, List < string > loopContent)

      {

        for (int x = 0; x < range; x++) {
          if (numberNames.Contains(looper)) {
            numberValues[numberNames.IndexOf(looper)] = x;
          } else {
            numberValues.Add(x);
            numberNames.Add(looper);
          }
          Compile(loopContent, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions);
        }
      }
    }
  }
}
