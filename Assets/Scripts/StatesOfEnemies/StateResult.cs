public class StateResult
{
    public readonly int NextStateId;
    public readonly object ResultData;

    public StateResult(int nextStateId, object resultData = null)
    {
        NextStateId = nextStateId;
        ResultData = resultData;
    }
}