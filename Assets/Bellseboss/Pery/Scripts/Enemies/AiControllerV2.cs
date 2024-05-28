using System;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;

public class AiControllerV2 : MonoBehaviour, IAiController
{
    [SerializeField] private float timeToWaitToChangePath;
    [SerializeField] private float timeBeforeAttack;
    private IEnemyV2 _enemyV2;
    private GameObject _target;
    private bool _isNearOfTarget;
    private int _indexOfPath;
    private bool _enemyIsDetected;
    private bool _enemyIsNear;
    private float _deltaTimeLocal;
    private TeaTime stateSaveBeforeExitToOtherState;

    //Patrolling
    private TeaTime _idle, _getTarget, _moving, _stayNearOfTarget, _watch;

    //Player Detected
    private TeaTime _watchEnemy, _patternOfMoving, _waitBeforeAttack, _calculatePath, _attack;

    //Receive Damage
    private TeaTime _receiveDamage, _recover, decideWhatToDo;

    public void Configure(IEnemyV2 enemyV2, ref Action onEndStunt)
    {
        _enemyV2 = enemyV2;
        onEndStunt += EndStunt;
        _enemyV2.OnDead += EnemyOnOnDead;
        _enemyV2.OnArriveToTarget += () =>
        {
            Debug.Log("Arrive to target");
            _isNearOfTarget = true;
        };
        _enemyV2.OnReceiveDamage += damage =>
        {
            Debug.Log($"Receive damage  {damage}");
            _idle.Stop();
            _getTarget.Stop();
            _moving.Stop();
            _stayNearOfTarget.Stop();
            _watch.Stop();
            _watchEnemy.Stop();
            _patternOfMoving.Stop();
            _waitBeforeAttack.Stop();
            _calculatePath.Stop();
            _attack.Stop();
            _receiveDamage.Play();
        };

        _enemyV2.OnPlayerDetected += isDetected =>
        {
            Debug.Log($"Player detected {isDetected}");
            if (_enemyV2.IsDead) return;

            _enemyIsDetected = isDetected;
        };

        _enemyV2.OnPlayerInNearZone += isNear =>
        {
            Debug.Log($"Player is near {isNear}");
            if (_enemyV2.IsDead) return;
            _enemyIsNear = isNear;
        };

        _idle = this.tt().Pause().Add(() => { Debug.Log($"Idle"); }).Add(() => { _getTarget.Play(); });

        _getTarget = this.tt().Pause().Add(() =>
        {
            Debug.Log($"Get target");
            _indexOfPath++;
            if (_indexOfPath >= _enemyV2.Paths().Count)
            {
                _indexOfPath = 0;
            }

            _target = _enemyV2.Paths()[_indexOfPath];
        }).Add(() => { _moving.Play(); });

        _moving = this.tt().Pause().Add(() =>
        {
            Debug.Log($"Moving");
            _enemyV2.MoveTo(_target);
        }).Add(() => { _stayNearOfTarget.Play(); });

        _stayNearOfTarget = this.tt().Pause().Add(() => { Debug.Log($"Stay near of target start"); }).Wait(() =>
        {
            if (_enemyIsDetected)
            {
                stateSaveBeforeExitToOtherState = _stayNearOfTarget;
                _watchEnemy.Play();
                _stayNearOfTarget.Stop();
            }

            return _isNearOfTarget;
        }).Add(() =>
        {
            Debug.Log($"Stay near of target end");
            _isNearOfTarget = false;
            _enemyV2.RotateToTargetIdle(_target, false);
        }).Add(timeToWaitToChangePath).Add(() => { _getTarget.Play(); });

        _watchEnemy = this.tt().Pause().Add(() =>
            {
                Debug.Log($"Watch enemy");
                _enemyV2.CanMove(false);
                _target = _enemyV2.GetPlayer().gameObject;
                _enemyV2.RotateToTargetIdle(_target, false);
            }).Wait(() => _isNearOfTarget)
            .Add(() =>
            {
                _isNearOfTarget = false;
                _patternOfMoving.Play();
            });

        _patternOfMoving = this.tt().Pause().Loop(handle =>
        {
            Debug.Log($"Pattern of moving");
            _enemyV2.MoveTo(_enemyV2.GetPlayer().gameObject);
            if (_isNearOfTarget)
            {
                _isNearOfTarget = false;
                handle.Break();
            }
        }).Add(() => { _waitBeforeAttack.Play(); });

        _waitBeforeAttack = this.tt().Pause().Add(() =>
            {
                Debug.Log($"Wait before attack");
                _enemyV2.CanMove(false);
            }).Add(timeBeforeAttack)
            .Add(() => { _patternOfMoving.Play(); });

        _receiveDamage = this.tt().Pause().Add(() =>
        {
            Debug.Log($"Receive damage");
            _enemyV2.CanMove(false);
        }).Add(() => { _recover.Play(); });
        _recover = this.tt().Pause().Add(() =>
        {
            Debug.Log($"Recover");
            _enemyV2.CanMove(true);
            stateSaveBeforeExitToOtherState.Play();
        });


        StartAi();
    }

    private void EnemyOnOnDead(EnemyV2 obj)
    {
        Debug.Log($"AiController: Enemy is dead {obj.gameObject.name}");
    }

    private void EndStunt()
    {
        Debug.Log("EndStunt");
        StartAi();
    }

    public void StartAi()
    {
        _idle.Play();
    }

    public void SetPlayer(CharacterV2 characterV2)
    {
    }
}