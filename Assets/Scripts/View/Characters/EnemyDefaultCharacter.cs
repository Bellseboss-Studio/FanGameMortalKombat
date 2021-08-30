using System;
using System.Collections.Generic;
using CharacterCustom;
using JetBrains.Annotations;
using StatesOfEnemies;
using UnityEngine;
using View.Characters.Enemy;
using View.Zone;

namespace View.Characters
{
    public class EnemyDefaultCharacter : Character, IEnemyCharacter
    {
        protected EnemyBehavior behaviorEnemy;
        private List<Vector3> _points;
        [SerializeField]private float toleranceToArrivedPoint;
        [SerializeField] private RedZoneComponent _redZoneComponent;
        private AreaZoneController yellowZone;
        private AreaZoneController greenZone;
        private GameObject target;

        public delegate void OnPlayerTrigger(GameObject player);

        protected override void ConfigureExplicit()
        {
            _points = new List<Vector3>();
        }

        public void SetBehavior(AreaZoneController yellowZone, AreaZoneController greenZone)
        {
            this.yellowZone = yellowZone;
            this.greenZone = greenZone;
            behaviorEnemy = gameObject.AddComponent<EnemyBehavior>();
            var enemyState = behaviorEnemy.Configuration(this);
            StartCoroutine(behaviorEnemy.StartState(enemyState));
        }

        public void MoveToPoint(Vector3 toPoint)
        {
            movementPlayer = (toPoint - transform.position).normalized;
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

        public void SubscribeOnPlayerEnterTrigger(OnPlayerTrigger action)
        {
            _redZoneComponent.OnPlayerEnter += action;
        }
        
        public void SubscribeOnPlayerExitTrigger(OnPlayerTrigger action)
        {
            _redZoneComponent.OnPlayerExit += action;
        }

        public bool IsPlayerInYellowZone()
        {
            return yellowZone.IsPlayerInThisZone();
        }

        public bool IsPlayerInGreenZone()
        {
            return greenZone.IsPlayerInThisZone();
        }

        public void LookTarget(Vector3 target)
        {
            movementPlayer = Vector3.zero;
            transform.LookAt(target);
        }

        public void SetPoints(List<GameObject> pointsList)
        {
            foreach (var point in pointsList)
            {
                _points.Add(point.transform.position);
            }
        }

    }
}