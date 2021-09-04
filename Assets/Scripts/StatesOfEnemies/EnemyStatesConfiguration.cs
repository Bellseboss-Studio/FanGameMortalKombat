using System.Collections.Generic;
using UnityEngine.Assertions;

namespace StatesOfEnemies
{
    public class EnemyStatesConfiguration
    {
        private int InitialState;

        public const int PatrolState = 1;
        public const int ComebackState = 2;
        public const int FollowTarget = 3;
        public const int LookPlayer = 4;
        public const int AttackPlayer = 5;

        private readonly Dictionary<int, IEnemyState> _states;

        public EnemyStatesConfiguration()
        {
            _states = new Dictionary<int, IEnemyState>();
        }


        public void AddInitialState(int id, IEnemyState state)
        {
            _states.Add(id, state);
            InitialState = id;
        }

        public void AddState(int id, IEnemyState state)
        {
            _states.Add(id, state);
        }

        public IEnemyState GetState(int stateId)
        {
            Assert.IsTrue(_states.ContainsKey(stateId), $"State with id {stateId} do not exit");
            return _states[stateId];
        }

        public IEnemyState GetInitialState()
        {
            return GetState(InitialState);
        }
    }
}