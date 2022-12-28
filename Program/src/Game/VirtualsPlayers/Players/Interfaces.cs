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
        double ProgresiveScore(Game game, Player player);
        double FinalState(Game game, Player player);
    }

    // public delegate double PlayScore<Game, Player>(Game game, Player player);
}

