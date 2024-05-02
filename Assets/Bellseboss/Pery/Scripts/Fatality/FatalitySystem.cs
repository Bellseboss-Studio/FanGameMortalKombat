using System;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;

public class FatalitySystem : MonoBehaviour, IFatalitySystem
{
    [SerializeField] private StatePatter _statePatterFatality;
    private IFatality _characterV2;

    public void Configure(IFatality characterV2, ICharacterV2 cV2)
    {
        
    }

    public void Fatality()
    {
        _statePatterFatality.StartStates();
    }

    public bool StartFatality()
    {
        return false;
    }

    public void StartCinematic()
    {
        
    }
}
