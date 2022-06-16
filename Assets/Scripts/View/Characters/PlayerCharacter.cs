using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using InputSystemCustom;
using ServiceLocatorPath;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace View.Characters
{
    public class PlayerCharacter : Character
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] protected GameObject pointToCamera, pointFarToCamera;
        [SerializeField] private string punch, kick;
        [SerializeField] private float angleAttack = 90;
        [SerializeField] private List<GameObject> _enemiesInCombat;
        [SerializeField] private float deltaAddingEnergy;
        [SerializeField] private float energy;// de 0 a 1
        public List<GameObject> EnemiesInCombat => _enemiesInCombat;
        public Action patada, punio, fatality;
        public Action<Vector2> movimiento;
        
        [SerializeField] private CameraChange cameraChange;
        private bool changeIdle;
        private EventsOfFightPlayerInput playerInputFight;

        private Vector3 pointInicialToPointToFar;
        public bool CanMove;
        public bool CanReadInputs;

        private bool isAiming;
        private bool _isOn = true;
        private CinemachineFreeLook _secondCamera;
        private CinemachineTargetGroup _group;
        private bool _powerOn;

        private Vector2 movementInputValue;
        
        private TargetingSystem.TargetingSystem _targetingSystem;
        protected override void Start()
        {
            _enemiesInCombat = new List<GameObject>();
            _targetingSystem = new TargetingSystem.TargetingSystem();
            base.Start();
            pointInicialToPointToFar = pointFarToCamera.transform.localPosition;
            OnPunchEvent+=OnPunchEventInPlayer;
            OnKickEvent += OnKickEventInPlayer;
            OnAimEvent += OnAimEventInPlayer;
            CanMove = true;
            CanReadInputs = true;
            playerInputFight = new EventsOfFightPlayerInput(this, playerInput);
            OnFinishedAnimatorFight += OnFinishedAnimatorPlayer;
        }

        private void OnFinishedAnimatorPlayer()
        {
            CanMove = true;
            CanReadInputs = true;
            OnInputChangedExtend(movementInputValue);
        }
        
        protected override void FinishedAnimatorDamage()
        {
            CanMove = true;
            CanReadInputs = true;
        }
        
        private void OnKickEventInPlayer()
        {
            if (!CanReadInputs) return;
            Debug.Log($"kick");
            if (_powerOn)
            {
                Debug.Log("poder 2");
            }
            else
            {
                _enemiesInCombat = new List<GameObject>(_targetingSystem.SetEnemiesOrder(_enemiesInCombat, transform.position));
                if (_enemiesInCombat.Count > 0) _targetingSystem.SetAutomaticTarget(5, _enemiesInCombat, instantiate, angleAttack);
                animator.SetTrigger(kick);
                CanMove = false;
                CanReadInputs = false;
                Move(Vector3.zero);
                OnInputChangedExtend(movementInputValue);
            }
        }

        private void OnPunchEventInPlayer()
        {
            if (!CanReadInputs) return;
            Debug.Log($"punch");
            if (_powerOn)
            {
                Debug.Log("poder 1");
            }
            else
            {
                _enemiesInCombat = new List<GameObject>(_targetingSystem.SetEnemiesOrder(_enemiesInCombat, transform.position));
                if (_enemiesInCombat.Count > 0) _targetingSystem.SetAutomaticTarget(5, _enemiesInCombat, instantiate, angleAttack);
                animator.SetTrigger(punch);
                CanMove = false;
                CanReadInputs = false;
                Move(Vector3.zero);
                OnInputChangedExtend(Vector2.zero);
            }
            if (!CanReadInputs) return;
            
        }

        protected override void OnAimEventInPlayer()
        {
            if (isAiming)
            {
                isAiming = false;
                if (_enemiesInCombat.Count > 0)
                {
                    cameraChange.RemoveGameObjectToTargetGroup(_enemiesInCombat[0].transform);
                    cameraChange.FreeLookCamera();
                }
                Debug.Log("DejaDeApuntar");
            }
            else
            {
                if (_enemiesInCombat.Count > 0)
                {
                    _enemiesInCombat = new List<GameObject>(_targetingSystem.SetEnemiesOrder(_enemiesInCombat, transform.position));
                    cameraChange.AddGameObjectToTargetGroup(_enemiesInCombat[0].transform);
                    cameraChange.LookEnemyAndPlayer();
                }

                isAiming = true;
                Debug.Log("Apunta");
            }
        }
        
        protected override void UpdateLegacy()
        {
            if (!changeIdle)
            {
                if (Random.Range(0, 100) < 2)
                {
                    changeIdle = true;
                    animator.SetTrigger("change_idle");
                    StartCoroutine(DelayToIdle());
                }
            }
            if (isAiming)
            {
                if (_enemiesInCombat.Count > 0) _targetingSystem.SetManualTarget(_enemiesInCombat[0],instantiate, transform);
            }
        }

        private IEnumerator DelayToIdle()
        {
            yield return new WaitForSeconds(5f);
            changeIdle = false;
        }

        protected override void ConfigureExplicit()
        {
            _inputCustom.playerInput = playerInput;
        }

        public override float GetDamageForKick()
        {
            return power * 4;
        }

        public override float GetDamageForPunch()
        {
            return power * 2;
        }

        protected override void Muerte()
        {
            {
                animator.SetTrigger("Muerte");
                
            }
        }

        public override Vector3 GetDirectionWithObjective()
        {
            if (_enemiesInCombat[0] != null)
            {
                var direction = transform.position - _enemiesInCombat[0].transform.position;
                direction.Normalize();
                return direction;
            }
            
            return transform.forward;
        }

        private void OnMovementControllers(InputValue value)
        {
            movementInputValue = value.Get <Vector2>();
            OnInputChangedExtend(movementInputValue);
        }

        private void OnCameraMovement(InputValue value)
        {
            if (IsInPause) return;
            var vector2 = value.Get<Vector2>();
            OnCameraMovementExtend(vector2);
        }

        public Transform GetPointToCamera()
        {
            return pointToCamera.transform;
        }

        public Transform GetPointToGroupCamera()
        {
            return pointFarToCamera.transform;
        }

        private void OnButtonsToAction(InputValue value)
        {
            if (_isOn)
            {
                IsOnPress();
            }
            else
            {
                IsOffPress();
            }
            Debug.Log($"is on {_isOn}");
        }

        private void IsOnPress()
        {
            OnLeftShitOn?.Invoke();
            Debug.Log($"Press On");
            _isOn = false;
        }

        private void IsOffPress()
        {
            OnLeftShitOff?.Invoke();
            Debug.Log($"Press Off");
            _isOn = true;
        }
        
        private void OnPowersButton()
        {
            if (_powerOn)
            {
                _powerOn = false;
            }
            else
            {
                _powerOn = true;
            }
        }

        public void OnPunch()
        {
            OnPunchEvent?.Invoke();
        }
        
        public void OnKick()
        {
            OnKickEvent?.Invoke();
        }

        public void OnAim()
        {
            if (IsInPause)
            {
                isAiming = !isAiming;
                return;
            }
            OnAimEvent?.Invoke();
        }
        
        public void AddEnemies(List<GameObject> gameObjectParameter)
        {
            foreach (var gameObjectp in gameObjectParameter)
            {
                _enemiesInCombat.Add(gameObjectp);
            }
        }
        public void RemoveEnemies(List<GameObject> enemies)
        {
            foreach (var enemy in enemies)
            {
                _enemiesInCombat.Remove(enemy);
                if (_enemiesInCombat.Count <= 0)
                {
                    cameraChange.FreeLookCamera();
                }
            }
        }

        public void RemoveEnemy(GameObject gameObjectt)
        {
            if (_enemiesInCombat.Contains(gameObjectt))
            {
                _enemiesInCombat.Remove(gameObjectt);
                if (_enemiesInCombat.Count <= 0)
                {
                    cameraChange.FreeLookCamera();
                }
            }
        }

        public void AddEnemy(GameObject characterEnemy)
        {
            _enemiesInCombat.Add(characterEnemy);
        }

        public bool CanRotate()
        {
            return !isAiming;
        }

        public void ConfigureCameras(CinemachineFreeLook camaraLibre, CinemachineFreeLook camaraGrupal, CinemachineTargetGroup group)
        {
            _secondCamera = camaraLibre;
            _group = group;
            cameraChange.Configure(group, camaraLibre, camaraGrupal,pointToCamera.transform, this);
        }

        public void ChangeInputCustom(bool b)
        {
            _inputCustom.ChangeInputCustom();
            if (b)
            {
                _inputCustom = new MovementControllerTargeting(this, _mainCamera);
            }
            else
            {
                _inputCustom = new MovementController(this, _mainCamera);
            }
        }
        
        private void OnStartButton(InputValue value)
        {
            ServiceLocator.Instance.GetService<IPauseMainMenu>().Pause();
        }
        
        public override void AddEnergy()
        {
            energy += deltaAddingEnergy;
            if (energy >= 1)
            {
                energy = 1;
            }
            OnAddingEnergy?.Invoke(energy);
        }

        public bool CanPlayFatality()
        {
            return Math.Abs(energy - 1) < 0.01f;
        }

        public void ResetEnergy()
        {
            energy = 0;
        }
    }
}