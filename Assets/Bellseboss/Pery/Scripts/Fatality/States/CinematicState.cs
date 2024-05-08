using System.Collections;
public class CinematicState : StatePatterFatality
{
    public CinematicState(IFatalitySystem fatalitySystem) : base(fatalitySystem)
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
        return STATE_FATALITY.INPUTS;
    }

    protected override void StartState()
    {
        base.StartState();
        //_fatalitySystem.FinishCinematic();
    }
}