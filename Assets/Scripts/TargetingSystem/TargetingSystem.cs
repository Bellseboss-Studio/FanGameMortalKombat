using System.Collections.Generic;
using UnityEngine;

namespace TargetingSystem
{
    public class TargetingSystem
    {
        
        public List<GameObject> SetEnemiesOrder(List<GameObject> enemies, Vector3 playerPosition)
        {
            GameObject temp;
            float distance1;
            float distance2;

            for (int i = 0;i < enemies.Count; i++){
                for (int j = 0; j< enemies.Count -1; j++)
                {
                    distance1 = Vector3.Distance(enemies[j].gameObject.transform.position, playerPosition);
                    distance2 = Vector3.Distance(enemies[j+1].gameObject.transform.position, playerPosition);
                    
                    if (distance1 > distance2)
                    {
                        temp = enemies[j]; 
                        enemies[j] = enemies[j+1]; 
                        enemies[j+1] = temp;
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
                    }
                }
            }
        }
        
        public void SetManualTarget(GameObject enemie, GameObject player)
        {
            player.transform.rotation = Quaternion.LookRotation(enemie.transform.position - player.transform.position, Vector3.up);
        }
        
        public float VerifyAttackAngle(Vector3 enemyPosition, GameObject player)
        {
            Vector3 playerPosition = player.transform.position;
            float angle = Vector3.Angle(enemyPosition - playerPosition, player.transform.forward);
            return angle;
        }
        
    }
}