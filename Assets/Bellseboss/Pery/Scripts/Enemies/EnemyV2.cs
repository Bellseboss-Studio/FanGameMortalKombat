using System;
using System.Collections.Generic;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class EnemyV2 : PJV2, IAnimationController, IEnemyV2, IMovementRigidBodyV2
{
    public event Action OnArriveToTarget;
    public event Action<bool> OnPlayerDetected;
    public event Action<bool> OnPlayerInNearZone;
    public event Action<EnemyV2> OnDead;

    [SerializeField] private string id;
    [SerializeField] private StatisticsOfCharacter statisticsOfCharacter;
    [SerializeField] private MovementADSR movementADSR;
    [SerializeField] private AnimationController animationController;
    [SerializeField] private GameObject model;
    [SerializeField] private AiController aiController;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float minDistanceToArriveToTarget;
    [SerializeField] private float minDistanceToArriveToEnemy;
    [SerializeField] private AttackMovementSystem attackMovementSystem;
    private GameObject _model;
    private StatisticsOfCharacter _statisticsOfCharacter;
    private CharacterV2 _characterV2;
    private bool _inFarZone;
    private bool _inNearZone;
    private List<GameObject> _paths;
    private GameObject _target;
    private bool _canMove, _canRotate, _canRotateToTarget;
    [SerializeField] private StatesOfEnemy _state;
    [SerializeField] private TargetFocus colliderToDamage;

    public string Id => id;
    public bool IsDead { get; private set; }

    private void Start()
    {
        _statisticsOfCharacter = Instantiate(statisticsOfCharacter);
        movementADSR.Configure(GetComponent<Rigidbody>(), _statisticsOfCharacter, this);
        _model = Instantiate(model, transform);
        animationController.Configure(_model.GetComponent<Animator>(), this);
        aiController.Configure(this);
        _state = StatesOfEnemy.NORMAL;

        var localPosition = new Vector3(0, -0.5f, 0);
        _model.transform.localPosition = localPosition;

        animationController.OnFinishAnimationDamage += () =>
        {
            Debug.Log("EnemyV2: Finish Animation Damage");
            _canMove = true;
            _canRotate = true;
        };
        attackMovementSystem.Configure(GetComponent<Rigidbody>(), _statisticsOfCharacter, this);
        attackMovementSystem.OnEndAttack += () =>
        {
            aiController.StartAi();
        };
    }

    private void Update()
    {
        if (_canRotate)
        {
            if (_canRotateToTarget)
            {
                var position = _target.transform.position;
                RotateToTarget(position);
            }
            else
            {
                var position = _target.transform.forward + transform.position;
                RotateToTarget(position);
            }
        }
        if(_canMove)
        {
            var direction = _target.transform.position - transform.position;
            direction.Normalize();
            direction *= GetSpeedToMove();
            rigidbody.velocity = direction * Time.deltaTime;
            if (Vector3.Distance(transform.position, _target.transform.position) < (GetPlayer() != null ? minDistanceToArriveToEnemy : minDistanceToArriveToTarget))
            {
                Debug.Log("EnemyV2: Arrive to target");
                _canMove = false;
                OnArriveToTarget?.Invoke();
            }
        }
        animationController.Movement(rigidbody.velocity.magnitude/10, 0);
    }

    public override void Stun(bool isStun)
    {
        _canMove = !isStun;
        _canRotate = !isStun;
    }

    private float GetSpeedToMove()
    {
        return _state switch
        {
            StatesOfEnemy.ANGRY => _statisticsOfCharacter.speedToMoveAngry,
            StatesOfEnemy.NORMAL => _statisticsOfCharacter.speedToMoveNormal,
            StatesOfEnemy.SCARED => _statisticsOfCharacter.speedToMoveScared,
            _ => 0
        };
    }

    public void RotateToTargetIdle(GameObject transformForward)
    {
        _canRotateToTarget = false;
        _target = transformForward;
    }

    public void TriggerAnimation(string nameOfAnimation)
    {
        animationController.ActivateTrigger(nameOfAnimation);
    }

    public CharacterV2 GetPlayer()
    {
        return _characterV2;
    }

    public void CanMove(bool b)
    {
        _canMove = b;
    }

    public void RotateToTarget(Vector3 transformForward)
    {
        //only rotate in Y axis
        var targetRotation = Quaternion.LookRotation(transformForward - transform.position);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }
    
    public override void ReceiveDamage(int damage, Vector3 direction)
    {
        if(IsDead) return;
        _statisticsOfCharacter.life -= damage;
        if (_statisticsOfCharacter.life <= 0)
        {
            IsDead = true;
            OnDead?.Invoke(this);
        }
        if (movementADSR.CanAttackAgain() && !IsDead)
        {
            movementADSR.Attack(direction);
        }
    }

    public void Died()
    {
        Debug.Log("EnemyV2: Die");
    }

    public float GetTimeToAttack()
    {
        return _statisticsOfCharacter.timeToAttack;
    }

    public string GetAttackAnimationName()
    {
        return _statisticsOfCharacter.attackAnimationName;
    }

    public float GetTimeBetweenAttacks()
    {
        return _statisticsOfCharacter.timeBetweenAttacks;
    }

    public bool CanActivateCollider(float delta)
    {
        _statisticsOfCharacter.timeToActivateCollider -= delta;
        if (_statisticsOfCharacter.timeToActivateCollider <= 0)
        {
            _statisticsOfCharacter.timeToActivateCollider = _statisticsOfCharacter.timeToEnableCollider;
            return true;
        }
        return false;
        
    }

    public void ColliderToAttack(bool enableCollider)
    {
        if (enableCollider)
        {
            colliderToDamage.EnableCollider();
        }
        else
        {
            colliderToDamage.DisableCollider();
        }
    }

    public void SendDamage()
    {
        _characterV2.ReceiveDamage(_statisticsOfCharacter.damage, transform.forward);
    }

    public void AttackPlayer()
    {
        attackMovementSystem.Attack(transform.forward, _statisticsOfCharacter.attackAnimationType);
    }

    public void SetState(StatesOfEnemy state)
    {
        _state = state;
    }

    public override void SetAnimationToHit(bool isQuickAttack, int numberOfCombos)
    {
        if(IsDead) return;
        Debug.Log($"EnemyV2: SetAnimationToHit isQuickAttack: {isQuickAttack} numberOfCombos: {numberOfCombos}");
        //TODO set animation to hit
        animationController.TakeDamage(isQuickAttack, numberOfCombos);
    }

    public void SetPlayer(CharacterV2 characterV2)
    {
        Debug.Log($"EnemyV2: SetPlayer {characterV2 != null}"); 
        _characterV2 = characterV2;
        if(_characterV2 != null)
        {
            aiController.SetPlayer(characterV2);
        }
    }

    public void IntoToFarZone(bool b)
    {
        _inFarZone = b;
        OnPlayerDetected?.Invoke(b);
    }

    public void IntoToNearZone(bool b)
    {
        _inNearZone = b;
        OnPlayerInNearZone?.Invoke(b);
    }

    public List<GameObject> Paths()
    {
        return _paths;
    }

    public void MoveTo(GameObject target)
    {
        _target = target;
        _canMove = true;
        _canRotate = true;
        _canRotateToTarget = true;
    }

    public void Configure(GameObject[] pathToFollow)
    {
        _paths = new List<GameObject>(pathToFollow);
    }

    public Action OnAction { get; set; }
    public void DisableControls()
    {
        
    }

    public void UpdateAnimation()
    {
        
    }

    public void ChangeToNormalJump()
    {
        
    }

    public void ChangeRotation(Vector3 rotation)
    {
    }

    public void RestoreRotation()
    {
    }

    public void EndAttackMovement()
    {
        
    }

    public void PlayerFall()
    {
        
    }

    public void PlayerRecovery()
    {
        
    }

    public bool IsAttacking()
    {
        throw new NotImplementedException();
    }
}

public interface IEnemyV2
{
    List<GameObject> Paths();
    void MoveTo(GameObject target);
    event Action OnArriveToTarget;
    event Action<bool> OnPlayerDetected;
    event Action<bool> OnPlayerInNearZone;
    event Action<EnemyV2> OnDead;
    void RotateToTargetIdle(GameObject transformForward);
    void TriggerAnimation(string nameOfAnimation);
    CharacterV2 GetPlayer();
    void CanMove(bool b);
    void Died();
    float GetTimeToAttack();
    string GetAttackAnimationName();
    float GetTimeBetweenAttacks();
    bool CanActivateCollider(float delta);
    void ColliderToAttack(bool enableCollider);
    void SendDamage();
    void AttackPlayer();
    void SetState(StatesOfEnemy state);
}

public class EnemiesV2Factory
{
    private readonly EnemiesV2Configuration EnemiesConfiguration;

    public EnemiesV2Factory(EnemiesV2Configuration enemiesConfiguration)
    {
        EnemiesConfiguration = Object.Instantiate(enemiesConfiguration);
    }
        
    public EnemyV2 Create(string id)
    {
        var prefab = EnemiesConfiguration.GetEnemyV2PrefabById(id);

        return Object.Instantiate(prefab);
    }
}