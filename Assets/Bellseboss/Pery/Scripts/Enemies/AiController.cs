using System;
using UnityEngine;

internal class AiController : MonoBehaviour
{
    [SerializeField] private float timeToWaitToChangePath;
    private IEnemyV2 _enemy;
    private TeaTime _idle, _numberOfPath, _moving, _stayNearOfTarget, _watch;
    private TeaTime _watchEnemy, _nearOfEnemy;
    private GameObject _target;
    private bool _isNearOfTarget;
    private int _indexOfPath;
    private bool _enemyIsDetected;
    private bool _enemyIsNear;
    
    

    public void Configure(IEnemyV2 enemy)
    {
        _indexOfPath = 0;
        _enemy = enemy;
        if(_enemy.Paths().Count == 0)
        {
            throw new Exception("Paths is empty");
        }
        _target = _enemy.Paths()[_indexOfPath];
        
        _enemy.OnArriveToTarget += () =>
        {
            _isNearOfTarget = true;
        };
        
        _enemy.OnPlayerDetected += isDetected =>
        {
            _idle.Stop();
            _numberOfPath.Stop();
            _moving.Stop();
            _stayNearOfTarget.Stop();
            _watch.Stop();
            _watchEnemy.Stop();
            _nearOfEnemy.Stop();
            
            if(isDetected)
            {
                _enemy.CanMove(false);
                _watchEnemy.Play();
            }
            else
            {
                _idle.Play();
            }
            _enemyIsDetected = isDetected;
        };
        
        _enemy.OnPlayerInNearZone += isNear =>
        {
            if(isNear)
            {
                
            }
            else
            {
                _idle.Stop();
                _numberOfPath.Stop();
                _moving.Stop();
                _stayNearOfTarget.Stop();
                _watch.Stop();
                _watchEnemy.Stop();
                _nearOfEnemy.Stop();
                
                _watchEnemy.Play();
            }
            _enemyIsNear = isNear;
        };
        
        _idle = this.tt().Pause().Add(() =>
        {
            //configure a some things
            _numberOfPath.Play();
        });
        _numberOfPath = this.tt().Pause().Add(() =>
        {
            if(_enemy.Paths().Count > 0)
            {
                _moving.Play();
            }
        });
        
        _moving = this.tt().Pause().Add(() =>
        {
            _enemy.MoveTo(_target);
        }).Add(() =>
        {
            _stayNearOfTarget.Play();
        });
        _stayNearOfTarget = this.tt().Pause().Add(() =>
        {
        }).Wait(()=>_isNearOfTarget).Add(() =>
        {
            //start animation to watch
            _enemy.TriggerAnimation("watch");
            _enemy.RotateToTargetIdle(_target);
        })
            .Add(timeToWaitToChangePath)
            .Add(() =>
        {
            if(_enemy.Paths().Count > 1)
            {
                if(_enemy.Paths().Count > _indexOfPath + 1)
                {
                    _indexOfPath++;
                }
                else
                {
                    _indexOfPath = 0;
                }
                _target = _enemy.Paths()[_indexOfPath];
                _isNearOfTarget = false;
                _moving.Play();
            }
            else
            {
                _watch.Play();
            }
        });
        
        _watch = this.tt().Pause().Add(() =>
        {
            _enemy.RotateToTargetIdle(_target);
        }).Wait(()=>_enemyIsDetected).Add(() =>
        {
            _watchEnemy.Play();
        });
        
        _watchEnemy = this.tt().Pause().Add(() =>
        {
            _enemy.MoveTo(_enemy.GetPlayer().gameObject);
            _enemy.CanMove(false);
        }).Wait(()=>_enemyIsNear).Add(() =>
        {
            _enemy.MoveTo(_enemy.GetPlayer().gameObject);
        }).Wait(()=>_isNearOfTarget).Add(() =>
        {
            _nearOfEnemy.Play();
        });
        
        _nearOfEnemy = this.tt().Pause().Add(() =>
        {
            _watchEnemy.Play();
        }); 
        
        _idle.Play();
    }
}

public enum StatesOfEnemy
{
    ANGRY,
    NORMAL,
    SCARED
}