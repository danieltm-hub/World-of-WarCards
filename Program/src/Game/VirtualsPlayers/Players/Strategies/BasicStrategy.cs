namespace GameProgram
{
    public class BasicStrategy : IStrategy
    {
        public double ProgresiveScore(Game game, Player player)
        {
            throw new NotImplementedException();
        }

        public double FinalState(Game game, Player player)
        {
            throw new NotImplementedException();
        }
    }

    public class BasicStadistics : IStadistic
    {
        public NodeMCTS SelectNode(List<NodeMCTS> nodes)
        {
            throw new NotImplementedException();
        }
    }
}