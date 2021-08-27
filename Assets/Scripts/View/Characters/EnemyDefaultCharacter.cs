using System;
using System.Collections.Generic;
using CharacterCustom;
using JetBrains.Annotations;
using StatesOfEnemies;
using UnityEngine;

namespace View.Characters
{
    public class EnemyDefaultCharacter : Character, IEnemyCharacter
    {
        protected EnemyBehavior behaviorEnemy;
        private List<Vector3> _points;
        [SerializeField]private float toleranceToArrivedPoint;
        
        public delegate void OnPlayerEnterTrigger(GameObject player);
        public delegate void OnPlayerExitTrigger(GameObject player);
        public event OnPlayerExitTrigger OnPlayerEnter;
        public event OnPlayerExitTrigger OnPlayerExit;

        protected override void ConfigureExplicit()
        {
            _points = new List<Vector3>();
        }

        public void SetBehavior()
        {
            behaviorEnemy = gameObject.AddComponent<EnemyBehavior>();
            var enemyState = behaviorEnemy.Configuration(this);
            StartCoroutine(behaviorEnemy.StartState(enemyState));
        }

        public void MoveToPoint(Vector3 toPoint)
        {
            movementPlayer = (toPoint - transform.position).normalized;
            //Debug.Log($"toPoint {toPoint} transform.position {transform.position}");
        }

        public List<Vector3> GetPoints()
        {
            return _points;
        }

        public bool IsEnemyArrived(Vector3 concurrentPoint)
        {
            var position = transform.position;
            Debug.Log($"(concurrentPoint - position).sqrMagnitude {(concurrentPoint - position).sqrMagnitude} toleranceToArrivedPoint {toleranceToArrivedPoint}");
            return (concurrentPoint - position).sqrMagnitude < toleranceToArrivedPoint;
        }

        public void SubscribeOnPlayerEnterTrigger(OnPlayerExitTrigger action)
        {
            OnPlayerEnter += action;
        }
        
        
        public void UnsubscribeOnPlayerEnterTrigger(OnPlayerExitTrigger action)
        {
            OnPlayerEnter -= action;
        }
        public void CleanOnPlayerEnterTrigger()
        {
            OnPlayerEnter = null;
        }
        public void SubscribeOnPlayerExitTrigger(OnPlayerExitTrigger action)
        {
            OnPlayerExit += action;
        }
        
        
        public void UnsubscribeOnPlayerExitTrigger(OnPlayerExitTrigger action)
        {
            OnPlayerExit -= action;
        }
        public void CleanOnPlayerExitTrigger()
        {
            OnPlayerExit = null;
        }

        public void SetPoints(List<GameObject> pointsList)
        {
            foreach (var point in pointsList)
            {
                _points.Add(point.transform.position);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnPlayerEnter?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnPlayerExit?.Invoke(other.gameObject);
            }
        }
    }
}