using static System.Console;

namespace Visual
{
    public class Menu
    {
        private int SelectedIndex;
        private string[] Options;
        private string Prompt;
        private Draw draw = new Draw();

        public Menu(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
            SelectedIndex = 0;
        }
        public void DisplayOptions()
            {
                ForegroundColor = ConsoleColor.Red;
                // Console.SetCursorPosition(1,1);
                System.Console.WriteLine();
                Console.WriteLine(Prompt);

                for (int i = 0; i < Options.Length; i++)
                {
                    string currentOption = Options[i];
                    string prefix;
                    if (i == SelectedIndex)
                    {
                        ForegroundColor = ConsoleColor.Red;
                        prefix = "*";
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.White;
                        prefix = "";
                    }
                    Console.SetCursorPosition(Console.BufferWidth/2, Console.BufferHeight/2 +i);
                    Console.WriteLine($"{prefix}  {currentOption}");
                }
                Draw.DrawBorders("#FF0000");
                ResetColor();
            }

        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions();
                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if(keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    SelectedIndex = SelectedIndex < 0 ? Options.Length - 1 : SelectedIndex;
                }
                else if(keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    SelectedIndex = SelectedIndex >= Options.Length ? 0 : SelectedIndex;
                }
            }
            while(keyPressed != ConsoleKey.Enter);

            return SelectedIndex;
        }
    }
}