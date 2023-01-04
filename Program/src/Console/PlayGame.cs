using System;
using Figgle;
using Pastel;
using AST;
using GameProgram;



namespace Visual
{
    class PlayGame
    {
        List<Card> WarCards;
        public PlayGame(List<Card> cards)
        {
            WarCards = cards;
        }
        public void StartAGame()
        {

            Player Pepe = new Player("Pepe", 100, 20, 6, WarCards);
            Player Juan = new Player("Juan", 100, 20, 6, WarCards);

            GameManager.StartGame(new List<Player>() { Juan, Pepe });
        }
        public void StartIAGame()
        {

            Player Pepe = new Player("Pepe", 20, 20, 6, WarCards);
            Player Juan = new Player("Juan", 20, 20, 6, WarCards);
            RandomPlayer Pepin = new RandomPlayer(Pepe);
            Pepe.SetCPU(Pepin);
            MCTS Tontin = new MCTS(BasicStratergy.BasicLifeLScore, Juan);
            Juan.SetCPU(Tontin);

            GameManager.StartGame(new List<Player>() { Juan, Pepe });
        }
        public void Start()
        {
            int x = Console.BufferWidth / 2;
            int y = Console.BufferHeight / 2;
            string[] loadingBar = new string[11]{"▒▒▒▒▒▒▒▒▒▒ 0%" ,
                                                 "█▒▒▒▒▒▒▒▒▒ 10%",
                                                 "██▒▒▒▒▒▒▒▒ 20%",
                                                 "███▒▒▒▒▒▒▒ 30%",
                                                 "████▒▒▒▒▒▒ 40%" ,
                                                 "█████▒▒▒▒▒ 50%  ",
                                                 "██████▒▒▒▒ 60%  ",
                                                 "███████▒▒▒ 70%  ",
                                                 "████████▒▒ 80%  ",
                                                 "█████████▒ 90%  ",
                                                 "██████████ 100%"};
            TextAnimation.AnimateFrames(new string[3] { "Iniciando juego.", "Iniciando juego..", "Iniciando juego..." }, 250, 3, "#00FF00", x, y);
            TextAnimation.AnimateFrames(loadingBar, 150, 1, "#00FF00", x, y);
            System.Console.WriteLine();
            Draw.PrintAt("Presione cualquier letra", x, y, "#00FF00");
            System.Console.ReadKey(true);
            RunMainMenu();
        }

        private void RunMainMenu()
        {
            Console.CursorVisible = false;
            string prompt = FiggleFonts.Larry3d.Render(" World of WarCards ");
            string[] options = { "JUGAR", "OPCIONES", "CREDITOS", "SALIR" };
            Menu mainMenu = new Menu(prompt, options);
            int SelectedIndex = mainMenu.Run();

            switch (SelectedIndex)
            {
                case 0:
                    RunChooseCharacterMenu();
                    break;
                case 1:
                    RunOptionsMenu();
                    break;
                case 2:
                    RunCreditsMenu();
                    break;
                case 3:
                    Exit();
                    break;
            }
        }

        private void RunChooseCharacterMenu()
        {
            List<Card> availableCards = WarCards;

            Player player1 = CreatePlayer(availableCards, 1);
            Player player2 = CreatePlayer(availableCards, 2);
            if (player1.Name == player2.Name)
            {
                Draw.PrintAt("Los nombres de los jugadores no pueden ser iguales", 1, 1);
                System.Console.ReadKey(true);
                RunChooseCharacterMenu();
            }
            List<Player> players = new List<Player>() { player1, player2 };

            Game newGame = new Game(players, 0, new EnemyDefeated());

            GameManager.StartGame(players);

            RunBattleMenu((0, 0));
        }

