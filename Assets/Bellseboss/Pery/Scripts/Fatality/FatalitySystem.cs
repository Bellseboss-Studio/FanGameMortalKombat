using System;
using System.Collections.Generic;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;
using UnityEngine.Serialization;

public class FatalitySystem : MonoBehaviour, IFatalitySystem
{
    [SerializeField] private StatePatter statePatterFatality;
    private IFatality _characterV2;
    private ICharacterV2 _cV2;

    public void Configure(IFatality characterV2, ICharacterV2 cV2)
    {
        _characterV2 = characterV2;
        _cV2 = cV2;
        var dic = new Dictionary<STATE_FATALITY, StatePatterFatality>
        {
            { STATE_FATALITY.IDLE, new IdleState(this) },
            { STATE_FATALITY.CINEMATIC, new CinematicState(this) }
        };
        statePatterFatality.Configure(this, STATE_FATALITY.IDLE, dic);
    }

    public void Fatality()
    {
        statePatterFatality.StartStates();
    }

    public bool IsStartFatality()
    {
        return false;
    }

    public void StartCinematic()
    {
        
    }
}
