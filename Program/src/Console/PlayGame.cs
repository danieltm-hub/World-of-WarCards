using System;
using Figgle;
using Pastel;
using AST;
using GameProgram;


namespace KeyboardMenu
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
            // #region  Players
            Player Pepe = new Player("Pepe", 20, 20, 6, new List<Card>() { });
            Player Juan = new Player("Juan", 20, 20, 6, new List<Card>() { });
            // RandomPlayer Pepin = new RandomPlayer(Pepe);
            // PlayerRandom Tontin = new PlayerRandom("Tontin", 20, 20, new List<Card>() { });
            // PlayerEasy tuHermana = new PlayerEasy("Tu Hermana", 20, 20, new List<Card>(){});
            // #endregion

             
            
            GameManager.StartGame(new List<Player>(){Juan, Pepe});
        }
        public void StartIAGame()
        {
            
            Player Pepe = new Player("Pepe", 20, 20, 6, new List<Card>() { });
            Player Juan = new Player("Juan", 20, 20, 6, new List<Card>() { });
            RandomPlayer Pepin = new RandomPlayer(Pepe);
            Pepe.SetCPU(Pepin);
            RandomPlayer Tontin = new RandomPlayer(Juan);
            Juan.SetCPU(Tontin);
                
            GameManager.StartGame(new List<Player>(){Juan, Pepe});
        }
        public void Start()
        {
            int x = Console.BufferWidth/2;
            int y = Console.BufferHeight/2;
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
            TextAnimation.AnimateFrames(new string[3]{"Iniciando juego.", "Iniciando juego..", "Iniciando juego..."}, 250, 3, "#00FF00", x ,y);
            TextAnimation.AnimateFrames(loadingBar, 150, 1, "#00FF00", x, y);
            System.Console.WriteLine();
            Draw.WriteAt("Presione cualquier letra", x , y, "#00FF00");
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
                    // StartAGame();
                    // RunBattleMenu();
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

            
            
            //!get from the DLL
            List<Card> availableCards = WarCards;
            
            Console.Clear();
            Draw.DrawBorders("#FF0000");

            Draw.WriteAt("Ingrese el nombre del Player 1: " , 1, 1);
            string name1 = Console.ReadLine()!;
            List<Card> cards1 = new List<Card>();
            
            PrintAvailableCards(availableCards);
            int n = 1;
            Console.CursorVisible = true;
            while (cards1.Count < 4) //!the top is 8 modify later
            {
                Console.SetCursorPosition(1, n + availableCards.Count + 4);
                //se podian repetir las cartas?
                try
                {
                    int cardNumber = int.Parse(Console.ReadLine()!);
                    cards1.Add(availableCards[cardNumber - 1]);
                    n++;
                }
                catch (Exception)
                {
                    Draw.WriteAt("Ingrese un numero valido", 1, n + availableCards.Count + 4);
                    n++;
                }
            }
            
            Console.Clear();
            Draw.DrawBorders("#FF0000");

            Draw.WriteAt("Ingrese el nombre del player 2: " , 1, 1);
            string name2 = Console.ReadLine()!;
            List<Card> cards2 = new List<Card>();

            PrintAvailableCards(availableCards);

            n = 1;
            while (cards2.Count < 4) //!the top is 8 modify later
            {
                Console.SetCursorPosition(1, n + availableCards.Count + 4);
                try
                {
                    int cardNumber = int.Parse(Console.ReadLine()!);
                    cards2.Add(availableCards[cardNumber - 1]);
                    n++;
                }
                catch (Exception)
                {
                    Draw.WriteAt("Ingrese un numero valido", 1, n + availableCards.Count + 4);
                    n++;
                }
            }


            List<Player> players = new List<Player>(){new Player(name1, 20, 20, 6, cards1), new Player(name2, 20, 20, 6, cards2)};
            Game newGame = new Game(players, 0, new EnemyDefeated());
            GameManager.StartGame(players);
            RunBattleMenu();
        }

        private void PrintAvailableCards(List<Card> availableCards)
        {
            Draw.WriteAt("Cartas disponibles" ,1,2);
            for (int i = 0; i < availableCards.Count; i++)
            {
                Draw.WriteAt($"{i + 1} - {availableCards[i].Name}", 1 , i + 3);
            }
            Draw.WriteAt("Ingrese el numero de la carta que desea agregar" , 1, availableCards.Count + 4);
        }
        private void RunBattleMenu()
        {
            #region Variables

            int selectedIndex;
            int selectedCard;
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

            string[] options = new string[4] { "JUGAR CARTA", "LEER CARTA", "PASAR", "SALIR" };

            
            List<Player> players = GameManager.CurrentGame.Players;
            BattleMenu battleMenu = new BattleMenu(options, players, borderLeft, borderRight, borderWidth, borderHeight, maxWidth, maxHeight, bottomBorderY, topBorderY, midConsole, fifthConsole, cardSHeight, cardSWidth, cardHeight, cardWidth);
            selectedCard = battleMenu.RunCards();
            selectedIndex = battleMenu.RunMenu();
            Draw.DrawPlayerStats(players);
            switch (selectedIndex)
            {
                case 0:
                    GameManager.CurrentGame.CurrentPlayer.Cards[selectedCard].Play();
                    Draw.WriteText($"se jugo la carta {selectedCard + 1}", borderLeft, 1, borderWidth, borderHeight);
                    if(GameManager.CurrentGame.IsOver())
                    {
                        RunEndGameMenu();
                        break;
                    }
                    Console.ReadKey(true);
                    RunBattleMenu();
                    break;
                case 1:
                    Draw.WriteText(GameManager.CurrentGame.CurrentPlayer.Cards[selectedCard].Description, borderLeft, 1, borderWidth, borderHeight);
                    Console.ReadKey(true);
                    RunBattleMenu();
                    break;
                case 2:
                    Draw.WriteText($"{GameManager.CurrentGame.CurrentPlayer.Name} ha decidido pasar turno", borderLeft, 1, borderWidth, borderHeight);
                    GameManager.CurrentGame.NextTurn();
                    Console.ReadKey(true);
                    RunBattleMenu();
                    break;
                case 3:
                    RunMainMenu();
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
                    StartIAGame();
                    SimulateVirtualGame();
                    break;
                case 2:
                    RunMainMenu();
                    break;
            }
        }

         private void SimulateVirtualGame()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            string prompt = FiggleFonts.Larry3d.Render(" SIMULACION DE VIRTUALES ");
            System.Console.WriteLine();
            System.Console.WriteLine(prompt);
            System.Console.WriteLine();
            TextAnimation.AnimateTyping(" Simulando juego virtual...");
            System.Console.WriteLine(" Presione cualquier letra para empezar");
            Draw.DrawBorders("#FF0000");
            System.Console.ReadKey(true);
            BattleIA();
        }

        private void BattleIA()
        {
            #region Variables
            int selectedIndex;
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


            string[] optionsIA = { "JUGAR", "SALIR" };

            
            List<Player> players = GameManager.CurrentGame.Players;
            BattleMenu battleMenu = new BattleMenu(optionsIA, players, borderLeft, borderRight, borderWidth, borderHeight, maxWidth, maxHeight, bottomBorderY, topBorderY, midConsole, fifthConsole, cardSHeight, cardSWidth, cardHeight, cardWidth);
            selectedIndex = battleMenu.RunMenuIA();

            switch(selectedIndex)
            {
                case 0:
                    //se garantiza que el jugador actual sea la IA
                    if(GameManager.CurrentGame.CurrentPlayer.CPU != null)
                    {
                        GameManager.CurrentGame.CurrentPlayer.CPU.Play();    
                    }
                    
;
                    if(GameManager.CurrentGame.IsOver())
                    {
                        RunEndGameMenu();
                        break;
                    }
                    Console.ReadKey(true);
                    BattleIA();
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
            Console.SetCursorPosition(1,1);
            System.Console.WriteLine();
            Console.WriteLine(prompt);
            string credits = @"
            Este juego fue creado por:
            
            Daniel Toledo
            Osvaldo Moreno
            José Antonio Concepción
            
            Este juego usa recursos de http://www.patorjk.com/
            Presione cualquier letra para volver al menu principal";

            TextAnimation.AnimateTyping(credits, 25, Console.BufferWidth/8, Console.BufferHeight/4, "#FF0000");
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
            System.Console.WriteLine(" Presione cualquier letra para volver");
            Draw.DrawBorders("#FF0000");
            Draw.PrintCards(WarCards);
            System.Console.ReadKey(true);
            RunOptionsMenu();
        }

        private void RunEndGameMenu()
        {
            Console.Clear();
            string winner = FiggleFonts.Larry3d.Render("El ganador es: " + GameManager.CurrentGame.Winner().Name);
            Console.SetCursorPosition(1,1);
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
                    GameManager.TerminateGame();
                    RunMainMenu();
                    break;
                case 1:
                    Exit();
                    break;
            }
        }
        private void Exit()
        {
            int x = Console.BufferWidth/2;
            int y = Console.BufferHeight/2;
            string[] exit = new string[3]{"Saliendo del juego.", "Saliendo del juego..", "Saliendo del juego..."};
            TextAnimation.AnimateFrames(exit, 250, 3, "#FFFF00", x, y);
            System.Console.WriteLine();
            TextAnimation.AnimateTyping("Gracias por jugar, presione cualquier tecla", 10, x, y, "FFFF00");
            System.Console.WriteLine();
            Console.CursorVisible = false;
            System.Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}