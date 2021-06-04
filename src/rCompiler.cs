using rC;

using System;
using System.Collections.Generic;
using System.IO;
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

    //define "=" : ">>" 
    //include "hea

    public static void Compile(
      List < string > code,
      List < string > numberNames,
      List < float > numberValues,
      List < string > strNames,
      List < string > strValues,
      List < string > references,
      List < string > strListNames,
      List < List < string >> strListValues,
      List < string > numListNames,
      List < List < float >> numListValues,
      List < List < string >> lines_for_functions,
      List < string > names_for_functions,
      List < string > definers_to_replace,
      List < string > defined_to_replace) {

      /*
      define =:>>
      // definers_to_replace
      // defined_to

      //to return something just declare a variable named "return", like "str return >>x"
      */

      string current_line = "";
      List < int > pixelX = new List < int > ();
      List < int > pixelY = new List < int > ();
      List < ConsoleColor > pixelColors = new List < ConsoleColor > ();
      List < string > charachtersToDraw = new List < string > ();
      List < int > pixelXChar = new List < int > ();
      List < int > pixelYChar = new List < int > ();
      Random rand = new Random();

      List < ConsoleColor > pixelColorsChar = new List < ConsoleColor > ();
      string indent_if = "	";
      string[] operands = {
        "==",
        "!=",
        ">",
        "<",
        "<=",
        ">="
      };
      //read code line by line
      // foreach (var __line in code){
      //   if(__line.Contains("$endl;")){
      //     code[code.IndexOf(__line)] = __line.Split(new [] {"$endl;"},StringSplitOptions.None)[0];
      //     int x = 0;
      //     foreach(var line__ in __line.Split(new []{"$endl;"},StringSplitOptions.None)){
      //       if(x!=0){
      //         code.Insert(code.IndexOf(__line+x),line__);
      //       }
      //       x++;
      //     }
      //   }
      // }
      try {
        //list(str) "Name"
        //str Name.Add >> "Value"
        //getStrListValue(Name[1])
        //updateStrListValue(Name[1])
        for (int _index = 0; _index < code.Count; _index++) {

          //TODO - Add general error matching case;        
          //        try{

          string line = code[_index];
          current_line = line;

          if (definers_to_replace.Count > 0) {
            for (int i = 0; i < definers_to_replace.Count; i++) {
              if (line.Contains(definers_to_replace[i])) {
                line = line.Replace(definers_to_replace[i], defined_to_replace[i]);
                code[_index] = line;
              }
            }
          }
          if(line.Split(' ').First()=="continue"){
            continue;
          }
          if (line == "\n" || line == "    \n" || line == "" || line.StartsWith("#")) {
            continue;
          }
          if (line.StartsWith("}")) {
            continue;
          }
          bool is_continue = false;
          foreach(var func_name in names_for_functions) {

            if (line.Contains(func_name + "(") && line.StartsWith(indent_if) == false && line.StartsWith("    ")==false) {
                List < string > add_args = line.Split(new [] {
                  func_name
                }, StringSplitOptions.None)[1].Split('(')[1].Split(')')[0].Split(new [] {
                ";;"
              }, StringSplitOptions.None).ToList();
              _Compile(add_args);
              if (strNames.Contains("return")) {
                strValues[strNames.IndexOf("return")] = "";
              }
              if (numberNames.Contains("return")) {
                numberValues[numberNames.IndexOf("return")] = 0;
              }

              Compile(lines_for_functions[names_for_functions.IndexOf(func_name)], numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions, definers_to_replace, defined_to_replace);
              //str x >>f(2)
              string func_call = func_name + line.Split(new [] {
                func_name
              }, StringSplitOptions.None)[1].Split('(')[0] + line.Split(new [] {
                func_name
              }, StringSplitOptions.None)[1].Split(')')[0] + ")";;
              if (strNames.Contains("return")) {
                if (strNames.Contains(func_call)) {
                  strValues[strNames.IndexOf(func_call)] = strValues[strNames.IndexOf("return")];
                } else {
                  //                  Console.WriteLine(func_call+":"+strValues[strNames.IndexOf("return")]);

                  strNames.Add(func_call);
                  strValues.Add(strValues[strNames.IndexOf("return")]);

                }
              }
              if (numberNames.Contains("return")) {
                if (numberNames.Contains(func_call)) {
                  numberValues[numberNames.IndexOf(func_call)] = numberValues[numberNames.IndexOf("return")];
                } else {
                  numberNames.Add(func_call);
                  numberValues.Add(numberValues[numberNames.IndexOf("return")]);
                }
              }

              is_continue = false;
              // break;
            }
            //int ocorrences = 0;
            // foreach(var func_name_2 in names_for_functions){
            //   if(func_name_2 == func_name){
            //     ocorrences++;
            //   }
            // }
            // if(ocorrences>=2){
            //     Console.WriteLine("There is more than 1 function named: "+func_name);
            // }

          }
          if (is_continue) {
            continue;
          }
          if (strNames.Contains("keyavailable") == false) {
            if (Console.KeyAvailable == true) {
              strNames.Add("keyavailable");
              strValues.Add("True");
            } else {
              strNames.Add("keyavailable");
              strValues.Add("False");
            }
          } else if (strNames.Contains("keyavailable") == true) {
            if (Console.KeyAvailable) {
              strValues[strNames.IndexOf("keyavailable")] = "True";
            } else {
              strValues[strNames.IndexOf("keyavailable")] = "False";
            }
          }
          if (line.Contains('$')) {
            foreach(var string_var in strNames) {
              try {
                string toReplace = "$" + string_var + "$";
                line = line.Replace(toReplace, strValues[strNames.IndexOf(string_var)]);
              } catch {}
            }
            foreach(var num_var in numberNames) {
              try {
                string toReplace = "$" + num_var + "$";
                line = line.Replace(toReplace, numberValues[numberNames.IndexOf(num_var)].ToString());

              } catch {

              }
            }

          }
          if (line.StartsWith("define ")) {
            // define "=":">>"
            if (definers_to_replace.Contains(line.Split(':')[0].Split('"')[1]) == false) {
              definers_to_replace.Add(line.Split(':')[0].Split('"')[1]);
            }

            if (defined_to_replace.Contains(line.Split(':')[1].Split('"')[1]) == false) {
              defined_to_replace.Add(line.Split(':')[1].Split('"')[1]);
            }

          }
          if (line.StartsWith("++")) {
            numberValues[numberNames.IndexOf(line.Split('+')[2].Split(';')[0])]++;
            continue;
          } else if (line.StartsWith("--")) {
            numberValues[numberNames.IndexOf(line.Split('-')[2].Split(';')[0])]--;
            continue;
          }
          if (line.StartsWith("->") && line.StartsWith("define") == false) {
            List < string > __tocompile = line.Split(new [] {
              "->"
            }, StringSplitOptions.None)[1].Split(new [] {
              ";;"
            }, StringSplitOptions.None).ToList();
            _Compile(__tocompile);
            continue;
          }

          if (line.Split(';')[0] == "stop") {
            return;

          } else if (line.Split(';')[0] == "stop_break") {
            break;
          }

          //
          // line = line.Replace("$path$", strValues[strNames.IndexOf("path")]);
          // line = line.Replace("$PATH$", strValues[strNames.IndexOf("path")]);
          if (line.StartsWith("Write") && line.Contains("\"") && line.StartsWith("WriteStr") == false && line.StartsWith("WriteNum") == false) {
            //check if it is a number 
            //var matchesNumber = numberNames.Where(x => line.Contains(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString()));

            Console.Write(line.Split(new [] {
              "Write \""
            }, StringSplitOptions.None).Last().ToString().Split(new [] {
              "\""
            }, StringSplitOptions.None).First().ToString());
            continue;
          } else if (line.StartsWith("if ")) {
            //if num:x==f:

            //  Write "Hi"

            string type_to_compare = line.Split(' ')[1].Split(':')[0];
            List<string> else_val = new List<string>();
            string operand = "";
            string statement = line.Split(':')[1].Split(':')[0];
            string name = "";
            bool _checked = false;
            foreach(var _operator in operands) {
              if (line.Contains(_operator)) {
                operand = _operator;
              }
            }
            string first_ = line.Split(new [] {
              operand
            }, StringSplitOptions.None)[0].Split(':')[1].Split(new [] {
              operand
            }, StringSplitOptions.None)[0];
            // Console.WriteLine(first_);
            string last_ = line.Split(new [] {
              operand
            }, StringSplitOptions.None)[1].Split(':')[0];
            if (type_to_compare == "str") {
              string first_to_compare = strValues[strNames.IndexOf(first_)];
              string last_to_compare = "";
              if (statement.Contains('"')) {
                last_to_compare = statement.Split('"')[1].Split('"')[0];
              } else {
                last_to_compare = strValues[strNames.IndexOf(statement.Split(new [] {
                  operand
                }, StringSplitOptions.None)[1])];
              }
              if (operand == "==") {
                _checked = first_to_compare == last_to_compare;
              } else if (operand == "!=") {
                _checked = first_to_compare != last_to_compare;
              } 

            } else if (type_to_compare == "num") {
              float first_to_compare = numberValues[numberNames.IndexOf(first_)];
              float last_to_compare = 0;
              if (numberNames.Contains(last_)) {
                last_to_compare = numberValues[numberNames.IndexOf(last_)];
              } else {
                last_to_compare = float.Parse(last_);
              }
              if (operand == "==") {
                _checked = first_to_compare == last_to_compare;
              } else if (operand == "!=") {
                _checked = first_to_compare != last_to_compare;
              } else if (operand == ">=") {
                _checked = first_to_compare >= last_to_compare;
              } else if (operand == "<=") {
                _checked = first_to_compare <= last_to_compare;
              } else if (operand == ">") {
                _checked = first_to_compare > last_to_compare;
              } else if (operand == "<") {
                _checked = first_to_compare < last_to_compare;
              }
            }

            if (_checked) {
              if (line.Split(new [] {
                  ":"
                }, StringSplitOptions.None)[1].Contains('"')) {
                name = line.Split(new [] {
                  ":"
                }, StringSplitOptions.None)[1].Split('"')[1].Split('"')[0];
              }
              try {
                int current_index = code.IndexOf(line);
                current_index++;
                List < string > start = code.GetRange(_index, code.Count - _index);

                List < string > to__compile = new List < string > ();
                 if(line.Split(':')[2]!=""){
                to__compile.Add(line.Split(':')[2]);
              }
              name = "";
              to__compile.Add("    ");
                
                for (int i = 1; i < start.Count; i++) {

                  //  Console.WriteLine(start[i]);
                  if (start[i].StartsWith(indent_if) || start[i].StartsWith("    ")) {

                    to__compile.Add(start[i]);
                  } else{
                    if(start[i]!=""){
                      break;
                    }
                  }
                }
                
                for (int i = 0; i < to__compile.Count; i++) {
                  if (to__compile[i].StartsWith(indent_if) || start[i].StartsWith("    ")) {
                    try {
                      if (to__compile[i].StartsWith(indent_if)) {
                        to__compile[i] = to__compile[i].Substring(indent_if.Length);
                      } else if (to__compile[i].StartsWith("    ")) {
                        to__compile[i] = to__compile[i].Substring("    ".Length);
                      }
                    } catch {}
                  }
                }
                _Compile(to__compile);
              } catch (Exception exc) {
                if (code.Contains("suppress_errors()") == false) {
                  Console.WriteLine("Incorrect/Missing end statement for if statement: " + name + exc);

                }
              }
              continue;
            }else
            {
              List < string > start = code.GetRange(_index, code.Count - _index);
              for (int i = 0; i < start.Count; i++)
              {
                  if(start[i].StartsWith("else:")){
                      List<string> else_start = start.GetRange(i, start.Count-i);
                      for (int p = 1; p < else_start.Count; p++)
                      {
                          if(else_start[p].StartsWith("    ")){
                            else_val.Add(else_start[p].Substring("    ".Length));
                          }else if(else_start[p].StartsWith(indent_if)){
                            else_val.Add(else_start[p].Substring(indent_if.Length));
                          }else if(else_start[p] != " " && else_start[p]!=""&&else_start[p]!="\n"){
                            break;
                          }
                      }
                    } 
              }
            
                _Compile(else_val);
            }
            continue;
          } else if (line.StartsWith("while ")) {
            //while type() 

            string type_to_compare = line.Split(' ')[1].Split(':')[0];
            string operand = "";
            string statement = line.Split(':')[1].Split(':')[0];
            string name = "";
            bool _checked = false;
            foreach(var _operator in operands) {
              if (line.Contains(_operator)) {
                operand = _operator;
              }
            }
            string first_ = line.Split(new [] {
              operand
            }, StringSplitOptions.None)[0].Split(':')[1];
            string last_ = line.Split(new [] {
              operand
            }, StringSplitOptions.None)[1].Split(':')[0];
            if (type_to_compare == "str") {
              string first_to_compare = strValues[strNames.IndexOf(first_)];
              string last_to_compare = "";
              if (statement.Contains('"')) {
                last_to_compare = statement.Split('"')[1].Split('"')[0];
              } else {
                last_to_compare = strValues[strNames.IndexOf(statement.Split(new [] {
                  operand
                }, StringSplitOptions.None)[1])];
              }
              if (operand == "==") {
                _checked = first_to_compare == last_to_compare;
              } else if (operand == "!=") {
                _checked = first_to_compare != last_to_compare;
              }

            } else if (type_to_compare == "num") {
              float first_to_compare = numberValues[numberNames.IndexOf(first_)];
              float last_to_compare = 0;
              if (numberNames.Contains(last_)) {
                last_to_compare = numberValues[numberNames.IndexOf(last_)];
              } else {
                last_to_compare = float.Parse(last_);
              }
              if (numberNames.Contains(last_)) {
                last_to_compare = numberValues[numberNames.IndexOf(last_)];
              } else {
                last_to_compare = float.Parse(last_);
              }
              if (operand == "==") {
                _checked = first_to_compare == last_to_compare;
              } else if (operand == "!=") {
                _checked = first_to_compare != last_to_compare;
              } else if (operand == ">=") {
                _checked = first_to_compare >= last_to_compare;
              } else if (operand == "<=") {
                _checked = first_to_compare <= last_to_compare;
              } else if (operand == ">") {
                _checked = first_to_compare > last_to_compare;
              } else if (operand == "<") {
                _checked = first_to_compare < last_to_compare;
              }
            }

            if (_checked) {
              while (_checked) {
                if (type_to_compare == "str") {
                  string first_to_compare = strValues[strNames.IndexOf(first_)];
                  string last_to_compare = "";
                  if (statement.Contains('"')) {
                    last_to_compare = statement.Split('"')[1].Split('"')[0];
                  } else {
                    last_to_compare = strValues[strNames.IndexOf(statement.Split(new [] {
                      operand
                    }, StringSplitOptions.None)[1])];
                  }
                  if (operand == "==") {
                    _checked = first_to_compare == last_to_compare;
                  } else if (operand == "!=") {
                    _checked = first_to_compare != last_to_compare;
                  }

                } else if (type_to_compare == "num") {
                  float first_to_compare = numberValues[numberNames.IndexOf(first_)];
                  float last_to_compare = 0;
                  if (numberNames.Contains(last_)) {
                    last_to_compare = numberValues[numberNames.IndexOf(last_)];
                  } else {
                    last_to_compare = float.Parse(last_);
                  }
                  if (operand == "==") {
                    _checked = first_to_compare == last_to_compare;
                  } else if (operand == "!=") {
                    _checked = first_to_compare != last_to_compare;
                  } else if (operand == ">=") {
                    _checked = first_to_compare >= last_to_compare;
                  } else if (operand == "<=") {
                    _checked = first_to_compare <= last_to_compare;
                  } else if (operand == ">") {
                    _checked = first_to_compare > last_to_compare;
                  } else if (operand == "<") {
                    _checked = first_to_compare < last_to_compare;
                  }
                }
                if (line.Split(new [] {
                    ":"
                  }, StringSplitOptions.None)[1].Contains('"')) {
                  name = line.Split(new [] {
                    ":"
                  }, StringSplitOptions.None)[1].Split('"')[1].Split('"')[0];
                }
                try {
                  int current_index = code.IndexOf(line);
                  current_index++;
                  List < string > to__compile = new List < string > ();
                  List < string > start = code.GetRange(_index, code.Count - _index);

                  to__compile = new List < string > ();
                  if(line.Split(':')[2]!=""){
                to__compile.Add(line.Split(':')[2]);
              }
              name = "";
              to__compile.Add("    ");
                  for (int i = 1; i < start.Count; i++) {

                    //  Console.WriteLine(start[i]);
                    if (start[i].StartsWith(indent_if) || start[i].StartsWith("    ")) {
                      to__compile.Add(start[i]);
                    } else {
                      
                      if(start[i]!=""){
                      break;
                    }
                    }

                  }
                  
                  
                  for (int i = 0; i < to__compile.Count; i++) {
                    if (to__compile[i].StartsWith(indent_if) || start[i].StartsWith("    ")) {
                      try {
                        if (to__compile[i].StartsWith(indent_if)) {
                          to__compile[i] = to__compile[i].Substring(indent_if.Length);
                        } else if (to__compile[i].StartsWith("    ")) {
                          to__compile[i] = to__compile[i].Substring("    ".Length);
                        }
                      } catch {}
                    }
                  }
                  if (_checked) {
                  
                    _Compile(to__compile);
                  }
                } catch {
                  if (code.Contains("suppress_errors()") == false) {
                    Console.WriteLine("Incorrect/Missing end statement for if statement: " + name);

                  }
                }
              }
              continue;
            }
            continue;
          }
          if (line.ToLower().StartsWith("newline") || line.ToLower().StartsWith("newln")) {
            Console.WriteLine("");
            continue;
          } else if (line.StartsWith("WriteStr") &&
            line.Contains("{") &&
            line.Contains("}")) {
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
            continue;
          } else if (line.StartsWith("WriteNum") &&
            line.Contains("{") &&
            line.Contains("}"))

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
            continue;
          } else if (line.StartsWith("str") &&
            line.Contains(">>") &&
            line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false &&
            line.Contains("$>") == false) {
            try {
              if (strNames.Contains(line.Split(' ')[1].Split('>').First())) {
                if (line.Split('>').Last().ToLower().StartsWith("$read") == false) {
                  if(line.Contains("\"")){
                    strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = line.Split('>')[2].Split('\"')[1].Split('\"').First();
                  }else{
                    
                      strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = strValues[strNames.IndexOf(line.Split('>')[2])];
                    
                  }
                } else if (line.Split('>').Last().ToLower().Split(' ')[0] == "$readkey") {
                  if (Console.KeyAvailable) {
                    strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Console.ReadKey().Key.ToString();
                  }
                } else if (line.Split('>').Last().ToLower().Split(' ')[0] == "$readline") {
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
                  if (Console.KeyAvailable) {
                    strNames.Add(line.Split(' ')[1].Split('>').First());
                    strValues.Add(Console.ReadKey().Key.ToString());
                  }

                } else if (line.Split('>').Last().ToLower() == ("$readline")) {
                  strNames.Add(line.Split(' ')[1].Split('>').First());
                  strValues.Add(Console.ReadLine());
                }
              }

            } catch (Exception exc) {
              int errorLine = code.IndexOf(line);
              Console.WriteLine(line);
              Console.WriteLine($"Invalid Syntax (Line {errorLine++}) Exception: " + exc);
            }
            if (line.ToLower().Contains("add") != true) {
              continue;
            }
          } else if (line.StartsWith("number ") || line.StartsWith("num ") && line.Contains(">>") &&
            line.ToLower().StartsWith("for") == false &&
            line.ToLower().Contains("in range %") == false &&
            line.Contains("$>") == false) {
            rand = new Random();
            try {
              if (numberNames.Contains(line.Split(' ')[1].Split('>').First())) {
                if (line.ToLower().Contains("$readline") == false) {
                  try {
                    if (line.Split('>').Last().Contains("rand:")) {
                      System.Threading.Thread.Sleep(1);
                      numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = rand.Next(Convert.ToInt32(line.Split(new [] {
                        "rand:"
                      }, StringSplitOptions.None).Last().Split(',').First()), Convert.ToInt32(line.Split(new [] {
                        "rand:"
                      }, StringSplitOptions.None).Last().Split(',').Last()));
                    } else {
                      numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = float.Parse(line.Split('>').Last());
                    }
                  } catch {
                    if (line.Split('>').Last().Contains("rand:")) {
                      System.Threading.Thread.Sleep(1);
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
                  numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = float.Parse(Console.ReadLine());
                }
              } else {
                if (line.ToLower().Contains("$readline") == false) {
                  numberNames.Add(line.Split(' ')[1].Split('>').First());
                  try {
                    if (line.Split('>').Last().Contains("rand:")) {
                      System.Threading.Thread.Sleep(1);
                      numberValues.Add(rand.Next(Convert.ToInt32(line.Split(new [] {
                        "rand:"
                      }, StringSplitOptions.None).Last().Split(',').First()), Convert.ToInt32(line.Split(new [] {
                        "rand:"
                      }, StringSplitOptions.None).Last().Split(',').Last())));
                    } else {
                      try {
                        numberValues.Add(float.Parse(line.Split('>').Last().Split(' ').Last()));
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
                      numberValues.Add(float.Parse(line.Split('>').Last()));
                    }
                  }
                } else if (line.ToLower().Contains("$readline") == true) {
                  numberNames.Add(line.Split(' ')[1].Split('>').First());
                  numberValues.Add(float.Parse(Console.ReadLine()));
                }

              }
            } catch {
              int errorLine = code.IndexOf(line);
              Console.WriteLine($"Invalid Syntax (Line {errorLine--})");
            }
            continue;
          } else if (line.ToLower().StartsWith("for") && line.ToLower().Contains("in range:")) {
            int range = 0;
            /*
                                     for x in range:

                                 */
            string looper = "";
            //     string indent = "";
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
                }, StringSplitOptions.None).Last().Split(':')[0]);
              } catch {
                try {
                  range = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                    "in range:"
                  }, StringSplitOptions.None).Last().Split(':')[0])]);
                } catch {
                  range = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split(new [] {
                    "in range:"
                  }, StringSplitOptions.None).Last().Split(':')[0])]);
                }
              }

              name = "";
              //              indent = "    ";
            } catch (Exception exc) {
              Console.WriteLine(exc);
            }
            List < string > to__compile = new List < string > ();
            try {
              int current_index = code.IndexOf(line);
              current_index++;
              //0
              //1
              //2
              //3
              //::4
              List < string > start = code.GetRange(_index, code.Count - _index);
              to__compile = new List < string > ();
              
              if(line.Split(':')[2]!=""){
                to__compile.Add(line.Split(':')[2]);
              }
              
              to__compile.Add("    ");

              for (int i = 1; i < start.Count; i++) {
                //  Console.WriteLine(start[i]);
                if (start[i].StartsWith(indent_if) || start[i].StartsWith("    ")) {
                  to__compile.Add(start[i]);
                  

                } else {
                  if(start[i]!=""){
                      break;
                    }
                }

              }
            
              for (int i = 0; i < to__compile.Count; i++) {

                if (to__compile[i].StartsWith(indent_if) || start[i].StartsWith("    ")) {
                  try {
                    if (to__compile[i].StartsWith(indent_if)) {
                      to__compile[i] = to__compile[i].Substring(indent_if.Length);
                    } else if (to__compile[i].StartsWith("    ")) {
                      to__compile[i] = to__compile[i].Substring("    ".Length);
                    }
                  } catch {}
                }
              }
            } catch (Exception exc){

              if (code.Contains("suppress_errors()") == false) {
                Console.WriteLine("Incorrect/Missing end statement for for loop: " + name);

              }
              if(code.Contains("show_exceptions()")==true){
                Console.WriteLine(exc);
              }
            }
            for (int x = 0; x < range; x++) {
              if (numberNames.Contains(looper)) {
                numberValues[numberNames.IndexOf(looper)] = x;
              } else {
                numberValues.Add(x);
                numberNames.Add(looper);
              }
              _Compile(to__compile);
              continue;
              //continue;
            }
            // _Compile(compileAfter);
            continue;
          } else if (references.Contains("threading") && line.StartsWith("newThread(")) {
            try {
              string nameof_func = line.Split('(')[1].Split('(')[0];
              void newThreadStart() {
                rCompiler.Compile(lines_for_functions[names_for_functions.IndexOf(nameof_func)], numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions, definers_to_replace, defined_to_replace);
                Console.WriteLine("");
              }

              ThreadStart thread_start = new ThreadStart(newThreadStart);
              Thread newThread = new Thread(thread_start);
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
            List < string > func_content = new List < string > ();
            List < string > compileAfter = new List < string > ();
            int current_index = code.IndexOf(line);
            current_index++;
            List < string > start = code.GetRange(_index, code.Count - _index);
            func_content = new List < string > ();
            for (int i = 1; i < start.Count; i++) {
              if (start[i].StartsWith(indent_if) || start[i].StartsWith("    ")) {
                func_content.Add(start[i]);

              } else {
                if(start[i]!=""){
                      break;
                    }
              }

            }

            // try {
            //   func_content = code.GetRange(current_index, code.Count - current_index).GetRange(0, (code.GetRange(current_index, code.Count - current_index).IndexOf("};") - 0));
            //   _curr_i++;
            // } catch (Exception exc) {
            //   Console.WriteLine(exc);
            //   try {
            //     func_content = code.GetRange(current_index, code.Count - current_index).GetRange(0, (code.GetRange(current_index, code.Count - current_index).IndexOf("}" + nameFunc + ";") - 0));
            //   } catch {

            //     Console.WriteLine("Incorrect/Missing end statement for function: " + nameFunc);
            //   }
            // }
            string indent = indent_if;
            for (int i = 0; i < func_content.Count; i++) {
              if (func_content[i].StartsWith(indent) || func_content[i].StartsWith("    ")) {
                try {
                  if (func_content[i].StartsWith(indent_if)) {
                    func_content[i] = func_content[i].Substring(indent.Length);
                  } else if (func_content[i].StartsWith("    ")) {
                    func_content[i] = func_content[i].Substring("    ".Length);
                  }
                } catch {}
              }
            }
            if(line.Split(':')[1]!=""){
                func_content.Add(line.Split(':')[1]);
              }
              
              func_content.Add("    ");
            lines_for_functions.Add(func_content);
            //Compile(compileAfter, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions, definers_to_replace, defined_to_replace);
            continue;
          }

          if (line.StartsWith("strip(")) {
            string toStrip = strValues[strNames.IndexOf(line.Split('(').Last().Split(')').First())];
            string cleaned = toStrip.Replace("\r", "").Replace("\n", "");
            strValues[strNames.IndexOf(line.Split('(').Last().Split(')').First())] = cleaned;
            continue;
          }

          if (line.StartsWith("strToNum(")) {
            string toConvert = strValues[strNames.IndexOf(line.Split('(').Last().Split(')').First())];
            if (numberNames.Contains(line.Split('(').Last().Split(')').First())) {
              numberValues[numberNames.IndexOf(line.Split('(').Last().Split(')').First())] = float.Parse(toConvert);
            } else {
              numberNames.Add(line.Split('(').Last().Split(')').First());
              numberValues.Add(float.Parse(toConvert));
            }
            continue;
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
              float stringToChange = numberValues[numberNames.IndexOf(line.Split('(')[1].Split(')')[0])];
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
          if (line.ToLower().StartsWith("sqrt")) {

            //sqrt(8) -> (1 )0
            string str_to_sqrt = line.Split('(')[1].Split(')')[0];
            if (numberNames.Contains(str_to_sqrt)) {
              numberValues[numberNames.IndexOf(str_to_sqrt)] = float.Parse(Math.Sqrt(Convert.ToDouble(numberValues[numberNames.IndexOf(str_to_sqrt)])).ToString());
            } else {
              numberNames.Add(line);
              numberValues.Add(float.Parse(Math.Sqrt(Convert.ToDouble(line.Split('(')[1].Split(')')[0])).ToString()));
            }
            continue;
          }

          if (line.StartsWith("list(str) ")) {
            strListNames.Add(line.Split(')')[1].Split('\"')[1].Split('\"').First());
            strListValues.Add(new List < string > ());

            // list(str) "my_list" >>["1,2,3,"4","5"]
            if (line.Contains(">>")) {
              List < string > values = line.Split('[').Last().Split(']')[0].Split(',').ToList();
              for (int i = 0; i < values.Count; i++) {
                if (values[i].Contains('"')) {
                  strListValues[strListNames.IndexOf(line.Split(')')[1].Split('\"')[1].Split('\"').First())].Add(values[i].Split('"')[1].Split('"')[0]);
                } else {
                  if (strNames.Contains(values[i])) {
                    strListValues[strListNames.IndexOf(line.Split(')')[1].Split('\"')[1].Split('\"').First())].Add(strValues[strNames.IndexOf(values[i])]);
                  } else {
                    Console.WriteLine("String " + values[i] + "does not exist");
                  }
                }
              }
            }
          }
          if (line.StartsWith("list(num)")) {
            numListNames.Add(line.Split(')')[1].Split('\"')[1].Split('\"').First());
            numListValues.Add(new List < float > ());
            if (line.Contains(">>")) {
              List < string > values = line.Split('[').Last().Split(']')[0].Split(',').ToList();
              for (int i = 0; i < values.Count; i++) {
                if (numberNames.Contains(values[i])) {
                  numListValues[numListNames.IndexOf(line.Split(')')[1].Split('\"')[1].Split('\"').First())].Add((numberValues[numberNames.IndexOf(values[i])]));
                } else {
                  try {
                    numListValues[numListNames.IndexOf(line.Split(')')[1].Split('\"')[1].Split('\"').First())].Add(float.Parse(values[i]));
                  } catch {
                    if (code.Contains("suppress_errors()") == false) {
                      Console.WriteLine("Error while adding: " + values[i] + " to list: " + line.Split(')')[1].Split('\"')[1].Split('\"').First());
                    }
                  }
                }
              }
            }

          }
          foreach(var listName in strListNames) {
            // if(line.Contains(listName) && line.Contains('[')){
            //   // try{
            //     string toParse = line.Split(new []{listName},StringSplitOptions.None)[1].Split(']')[0];
            //     toParse = listName+toParse+']';
            //     int indx = 0;
            //     string isNum = toParse.Split('[')[1].Split(']')[0];
            //     if(numberNames.Contains(isNum)){
            //       indx = Convert.ToInt32(numberValues[numberNames.IndexOf(isNum)]);
            //     }else{
            //       indx = Convert.ToInt32(isNum);
            //     }
            //      if(strNames.Contains(toParse)){
            //        strValues[strNames.IndexOf(toParse)] = strListValues[strListNames.IndexOf(listName)][indx]; 
            //      }else{

            //        strNames.Add(toParse);
            //        Console.WriteLine(strListNames.Count());
            //      }
            // }catch{
            //   Console.WriteLine("Error at line: "+line);
            // }
            // }

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

            if (line.StartsWith(listName + ".RemoveAt")) {
              int index = 0;
              if (numberNames.Contains(line.Split('(')[1].Split(')')[0])) {
                index = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('(')[1].Split(')')[0])]);
              } else {
                index = Convert.ToInt32(line.Split('(')[1].Split(')')[0]);
              }
              strListValues[strListNames.IndexOf(listName)].RemoveAt(index);
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
                }, StringSplitOptions.None).Last().Split(';').First())] = numListValues[numListNames.IndexOf(listName)].IndexOf(float.Parse(stringToGetIndex));
              } catch {
                throw new Exception("Fatal error on line: " + code.IndexOf(line));
              }

            }
            if (line.StartsWith(listName + ".RemoveAt")) {
              int index = 0;
              if (numberNames.Contains(line.Split('(')[1].Split(')')[0])) {
                index = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('(')[1].Split(')')[0])]);
              } else {
                index = Convert.ToInt32(line.Split('(')[1].Split(')')[0]);
              }
              numListValues[numListNames.IndexOf(listName)].RemoveAt(index);
            }
            foreach(var name in numberNames) {
              if (name.ToLower().StartsWith(listName.ToLower() + ".add")) {
                if (numListValues[numListNames.IndexOf(listName)].Contains(numberValues[numberNames.IndexOf(name)]) == false) {
                  numListValues[numListNames.IndexOf(listName)].Add(numberValues[numberNames.IndexOf(name)]);
                }
              }
            }
          }
          if (line.ToLower().StartsWith("lnval(") || line.StartsWith("lnval (")) {
            int index = 0;
            try {
              if (numberNames.Contains(line.Split('[')[1].Split(']')[0])) {
                index = Convert.ToInt32(numberValues[numberNames.IndexOf(line.Split('[')[1].Split(']')[0])]);
              }else {
                index = Convert.ToInt32(line.Split('[')[1].Split(']')[0]);
              }
                if (numberNames.Contains(line.Split('(')[1].Split(')')[0])) {
                  numberValues[numberNames.IndexOf(line.Split('(')[1].Split(')')[0])] = numListValues[numListNames.IndexOf(line.Split('(')[1].Split('[')[0])][index];
                } else {
                  numberNames.Add(line.Split('(')[1].Split(')')[0]);
                  numberValues.Add(numListValues[numListNames.IndexOf(line.Split('(')[1].Split('[')[0])][index]);
                }
            
            } catch {
              Console.WriteLine("\nFatal Error (Syntax) On Line: " + code.IndexOf(line));
            }
            continue;
          }
          if (line.ToLower().StartsWith("lsval(") || line.StartsWith("lsval (")) {
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
            continue;
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

          if (line.StartsWith("numToStr")) {

            string to_convert = line.Split('(')[1].Split(')')[0];
            if (strNames.Contains(to_convert) && numberNames.Contains(to_convert)) {
              strValues[strNames.IndexOf(to_convert)] = numberValues[numberNames.IndexOf(to_convert)].ToString();
            } else {
              strNames.Add(to_convert);
              strValues.Add(numberValues[numberNames.IndexOf(to_convert)].ToString());
            }
            continue;

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
              continue;
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
            // if (line == "exit()") {
            //   Environment.Exit(1);
            // }
            is_continue = false;
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
                  List < string > add_args = line.Split('(')[1].Split(')')[0].Split(new [] {
                    ";;"
                  }, StringSplitOptions.None).ToList();
                  _Compile(add_args);
                  Compile(linesFromFile, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions, definers_to_replace, defined_to_replace);
                  //Environment.Exit(1);
                  is_continue = true;
                  break;
                } catch {
                  if (code.Contains("suppress_errors()") == false) {
                    Console.WriteLine("Method does not exist at line of index: " + code.IndexOf(line));
                  }
                }
              }
            }
            if (is_continue) {
              continue;
            }
            if (line.StartsWith("include ")) {
              //include "headers.h.rcode"
              string file_to_read = line.Split('"')[1].Split('"')[0];
              StreamReader _line_reader = File.OpenText(file_to_read);
              List < string > to__compile = new List < string > ();
              string _current_line = "";
              while ((_current_line = _line_reader.ReadLine()) != null) {
                to__compile.Add(_current_line);
              }
              _Compile(to__compile);
              continue;
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
                Compile(newCompile.GetRange(firstIndex, (lastIndex - firstIndex)), numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions, definers_to_replace, defined_to_replace);
              } catch {
                if (line.Split(new [] {
                    "content:"
                  }, StringSplitOptions.None).Last().Split(';').First() == "all") {
                  List < string > newCompile = code;
                  Compile(newCompile, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions, definers_to_replace, defined_to_replace);
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
                    Compile(newCompile.GetRange(firstIndex, (lastIndex - firstIndex)), numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions, definers_to_replace, defined_to_replace);
                  } catch {

                  }
                }
              }
            }
            bool is_continue_num = false;
            foreach(var num in numberNames) {

              if (line.StartsWith(num.ToString() + "++")) {
                is_continue_num = true;
                float newNum = numberValues[numberNames.IndexOf(num)];
                newNum++;
                numberValues[numberNames.IndexOf(num)] = newNum;
                break;

              } else if (line.StartsWith(num.ToString() + "--")) {
                is_continue_num = true;

                float newNum = numberValues[numberNames.IndexOf(num)];
                newNum--;
                numberValues[numberNames.IndexOf(num)] = newNum;
                break;

              } else if (line.StartsWith(num.ToString() + "+")) {
                is_continue_num = true;

                try {
                  float newNum = numberValues[numberNames.IndexOf(num)];
                  newNum = newNum + float.Parse(line.Split('+').Last());
                  numberValues[numberNames.IndexOf(num)] = newNum;
                  break;

                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      if (line.Split('+').Last() == getNum) {
                        float newNum = numberValues[numberNames.IndexOf(num)];
                        newNum = newNum + numberValues[numberNames.IndexOf(getNum)];
                        numberValues[numberNames.IndexOf(num)] = newNum;
                        break;
                      }
                    }
                  } catch {

                  }
                }
              }
              if (line.StartsWith(num.ToString() + "-")) {
                is_continue_num = true;

                try {
                  numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] - float.Parse(line.Split('-').Last());
                  continue;

                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      if (line.Split('-').Last() == getNum) {
                        numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] - numberValues[numberNames.IndexOf(getNum)];
                        break;
                      }
                    }
                  } catch {

                  }
                }
              }
              if (line.StartsWith(num.ToString() + "/")) {
                is_continue_num = true;

                try {
                  numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] / float.Parse(line.Split('/').Last());
                  break;

                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      if (line.Split('/').Last() == getNum) {
                        numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] / numberValues[numberNames.IndexOf(getNum)];
                        break;
                      }
                    }
                  } catch {

                  }
                }
              }
              if (line.StartsWith(num.ToString() + "*")) {
                is_continue_num = true;

                try {
                  numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] * float.Parse(line.Split('*').Last());
                  break;
                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      is_continue_num = true;

                      if (line.Split('*').Last() == getNum) {
                        numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] * numberValues[numberNames.IndexOf(getNum)];
                        break;
                      }
                    }
                  } catch {

                  }
                }
              }
              if (line.StartsWith(num.ToString() + "%")) {
                is_continue_num = true;

                try {
                  numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] % float.Parse(line.Split('%').Last());
                  break;
                } catch {
                  try {
                    foreach(var getNum in numberNames) {
                      if (line.Split('%').Last() == getNum) {
                        numberValues[numberNames.IndexOf(num)] = numberValues[numberNames.IndexOf(num)] % numberValues[numberNames.IndexOf(getNum)];
                        break;
                      }
                    }
                  } catch {

                  }
                }
              }
              continue;
            }
            if (is_continue_num) {
              continue;
            }
            //to lower
            /*USAGE
            ___________________________________________________________________________________________________
            str f >> 
            str name >>$readline
            toLower (f>>name)      <--- gets the value of name.toLower and assigns it to the variable f
            toLower (name)    <--- gets the value of name.toLower and assigns it to itself
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
                continue;
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
                string filename = "";
                try {
                  filename = line.Split(new [] {
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
                } catch {
                  Console.WriteLine("File: " + filename + " not found!");
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
              continue;
            } else if (line.ToLower().StartsWith("color.blue")) {
              Console.ForegroundColor = ConsoleColor.Blue;
              continue;

            } else if (line.ToLower().StartsWith("color.red")) {
              Console.ForegroundColor = ConsoleColor.Red;
              continue;

            } else if (line.ToLower().StartsWith("color.magenta")) {
              Console.ForegroundColor = ConsoleColor.Magenta;
              continue;

            } else if (line.ToLower().StartsWith("color.yellow")) {
              Console.ForegroundColor = ConsoleColor.Yellow;
              continue;

            } else if (line.ToLower().StartsWith("color.white")) {
              Console.ForegroundColor = ConsoleColor.White;
              continue;

            }

            if (line.ToLower() == ("$readline")) {
              Console.ReadLine();
              continue;

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
              continue;

            }

            //string definer

            //Write 

            /* foreach (var item in numberValues)
             {
                 Console.WriteLine(item);
             }
             foreach (var d in numberNames)
             {nu
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
        }
      } catch (Exception exc) {
        string f = current_line;
        bool except_errors = code.Contains("suppress_errors()");
        bool show_exceptions = code.Contains("show_exceptions()");
        if (show_exceptions) {
          Console.WriteLine("Error, Exception: " + exc);
        }
        if (except_errors == true) {

        } else {
          Console.WriteLine("Error while running the following: " + f);
        }
      }

      // Console.ResetColor();
      // void ForLoop(int range,
      //   string looper, List < string > loopContent)

      // {

      // }
      void _Compile(List < string > to_compile_) {
        Compile(to_compile_, numberNames, numberValues, strNames, strValues, references, strListNames, strListValues, numListNames, numListValues, lines_for_functions, names_for_functions, definers_to_replace, defined_to_replace);

      }
    }

  }
}
