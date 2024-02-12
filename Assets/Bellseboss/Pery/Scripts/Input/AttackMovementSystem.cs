using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AttackMovementSystem : MonoBehaviour
{
    public Action OnAttack, OnMidAir, OnRelease, OnSustain, OnEndJump;
    [SerializeField] private AttackMovementData attackMovementDataQuick;
    [SerializeField] private AttackMovementData attackMovementDataPower;
    [SerializeField] private bool canAttackAgain = true;
    [SerializeField] private bool attacking;
    private TeaTime _attack, _decresing, _sustain, _release;
    private Rigidbody _rigidbody;
    [SerializeField] private float _deltatimeLocal;
    private RigidbodyConstraints _rigidbodyConstraints;
    [SerializeField] private Vector3 _distance;
    private AttackMovementData _attackMovementData;
    [SerializeField] private int maxNumberOfCombos;
    [SerializeField] private int _numberOfCombos;


    public void Configure(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
        var gameObjectToPlayer = rigidbody.gameObject;
        _rigidbodyConstraints = _rigidbody.constraints;
        _attack = this.tt().Pause().Add(() =>
        {
            _deltatimeLocal = 0;
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ |
                                     RigidbodyConstraints.FreezeRotationX;
            if (attacking)
            {
                if (_numberOfCombos <= maxNumberOfCombos)
                {
                    _numberOfCombos++;
                }
            }
        }).Add(() =>
        {
            OnAttack?.Invoke();
            Debug.Log("AttackMovementSystem: Start Attack");
        }).Loop(loop =>
        {
            _deltatimeLocal += loop.deltaTime;
            if (_deltatimeLocal >= _attackMovementData.timeToAttack)
            {
                loop.Break();
            }

            float t = _deltatimeLocal / _attackMovementData.timeToAttack;
            float heightMultiplier = Mathf.Cos(t * Mathf.PI * 0.5f);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position,
                position + transform.forward * (_attackMovementData.maxDistance * heightMultiplier),
                _attackMovementData.forceToAttack * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Add(() =>
        {
            Debug.Log("AttackMovementSystem: Attack End");
            _decresing.Play();
        });

        _decresing = this.tt().Pause().Add(() =>
        {
            Debug.Log("AttackMovementSystem: Decresing Start");
            canAttackAgain = true;
            OnMidAir?.Invoke();
        }).Loop(loop =>
        {
            _deltatimeLocal += loop.deltaTime;
            if (_deltatimeLocal >= _attackMovementData.timeToAttack + _attackMovementData.timeToDecreasing)
            {
                canAttackAgain = false;
                loop.Break();
            }

            float t = (_deltatimeLocal - _attackMovementData.timeToAttack) / _attackMovementData.timeToDecreasing;
            float heightMultiplier = Mathf.Log(1 + t * 4);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position,
                position - transform.forward * (_attackMovementData.distanceToDecresing * heightMultiplier),
                _attackMovementData.forceToDecreasing * loop.deltaTime);
            if (!double.IsNaN(position.x) && !double.IsNaN(position.y) && !double.IsNaN(position.z))
            {
                gameObjectToPlayer.transform.position = position;
            }
        }).Add(() =>
        {
            Debug.Log("AttackMovementSystem: Decresing End");
            _sustain.Play();   
        });

        _sustain = this.tt().Pause().Add(() =>
        {
            Debug.Log("AttackMovementSystem: Sustain Start");
            OnSustain?.Invoke();
        }).Loop(loop =>
        {
            _deltatimeLocal += loop.deltaTime;
            if (_deltatimeLocal >= _attackMovementData.timeToAttack + _attackMovementData.timeToDecreasing +
                _attackMovementData.timeToSustain)
            {
                loop.Break();
            }
        }).Add(() =>
        {
            Debug.Log("AttackMovementSystem: Sustain End");
            _release.Play();
        });
        
        _release = this.tt().Pause().Add(() =>
        {
            Debug.Log("AttackMovementSystem: Release Start");
            OnRelease?.Invoke();
        }).Loop(loop=>
        {
            _deltatimeLocal += loop.deltaTime;
            if (_deltatimeLocal >= _attackMovementData.timeToAttack + _attackMovementData.timeToDecreasing + _attackMovementData.timeToSustain + _attackMovementData.timeToRelease)
            {
                loop.Break();
            }

            var t = _deltatimeLocal / _attackMovementData.timeToAttack;
            var heightMultiplier = Mathf.Log(1 + t * _attackMovementData.forceToDecreasing);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position, position - transform.forward * (_attackMovementData.maxDistance * heightMultiplier), _attackMovementData.forceToDecreasing * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Add(() =>
        {
            canAttackAgain = true;
            _numberOfCombos = 0;
            attacking = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.useGravity = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            Debug.Log("AttackMovementSystem: End Attack");
            OnEndJump?.Invoke();
        });
    }

    public void Attack(Vector3 distance, TypeOfAttack typeOfAttack)
    {
        Debug.Log("AttackMovementSystem: Attack");
        canAttackAgain = false;
        attacking = true;
        _distance = distance;
        _attackMovementData = typeOfAttack == TypeOfAttack.Quick ? attackMovementDataQuick : attackMovementDataPower;
        _decresing.Stop();
        _attack.Stop().Play();
    }
    
    public enum TypeOfAttack
    {
        Quick,
        Power
    }
    public bool CanAttackAgain()
    {
        return canAttackAgain;
    }

    public bool FullCombo()
    {
        return _numberOfCombos >= maxNumberOfCombos;
    }
}