using System;
using System.Collections.Generic;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class EnemyV2 : MonoBehaviour, IAnimationController, IEnemyV2
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
    private GameObject _model;
    private StatisticsOfCharacter _statisticsOfCharacter;
    private CharacterV2 _characterV2;
    private bool _inFarZone;
    private bool _inNearZone;
    private List<GameObject> _paths;
    private GameObject _target;
    private bool _canMove, _canRotate, _canRotateToTarget;
    [SerializeField] private StatesOfEnemy _state;

    public string Id => id;
    public bool IsDead { get; private set; }

    private void Start()
    {
        _statisticsOfCharacter = Instantiate(statisticsOfCharacter);
        movementADSR.Configure(GetComponent<Rigidbody>(), _statisticsOfCharacter);
        _model = Instantiate(model, transform);
        animationController.Configure(_model.GetComponent<Animator>(), this);
        aiController.Configure(this);
        _state = StatesOfEnemy.NORMAL;
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
            if (Vector3.Distance(transform.position, _target.transform.position) < minDistanceToArriveToTarget)
            {
                Debug.Log("EnemyV2: Arrive to target");
                _canMove = false;
                OnArriveToTarget?.Invoke();
            }
        }
        animationController.Movement(rigidbody.velocity.magnitude/10, 0);
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
    
    public void ReceiveDamage(int damage, Vector3 direction)
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

    public void SetAnimationToHit(bool isQuickAttack, int numberOfCombos)
    {
        Debug.Log($"EnemyV2: SetAnimationToHit isQuickAttack: {isQuickAttack} numberOfCombos: {numberOfCombos}");
    }

    public void SetPlayer(CharacterV2 characterV2)
    {
        _characterV2 = characterV2;
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