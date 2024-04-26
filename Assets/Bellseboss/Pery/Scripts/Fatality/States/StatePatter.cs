using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatter : MonoBehaviour
{
    private IFatalitySystem _fatalitySystem;
    private Dictionary<int, StatePatterFatality> _statePatterFatality;
    private StatePatterFatality _currentState;
    private bool _isRunning;

    public void Configure(IFatalitySystem fatalitySystem)
    {
        _fatalitySystem = fatalitySystem;
        _statePatterFatality = new Dictionary<int, StatePatterFatality>();
        
    }

    public void StartStates()
    {
        _isRunning = true;
        StartCoroutine(StatePatterCurrutine());
    }

    private IEnumerator StatePatterCurrutine()
    {
        while (_isRunning)
        {
            yield return _currentState.Starting();
            yield return new WaitForSeconds(0.1f);
            yield return _currentState.Execute();
            yield return new WaitForSeconds(0.1f);
            yield return _currentState.End();
            _currentState = _statePatterFatality[_currentState.NextState()];
        }
    }

}