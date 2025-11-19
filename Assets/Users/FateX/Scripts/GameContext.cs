namespace Users.FateX.Scripts
{
    public class GameContext
    {
        public SnakeController SnakeController { get; private set; }

        public void Init(SnakeController snakeController)
        {
            SnakeController = snakeController;
        }
    }
}