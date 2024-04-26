using System;
using UnityEngine;

public class FatalitySystem : MonoBehaviour, IFatalitySystem
{
    [SerializeField] private StatePatter _statePatterFatality;
    private IFatality _characterV2;

    public void Fatality()
    {
        _statePatterFatality.StartStates();
    }

    void IFatalitySystem.Configure(IFatality characterV2)
    {
        _characterV2 = characterV2;
        _statePatterFatality.Configure(this);
    }
}
