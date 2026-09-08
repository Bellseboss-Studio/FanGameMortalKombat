using Bellseboss.Pery.Scripts.Input;
using UnityEngine;

namespace _Scripts.Player
{
    internal class RotationCharacterV2 : MonoBehaviour
    {
        private GameObject _player;
        private GameObject _camera;
        private bool _isConfigured;
        private IRotationCharacterV2 _rotationCharacterV2;

        [Header("Rotación")] 
        [SerializeField] private float baseRotationSpeed = 8f;
        [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float accelerationTime = 0.3f; // tiempo en segundos para alcanzar velocidad completa

        [Header("Air Control")]
        [Tooltip("Multiplicador de velocidad de rotación durante el salto")] [SerializeField] 
        private float airRotationSpeedMultiplier = 0.7f;
        [Tooltip("Referencia al sistema de movimiento para detectar salto")] [SerializeField]
        private MovementRigidbodyV2 movementSystem;

        [Header("Input Quantization (sync with MovementRigidbodyV2)")]
        [Tooltip("Referencia al MovementRigidbodyV2 para obtener configuración de input")] [SerializeField]
        private MovementRigidbodyV2 movementReference;

        [Header("Rotation Stability")]
        [Tooltip("Ángulo mínimo de diferencia para cambiar la dirección objetivo (grados)")] [SerializeField]
        private float directionChangeThreshold = 5f;
        [Tooltip("Ángulo mínimo para aplicar rotación (grados)")] [SerializeField] 
        private float rotationThreshold = 1f;
        [Tooltip("Velocidad mínima de rotación para mantener estabilidad")] [SerializeField]
        private float minRotationVelocity = 0.1f;
        [Tooltip("Umbral de alineación entre dirección de movimiento y input (0-1, mayor = más estricto) ")] [SerializeField]
        private float alignmentThreshold = 0.7f;
        [Tooltip("Histeresis para evitar toggles cerca del umbral de alineación (0..0.2)")]
        [SerializeField]
        private float alignmentHysteresis = 0.03f;

        [Header("Smoothing")]
        [Tooltip("Factor de suavizado para la dirección objetivo (0=no smoothing, 0.1..0.3 recomendado)")]
        [SerializeField]
        [Range(0f, 0.9f)]
        private float directionSmoothing = 0.12f;

        [Header("Debug")]
        [Tooltip("Mostrar logs de depuración para alineación de rotación")] [SerializeField]
        private bool enableDebugLogs = false;

        private float _rotationVelocity; // valor 0–1 que aumenta cuando hay input
        private Vector2 _vector2;
        private Vector3 _lastDirection;
        private Vector3 _smoothedDesiredDir;
        private bool _canChangeDirection;
        private bool _canRotate;
        private bool _canRotateWhileAttack;
        private bool _isChangingDirection;

        private float _currentRotationSpeed;
        private float _forceRotation;
        
        // Control de sincronización
        private bool _usingSyncDirection = false;
        private Vector3 _syncDirection;
        private float _lastSyncTime;

        public void Configure(GameObject cameraObj, GameObject player, IRotationCharacterV2 rotationCharacterV2,
            float forceRotation)
        {
            _camera = cameraObj;
            _player = player;
            _rotationCharacterV2 = rotationCharacterV2;
            _forceRotation = forceRotation;
            _isConfigured = true;
            _canRotate = true;

            // Design "movementReference null": quantization is single-sourced in
            // MovementRigidbodyV2, so rotation must hold ZERO duplicate thresholds.
            // Auto-resolve the movement system on the same GameObject wherever both
            // components coexist (they do on PlayerV2Update). When it is still null
            // the expected direction degrades to zero and the sync gate accepts
            // movement's direction unconditionally (graceful, no drift).
            if (movementReference == null)
            {
                movementReference = GetComponent<MovementRigidbodyV2>();
            }
        }

        public void Direction(Vector2 vector2) => _vector2 = vector2;
        public void Direction(Vector3 vector3) => _lastDirection = vector3;

        /// <summary>
        /// Recibe la dirección de movimiento ya calculada desde MovementRigidbodyV2 para perfecta sincronización
        /// </summary>
        public void SetMovementDirection(Vector3 worldDirection)
        {
            if (worldDirection.sqrMagnitude > 0.0001f)
            {
                Vector3 normalizedDirection = worldDirection.normalized;
                
                // Calcular la dirección esperada basada en el input actual
                // (single source: movement.QuantizeInput + BuildCanonicalDirection)
                Vector3 expectedDirection = BuildExpectedDirection();
                
                // Solo usar la dirección sincronizada si está alineada con la expectativa del input
                if (expectedDirection != Vector3.zero)
                {
                    float alignment = Vector3.Dot(normalizedDirection, expectedDirection);
                    float acceptThreshold = alignmentThreshold + alignmentHysteresis;
                    float rejectThreshold = alignmentThreshold - alignmentHysteresis;

                    if (alignment > acceptThreshold)
                    {
                        // Aceptar sincronización
                        _lastDirection = normalizedDirection;
                        _syncDirection = _lastDirection;
                        _usingSyncDirection = true;
                        _lastSyncTime = Time.time;

                        if (enableDebugLogs)
                        {
                            Debug.Log($"[RotationCharacterV2] Sync direction accepted: {_lastDirection}, alignment: {alignment:F3}");
                        }
                    }
                    else
                    {
                        // En banda de histéresis: mantener previo; si está claramente desalineado => usar expected
                        if (alignment >= rejectThreshold)
                        {
                            // mantener previo
                        }
                        else
                        {
                            _lastDirection = expectedDirection;
                            _usingSyncDirection = false;
                        }

                        if (enableDebugLogs)
                        {
                            Debug.Log($"[RotationCharacterV2] Sync direction rejected due to misalignment: {alignment:F3}, using expected: {expectedDirection}");
                        }
                    }
                }
                else
                {
                    // Fallback normal
                    _lastDirection = normalizedDirection;
                    _syncDirection = _lastDirection;
                    _usingSyncDirection = true;
                    _lastSyncTime = Time.time;
                }
            }
        }

        /// <summary>
        /// Expected world direction for the current input, built from the SHARED
        /// single sources (spec player-v2-movement-orientation-alignment): input
        /// quantization lives in MovementRigidbodyV2.QuantizeInput and the
        /// yaw-only camera-rotation XZ world direction (camera forward projected
        /// to XZ; position-independent) lives in
        /// InputMovementCustomV2.BuildCanonicalDirection. Rotation holds no
        /// duplicated thresholds or basis. Returns zero (no rotation, no drift)
        /// when the camera, player or movement reference is unavailable.
        /// </summary>
        private Vector3 BuildExpectedDirection()
        {
            if (_camera == null || _player == null || movementReference == null) return Vector3.zero;
            var quantized = movementReference.QuantizeInput(_vector2);
            return InputMovementCustomV2.BuildCanonicalDirection(quantized, _camera, _player);
        }

        private void Update()
        {
            if (!_isConfigured || !_canRotate) return;

            // Si está rotando hacia un target (ataque, lock-on, etc)
            if (_canRotateWhileAttack)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_lastDirection);
                _player.transform.rotation = Quaternion.Slerp(
                    _player.transform.rotation,
                    targetRotation,
                    _forceRotation * Time.deltaTime
                );
                return;
            }

