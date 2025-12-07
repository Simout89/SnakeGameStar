using System;
using UnityEngine;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/SoundsData")]
    public class SoundsData : ScriptableObject
    {
        [field: SerializeField] public WeaponSoundsData WeaponSoundsData { get; private set; }
        [field: SerializeField] public SlotMachine SlotMachine { get; private set; }
        [field: SerializeField] public AK.Wwise.Event DamageSound { get; private set; }
        [field: SerializeField] public AK.Wwise.Event CardSelected { get; private set; }
        [field: SerializeField] public AK.Wwise.Event CollectCoin { get; private set; }
        [field: SerializeField] public AK.Wwise.Event DisplayMessage { get; private set; }
        [field: SerializeField] public AK.Wwise.Event EatApple { get; private set; }
        [field: SerializeField] public AK.Wwise.Event LvlUp { get; private set; }
        [field: SerializeField] public AK.Wwise.Event PickUp { get; private set; }
        [field: SerializeField] public AK.Wwise.Event SnakeDie { get; private set; }
        [field: SerializeField] public AK.Wwise.Event TrialTowerCapture { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Xp { get; private set; }
    }

    [Serializable]
    public class WeaponSoundsData
    {
        [field: SerializeField] public AK.Wwise.Event Tesla { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Mortar { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Explosion { get; private set; }
        [field: SerializeField] public AK.Wwise.Event MachineGun { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Pentagram { get; private set; }
        [field: SerializeField] public AK.Wwise.Event SpikeGun { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Sword { get; private set; }
    }
    
    [Serializable]
    public class SlotMachine
    {
        [field: SerializeField] public AK.Wwise.Event LoopPlay { get; private set; }
        [field: SerializeField] public AK.Wwise.Event LoopStop { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Win { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Complection { get; private set; }
    }
}