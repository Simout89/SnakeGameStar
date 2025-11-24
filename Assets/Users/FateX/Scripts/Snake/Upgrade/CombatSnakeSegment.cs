namespace Users.FateX.Scripts.Upgrade
{
    public class CombatSnakeSegment: SnakeSegmentBase, IDamageDealer
    {
        public override void Tick()
        {
            base.Tick();
            
            if(!CheckOnTimeDone())
                return;

            Attack();
        }

        public virtual void Attack()
        {
            
        }

        protected void DealDamage(DamageInfo damageInfo)
        {
            GameEvents.DamageDealt(damageInfo);
        }
    }
}