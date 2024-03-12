using UnityEngine;

[CreateAssetMenu(menuName = "Bellseboss/CharacterStatistics", fileName = "StatisticsOfCharacter", order = 0)]
public class StatisticsOfCharacter : ScriptableObject
{
    public int life;
    public int damage;
    public float speedToMove;
    
}