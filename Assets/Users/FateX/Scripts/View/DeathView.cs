using UnityEngine;
using UnityEngine.SceneManagement;
using Users.FateX.Scripts.View.Entry;
using Zenject;
using Скриптерсы.Services;

namespace Users.FateX.Scripts.View
{
    public class DeathView: MonoBehaviour
    {
        [SerializeField] private GameObject body;
        [SerializeField] private Transform container;
        [SerializeField] private StatisticsViewEntry _statisticsViewEntry;
        [Inject] private IStatisticsService _statisticsService;

        public void Show()
        {
            body.SetActive(true);

            foreach (var entry in _statisticsService.GetStatistics())
            {
                Instantiate(_statisticsViewEntry, container).Init(entry);
            }
        }

        public void Hide()
        {
            body.SetActive(false);
        }

        public void OnRestartClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void OnMenuClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}