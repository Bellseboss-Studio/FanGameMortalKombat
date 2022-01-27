using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StatesOfEnemies
{
    public class Death : IEnemyState
    {
        public IEnumerator DoAction(IBehavior behavior)
        {
            behavior.SetExitStatesSystem(true);
            yield return new WaitForSeconds(10);
            behavior.CleanAndDestroy();
        }
    }
}