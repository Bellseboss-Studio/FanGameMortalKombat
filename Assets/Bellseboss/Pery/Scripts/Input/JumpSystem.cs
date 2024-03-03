using System;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;
using Object = UnityEngine.Object;

public class JumpSystem : MonoBehaviour
{
    public Action OnAttack, OnMidAir, OnRelease, OnSustain, OnEndJump;
    [SerializeField, InterfaceType(typeof(IBehaviourOfJumpSystem))]
    private MonoBehaviour behaviourOfJumpSystemNormal;
    private IBehaviourOfJumpSystem BehaviourOfJumpSystemNormal => behaviourOfJumpSystemNormal as IBehaviourOfJumpSystem;
    [SerializeField, InterfaceType(typeof(IBehaviourOfJumpSystem))]
    private MonoBehaviour behaviourOfJumpSystemWalls;
    private IBehaviourOfJumpSystem BehaviourOfJumpSystemWalls => behaviourOfJumpSystemWalls as IBehaviourOfJumpSystem;
    private TeaTime _attack, _decresing, _sustain, _release, _endJump;
    private Rigidbody _rigidbody;
    private float _deltatimeLocal;
    private RigidbodyConstraints _rigidbodyConstraints;
    private bool _isScalableWall;

    public void Configure(Rigidbody rigidbody, IMovementRigidBodyV2 movementRigidBodyV2, FloorController floorController)
    {
        Debug.Log($"Configured JumpSystem: {rigidbody.gameObject.name}");
        BehaviourOfJumpSystemWalls.Configure(rigidbody);
        BehaviourOfJumpSystemNormal.Configure(rigidbody);
        _rigidbody = rigidbody;
        var gameObjectToPlayer = rigidbody.gameObject;
        _rigidbodyConstraints = _rigidbody.constraints;
        
        IsScalableWall(false, floorController, gameObject);
    }

    public void Jump()
    {
        _attack.Play();
    }

    public void IsScalableWall(bool isScalableWall, FloorController floorController, GameObject wall)
    {
        if(isScalableWall && !floorController.IsTouchingFloor())
        {
            _attack = BehaviourOfJumpSystemWalls.GetAttack();
            _decresing = BehaviourOfJumpSystemWalls.GetDecay();
            _sustain = BehaviourOfJumpSystemWalls.GetSustain();
            _release = BehaviourOfJumpSystemWalls.GetRelease();
            _endJump = BehaviourOfJumpSystemWalls.GetEndJump();
            BehaviourOfJumpSystemWalls.OnAttack = () =>
            {
                OnAttack?.Invoke();
            };
            BehaviourOfJumpSystemWalls.OnMidAir = () =>
            {
                OnMidAir?.Invoke();
            };
            BehaviourOfJumpSystemWalls.OnSustain = () =>
            {
                OnSustain?.Invoke();
            };
            BehaviourOfJumpSystemWalls.OnRelease = () =>
            {
                OnRelease?.Invoke();
            };
            BehaviourOfJumpSystemWalls.OnEndJump = () =>
            {
                OnEndJump?.Invoke();
            };
            var behaviourOfJumpSystemWallsMono = BehaviourOfJumpSystemWalls as BehaviourOfJumpSystemWalls;
            System.Diagnostics.Debug.Assert(behaviourOfJumpSystemWallsMono != null, nameof(behaviourOfJumpSystemWallsMono) + " != null");
            behaviourOfJumpSystemWallsMono.ConfigureWall(wall);
        }
        else
        {
            _attack = BehaviourOfJumpSystemNormal.GetAttack();
            _decresing = BehaviourOfJumpSystemNormal.GetDecay();
            _sustain = BehaviourOfJumpSystemNormal.GetSustain();
            _release = BehaviourOfJumpSystemNormal.GetRelease();
            _endJump = BehaviourOfJumpSystemNormal.GetEndJump();
            BehaviourOfJumpSystemNormal.OnAttack = () =>
            {
                OnAttack?.Invoke();
            };
            BehaviourOfJumpSystemNormal.OnMidAir = () =>
            {
                OnMidAir?.Invoke();
            };
            BehaviourOfJumpSystemNormal.OnSustain = () =>
            {
                OnSustain?.Invoke();
            };
            BehaviourOfJumpSystemNormal.OnRelease = () =>
            {
                OnRelease?.Invoke();
            };
            BehaviourOfJumpSystemNormal.OnEndJump = () =>
            {
                OnEndJump?.Invoke();
            };
        }
    }
}