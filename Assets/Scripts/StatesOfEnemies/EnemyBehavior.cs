using System;
using System.Collections;
using UnityEngine;
using View.Characters;
using Random = UnityEngine.Random;

namespace StatesOfEnemies
{
    public class EnemyBehavior : MonoBehaviour, IBehavior
    {
        private int _nextState;
        private EnemyStatesConfiguration _enemyStatesConfiguration;
        private IEnemyCharacter _enemyCharacter;
        private GameObject targer;
        private bool playerIsInRedZone;

        public IEnemyState Configuration(IEnemyCharacter enemyCharacter)
        {
            _enemyCharacter = enemyCharacter;
            _enemyStatesConfiguration = new EnemyStatesConfiguration();
            _enemyStatesConfiguration.AddInitialState(EnemyStatesConfiguration.PatrolState, new PatrolState(_enemyCharacter.GetPoints()));
            _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.ComebackState, new ComeBackState());
            _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.FollowTarget, new FollowTarget());
            _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.LookPlayer, new WaitState());
            _nextState = 0;
            _enemyCharacter.SubscribeOnPlayerEnterTrigger((player) =>
            {
                targer = player;
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
            return targer.transform.position;
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

        public void SetNextState(int nextStateFromState)
        {
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