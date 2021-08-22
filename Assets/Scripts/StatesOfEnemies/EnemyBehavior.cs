using System;
using System.Collections;
using UnityEngine;
using View.Characters;

namespace StatesOfEnemies
{
    public class EnemyBehavior : MonoBehaviour, IBehavior
    {
        [SerializeField] private bool playerInRedZone;
        private int _nextState;
        private EnemyStatesConfiguration _enemyStatesConfiguration;
        private IEnemyCharacter _enemyCharacter;

        public IEnemyState Configuration(IEnemyCharacter enemyCharacter)
        {
            _enemyCharacter = enemyCharacter;
            _enemyStatesConfiguration = new EnemyStatesConfiguration();
            _enemyStatesConfiguration.AddInitialState(EnemyStatesConfiguration.patrolState, new PatrolState(_enemyCharacter.GetPointA(), _enemyCharacter.GetPointB()));
            _enemyStatesConfiguration.AddState(EnemyStatesConfiguration.comebackState, new ComeBackState());
            _nextState = 0;
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

        private int GetNextState()
        {
            return _nextState;
        }

        public void SetNextState(int nextStateFromState)
        {
            _nextState = nextStateFromState;
        }

        public void WalkToPoint(Vector3 toPoint)
        {
            _enemyCharacter.MoveToPoint(toPoint);
        }

        public bool IsPLayerInRedZone()
        {
            return playerInRedZone;
        }
    }
}