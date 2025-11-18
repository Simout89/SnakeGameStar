namespace Users.FateX.Scripts.Upgrade
{
    public class MortarSnakeSegment: SnakeSegmentBase
    {
        public override void Tick()
        {
            base.Tick();
            
            if(!CheckOnTimeDone())
                return;
            
            
        }
    }
}