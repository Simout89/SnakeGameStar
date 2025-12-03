using Скриптерсы.Utils;

namespace Users.FateX.Scripts
{
    public class PlayerStats
    {
        public ValueCompositeAdditive<float> CoinDropChance = new ValueCompositeAdditive<float>(0);
        public ValueCompositeAdditive<int> Exile = new ValueCompositeAdditive<int>(0);
        public ValueCompositeAdditive<int> Rerolls = new ValueCompositeAdditive<int>(1);
        public ValueCompositeAdditive<int> Health = new ValueCompositeAdditive<int>(0);
        public ValueCompositeAdditive<float> Damage = new ValueCompositeAdditive<float>(0);
        public ValueCompositeAdditive<int> ProjectileCount = new ValueCompositeAdditive<int>(0);
        public ValueCompositeAdditive<float> PickUpRange = new ValueCompositeAdditive<float>(0);
        public ValueCompositeAdditive<float> MoveSpeed = new ValueCompositeAdditive<float>(0);
    }
}