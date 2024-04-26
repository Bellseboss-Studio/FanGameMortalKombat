using System.Collections;

public class IdleState : StatePatterFatality
{
    public IdleState(IFatalitySystem fatalitySystem) : base(fatalitySystem)
    {
    }

    internal override IEnumerator Execute()
    {
        yield return null;
    }

    public override int NextState()
    {
        return 1;
    }
}

