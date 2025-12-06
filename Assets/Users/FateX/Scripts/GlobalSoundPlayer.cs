using Users.FateX.Scripts.Data;
using Zenject;

namespace Users.FateX.Scripts
{
    public class GlobalSoundPlayer
    {
        [Inject] private MonoHelper _monoHelper;
        [Inject] private GameConfig _gameConfig;
        public SoundsData SoundsData => _gameConfig.GameConfigData.SoundsData;

        public void Play(AK.Wwise.Event eventWise)
        {
            eventWise.Post(_monoHelper.gameObject);
        }
    }
}