using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Users.FateX.Scripts.Data;

namespace Users.FateX.Scripts.SlotMachine
{
    public class SlotMachineView: MonoBehaviour
    {
        [SerializeField] private GameObject body;
        [SerializeField] private Image[] _image;

        public void StartGambling(SlotMachinePrizeData[] slotMachinePrizeDatas)
        {
            StartSpin(slotMachinePrizeDatas).Forget();
        }

        private async UniTask StartSpin(SlotMachinePrizeData[] slotMachinePrizeDatas)
        {
            for (int i = 0; i < _image.Length; i++)
            {
                _image[i].sprite = slotMachinePrizeDatas[i].Icon;
            }
        }
    }
}