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
        public Draw()
        {
            origRow = Console.CursorTop;
            origCol = Console.CursorLeft;
            
        }
        
        protected static int origRow;
        protected static int origCol;
        
        public static void WriteAt(string s, int x, int y, string hexColor = "#FFFFFF")
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
        protected static void WriteImage(string[] s, int x, int y, string hexColor = "#FFFFFF")
        {
            int n = 0;
            for (int i = s.Length-1; i >= 0; i--)
            {
                Console.SetCursorPosition(x, y+n);
                Console.Write(s[i].Pastel(hexColor));
                n--;
            }
            
                
        }
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

        public static void WriteText(string text,int x, int y, int width, int height, string hexColor = "#FFFFFF", string bgColor = "#000000")
        {
            string normal;
            normal = Regex.Replace(text,"\n", "");
            List<string> print = TextManagment.Normalize(normal, width, height);
            foreach (string item in print)
            {
                WriteAt(item.PastelBg(bgColor), x, y, hexColor);
                y++;
            }
        }
        public static void DrawCard(string cardName, double value, double cooldown, int x, int cardWidth, int cardHeight, string heColor = "#FFFFFF")//hacer despues el bottom)
        {
            if(cardName.Length > cardWidth-1) cardName = cardName.Substring(0, cardWidth-1);
            string width = "";
            for (int i = 0; i < cardWidth-2; i++)
            {
                width += "═";
            }
            WriteAt(("╔" + width + "╗").Pastel(heColor), x*cardWidth+1, cardHeight);
            WriteAt(cardName, x*cardWidth + 2, cardHeight + 1);
            WriteAt("║".Pastel(heColor), x*cardWidth + 1, cardHeight + 1, x*cardWidth + 1, Console.BufferHeight-1);
            if(x!=0) WriteAt("║".Pastel(heColor), (x+1)*cardWidth, cardHeight + 1, (x+1)*cardWidth, Console.BufferHeight-1);
            else{ WriteAt("║".Pastel(heColor), cardWidth, cardHeight + 1, cardWidth , Console.BufferHeight-1);}
            WriteAt(("╠" + width +"╣").Pastel(heColor), x*cardWidth + 1, cardHeight + 2);
            WriteAt(("╚"+width+"╝").Pastel(heColor), x*cardWidth + 1, Console.BufferHeight-2);
            WriteAt($"VAL: {value}".Pastel(heColor), x*cardWidth + 2, cardHeight + 3);
            WriteAt($"COOL: {cooldown}".Pastel(heColor), x*cardWidth + 2, cardHeight + 4);
        }
        public static void DrawSelectedCard(string cardSName, double value, double cooldown, string about, int x, int cardSHeight, int cardSWidth,string heColor)
        {
            
            string hexColor = "#FFFFFF";
            if(cardSName.Length > cardSWidth-1) cardSName = cardSName.Substring(0, cardSWidth-1);
            string width = "";
            for (int i = 0; i < cardSWidth-2; i++)
            {
                width += "═";
            }
            WriteAt(" ".PastelBg("#FF9898"), x*cardSWidth + 1 , cardSHeight, x*cardSWidth + cardSWidth, cardSHeight + cardSHeight); //background
            WriteAt(("╔" + width +"╗").PastelBg("#FF9898"), x*cardSWidth + 1 , cardSHeight);
            WriteAt(cardSName.PastelBg("#FF9898"), x*cardSWidth + 2, cardSHeight + 1);
            WriteAt("║".PastelBg("#FF9898"), x*cardSWidth + 1, cardSHeight + 1, x*cardSWidth + 1, Console.BufferHeight-1);
            if(x!=0) WriteAt("║".PastelBg("#FF9898"), (x+1)*cardSWidth, cardSHeight + 1, (x+1)*cardSWidth, Console.BufferHeight-1);
            else{ WriteAt("║".PastelBg("#FF9898"), cardSWidth, cardSHeight + 1, cardSWidth , Console.BufferHeight-1);}
            WriteAt(("╠" + width + "╣").PastelBg("#FF9898"), x*cardSWidth + 1, cardSHeight + 2);
            WriteAt(("╚" + width + "╝").PastelBg("#FF9898"), x*cardSWidth + 1, Console.BufferHeight-2);
            WriteAt($"VAL: {value}".PastelBg("#FF9898"), x*cardSWidth + 2, cardSHeight + 3);
            WriteAt($"COOL: {cooldown}".PastelBg("#FF9898"), x*cardSWidth + 2, cardSHeight + 4);
            WriteText(about, x*cardSWidth + 2, cardSHeight + 5, cardSWidth -2, cardSHeight, hexColor, "#FF9898");
        }
        public static void DrawBorders(string hexColor = "#FFFFFF")
        {
            WriteAt("═", 0, 0, Console.BufferWidth, 0, hexColor);
            WriteAt("═", 0, Console.BufferHeight, Console.BufferWidth, Console.BufferHeight, hexColor);
            WriteAt("║", 0, 0, 0, Console.BufferHeight, hexColor);
            WriteAt("║", Console.BufferWidth, 0, Console.BufferWidth, Console.BufferHeight, hexColor);
            WriteAt("╔", 0, 0, hexColor);
            WriteAt("╗", Console.BufferWidth, 0, hexColor);
            WriteAt("╚", 0, Console.BufferHeight, hexColor);
            WriteAt("╝", Console.BufferWidth, Console.BufferHeight, hexColor);
        }
        public static void DrawBordersExtra(int midConsole, int fifthConsole, int bottomBorderY, int maxHeight)
        {
            WriteAt("╦", midConsole - fifthConsole, 0, "#FF0000"); WriteAt("╦", midConsole + fifthConsole, 0, "#FF0000");
            
            WriteAt("║", midConsole - fifthConsole, 1, midConsole - fifthConsole, maxHeight - bottomBorderY, "#FF0000"); //vertical top
            WriteAt("║", midConsole + fifthConsole, 1, midConsole + fifthConsole, maxHeight - bottomBorderY, "#FF0000"); //vertical
            WriteAt("═", midConsole - fifthConsole, maxHeight - bottomBorderY, midConsole + fifthConsole, maxHeight - bottomBorderY, "#FF0000"); //top
            WriteAt("╚", midConsole - fifthConsole, maxHeight - bottomBorderY, "#FF0000");
            WriteAt("╝", midConsole + fifthConsole, maxHeight - bottomBorderY, "#FF0000");
            WriteAt($"{GameManager.CurrentGame.CurrentPlayer.Name} es tu turno", midConsole - fifthConsole + 1, 1, "#FF0000");

            //borders bottom
            WriteAt("═", 1, bottomBorderY, Console.BufferWidth - 1, bottomBorderY, "#FF0000"); //cartas
            WriteAt("╣", Console.BufferWidth, bottomBorderY, "#FF0000");
            WriteAt("╠", 0, bottomBorderY, "#FF0000");
        }
        public static void DrawFloor(int bottomBorderY)
        {
            WriteAt("█",1, bottomBorderY-2, Console.BufferWidth-2, bottomBorderY-1, "#AB6A18");
        }
        public static void DrawPlayerStats(List<Player> list)
        {
            int x = 2;
            int y = 1;
            string hexColor = "#FF0000";
            foreach (Player player in list)
            {
                WriteAt(player.Name, x, y, hexColor);
                WriteAt($" Health: {player.Health}", x, y + 1, hexColor);
                WriteAt($" Energy: {player.Energy}", x, y + 2, hexColor);
                x = Console.BufferWidth / 2 + Console.BufferWidth / 5 + 1;
                hexColor = "#0000FF";
            }

        }

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

            string[] elf = new string[]{"                              ", 
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

            WriteImage(orc, Console.BufferWidth/2 - 2*Console.BufferWidth/5, bottomBorderY-1, "#FF0000");
            WriteImage(elf, Console.BufferWidth/2 + Console.BufferWidth/5, bottomBorderY-1, "#0000FF");
        }
    
        public static void PrintCards(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                DrawCard(cards[i].Name, cards[i].EnergyCostValue, cards[i].CurrentColdown, i, Console.BufferWidth/10+1, Console.BufferHeight/4);
            }
        }
    }
}