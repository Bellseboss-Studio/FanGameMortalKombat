using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Bellseboss.Pery.Scripts.Input;

namespace Bellseboss.Angel.CombatSystem
{
    public class CombatSystemAngel : MonoBehaviour, IFocusTarget
    {
        [SerializeField] private List<CombatMovement> combatMovements;
        [SerializeField] private List<TypeOfAttack> currentComboSequence;
        /*public MovementRigidbodyV2 movementRigidbodyV2;
        [SerializeField] private float runningDistanceQuickAttack;
        [SerializeField] private float runningDistancePowerAttack;
        [SerializeField] private Vector3 runningDirectionQuickAttack;
        [SerializeField] private Vector3 runningDirectionPowerAttack;*/


        public Action OnAttack, OnMidAir, OnRelease, OnSustain, OnEndAttack, oneTimeOnEndAttack;
        /*[SerializeField] private AttackMovementData attackMovementDataQuick;
        [SerializeField] private AttackMovementData attackMovementDataPower;*/
        [SerializeField] private bool canAttackAgain = true;
        [SerializeField] private bool attacking;
        [SerializeField] private float _deltatimeLocal;
        /*[SerializeField] private int maxNumberOfCombosQuick;
        [SerializeField] private int maxNumberOfCombosPower;*/
        [SerializeField] private int _numberOfCombosQuick;
        [SerializeField] private int _numberOfCombosPower;
        [SerializeField] private TargetFocus targetFocus;
        private TeaTime _attack, _decresing, _sustain, _release;
        private Rigidbody _rigidbody;
        private RigidbodyConstraints _rigidbodyConstraints;
        /*private AttackMovementData _attackMovementData;*/
        private bool _isQuickAttack;
        private List<EnemyV2> _enemies = new List<EnemyV2>();
        private StatisticsOfCharacter _statisticsOfCharacter;
        private CombatMovement _currentAttack;

        public bool Attacking => attacking;

        public void Kick(Action<string> animationAction)
        {
            if (!canAttackAgain) return;
            currentComboSequence.Add(TypeOfAttack.Power);
            bool found = false;
            foreach (var combatMovement in combatMovements.Where(combatMovement => combatMovement.comboSequence.SequenceEqual(currentComboSequence)))
            {
                found = true;
                _currentAttack = combatMovement;
            }

            if (!found)
            {
                currentComboSequence = new List<TypeOfAttack>();
                return;
            }
            
            animationAction.Invoke(_currentAttack.transitionParameterName);
            Attack(_currentAttack);
        }

        public void Punch(Action<string> animationAction)
        {
            if (!canAttackAgain) return;
            currentComboSequence.Add(TypeOfAttack.Quick);
            bool found = false;
            foreach (var combatMovement in combatMovements.Where(combatMovement => combatMovement.comboSequence.SequenceEqual(currentComboSequence)))
            {
                found = true;
                _currentAttack = combatMovement;
            }

            if (!found)
            {
                currentComboSequence = new List<TypeOfAttack>();
                return;
            }
            
            animationAction.Invoke(_currentAttack.transitionParameterName);
            Attack(_currentAttack);
        }