            // Determinar si usar dirección sincronizada o calcular localmente
            bool hasInput = _vector2.sqrMagnitude > 0.01f;
            bool syncRecent = _usingSyncDirection && (Time.time - _lastSyncTime) < 0.1f;
            
            Vector3 desiredMoveDir;
            
            // SIEMPRE calcular la dirección esperada del input
            Vector3 expectedFromInput = BuildExpectedDirection();
            
            if (syncRecent && expectedFromInput != Vector3.zero)
            {
                // Verificar alineación entre dirección sincronizada y esperada
                float alignment = Vector3.Dot(_syncDirection, expectedFromInput);
                
                if (alignment > alignmentThreshold)
                {
                    // Usar dirección sincronizada si está bien alineada
                    desiredMoveDir = _syncDirection;
                    if (enableDebugLogs)
                    {
                        Debug.Log($"[RotationCharacterV2] Using SYNC direction: {desiredMoveDir}, alignment: {alignment:F3}");
                    }
                }
                else
                {
                    // Priorizar dirección esperada si hay desalineación
                    desiredMoveDir = expectedFromInput;
                    if (enableDebugLogs)
                    {
                        Debug.Log($"[RotationCharacterV2] OVERRIDE sync due to misalignment: {alignment:F3}, using input direction: {desiredMoveDir}");
                    }
                }
            }
            else
            {
                // Fallback: usar dirección calculada del input
                desiredMoveDir = expectedFromInput;
                if (enableDebugLogs && hasInput)
                {
                    Debug.Log($"[RotationCharacterV2] Using LOCAL direction: {desiredMoveDir}");
                }
            }
            
