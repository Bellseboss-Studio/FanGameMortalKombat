using System.Collections;
using UnityEngine;
using View;
using View.Characters;
using Random = UnityEngine.Random;

namespace StatesOfEnemies
{
    public class EnemyBehavior : MonoBehaviour, IBehavior
    {
        private int _nextState;
        private EnemyStatesConfiguration _enemyStatesConfiguration;
        private IEnemyCharacter _enemyCharacter;
        private GameObject target;
        private bool playerIsInRedZone;
        private bool iAmDeath;
        private bool exitStatesSystem = false;
        private IBehavior _behaviorImplementation;

        public IEnemyState Configuration(IEnemyCharacter enemyCharacter)
        {
            _enemyCharacter = enemyCharacter;
            _enemyStatesConfiguration = new EnemyStatesConfiguration();
            _enemyStatesConfiguration.AddInitialState(EnemyStatesConfiguration.PatrolState, new PatrolState(_enemyCharacter.GetPoints()));
            _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.ComebackState, new ComeBackState());
            _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.FollowTarget, new FollowTarget());
            _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.LookPlayer, new WaitState());
            _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.AttackPlayer, new AttackPlayerState());
            _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.Death, new Death());
            _nextState = 0;
            _enemyCharacter.SubscribeOnPlayerEnterTrigger((player) =>
            {
                target = player;
                playerIsInRedZone = true;
            });
            _enemyCharacter.SubscribeOnPlayerExitTrigger((player) =>
            {
                playerIsInRedZone = false;
            });
            return _enemyStatesConfiguration.GetInitialState();
        }

        public IEnumerator StartState(IEnemyState state)
        {
            if (exitStatesSystem) yield break;
            StartCoroutine(state.DoAction(this));
            while (GetNextState() == 0)
            {
                yield return new WaitForSeconds(1f);
            }
            var nextClassState = _enemyStatesConfiguration.GetState(GetNextState());
            CleanState();
            state = nextClassState;
            StartCoroutine(StartState(state));
        }

        private void CleanState()
        {
            _nextState = 0;
        }

        public int GetNextState()
        {
            return _nextState;
        }

        public int GetRandom(int start, int end)
        {
            return Random.Range(start, end);
        }

        public Vector3 GetTarget()
        {
            return target.transform.position;
        }

        public bool IsPlayerInYellowZone()
        {
            return _enemyCharacter.IsPlayerInYellowZone();
        }

        public void LookPlayer(Vector3 target)
        {
            _enemyCharacter.LookTarget(target);
        }

        public bool IsPlayerInGreenZone()
        {
            return _enemyCharacter.IsPlayerInGreenZone();
        }

        public bool IsPlayerInRangeOfAttack()
        {
            return _enemyCharacter.IsPlayerInRangeOfAttack();
        }

        public void StopMovementForAttack()
        {
            _enemyCharacter.StopMovement();
        }

        public float GetEnemyVelocity()
        {
            return _enemyCharacter.GetVelocity();
        }

        public void Attack()
        {
            if (target.TryGetComponent<Character>(out var character))
            {
                _enemyCharacter.Attack(character, 3);   
            }
        }

        public bool GetIAmDeath()
        {
            return iAmDeath;
        }

        public void SetIAmDeath(bool value)
        {
            iAmDeath = value;
        }

        public void CleanAndDestroy()
        {
            Destroy(gameObject);
        }

        public bool GetExitStatesSystem()
        {
            return exitStatesSystem;
        }

        public void SetExitStatesSystem(bool value)
        {
            exitStatesSystem = value;
        }

        public void WalkToPlayer()
        {
            var point = _enemyCharacter.GetPointAccordingPlayer(gameObject);
            WalkToPoint(new Vector3(point.x, target.transform.position.y, point.z));
        }

        public void SetNextState(int nextStateFromState)
        {
            /*Debug.Log(nextStateFromState);*/
            _nextState = nextStateFromState;
        }

        public void WalkToPoint(Vector3 toPoint)
        {
            _enemyCharacter.MoveToPoint(toPoint);
        }

        public bool IsPlayerInRedZone()
        {
            return playerIsInRedZone;
        }

        public bool IsEnemyArrived(Vector3 concurrentPoint)
        {
            return _enemyCharacter.IsEnemyArrived(concurrentPoint);
        }


    }
}