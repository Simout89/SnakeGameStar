using UnityEngine;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts
{
    public class GameConfig
    {
        private GameConfigData _gameConfigData;

        public GameConfigData GameConfigData
        {
            get
            {
                if (_gameConfigData == null)
                    _gameConfigData = (Resources.LoadAll<GameConfigData>("Data")[0]);
                return _gameConfigData;
            }

            private set { _gameConfigData = value; }
        }

        public void SetConfig(GameConfigData gameConfigData)
        {
            GameConfigData = gameConfigData;
        }
    }
}