            // Aplicar suavizado a la dirección objetivo para reducir jitter en diagonales
            if (_smoothedDesiredDir == Vector3.zero)
                _smoothedDesiredDir = desiredMoveDir;
            else
                _smoothedDesiredDir = Vector3.Slerp(_smoothedDesiredDir, desiredMoveDir, Mathf.Clamp01(directionSmoothing));
            
            bool hasMovement = _smoothedDesiredDir != Vector3.zero;
            
            if (hasMovement)
            {
                // Solo cambiar dirección si es significativamente diferente
                float angleDifference = Vector3.Angle(_lastDirection, _smoothedDesiredDir);
                bool shouldUpdateDirection = _lastDirection == Vector3.zero || angleDifference > directionChangeThreshold;
                
                if (shouldUpdateDirection)
                {
                    _lastDirection = _smoothedDesiredDir;
                    _rotationVelocity = 1f; // Activar rotación inmediata hacia nueva dirección
                    
                    if (enableDebugLogs)
                    {
                        Debug.Log($"[RotationCharacterV2] Direction changed by {angleDifference:F1}°, updating to: {_lastDirection}");
                    }
                }
                else
                {
                    // Dirección estable, mantener rotación actual
                    _rotationVelocity = Mathf.Max(_rotationVelocity - Time.deltaTime / accelerationTime, minRotationVelocity);
                }
            }
             else
             {
                 _rotationVelocity -= Time.deltaTime / accelerationTime;   // Desacelera progresivamente
             }

            _rotationVelocity = Mathf.Clamp01(_rotationVelocity);
            float curveValue = rotationCurve.Evaluate(_rotationVelocity);
            _currentRotationSpeed = baseRotationSpeed * curveValue;

            // Aplicar multiplicador si está saltando
            if (movementSystem != null && movementSystem.IsJump)
            {
                _currentRotationSpeed *= airRotationSpeedMultiplier;
            }

            // Solo rotar si hay una dirección válida Y la velocidad de rotación es significativa
            if (_lastDirection != Vector3.zero && _rotationVelocity > 0.05f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_lastDirection);

                // Solo aplicar rotación si el ángulo diferencia es mayor al umbral mínimo configurable
                float currentAngleDiff = Quaternion.Angle(_player.transform.rotation, targetRotation);

                if (currentAngleDiff > rotationThreshold)
                {
                    _player.transform.rotation = Quaternion.RotateTowards(
                        _player.transform.rotation,
                        targetRotation,
                        _currentRotationSpeed * Time.deltaTime
                    );

                    if (enableDebugLogs)
                    {
                        Debug.Log($"[RotationCharacterV2] Rotating towards {_lastDirection}, angle diff: {currentAngleDiff:F1}°, speed: {_currentRotationSpeed:F1}");
                    }
                }
                else if (enableDebugLogs)
                {
                    Debug.Log($"[RotationCharacterV2] Rotation stable, angle diff: {currentAngleDiff:F1}°");
                }
            }
        }

        public void CanRotate(bool canRotate)
        {
            _vector2 = Vector2.zero;
            _canRotate = canRotate;
        }

        public bool CanRotate()
        {
            return _canRotate;
        }

        public void RotateToDirection(Vector3 direction)
        {
            //invert direction
            direction = -direction;
            direction = new Vector3(direction.x, 0, direction.z);
            _player.transform.rotation = Quaternion.LookRotation(direction);
            _lastDirection = direction;
        }

        public void ChangeDirection(Vector3 rotation)
        {
            _canChangeDirection = true;
            _lastDirection = rotation;
        }

        public void RestoreRotation()
        {
            _canChangeDirection = false;
        }

        public void CanRotateWhileAttack(bool canRotateWhileAttack)
        {
            _canRotateWhileAttack = canRotateWhileAttack;
        }

        public void RotateToLookTheTarget(Vector3 getTarget)
        {
            if (getTarget != Vector3.zero)
            {
                _lastDirection = getTarget - _player.transform.position;
                _lastDirection.y = 0;
            }
        }

        public void RotateInstant(Vector3 direction)
        {
            direction.y = 0;
            if (direction == Vector3.zero) return;

            _player.transform.rotation = Quaternion.LookRotation(direction);
            _lastDirection = direction;
        }
    }
}
