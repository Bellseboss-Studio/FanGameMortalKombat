using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TargetingSystem
{
    public class TargetingSystem
    {
        
        public List<GameObject> SetEnemiesOrder(List<GameObject> enemies, Vector3 playerPosition)
        {
            for (int i = 0;i < enemies.Count; i++){
                for (int j = 0; j< enemies.Count -1; j++)
                {
                    var distance1 = Vector3.Distance(enemies[j].gameObject.transform.position, playerPosition);
                    var distance2 = Vector3.Distance(enemies[j+1].gameObject.transform.position, playerPosition);
                    
                    if (distance1 > distance2)
                    {
                        (enemies[j], enemies[j+1]) = (enemies[j+1], enemies[j]);
                    }
                }
            }
            return enemies;
        }
        
        public void SetAutomaticTarget(float distance, List<GameObject> enemies, GameObject player, float attackAngle)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (Vector3.Distance(enemies[i].gameObject.transform.position, player.transform.position) <= distance)
                {
                    if (VerifyAttackAngle(enemies[i].gameObject.transform.position, player) < attackAngle)
                    {
                        player.transform.rotation = Quaternion.LookRotation(enemies[i].transform.position - player.transform.position, Vector3.up);
                        break;
                    }
                }
            }
        }
        
        public void SetManualTarget(GameObject enemy, GameObject player)
        {
            player.transform.rotation = Quaternion.LookRotation(enemy.transform.position - player.transform.position, Vector3.up);
        }
        
        private float VerifyAttackAngle(Vector3 enemyPosition, GameObject player)
        {
            var playerPosition = player.transform.position;
            var angle = Vector3.Angle(enemyPosition - playerPosition, player.transform.forward);
            Debug.Log(angle);
            return angle;
        }
        
    }
}