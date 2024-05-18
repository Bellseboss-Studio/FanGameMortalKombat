using System;
using System.Collections.Generic;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.Playables;

public class FatalitySystem : MonoBehaviour, IFatalitySystem
{
    [SerializeField] private StatePatter statePatterFatality;
    [SerializeField] private CinemachineTargetGroup cinematicTargetGroup;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private int countOfInputToRead;
    [SerializeField] private List<InputRead> inputsRead = new List<InputRead>();
    [SerializeField] private List<GroupInputs> inputsToValidate;
    [SerializeField] private CinemachineVirtualCameraBase cinematicVirtualCameraBase;
    [SerializeField] private GameObject panelTitleFatality, panelInputs;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private UiFatalityInputs uiFatalityInputs;
    public Action<INPUTS> OnInputPressed;
    private IFatality _characterV2;
    private ICharacterV2 _cV2;
    private bool _isFatality;
    private GameObject enemy;
    private bool _isCinematicFinished;

    public void Configure(IFatality characterV2, ICharacterV2 cV2)
    {
        _characterV2 = characterV2;
        _cV2 = cV2;
        var dic = new Dictionary<STATE_FATALITY, StatePatterFatality>
        {
            { STATE_FATALITY.IDLE, new IdleState(this) },
            { STATE_FATALITY.CINEMATIC, new CinematicState(this) },
            { STATE_FATALITY.INPUTS, new WaitForInputs(this, countOfInputToRead) },
            { STATE_FATALITY.VALIDATE, new ValidateInputs(this, inputsToValidate) },
            { STATE_FATALITY.FATALITY, new FatalityState(this) },
            { STATE_FATALITY.NO_FATALITY, new NoFatalityState(this) }
        };
        statePatterFatality.Configure(this, STATE_FATALITY.IDLE, dic);
        statePatterFatality.StartStates();
        cinematicVirtualCameraBase.gameObject.SetActive(false);
        uiFatalityInputs.Configure(this);
    }

    public void Fatality()
    {
        _cV2.DisableControls();
        _isFatality = true;
        enemy = _characterV2.GetEnemyToKillWithFatality();
        cinematicTargetGroup.m_Targets = new[]
        {
            new CinemachineTargetGroup.Target {target = _cV2.GetGameObject(), radius = 1, weight = 1},
            new CinemachineTargetGroup.Target {target = enemy.transform, radius = 1, weight = 1}
        };
        cinematicVirtualCameraBase.gameObject.SetActive(true);
    }

    public bool IsStartFatality()
    {
        return _isFatality;
    }

    public void StartCinematic()
    {
        playableDirector.Play();
        ShowPanelTitle("Finished Him!");
    }

    public bool FinishedCinematic()
    {
        return playableDirector.state != PlayState.Playing;
    }

    public bool ReadInput(out INPUTS input)
    {
        if (_characterV2.ReadInput(out input))
        {
            OnInputPressed?.Invoke(input);
            return true;
        }
        return false;
    }

    public List<InputRead> GetInputs()
    {
        return inputsRead;
    }

    public void ContinueCinematic()
    {
        playableDirector.Resume();
    }

    public void EndOfFatality()
    {
        _isFatality = false;
        _cV2.EnableControls();
        cinematicVirtualCameraBase.gameObject.SetActive(false);
        inputsRead.Clear();
    }

    public void HidePanelTitle()
    {
        panelTitleFatality.SetActive(false);
    }

    public void ShowPanelInputs()
    {
        panelInputs.SetActive(true);
    }
    
    public void HidePanelInputs()
    {
        panelInputs.SetActive(false);
    }

    public void ShowPanelTitle()
    {
        panelTitleFatality.SetActive(true);
    }

    public void ShowPanelTitle(string title)
    {
        panelTitleFatality.SetActive(true);
        this.title.text = title;
    }

    public void CanReadInputs(bool b)
    {
        _characterV2.StartToReadInputs(b);
        if (b)
        {
            uiFatalityInputs.DefaultValue();
        }
    }

    public void PauseCinematic()
    {
        playableDirector.Pause();
    }
}