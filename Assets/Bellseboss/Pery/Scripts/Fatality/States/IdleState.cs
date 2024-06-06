using System.Collections;

public class IdleState : StatePatterFatality
{
    public IdleState(IFatalitySystem fatalitySystem) : base(fatalitySystem)
    {
    }

    internal override IEnumerator Execute()
    {
        while (!_fatalitySystem.IsStartFatality())
        {
            yield return null;
        }
    }

    public override STATE_FATALITY NextState()
    {
        return STATE_FATALITY.CINEMATIC;
    }
}