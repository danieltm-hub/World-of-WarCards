using static System.Console;
using AST;
using GameProgram;
using Pastel;
namespace Visual
{
    class BattleMenu
    {
        public void DisplayOptions()
        {
            CursorVisible = false;
            Console.Clear();
            if (GameManager.CurrentGame.CurrentPlayer.Controller is RandomPlayer)
            {
                DisplayOptionsForIA();
            }
            else
            {
                for (int i = 0; i < Options.Length; i++)
                {
                    string currentOption = Options[i];
                    SetCursorPosition(Console.BufferWidth - 19, (Console.BufferHeight - 7) + i);
                    Console.WriteLine($"{currentOption}".Pastel("#8900ff"));

                }
                ResetColor();

                for (int j = 0; j < GameManager.CurrentGame.CurrentPlayer.Cards.Count; j++)
                {
                    Player currentPlayer = GameManager.CurrentGame.CurrentPlayer;
                    Card currentCard = currentPlayer.Cards[j];

                    string hexColor;

                    if (j == Indexes.Item1) continue;

                    hexColor = "#FF50FF";

                    if (currentPlayer.Cooldowns[j] > 0)
                    {
                        Draw.PrintCard((currentCard.Name), currentCard.EnergyCostValue, currentPlayer.Cooldowns[j], j, cardWidth, cardHeight, "A9A9A9");
                    }

                    else { Draw.PrintCard((currentCard.Name), currentCard.EnergyCostValue, currentPlayer.Cooldowns[j], j, cardWidth, cardHeight, hexColor); }
                }

                Card selectedCard = GameManager.CurrentGame.CurrentPlayer.Cards[Indexes.Item1];
                Draw.DrawSelectedCard(selectedCard.Name, selectedCard.Description, Indexes.Item1 > 7 ? 6 : Indexes.Item1, cardSHeight, cardSWidth);

                ResetColor();
                Draw.PrintPlayerStats(GameManager.CurrentGame.Players);
                Draw.DrawPlayerImage(bottomBorderY);
                Draw.DrawBorders("#FF0000");
                Draw.DrawBordersExtra(midConsole, fifthConsole, bottomBorderY, maxHeight);
                Draw.DrawFloor(bottomBorderY);
            }
        }
        public void DisplayOptionsForIA()
        {
            CursorVisible = false;
            Console.Clear();
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                SetCursorPosition(Console.BufferWidth - 20, Console.BufferHeight - 1 + i * 10);
                Console.WriteLine($"{currentOption}".Pastel("#8900ff"));
            }

            ResetColor();
            Draw.PrintPlayerStats(GameManager.CurrentGame.Players);
            Draw.DrawPlayerImage(bottomBorderY);
            Draw.DrawBorders("#FF0000");
            Draw.DrawBordersExtra(midConsole, fifthConsole, bottomBorderY, maxHeight);
            Draw.DrawFloor(bottomBorderY);
        }

        public (int, int) RunMenuIA()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptionsForIA();
                string[] optionsIA = { "JUGAR", "SALIR" };
                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;
                if (keyPressed == ConsoleKey.D1)
                {
                    Indexes.Item2 = 0;
                    return Indexes;
                }
                else if (keyPressed == ConsoleKey.D2)
                {
                    Indexes.Item2 = 1;
                    return Indexes;
                }
            }
            while (keyPressed != ConsoleKey.Escape);
            Indexes.Item2 = 1;
            return Indexes;
        }
        public (int, int) GetIndexes()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                DisplayOptions();
                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.LeftArrow)
                {
                    Indexes.Item1--;
                    Indexes.Item1 = Indexes.Item1 < 0 ? GameManager.CurrentGame.CurrentPlayer.Cards.Count - 1 : Indexes.Item1;
                    return Indexes;
                }
                else if (keyPressed == ConsoleKey.RightArrow)
                {
                    Indexes.Item1++;
                    Indexes.Item1 = Indexes.Item1 >= GameManager.CurrentGame.CurrentPlayer.Cards.Count ? 0 : Indexes.Item1;
                    return Indexes;
                }

            }
            while (keyPressed != ConsoleKey.D1 && keyPressed != ConsoleKey.Escape && keyPressed != ConsoleKey.D2 && keyPressed != ConsoleKey.D3 && keyPressed != ConsoleKey.D4);
            switch (keyPressed)
            {
                case ConsoleKey.D1:
                    Indexes.Item2 = -2;
                    return Indexes;
                case ConsoleKey.D2:
                    Indexes.Item2 = -3;
                    return Indexes;
                case ConsoleKey.D3:
                    Indexes.Item2 = -4;
                    return Indexes;
                case ConsoleKey.Escape:
                    Indexes.Item2 = -1;
                    return Indexes;
                case ConsoleKey.D4:
                    Indexes.Item2 = -1;
                    return Indexes;
            }
            return Indexes;
        }


        #region Variables
        private (int, int) Indexes = (0, 0);
        private string[] Options;

        List<Player> Players;

        public int borderLeft;
        int borderRight;
        public int borderWidth;
        public int borderHeight;
        int maxWidth;
        int maxHeight;
        int bottomBorderY;
        int topBorderY;
        int midConsole;
        int fifthConsole;
        int cardSHeight;
        int cardSWidth;
        int cardHeight;
        int cardWidth;

        public BattleMenu(string[] options, List<Player> players, int borderLeft, int borderRight, int borderWidth, int borderHeight, int maxWidth, int maxHeight, int bottomBorderY, int topBorderY, int midConsole, int fifthConsole, int cardSHeight, int cardSWidth, int cardHeight, int cardWidth)
        {
            Options = options;
            Players = players;
            this.borderLeft = borderLeft;
            this.borderRight = borderRight;
            this.borderWidth = borderWidth;
            this.borderHeight = borderHeight;
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
            this.bottomBorderY = bottomBorderY;
            this.topBorderY = topBorderY;
            this.midConsole = midConsole;
            this.fifthConsole = fifthConsole;
            this.cardSHeight = cardSHeight;
            this.cardSWidth = cardSWidth;
            this.cardHeight = cardHeight;
            this.cardWidth = cardWidth;
        }
        #endregion

    }
}