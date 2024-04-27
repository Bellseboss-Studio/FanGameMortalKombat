using System;
using UnityEngine;
using UnityEngine.Playables;
using Bellseboss.Pery.Scripts.Input;

public class FatalitySystem : MonoBehaviour, IFatalitySystem
{
    [SerializeField] private StatePatter _statePatterFatality;
    [SerializeField] private PlayableDirector _cinematic;
    private IFatality _characterV2;
    private ICharacterV2 _cV2;
    private bool startFatality;

    public void Fatality()
    {
        startFatality = true;
    }

    void Configure(IFatality characterV2, ICharacterV2 cV2)
    {
        _characterV2 = characterV2;
        _cV2 = cV2;
        _statePatterFatality.Configure(this, STATE_FATALITY.IDLE);
        _statePatterFatality.StartStates();
    }

    public bool StartFatality()
    {
        if(startFatality)
        {
            startFatality = false;
            return true;
        }
        return startFatality;
    }

    public void StartCinematic()
    {
        _cV2.DisableControls();
        _cinematic.Play();
    }
}
