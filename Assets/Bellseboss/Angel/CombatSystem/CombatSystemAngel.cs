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
        
        private ICombatSystemAngel _combatSystemAngel;


        public Action OnAttack, OnMidAir, OnRelease, OnSustain, OnEndAttack, oneTimeOnEndAttack;
        [SerializeField] private bool canAttackAgain = true;
        [SerializeField] private bool attacking;
        [SerializeField] private float _deltatimeLocal;
        [SerializeField] private int _numberOfCombosQuick;
        [SerializeField] private int _numberOfCombosPower;
        [SerializeField] private TargetFocus targetFocus;
        private TeaTime _attack, _decresing, _sustain, _release;
        private Rigidbody _rigidbody;
        private RigidbodyConstraints _rigidbodyConstraints;
        private bool _isQuickAttack;
        private List<EnemyV2> _enemies = new List<EnemyV2>();
        private StatisticsOfCharacter _statisticsOfCharacter;
        private CombatMovement _currentAttack;
        private List<CombatMovement> _movementsQueue;
        private Action<string> _actionToAnimate;
        private MoveAttackingSystem _moveAttackingSystem;

        public bool Attacking => attacking;

        private void Start()
        {
        }

        public void ExecuteMovement(TypeOfAttack typeOfAttack)
        {
            currentComboSequence.Add(typeOfAttack);
            bool found = false;
            CombatMovement combatMovement1 = null;
            foreach (var combatMovement in combatMovements.Where(combatMovement => combatMovement.comboSequence.SequenceEqual(currentComboSequence)))
            {
                combatMovement1 = combatMovement;
                found = true;
            }

            if (!found)
            {
                /*currentComboSequence = new List<TypeOfAttack>();*/
                currentComboSequence.Remove(currentComboSequence[currentComboSequence.Count - 1]);
                return;
            }

            if (canAttackAgain)
            {
                _currentAttack = combatMovement1;
                _actionToAnimate.Invoke(_currentAttack.transitionParameterName);
                Attack(_currentAttack);
            }
            else
            {
                _movementsQueue.Add(combatMovement1);
            }
        }

        public void Configure(Rigidbody rigidbody, StatisticsOfCharacter statisticsOfCharacter,
            IMovementRigidBodyV2 movementRigidBodyV2, ICombatSystemAngel characterV2)
        {
            var gameObjectToPlayer = rigidbody.gameObject;
            _moveAttackingSystem = new MoveAttackingSystem(gameObjectToPlayer, transform);
            _combatSystemAngel = characterV2;
            _movementsQueue = new List<CombatMovement>();
            _statisticsOfCharacter = statisticsOfCharacter;
            targetFocus.Configure(this);
            _rigidbody = rigidbody;
            _rigidbodyConstraints = _rigidbody.constraints;
            _attack = this.tt().Pause().Add(() =>
            {
                _deltatimeLocal = 0;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
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

                _moveAttackingSystem.MovePlayer(gameObjectToPlayer, heightMultiplier, loop, _currentAttack.forceToAttack, _currentAttack.maxDistance);
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
                if (_movementsQueue.Count > 0)
                {
                    _currentAttack = _movementsQueue[0];
                    _movementsQueue.RemoveAt(0);
                    _actionToAnimate.Invoke(_currentAttack.transitionParameterName);
                    Attack(_currentAttack);
                }
            }).Loop(loop =>
            {
                _deltatimeLocal += loop.deltaTime;
                if (_deltatimeLocal >= _currentAttack.timeToAttack + _currentAttack.timeToDecreasing)
                {
                    loop.Break();
                }

                float t = (_deltatimeLocal - _currentAttack.timeToAttack) / _currentAttack.timeToDecreasing;
                float heightMultiplier = Mathf.Log(1 + t * 4);
                
                _moveAttackingSystem.MovePlayer(gameObjectToPlayer, heightMultiplier, loop, _currentAttack.forceToDecreasing, _currentAttack.distanceToDecresing, true);

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
                
                _moveAttackingSystem.MovePlayer(gameObjectToPlayer, heightMultiplier, loop, _currentAttack.forceToDecreasing, _currentAttack.maxDistance, true);

            }).Add(() =>
            {
                currentComboSequence = new List<TypeOfAttack>();
                canAttackAgain = true;
                _numberOfCombosQuick = 0;
                _numberOfCombosPower = 0;
                attacking = false;
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                OnEndAttack?.Invoke();
                oneTimeOnEndAttack?.Invoke();
                oneTimeOnEndAttack = null;
                movementRigidBodyV2.EndAttackMovement();
            });

            _actionToAnimate = _combatSystemAngel.GetActionToAnimate();
        }

        private void Attack(CombatMovement currentAttack)
        {
            _decresing.Stop();
            _sustain.Stop();
            _release.Stop();
            _attack.Stop().Play();
            attacking = true;
            canAttackAgain = false;
        }
    }
}