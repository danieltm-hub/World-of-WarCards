using System;
using Figgle;
using Pastel;
using AST;
using GameProgram;
using System.Text.RegularExpressions;


namespace Visual
{
    class Draw
    {
        ///<summary>Prints a string in a given position (x,y) of the console </summary>
        public static void PrintAt(string s, int x, int y, string hexColor = "#FFFFFF")
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s.Pastel(hexColor));
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
        ///<summary>Prints an array of pixels in a given position (x,y) of the console </summary>
        protected static void PrintImage(string[] s, int x, int y, string hexColor = "#FFFFFF")
        {
            int n = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                Console.SetCursorPosition(x, y + n);
                Console.Write(s[i].Pastel(hexColor));
                n--;
            }
        }
        ///<summary>Prints a string in a given area (x-xx,y-yy) of the console </summary>
        protected static void WriteAt(string s, int x, int y, int xx, int yy, string hexColor = "#FFFFFF")
        {
            try
            {
                for (int i = x; i <= xx; i++)
                {
                    for (int j = y; j <= yy; j++)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(s.Pastel(hexColor));
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }
        ///<summary>Writes a formated text in a given area (x-width,y-height) of the console </summary>
        public static void WriteText(string text, int x, int y, int width, int height, string hexColor = "#FFFFFF", string bgColor = "#000000")
        {
            List<string> print = TextManagment.Normalize(text, width, height);
            foreach (string item in print)
            {
                PrintAt(item.PastelBg(bgColor), x, y, hexColor);
                y++;
            }
        }
        ///<summary>Prints a game card </summary>
        public static void PrintCard(string cardName, double value, double cooldown, int x, int cardWidth, int cardHeight, string heColor = "#FFFFFF")//hacer despues el bottom)
        {
            if (cardName.Length > cardWidth - 1) cardName = cardName.Substring(0, cardWidth - 1);
            string width = "";
            for (int i = 0; i < cardWidth - 2; i++)
            {
                width += "═";
            }
            PrintAt(("╔" + width + "╗").Pastel(heColor), x * cardWidth + 1, cardHeight);
            PrintAt(cardName, x * cardWidth + 2, cardHeight + 1);
            WriteAt("║".Pastel(heColor), x * cardWidth + 1, cardHeight + 1, x * cardWidth + 1, Console.BufferHeight - 1);
            if (x != 0) WriteAt("║".Pastel(heColor), (x + 1) * cardWidth, cardHeight + 1, (x + 1) * cardWidth, Console.BufferHeight - 1);
            else { WriteAt("║".Pastel(heColor), cardWidth, cardHeight + 1, cardWidth, Console.BufferHeight - 1); }
            PrintAt(("╠" + width + "╣").Pastel(heColor), x * cardWidth + 1, cardHeight + 2);
            PrintAt(("╚" + width + "╝").Pastel(heColor), x * cardWidth + 1, Console.BufferHeight - 2);
        }
        ///<summary>Prints a game card with the description</summary>
        public static void DrawSelectedCard(string cardSName, string about, int x, int cardSHeight, int cardSWidth)
        {
            string hexColor = "#FFFFFF";
            if (cardSName.Length > cardSWidth - 1) cardSName = cardSName.Substring(0, cardSWidth - 1);
            string width = "";

            for (int i = 0; i < cardSWidth - 2; i++)
            {
                width += "═";
            }

            WriteAt(" ".PastelBg("#222582"), x * cardSWidth + 1, cardSHeight, x * cardSWidth + cardSWidth, cardSHeight + cardSHeight); //background
            PrintAt(("╔" + width + "╗").PastelBg("#222582"), x * cardSWidth + 1, cardSHeight);
            PrintAt(cardSName.PastelBg("#222582"), x * cardSWidth + 2, cardSHeight + 1);
            WriteAt("║".PastelBg("#222582"), x * cardSWidth + 1, cardSHeight + 1, x * cardSWidth + 1, Console.BufferHeight - 1);
            if (x != 0) WriteAt("║".PastelBg("#222582"), (x + 1) * cardSWidth, cardSHeight + 1, (x + 1) * cardSWidth, Console.BufferHeight - 1);
            else { WriteAt("║".PastelBg("#222582"), cardSWidth, cardSHeight + 1, cardSWidth, Console.BufferHeight - 1); }
            PrintAt(("╠" + width + "╣").PastelBg("#222582"), x * cardSWidth + 1, cardSHeight + 2);
            PrintAt(("╚" + width + "╝").PastelBg("#222582"), x * cardSWidth + 1, Console.BufferHeight - 2);
            WriteText(about, x * cardSWidth + 2, cardSHeight + 3, cardSWidth - 2, cardSHeight, hexColor, "#222582");
        }
        ///<summary>Draws the border of the console</summary>
        public static void DrawBorders(string hexColor = "#FFFFFF")
        {
            WriteAt("═", 0, 0, Console.BufferWidth, 0, hexColor);
            WriteAt("═", 0, Console.BufferHeight, Console.BufferWidth, Console.BufferHeight, hexColor);
            WriteAt("║", 0, 0, 0, Console.BufferHeight, hexColor);
            WriteAt("║", Console.BufferWidth, 0, Console.BufferWidth, Console.BufferHeight, hexColor);
            PrintAt("╔", 0, 0, hexColor);
            PrintAt("╗", Console.BufferWidth, 0, hexColor);
            PrintAt("╚", 0, Console.BufferHeight, hexColor);
            PrintAt("╝", Console.BufferWidth, Console.BufferHeight, hexColor);
        }
        ///<summary>Draws the border of the game</summary>
        public static void DrawBordersExtra(int midConsole, int fifthConsole, int bottomBorderY, int maxHeight)
        {
            PrintAt("╦", midConsole - fifthConsole, 0, "#FF0000"); PrintAt("╦", midConsole + fifthConsole, 0, "#FF0000");
            WriteAt("║", midConsole - fifthConsole, 1, midConsole - fifthConsole, maxHeight - bottomBorderY, "#FF0000");
            WriteAt("║", midConsole + fifthConsole, 1, midConsole + fifthConsole, maxHeight - bottomBorderY, "#FF0000");
            WriteAt("═", midConsole - fifthConsole, maxHeight - bottomBorderY, midConsole + fifthConsole, maxHeight - bottomBorderY, "#FF0000");
            PrintAt("╚", midConsole - fifthConsole, maxHeight - bottomBorderY, "#FF0000");
            PrintAt("╝", midConsole + fifthConsole, maxHeight - bottomBorderY, "#FF0000");
            PrintAt($"{GameManager.CurrentGame.CurrentPlayer.Name} es tu turno", midConsole - fifthConsole + 1, 1, "#FF0000");

            //borders bottom
            WriteAt("═", 1, bottomBorderY, Console.BufferWidth - 1, bottomBorderY, "#FF0000"); //cartas
            PrintAt("╣", Console.BufferWidth, bottomBorderY, "#FF0000");
            PrintAt("╠", 0, bottomBorderY, "#FF0000");
        }
        ///<summary>Draws the floor of the game</summary>
        public static void DrawFloor(int bottomBorderY)
        {
            WriteAt("█", 1, bottomBorderY - 2, Console.BufferWidth - 2, bottomBorderY - 1, "#AB6A18");
        }
        ///<summary>Prints the players Stats</summary>
        public static void PrintPlayerStats(List<Player> list)
        {
            int x = 2;
            int y = 1;
            string hexColor = "#FF0000";
            foreach (Player player in list)
            {
                PrintAt(player.Name, x, y, hexColor);
                PrintAt($" Health: {Bars(player.Health, player.MaxHealth)} " + $"{player.Health}/{player.MaxHealth}", x, y + 1, "#00FF00");
                PrintAt($" Energy: {Bars(player.Energy, player.MaxEnergy)} " + $"{player.Energy}/{player.MaxEnergy}", x, y + 3, "#FFFF00");
                PrintAt($" Will: {WillDots(player.GetWill())} ", x, y + 5, "#e6b700");
                x = Console.BufferWidth / 2 + Console.BufferWidth / 5 + 1;
                hexColor = "#0000FF";
            }
        }

        private static string Bars(double actual, double max)
        {
            int adjusted = (int)((actual*40)/max);
            string bar = "<";
            for (int i = 0; i < 40; i++)
            {
                if (i < adjusted) bar += "█";
                else bar += "░";
            }
            bar += ">";
            return bar;
        }

        private static string WillDots(int n)
        {
            string dots = "";
            for (int i = 0; i < n; i++)
            {
                dots += " • ";
            }
            return dots;
        }

        ///<summary>Draws the characters image</summary>
        public static void DrawPlayerImage(int bottomBorderY)
        {
            string[] orc =    {"            █        █      █ ",
                               "            █ ██████ █       █",
                               "             ██ ██ ██        █",
                               "             ████████       █ ",
                               "             ████          █  ",
                               "  █████████  ████████     █   ",
                               "  █████████  ████████    ██   ",
                               "  ████████████████████  ██    ",
                               "  █████████  ████████████     ",
                               "   ███████   ██████████       ",
                               "    █████    ███████          ",
                               "             █▒▒▒█            ",
                               "             █▒▒▒█            ",
                               "             █▒▒▒█            ",
                               "▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒"};

            string[] elf =    {"                              ",
                               "           ▒▒▒▒▒▒░            ",
                               "           █████▒▒            ",
                               "           █ █ █▒▒▒░          ",
                               "          ██████▒▒▒▒░         ",
                               "       █  ██████▒▒▒▒▒░        ",
                               "      █ ▒ ██████▒▒▒▒▒░        ",
                               "     █   ▒  ████ ▒▒░          ",
                               "  <--███████████ ▒            ",
                               "     █   ▒  ████▌             ",
                               "      █ ▒   ████▌             ",
                               "       █    █░░█              ",
                               "            █░░█              ",
                               "            █░░█              ",
                               "░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░"};

            PrintImage(orc, Console.BufferWidth / 2 - 2 * Console.BufferWidth / 5, bottomBorderY - 1, "#FF0000");
            PrintImage(elf, Console.BufferWidth / 2 + Console.BufferWidth / 5, bottomBorderY - 1, "#0000FF");
        }

        public static void PrintCards(List<Card> cards)
        {
            int j = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                if((j * (Console.BufferWidth/6)) > Console.BufferWidth)
                {
                    j = 0;
                    DrawBorders("#FF0000");
                    Console.ReadKey();
                    WriteAt(" ", 1, Console.BufferHeight/2, Console.BufferWidth-1, Console.BufferHeight-1);
                }
                DrawSelectedCard(cards[i].Name, cards[i].Description, j, Console.BufferWidth / 6, Console.BufferHeight / 2);
                j++;
            }
            DrawBorders("#FF0000");
            Console.ReadKey();
            PrintAt("Ha visto todas las cartas. Presione Enter para volver", Console.BufferWidth/2, Console.BufferHeight/2, "FFFF00");
        }
    }
}