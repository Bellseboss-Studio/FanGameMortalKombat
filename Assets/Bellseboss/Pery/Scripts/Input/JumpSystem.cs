using System;
using Bellseboss.Pery.Scripts.Input;
using UnityEngine;
using Object = UnityEngine.Object;

public class JumpSystem : MonoBehaviour, IJumpSystem
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
    private FloorController _floorController;
    private IMovementRigidBodyV2 _movementRigidBodyV2;

    public void Configure(Rigidbody rigidbody, IMovementRigidBodyV2 movementRigidBodyV2, FloorController floorController)
    {
        Debug.Log($"Configured JumpSystem: {rigidbody.gameObject.name}");
        BehaviourOfJumpSystemWalls.Configure(rigidbody, this);
        BehaviourOfJumpSystemNormal.Configure(rigidbody, this);
        _rigidbody = rigidbody;
        var gameObjectToPlayer = rigidbody.gameObject;
        _rigidbodyConstraints = _rigidbody.constraints;
        _floorController = floorController;
        _movementRigidBodyV2 = movementRigidBodyV2;
        
        movementRigidBodyV2.ChangeToNormalJump();
    }

    public void Jump()
    {
        _attack.Play();
    }

    public void IsScalableWall(bool isScalableWall, FloorController floorController, Vector3 direction)
    {
        _attack?.Stop();
        _decresing?.Stop();
        _sustain?.Stop();
        _release?.Stop();
        _endJump?.Stop();
        
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
            behaviourOfJumpSystemWallsMono.ConfigureWall(direction);
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

    public void ChangeNormalWall()
    {
        _movementRigidBodyV2.ChangeToNormalJump();
        _release.Play();
    }

    public void ChangeRotation(Vector3 rotation)
    {
        _movementRigidBodyV2.ChangeRotation(rotation);
    }

    public void RestoreRotation()
    {
        _movementRigidBodyV2.RestoreRotation();
    }

    public void ExitToWall()
    {
        //TODO: doing something went exit to wall
    }
}

public interface IJumpSystem
{
    void ChangeNormalWall();
    void ChangeRotation(Vector3 rotation);
    void RestoreRotation();
}