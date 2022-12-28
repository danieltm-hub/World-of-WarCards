namespace GameProgram
{
    /*
    Esto seria una idea para lo de escoger mejores partidas y asignar scores, 
    la idea es que los scores esten asignados de con un limite superior y un limete inferior,
    de manera que gnaar sea el superior y peder sea asignado por uno de los metodos de score
    segun la gravedad de la derrota(FinalState), y asi con los demas estados de juego(progressive score).
    */
    public interface IStrategy
    {
        double ProgresiveScore(Game game, Player player); // progresion de los movimientos, esto se utilizaria para podas
        double FinalState(Game game, Player player); //dada una partida y un jugador, devuelve un score
    }

    public interface IStadistic //Esto seria lo que se le pasa a MCTS para que elija la mejor partida
    {
        // esta muy especifico lo de nodeMCTS, pero es por legibilidad
        NodeMCTS SelectNode(List<NodeMCTS> nodes); //dado un conjunto de caminos, devuelve el mejor
    }

    // public delegate double PlayScore<Game, Player>(Game game, Player player);
}

