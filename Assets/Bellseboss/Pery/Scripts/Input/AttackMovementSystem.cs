using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AttackMovementSystem : MonoBehaviour
{
    public Action OnAttack, OnMidAir, OnRelease, OnSustain, OnEndJump;
    [SerializeField] private AttackMovementData attackMovementDataQuick;
    [SerializeField] private AttackMovementData attackMovementDataPower;
    private TeaTime _attack, _decresing, _sustain, _release;
    private Rigidbody _rigidbody;
    private float _deltatimeLocal;
    private RigidbodyConstraints _rigidbodyConstraints;
    private Vector3 _distance;
    private AttackMovementData _attackMovementData;


    public void Configure(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
        var gameObjectToPlayer = rigidbody.gameObject;
        _rigidbodyConstraints = _rigidbody.constraints;
        _attack = this.tt().Pause().Add(() =>
        {
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            Debug.Log("AttackMovementSystem: Attack");
        }).Add(() =>
        {
            OnAttack?.Invoke();
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
            position = Vector3.Lerp(position, position + transform.forward * (_attackMovementData.maxDistance * heightMultiplier), _attackMovementData.forceToAttack * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Add(() =>
        {
            OnMidAir?.Invoke();
        }).Loop(loop =>
        {
            _deltatimeLocal += loop.deltaTime;
            if(_deltatimeLocal >= _attackMovementData.timeToAttack + _attackMovementData.timeToDecreasing)
            {
                loop.Break();
            }

            float t = (_deltatimeLocal - _attackMovementData.timeToAttack) / _attackMovementData.timeToDecreasing;
            float heightMultiplier = Mathf.Log(1 + t * 4);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position, position - transform.forward * (_attackMovementData.distanceToDecresing * heightMultiplier), _attackMovementData.forceToDecreasing * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Add(() =>
        {
            OnSustain?.Invoke();
        }).Loop(loop =>
        {
            _deltatimeLocal += loop.deltaTime;
            if (_deltatimeLocal >= _attackMovementData.timeToAttack + _attackMovementData.timeToDecreasing + _attackMovementData.timeToSustain)
            {
                loop.Break();
            }
        }).Add(() =>
        {
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
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.useGravity = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            _deltatimeLocal = 0;
            Debug.Log("AttackMovementSystem: End Attack");
            OnEndJump?.Invoke();
        });
    }

    public void Attack(Vector3 distance, TypeOfAttack typeOfAttack)
    {
        Debug.Log("AttackMovementSystem: Attack");
        _distance = distance;
        _attackMovementData = typeOfAttack == TypeOfAttack.Quick ? attackMovementDataQuick : attackMovementDataPower;
        _attack.Play();
    }
    
    public enum TypeOfAttack
    {
        Quick,
        Power
    }
}