using System;
using UnityEngine;

namespace Users.FateX.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/SoundsData")]
    public class SoundsData : ScriptableObject
    {
        [field: SerializeField] public AK.Wwise.Bank Bank;
        [field: SerializeField] public WeaponSoundsData WeaponSoundsData { get; private set; }
        [field: SerializeField] public Music Music { get; private set; }
        [field: SerializeField] public UiSound UiSound { get; private set; }
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

    [Serializable]
    public class UiSound
    {
        [field: SerializeField] public AK.Wwise.Event Select { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Swipe { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Buy { get; private set; }
        [field: SerializeField] public AK.Wwise.Event Denied { get; private set; }
    }

    [Serializable]
    public class Music
    {
        [field: SerializeField] public AK.Wwise.Event PlayGameMusic { get; private set; }
        [field: SerializeField] public AK.Wwise.Event StopGameMusic { get; private set; }
        [field: SerializeField] public AK.Wwise.Event PlayMainMusic { get; private set; }
        [field: SerializeField] public AK.Wwise.Event StopMainMusic { get; private set; }

    }
}