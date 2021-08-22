using System.Collections;

namespace StatesOfEnemies
{
    public interface IEnemyState
    {
        IEnumerator DoAction(IBehavior behavior);
    }
}