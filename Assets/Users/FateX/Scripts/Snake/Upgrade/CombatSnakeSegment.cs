namespace Users.FateX.Scripts.Upgrade
{
    public class CombatSnakeSegment: SnakeSegmentBase
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
    }
}