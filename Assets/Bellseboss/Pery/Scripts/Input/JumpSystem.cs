using Bellseboss.Pery.Scripts.Input;
using UnityEngine;

public class JumpSystem : MonoBehaviour
{
    
    [SerializeField] private float timeToAttack, timeToDecreasing, timeToSustain, timeToRelease;
    [SerializeField] private float maxHeighJump, heightDecreasing;
    [SerializeField] private float forceToAttack, forceToDecreasing;
    private TeaTime _attack, _decresing, _sustain, _release;
    private Rigidbody _rigidbody;
    private float _deltatimeLocal;
    private RigidbodyConstraints _rigidbodyConstraints;

    public void Configure(Rigidbody rigidbody, IMovementRigidBodyV2 movementRigidBodyV2, FloorController floorController)
    {
        _rigidbody = rigidbody;
        var gameObjectToPlayer = rigidbody.gameObject;
        _rigidbodyConstraints = _rigidbody.constraints;
        _attack = this.tt().Pause().Add(() =>
        {
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            Debug.Log("JumpSystem: Attack");
        }).Loop(loop =>
        {
            //Debug.Log("JumpSystem: Attack Loop");
            _deltatimeLocal += loop.deltaTime;
            if (_deltatimeLocal >= timeToAttack)
            {
                loop.Break();
            }
            
            float t = _deltatimeLocal / timeToAttack;
            float heightMultiplier = Mathf.Cos(t * Mathf.PI * 0.5f);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position, position + Vector3.up * (maxHeighJump * heightMultiplier), forceToAttack * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Loop(loop =>
        {
            //Debug.Log("JumpSystem: Decreasing Loop");
            _deltatimeLocal += loop.deltaTime;
            if(_deltatimeLocal >= timeToAttack + timeToDecreasing)
            {
                loop.Break();
            }

            float t = (_deltatimeLocal - timeToAttack) / timeToDecreasing;
            float heightMultiplier = Mathf.Log(1 + t * 4);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position, position + Vector3.up * (heightDecreasing * heightMultiplier), forceToDecreasing * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Loop(loop =>
        {
            //Debug.Log("JumpSystem: Sustain Loop");
            _deltatimeLocal += loop.deltaTime;
            if (_deltatimeLocal >= timeToAttack + timeToDecreasing + timeToSustain)
            {
                loop.Break();
            }
        }).Loop(loop=>
        {
            //Debug.Log("JumpSystem: Release Loop");
            _deltatimeLocal += loop.deltaTime;
            if (floorController.IsTouchingFloor())
            {
                loop.Break();
            }

            var t = _deltatimeLocal / timeToAttack;
            var heightMultiplier = Mathf.Log(1 + t * forceToDecreasing);

            var position = gameObjectToPlayer.transform.position;
            position = Vector3.Lerp(position, position - Vector3.up * (maxHeighJump * heightMultiplier), forceToDecreasing * loop.deltaTime);
            gameObjectToPlayer.transform.position = position;
        }).Add(() =>
        {
            _rigidbody.useGravity = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            _deltatimeLocal = 0;
            Debug.Log("JumpSystem: Attack End");
        });
    }

    public void Jump()
    {
        Debug.Log("JumpSystem: Jump");
        _attack.Play();
    }
}