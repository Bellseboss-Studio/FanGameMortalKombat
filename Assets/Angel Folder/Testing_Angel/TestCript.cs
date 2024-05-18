using System.Collections.Generic;
using TargetingSystemPath;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Testing_Angel
{
    
    public class TestCript : MonoBehaviour
    {
        private TargetingSystem _targetingSystem;
        [SerializeField] private List<GameObject> enemiesInCombat;
        //[SerializeField] private PlayerInput playerInput;
        [SerializeField] private float angleAttack = 60;
        
        
        private void Awake()
        {
            _targetingSystem = new TargetingSystem();
        }

        private void OnTest()
        {
            enemiesInCombat = new List<GameObject>(_targetingSystem.SetEnemiesOrder(enemiesInCombat, transform.position));
            _targetingSystem.SetAutomaticTarget(5, enemiesInCombat, gameObject, angleAttack);
        }

        private void OnManualTarget()
        {
            enemiesInCombat = new List<GameObject>(_targetingSystem.SetEnemiesOrder(enemiesInCombat, transform.position));
        }

        private void Update()
        {
            if (Keyboard.current.sKey.isPressed)
            {
                //_targetingSystem.SetManualTarget(enemiesInCombat[0],gameObject);
            }
        }
    }
}