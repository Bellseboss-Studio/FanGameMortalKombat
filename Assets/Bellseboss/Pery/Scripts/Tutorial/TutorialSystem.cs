using System.Collections.Generic;
using System.Linq;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private List<ActivableTutorial> activables;
    [SerializeField] private bool isFinished;
    public UnityEvent OnStart;
    public UnityEvent OnFinish;
    private TeaTime _flow;
    private bool _allFinished;
    private CharacterV2 _character;

    private void Start()
    {
        _flow = this.tt().Pause().Add(() =>
        {
            foreach (var activable in activables)
            {
                activable.Activate();
            }

            _character?.DisableControls();
        }).Loop(h =>
        {
            _allFinished = true;
            foreach (var unused in activables.Where(actionable => !actionable.IsFinished))
            {
                _allFinished = false;
            }

            if (_allFinished)
            {
                h.Break();
            }
        }).Add(() => { _character?.EnableControls(); }).Add(() =>
        {
            _character = null; isFinished = true;
            OnFinish?.Invoke();
        });
        OnStart?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<CharacterV2>(out var character) && !isFinished)
        {
            _character = character;
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        _flow.Play();
    }
}