        public void Configure(Rigidbody rigidbody, StatisticsOfCharacter statisticsOfCharacter,
            IMovementRigidBodyV2 movementRigidBodyV2)
        {
            _statisticsOfCharacter = statisticsOfCharacter;
            targetFocus.Configure(this);
            _rigidbody = rigidbody;
            var gameObjectToPlayer = rigidbody.gameObject;
            _rigidbodyConstraints = _rigidbody.constraints;
            _attack = this.tt().Pause().Add(() =>
            {
                _deltatimeLocal = 0;
                //_rigidbody.useGravity = false;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ |
                                         RigidbodyConstraints.FreezeRotationX;
                if (attacking)
                {
                    /*if (_isQuickAttack)
                    {
                        if (_numberOfCombosQuick <= maxNumberOfCombosQuick)
                        {
                            _numberOfCombosQuick++;
                        }
                    }
                    else
                    {
                        if (_numberOfCombosPower <= maxNumberOfCombosPower)
                        {
                            _numberOfCombosPower++;
                        }
                    }*/
                }
            }).Add(() =>
            {
                OnAttack?.Invoke();
                //Debug.Log("AttackMovementSystem: Start Attack");
                targetFocus.EnableCollider();
            }).Loop(loop =>
            {
                _deltatimeLocal += loop.deltaTime;
                if (_deltatimeLocal >= _currentAttack.timeToAttack)
                {
                    loop.Break();
                }

                float t = _deltatimeLocal / _currentAttack.timeToAttack;
                float heightMultiplier = Mathf.Cos(t * Mathf.PI * 0.5f);

                var position = gameObjectToPlayer.transform.position;
                position = Vector3.Lerp(position,
                    position + transform.forward * (_currentAttack.maxDistance * heightMultiplier),
                    _currentAttack.forceToAttack * loop.deltaTime);
                gameObjectToPlayer.transform.position = position;
            }).Add(() =>
            {
                //Debug.Log("AttackMovementSystem: Attack End");
                _decresing.Play();
                foreach (var enemy in targetFocus.GetEnemies<EnemyV2>())
                {
                    enemy.ReceiveDamage(_statisticsOfCharacter.damage, gameObject.transform.forward);
                    enemy.SetAnimationToHit(_isQuickAttack,
                        _isQuickAttack ? _numberOfCombosQuick : _numberOfCombosPower);
                }

                targetFocus.CleanEnemies();
                targetFocus.DisableCollider();
            });

            _decresing = this.tt().Pause().Add(() =>
            {
                //Debug.Log("AttackMovementSystem: Decresing Start");
                canAttackAgain = true;
                OnMidAir?.Invoke();
            }).Loop(loop =>
            {
                _deltatimeLocal += loop.deltaTime;
                if (_deltatimeLocal >= _currentAttack.timeToAttack + _currentAttack.timeToDecreasing)
                {
                    canAttackAgain = false;
                    currentComboSequence = new List<TypeOfAttack>();
                    loop.Break();
                }

                float t = (_deltatimeLocal - _currentAttack.timeToAttack) / _currentAttack.timeToDecreasing;
                float heightMultiplier = Mathf.Log(1 + t * 4);

                var position = gameObjectToPlayer.transform.position;
                position = Vector3.Lerp(position,
                    position - transform.forward * (_currentAttack.distanceToDecresing * heightMultiplier),
                    _currentAttack.forceToDecreasing * loop.deltaTime);
                if (!double.IsNaN(position.x) && !double.IsNaN(position.y) && !double.IsNaN(position.z))
                {
                    gameObjectToPlayer.transform.position = position;
                }
            }).Add(() =>
            {
                //Debug.Log("AttackMovementSystem: Decresing End");
                _sustain.Play();
            });

            _sustain = this.tt().Pause().Add(() =>
            {
                //Debug.Log("AttackMovementSystem: Sustain Start");
                OnSustain?.Invoke();
            }).Loop(loop =>
            {
                _deltatimeLocal += loop.deltaTime;
                if (_deltatimeLocal >= _currentAttack.timeToAttack + _currentAttack.timeToDecreasing +
                    _currentAttack.timeToSustain)
                {
                    loop.Break();
                }
            }).Add(() =>
            {
                //Debug.Log("AttackMovementSystem: Sustain End");
                _release.Play();
            });

            _release = this.tt().Pause().Add(() =>
            {
                //Debug.Log("AttackMovementSystem: Release Start");
                OnRelease?.Invoke();
            }).Loop(loop =>
            {
                _deltatimeLocal += loop.deltaTime;
                if (_deltatimeLocal >= _currentAttack.timeToAttack + _currentAttack.timeToDecreasing +
                    _currentAttack.timeToSustain + _currentAttack.timeToRelease)
                {
                    loop.Break();
                }

                var t = _deltatimeLocal / _currentAttack.timeToAttack;
                var heightMultiplier = Mathf.Log(1 + t * _currentAttack.forceToDecreasing);

                var position = gameObjectToPlayer.transform.position;
                position = Vector3.Lerp(position,
                    position - transform.forward * (_currentAttack.maxDistance * heightMultiplier),
                    _currentAttack.forceToDecreasing * loop.deltaTime);
                gameObjectToPlayer.transform.position = position;
            }).Add(() =>
            {
                canAttackAgain = true;
                _numberOfCombosQuick = 0;
                _numberOfCombosPower = 0;
                attacking = false;
                _rigidbody.velocity = Vector3.zero;
                //_rigidbody.useGravity = true;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                //Debug.Log("AttackMovementSystem: End Attack");
                OnEndAttack?.Invoke();
                oneTimeOnEndAttack?.Invoke();
                oneTimeOnEndAttack = null;
                movementRigidBodyV2.EndAttackMovement();
            });
        }

        public void Attack(CombatMovement currentAttack)
        {
            //Debug.Log("AttackMovementSystem: Attack");
            canAttackAgain = false;
            attacking = true;
            /*if (typeOfAttack == TypeOfAttack.Quick)
            {
                _isQuickAttack = true;
                _attackMovementData = attackMovementDataQuick;
            }
            else
            {
                _isQuickAttack = false;
                _attackMovementData = attackMovementDataPower;
            }*/

            _decresing.Stop();
            _attack.Stop().Play();
        }

        public enum TypeOfAttack
        {
            Quick,
            Power
        }

        /*public bool CanAttackAgain()
        {
            return canAttackAgain;
        }

        public bool FullCombo()
        {
            return _numberOfCombosQuick >= maxNumberOfCombosQuick || _numberOfCombosPower >= maxNumberOfCombosPower;
        }*/
    }
}