        private Player CreatePlayer(List<Card> availableCards, int index)
        {
            Console.Clear();
            Draw.DrawBorders("#FF0000");

            Draw.PrintAt($"Ingrese el nombre del Player {index}: ", 1, 1, "#FF0000");

            string name = Console.ReadLine()!;

            List<Card> cards = new List<Card>();

            PrintAvailableCards(availableCards);

            int n = 1;

            Console.CursorVisible = true;

            while (cards.Count < 6)
            {
                Console.SetCursorPosition(1, n + availableCards.Count + 4);

                try
                {
                    int cardNumber = int.Parse(Console.ReadLine()!);
                    cards.Add(availableCards[cardNumber - 1]);
                    n++;
                }
                catch (Exception)
                {
                    Draw.PrintAt("Ingrese un numero valido", 1, n + availableCards.Count + 4);
                    n++;
                }
            }
            Draw.PrintAt("Escoja el tipo de player", 1, n + availableCards.Count + 4);
            Draw.PrintAt("1 - Humano", 1, n + availableCards.Count + 5);
            Draw.PrintAt("2 - Player Random", 1, n + availableCards.Count + 6);
            Draw.PrintAt("3 - Player IA Fuerte", 1, n + availableCards.Count + 7);
            Console.SetCursorPosition(1, n + availableCards.Count + 8);

            try
            {
                int playerType = int.Parse(Console.ReadLine()!);
                switch (playerType)
                {
                    case 1:
                        return new Player(name, 100, 15, 5, cards);
                    case 2:
                        Player random = new Player(name, 100, 15, 5, cards);
                        RandomPlayer randomIA = new RandomPlayer(random);
                        random.SetCPU(randomIA);
                        return random;
                    case 3:
                        Player mts = new Player(name, 100, 15, 5, cards);
                        MCTS mtsIA = new MCTS(BasicStratergy.BasicLifeLScore, mts);
                        mts.SetCPU(mtsIA);
                        return mts;
                    default:
                        throw (new Exception("Escoja un tipo de player valido"));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void PrintAvailableCards(List<Card> availableCards)
        {
            Draw.PrintAt("Cartas disponibles", 1, 2);
            int x = 0;
            int y = 0;
            for (int i = 0; i < availableCards.Count; i++)
            {
                Draw.PrintAt($"{i + 1} - {availableCards[i].Name}", 1 + x, y + 3);
                y++;
                if (i % 7 == 0 && i != 0)
                {
                    x += 18;
                    y = 0;
                }
            }
            Draw.PrintAt("Ingrese el numero de la carta que desea agregar", 1, availableCards.Count + 4);
        }
        private void RunBattleMenu((int, int) index)
        {
            #region Variables

            (int, int) indexes = index;
            int borderLeft = Console.BufferWidth / 2 - Console.BufferWidth / 5 + 1;
            int borderRight = Console.BufferWidth / 2 + Console.BufferWidth / 5 - 1;
            int borderWidth = borderRight - borderLeft;
            int borderHeight = Console.BufferHeight / 4 - 1;
            int maxWidth = Console.BufferWidth;
            int maxHeight = Console.BufferHeight;
            int bottomBorderY = maxHeight - maxHeight / 4;
            int topBorderY = maxHeight / 4;
            int midConsole = maxWidth / 2;
            int fifthConsole = maxWidth / 5;
            int cardSHeight = bottomBorderY + 1;
            int cardSWidth = maxWidth / 10 + 1;
            int cardHeight = bottomBorderY + 2;
            int cardWidth = maxWidth / 11;
            #endregion Variables

            string[] options = new string[4] { "[1] JUGAR CARTA", "[2] LEER CARTA", "[3] PASAR", "[4] SALIR" };
            List<Player> players = GameManager.CurrentGame.Players;
            BattleMenu battleMenu = new BattleMenu(options, players, borderLeft, borderRight, borderWidth, borderHeight, maxWidth, maxHeight, bottomBorderY, topBorderY, midConsole, fifthConsole, cardSHeight, cardSWidth, cardHeight, cardWidth);

            if (!(GameManager.CurrentGame.CurrentPlayer.Controller is Human))
            {
                BattleIA();
            }
            else
            {
                indexes = battleMenu.GetIndexes();
                Choise(indexes, battleMenu);
            }
        }
        private void Choise((int, int) index, BattleMenu battle)
        {
            if (index.Item2 == 0)
            {
                battle.DisplayOptions();
                Choise(battle.GetIndexes(), battle);
            }
            Player currentPlayer = GameManager.CurrentGame.CurrentPlayer;
            Card toPlay = currentPlayer.Cards[index.Item1];

            switch (index.Item2)
            {
                case -2:
                    if (currentPlayer.Cooldowns[index.Item1] > 0)
                    {
                        Draw.WriteText($"La carta {toPlay.Name} está en Cooldown", battle.borderLeft, 2, battle.borderWidth, battle.borderHeight, "#8900FF");
                        TextAnimation.AnimateTyping("Presione cualquier tecla para continuar", 5, battle.borderLeft, 3, "#8900ff");
                        Console.ReadKey(true);
                        index.Item2 = 0;
                        RunBattleMenu(index);
                        break;
                    }
                    else if (toPlay.EnergyCostValue > currentPlayer.Energy)
                    {
                        Draw.WriteText($"No tienes suficiente energía para jugar la carta {toPlay.Name}", battle.borderLeft, 2, battle.borderWidth, battle.borderHeight, "#8900FF");
                        Console.ReadKey(true);
                        index.Item2 = 0;
                        RunBattleMenu(index);
                        break;
                    }
                    else if (currentPlayer.Will == 0)
                    {
                        Draw.WriteText($"No tienes voluntad para jugar, pasa turno", battle.borderLeft, 2, battle.borderWidth, battle.borderHeight, "#8900FF");
                        Console.ReadKey(true);
                        index.Item2 = 0;
                        RunBattleMenu(index);
                        break;
                    }
                    GameManager.CurrentGame.PlayCard(index.Item1);
                    Draw.WriteText($"Se jugó la carta {toPlay.Name}", battle.borderLeft, 2, battle.borderWidth, battle.borderHeight, "#8900FF");
                    Console.ReadKey(true);
                    if (GameManager.CurrentGame.IsOver())
                    {
                        RunEndGameMenu();
                        break;
                    }
                    index.Item2 = 0;
                    RunBattleMenu(index);
                    break;
                case -3:
                    Draw.WriteText(toPlay.Description, battle.borderLeft, 2, battle.borderWidth, battle.borderHeight, "#8900FF");
                    Console.ReadKey(true);
                    index.Item2 = 0;
                    RunBattleMenu(index);
                    break;
                case -4:
                    Draw.WriteText($"{GameManager.CurrentGame.CurrentPlayer.Name} ha decidido pasar turno", battle.borderLeft, 1, battle.borderWidth, battle.borderHeight, "#8900FF");
                    GameManager.CurrentGame.NextTurn();
                    Console.ReadKey(true);
                    index.Item2 = 0;
                    RunBattleMenu(index);
                    break;
                case -1:
                    RunMainMenu();
                    // Console.Clear();
                    break;
            }
        }
        private void RunOptionsMenu()
        {
            string prompt = FiggleFonts.Larry3d.Render(" OPCIONES ");
            string[] options = { "VER CARTAS", "SIMULACION DE VIRTUALES", "VOLVER" };
            Menu optionsMenu = new Menu(prompt, options);
            int SelectedIndex = optionsMenu.Run();

            switch (SelectedIndex)
            {
                case 0:
                    PrintCards();
                    break;
                case 1:
                    SimulateVirtualGame();
                    break;
                case 2:
                    RunMainMenu();
                    break;
            }
        }

        private void SimulateVirtualGame()
        {
            int x = Console.BufferWidth / 2;
            int y = Console.BufferHeight / 2;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            string prompt = FiggleFonts.Larry3d.Render(" SIMULACION DE VIRTUALES ");
            System.Console.WriteLine(prompt);
            ChooseIAs();
            TextAnimation.AnimateTyping(" Simulando juego virtual...", 50, x, y);
            System.Console.WriteLine();
            Draw.PrintAt(" Presione cualquier letra para empezar", x, y + 1);
            System.Console.ReadKey(true);
            BattleIA();
        }

        private void ChooseIAs()
        {
            Draw.PrintAt("Escoja la opcion deseada", Console.BufferWidth / 2 - 10, Console.BufferHeight / 2 -10);
            Draw.PrintAt("1- Random vs Random", Console.BufferWidth / 2 - 10, Console.BufferHeight / 2 -9);
            Draw.PrintAt("2- Random vs Smart", Console.BufferWidth / 2 - 10, Console.BufferHeight / 2- 8);
            Draw.PrintAt("3- Smart vs Smart", Console.BufferWidth / 2 - 10, Console.BufferHeight / 2 -7);
            Console.CursorVisible = true;
            Console.SetCursorPosition(Console.BufferWidth/2 -10, Console.BufferHeight/2 - 6);
            Console.CursorVisible = false;
            try
            {
                int option = int.Parse(Console.ReadLine()!);
                switch (option)
                {
                    case 1:
                        Player Pepe = new Player("Pepe", 20, 20, 6, WarCards);
                        Player Juan = new Player("Juan", 20, 20, 6, WarCards);
                        RandomPlayer Pepin = new RandomPlayer(Pepe);
                        Pepe.SetCPU(Pepin);
                        RandomPlayer Tontin = new RandomPlayer(Juan);
                        Juan.SetCPU(Tontin);
                        GameManager.StartGame(new List<Player>() { Juan, Pepe });
                        break;
                    case 2:
                        Player Pedro = new Player("Pepe", 20, 20, 6, WarCards);
                        Player Juana = new Player("Juan", 20, 20, 6, WarCards);
                        RandomPlayer Pedrito = new RandomPlayer(Pedro);
                        Pedro.SetCPU(Pedrito);
                        MCTS Juanina = new MCTS(BasicStratergy.BasicLifeLScore, Juana);
                        Juana.SetCPU(Juanina);
                        GameManager.StartGame(new List<Player>() { Juana, Pedro });
                        break;
                    case 3:
                        Player Stuart = new Player("Pepe", 20, 20, 6, WarCards);
                        Player Merlin = new Player("Juan", 20, 20, 6, WarCards);
                        MCTS Sturt = new MCTS(BasicStratergy.BasicLifeLScore, Stuart);
                        Stuart.SetCPU(Sturt);
                        MCTS Merlina = new MCTS(BasicStratergy.BasicLifeLScore, Merlin);
                        Merlin.SetCPU(Merlina);
                        GameManager.StartGame(new List<Player>() { Stuart, Merlin });
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }

        private void BattleIA()
        {
            #region Variables
            (int, int) indexes;
            int borderLeft = Console.BufferWidth / 2 - Console.BufferWidth / 5 + 1;
            int borderRight = Console.BufferWidth / 2 + Console.BufferWidth / 5 - 1;
            int borderWidth = borderRight - borderLeft;
            int borderHeight = Console.BufferHeight / 4 - 1;
            int maxWidth = Console.BufferWidth;
            int maxHeight = Console.BufferHeight;
            int bottomBorderY = maxHeight - maxHeight / 4;
            int topBorderY = maxHeight / 4;
            int midConsole = maxWidth / 2;
            int fifthConsole = maxWidth / 5;
            int cardSHeight = bottomBorderY + 1;
            int cardSWidth = maxWidth / 10 + 1;
            int cardHeight = bottomBorderY + 2;
            int cardWidth = maxWidth / 11;
            #endregion Variables


            string[] optionsIA = { "[1] JUGAR", "[ESC] SALIR" };


            List<Player> players = GameManager.CurrentGame.Players;
            BattleMenu battleMenu = new BattleMenu(optionsIA, players, borderLeft, borderRight, borderWidth, borderHeight, maxWidth, maxHeight, bottomBorderY, topBorderY, midConsole, fifthConsole, cardSHeight, cardSWidth, cardHeight, cardWidth);
            indexes = battleMenu.RunMenuIA();

            switch (indexes.Item2)
            {
                case 0:
                    //se garantiza que el jugador actual sea la IA
                    if (GameManager.CurrentGame.IsOver())
                    {
                        RunEndGameMenu();
                        break;
                    }
                    if (GameManager.CurrentGame.CurrentPlayer.Controller != null)
                    {
                        GameManager.CurrentGame.CurrentPlayer.Controller.Play();
                    }
                    Console.ReadKey(true);
                    if (GameManager.CurrentGame.CurrentPlayer.Controller is Human)
                    {
                        RunBattleMenu((0, 0));
                    }
                    else
                    {
                        BattleIA();
                    }
                    break;
                case 1:
                    Draw.WriteText("SE SALDRA EN BREVE", borderLeft, 1, borderWidth, borderHeight);
                    Console.ReadKey(true);
                    RunMainMenu();
                    break;
            }
        }

        private void RunCreditsMenu()
        {
            Console.Clear();
            string prompt = FiggleFonts.Larry3d.Render(" CREDITOS ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(1, 1);
            System.Console.WriteLine();
            Console.WriteLine(prompt);
            string credits = @"
            Este juego fue creado por:
            
            Daniel Toledo
            Osvaldo Moreno
            José Antonio Concepción
            
            Este juego usa recursos de http://www.patorjk.com/
            Presione cualquier letra para volver al menu principal";

            TextAnimation.AnimateTyping(credits, 25, Console.BufferWidth / 8, Console.BufferHeight / 4, "#FF0000");
            System.Console.ReadKey(true);
            RunMainMenu();

        }
        private void PrintCards()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            string prompt = FiggleFonts.Larry3d.Render(" VER CARTAS ");
            System.Console.WriteLine();
            System.Console.WriteLine(prompt);
            System.Console.WriteLine();
            System.Console.WriteLine(" Imprimiendo cartas...");
            System.Console.WriteLine(" Presione Enter");
            Draw.PrintCards(WarCards);
            Draw.DrawBorders("#FF0000");
            System.Console.ReadKey(true);
            RunOptionsMenu();
        }
        private void RunEndGameMenu()
        {
            Console.Clear();
            string winner = FiggleFonts.Larry3d.Render("El ganador es: " + GameManager.CurrentGame.Winner().Name);
            Console.SetCursorPosition(1, 1);
            Console.WriteLine(winner.Pastel("#00FF00"));
            Draw.DrawBorders("#00FF00");
            string prompt = FiggleFonts.Larry3d.Render(" FIN DEL JUEGO");
            string[] options = { "VOLVER AL MENU PRINCIPAL", "SALIR DEL JUEGO" };
            Menu endGameMenu = new Menu(prompt, options);
            Console.ReadKey(true);
            int SelectedIndex = endGameMenu.Run();
            switch (SelectedIndex)
            {
                case 0:
                    RunMainMenu();
                    break;
                case 1:
                    Exit();
                    break;
            }
        }
        private void Exit()
        {
            int x = Console.BufferWidth / 2;
            int y = Console.BufferHeight / 2;
            string[] exit = new string[3] { "Saliendo del juego.", "Saliendo del juego..", "Saliendo del juego..." };
            TextAnimation.AnimateFrames(exit, 250, 3, "#FFFF00", x, y);
            System.Console.WriteLine();
            TextAnimation.AnimateTyping("Gracias por jugar, presione cualquier tecla", 10, x, y, "FFFF00");
            System.Console.WriteLine();
            Console.CursorVisible = false;
            System.Console.ReadKey(true);
            Console.Clear();
            Environment.Exit(0);
        }
    }
}