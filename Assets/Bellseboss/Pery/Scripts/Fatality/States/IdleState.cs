using System.Collections;

public class IdleState : StatePatterFatality
{
    public IdleState(IFatalitySystem fatalitySystem) : base(fatalitySystem)
    {
    }

    internal override IEnumerator Execute()
    {
        while (!_fatalitySystem.StartFatality())
        {
            yield return null;
        }
    }

    public override int NextState()
    {
        return STATE_FATALITY.CINEMATIC;
    }
}