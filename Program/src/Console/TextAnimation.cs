using System;
using Pastel;

namespace Visual
{
    public class TextAnimation
    {
        public static void Blink(string text, int count, int onScreen = 500, int offScreen = 200)
        {
            Console.CursorVisible = false;
            for (int i = 0; i < count; i++)
            {
                Console.Write(text);
                Task.Delay(onScreen).Wait(); // o Thread.Sleep(delay);
                Console.Write("\b \b");
                Task.Delay(offScreen).Wait();
            }
            Console.CursorVisible = true;
        }

        public static void AnimateTyping(string text, int delay = 25, int x = 0, int y = 0, string hexColor = "#FFFFFFF")
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(x,y);
            foreach (char c in text)
            {
               Console.Write(c.ToString().Pastel(hexColor));
                Task.Delay(delay).Wait(); // o Thread.Sleep(delay);

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        Console.Write(text.Substring(text.IndexOf(c) + 1));
                        break;
                    }
                }
            }
            Console.CursorVisible = true;
        }

        public static void AnimateFrames(string[] frames, int delay = 100, int count = 1, string print = "#FFFFFF", int x = 0, int y = 0)
        {
            Console.CursorVisible = false;
            
            for (int i = 0; i < count; i++)
            {
                foreach (string frame in frames)
                {
                    Console.Clear();
                    Draw.PrintAt(frame, x, y, print);
                    Task.Delay(delay).Wait(); // o Thread.Sleep(delay);
                }
            }
            Console.CursorVisible = true;
        }
    }
}