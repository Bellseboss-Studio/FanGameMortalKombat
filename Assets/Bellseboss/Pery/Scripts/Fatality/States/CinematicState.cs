using System.Collections;
public class CinematicState : StatePatterFatality
{
    public CinematicState(IFatalitySystem fatalitySystem) : base(fatalitySystem)
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
        return STATE_FATALITY.INPUTS;
    }

    protected override void StartState()
    {
        base.StartState();
        _fatalitySystem.FinishCinematic();
    }
}