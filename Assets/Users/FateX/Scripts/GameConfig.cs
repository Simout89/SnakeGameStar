using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts
{
    public class GameConfig
    {
        public GameConfigData GameConfigData { get; private set; }
        
        public void SetConfig(GameConfigData gameConfigData)
        {
            GameConfigData = gameConfigData;
        }
    }
}