using UnityEngine;

internal class AiController : MonoBehaviour
{
    private IEnemyV2 _enemy;

    public void Configure(IEnemyV2 enemy)
    {
        _enemy = enemy;
    }
}