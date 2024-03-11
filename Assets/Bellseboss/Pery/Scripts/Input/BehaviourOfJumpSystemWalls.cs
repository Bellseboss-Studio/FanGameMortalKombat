using System;
using UnityEngine;

namespace Bellseboss.Pery.Scripts.Input
{
    public class BehaviourOfJumpSystemWalls : MonoBehaviour, IBehaviourOfJumpSystem
    {
        public Action OnAttack { get; set; }
        public Action OnMidAir { get; set; }
        public Action OnSustain { get; set; }
        public Action OnRelease { get; set; }
        public Action OnEndJump { get; set; }
        
        private TeaTime _attack, _decresing, _sustain, _release, _endJump, _delayToJump;
        private float _deltatimeLocal, _deltatimeLocalToJump;
        [SerializeField] private float timeToAttack, timeToDecreasing, timeToSustain, timeToRelease, timeToWaitToJump;
        [SerializeField] private float maxHeighJump, heightDecreasing;
        [SerializeField] private float forceToAttack, forceToDecreasing;
        [SerializeField] private FloorController floorController;
        [SerializeField] private float lenghtOfArrow;
        [SerializeField] private Vector3 _direction;
        private bool canJump;
        private bool isJumping;
        private bool isColliding;


        public void Configure(Rigidbody _rigidbody, IJumpSystem jumpSystem)
        {
            Debug.Log($"Configured BehaviourOfJumpSystemWall: {_rigidbody.gameObject.name}");
            var gameObjectToPlayer = _rigidbody.gameObject;
            _attack = this.tt().Pause().Add(() =>
            {
                _rigidbody.useGravity = false;
                _rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ |
                                         RigidbodyConstraints.FreezeRotationX;
                _deltatimeLocalToJump = 0;
                Debug.Log("JumpSystem BehaviourOfJumpSystemWall: Attack");
                isJumping = true;
                isColliding = false;
            }).Add(()=>
            {
                //rotate the player to the direction of the wall
                //_direction = Quaternion.LookRotation(_direction)
                jumpSystem.ChangeRotation(_direction);
            }).Add(() => { OnAttack?.Invoke(); }).Loop(loop =>
            {
                //Debug.Log("JumpSystem BehaviourOfJumpSystemWall: Attack Loop");
                _deltatimeLocalToJump += loop.deltaTime;
                if (_deltatimeLocalToJump >= timeToAttack || isColliding)
                {
                    loop.Break();
                }
                float t = _deltatimeLocalToJump / timeToAttack;
                float heightMultiplier = Mathf.Cos(t * Mathf.PI * 0.5f);

                var position = gameObjectToPlayer.transform.position;
                position = Vector3.Lerp(position, position + _direction * (maxHeighJump * heightMultiplier),
                    forceToAttack * loop.deltaTime);
                Debug.Log($"position {gameObjectToPlayer.transform.position} target {position} con direccion {_direction} con time {_deltatimeLocalToJump}");
                gameObjectToPlayer.transform.position = position;
            }).Add(() =>
            {
                //_decresing.Play();
                Debug.Log("JumpSystem BehaviourOfJumpSystemWall: End Attack");
                _rigidbody.useGravity = true;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                jumpSystem.ChangeNormalWall();
                isJumping = false;
                jumpSystem.RestoreRotation();
            });
            _delayToJump = this.tt().Add(() =>
            {
                Debug.Log("JumpSystem: Delay to Jump");
                _deltatimeLocal = 0;
                _rigidbody.useGravity = false;
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                canJump = true;
                isColliding = true;
            }).Loop(loop =>
            {
                _deltatimeLocal += loop.deltaTime;
                if (_deltatimeLocal >= timeToWaitToJump || isJumping)
                {
                    loop.Break();
                }
            }).Add(() =>
            {
                canJump = false;
                if(!isJumping)
                {
                    Debug.Log("JumpSystem: Delay to Jump End");
                    _rigidbody.useGravity = true;
                    _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                    jumpSystem.ChangeNormalWall();
                }
            });

            _decresing = this.tt().Pause().Add(() =>
            {
                Debug.Log("JumpSystem BehaviourOfJumpSystemWall: Decreasing");
            });
            _sustain = this.tt().Pause().Add(() =>
            {
                Debug.Log("JumpSystem BehaviourOfJumpSystemWall: Sustain");
            });
            _release = this.tt().Pause().Add(() =>
            {
                Debug.Log("JumpSystem BehaviourOfJumpSystemWall: Release");
            });
            _endJump = this.tt().Pause().Add(() =>
            {
                Debug.Log("JumpSystem BehaviourOfJumpSystemWall: End Jump");
            });
            
        }

        public TeaTime GetAttack()
        {
            return _attack;
        }

        public TeaTime GetDecay()
        {
            return _decresing;
        }

        public TeaTime GetSustain()
        {
            return _sustain;
        }

        public TeaTime GetRelease()
        {
            return _release;
        }

        public TeaTime GetEndJump()
        {
            return _endJump;
        }

        public void StopAll()
        {
            _attack.Stop();
            _decresing.Stop();
            _sustain.Stop();
            _release.Stop();
            _endJump.Stop();
        }

        public void ConfigureWall(Vector3 direction)
        {
            _direction = direction;
            _delayToJump.Play();
            Debug.Log($"JumpSystem BehaviourOfJumpSystemWall: ConfigureWall {direction}");
        }
        
        
        private void OnDrawGizmos()
        {
            //draw a line to direction of object with gizmo
            var direc = _direction;
            if (direc == Vector3.zero) return;
            //draw a line to front of object with gizmo
            Gizmos.color = Color.red;
            //Target to front of object
            var target = transform.position + direc * lenghtOfArrow;
            Gizmos.DrawLine(transform.position, target);
        
            // Dibujar la punta de la flecha en orientacion de la direccion
            Vector3 rightArrowPoint = Quaternion.LookRotation(direc) * Quaternion.Euler(0, 180 + 20, 0) * new Vector3(0, 0, 1);
            Vector3 leftArrowPoint = Quaternion.LookRotation(direc) * Quaternion.Euler(0, 180 - 20, 0) * new Vector3(0, 0, 1);

            Gizmos.DrawLine(target, target + rightArrowPoint * (lenghtOfArrow/5));
            Gizmos.DrawLine(target, target + leftArrowPoint * (lenghtOfArrow/5));

        }
    }
}