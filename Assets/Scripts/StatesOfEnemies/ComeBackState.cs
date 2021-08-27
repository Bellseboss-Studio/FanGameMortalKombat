using System.Collections;
using UnityEngine;

namespace StatesOfEnemies
{
    public class ComeBackState : IEnemyState
    {
        public ComeBackState()
        {
            
        }

        public IEnumerator DoAction(IBehavior behavior)
        {
            Debug.Log("ComeBackState");
            yield return new WaitForSeconds(3f);
            behavior.SetNextState(EnemyStatesConfiguration.PatrolState);
        }
    }
}