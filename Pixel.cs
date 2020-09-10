using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rC
{
    public class Pixel
    {
        public static void Draw(int x, int y, ConsoleColor colorToDraw)
        {
            var startPixelTop = Console.CursorTop;
        
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = colorToDraw;
            Console.Write(" ");
            Console.ResetColor();
            Console.SetCursorPosition(0, startPixelTop);
            
        }
        public static void DrawChar(int x, int y, ConsoleColor colorToDraw, string characterToDraw)
        {
            var startPixelTop = Console.CursorTop;

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = colorToDraw;
            Console.Write(characterToDraw);
            Console.ResetColor();
            Console.SetCursorPosition(0, startPixelTop);

        }
    }
}
