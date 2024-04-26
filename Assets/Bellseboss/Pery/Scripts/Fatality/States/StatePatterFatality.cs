using System;
using System.Collections;

public abstract class StatePatterFatality
{
    protected IFatalitySystem _fatalitySystem;

    public StatePatterFatality(IFatalitySystem fatalitySystem)
    {
        _fatalitySystem = fatalitySystem;
    }

    internal IEnumerator Starting()
    {
        yield return null;
        StartState();
    }

    internal abstract IEnumerator Execute();

    internal IEnumerator End()
    {
        yield return null;
        EndState();
    }

    protected virtual void EndState()
    {

    }

    protected virtual void StartState()
    {

    }

    public abstract int NextState();
}

