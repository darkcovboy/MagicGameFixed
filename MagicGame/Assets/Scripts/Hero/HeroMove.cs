using System;
using Data;
using DefaultNamespace;
using DefaultNamespace.CameraLogic;
using DefaultNamespace.Data;
using Infrastructure.Services.PersistentProgress;
using Services.Input;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

namespace Hero
{
    [RequireComponent(typeof(CharacterController))]
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        private const string KeyboardMouse = "KeyboardMouse";
        
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private Transform _cameraRoot;

        [Header("MovementSettings")] 
        [Header("SpeedSettings")]
        [SerializeField,Min(1)] private float _movementSpeed = 2f;
        [SerializeField] private float _sprintSpeed = 6f;
        [SerializeField] private float _speedChangeRate = 10.0f;
        [Range(0.0f, 0.3f)] [SerializeField] private float _rotationSmoothTime = 0.12f;
        [Header("JumpSettings")]
        [SerializeField] private float _groundOffset;

        [SerializeField] private float _jumpTimeOut = 0.50f;
        [SerializeField] private float _fallTimeOut = 0.15f;
        [SerializeField] private float _jumpHeight = 1.2f;
        [SerializeField] private float _gravity = -15.0f;
        [SerializeField] private bool _isGrounded;
        [SerializeField] private float _groundedRadius;
        [SerializeField] private LayerMask _groundedLayers;
        [SerializeField] private float _thresshold = 0.01f;

        [Header("Cinemachine")] 
        [SerializeField] private float _topClamp = 70.0f;
        [SerializeField] private float _bottomClamp = -30.0f;
        [SerializeField] private bool _lockCameraPosition = false;
        [SerializeField] private float _cameraAngleOverride = 0.0f;
        
        private PlayerInput _playerInput;
        private IInputSevice _inputService;
        private StarterAssetsInputs _input;

        private float _speed;
        private float _targetRotation = 0.0f;
        private float _verticalVelocity;
        private float _rotationVelocity;
        private float _terminalVelocity = 53.0f;
        private float _animtionBlend;
        
        private Vector2 _currentMouseDelta;
        private Vector2 _currentMouseDeltaVelocity;
        private bool _isCurrentDeviceMouse;

        private GameObject _mainCamera;
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        private CameraFollow _cinemachineCamera;
        private float _fallTimeoutDelta;
        private float _jumpTimeOutDelta;


        [Inject]
        public void Constructor(IInputSevice inputSevice)
        {
            _inputService = inputSevice;
        }

        private void Start()
        {
            _cinemachineTargetYaw = _cameraRoot.transform.rotation.eulerAngles.y;
            _input = GetComponent<StarterAssetsInputs>();
            _playerInput = GetComponent<PlayerInput>();
            _mainCamera = Camera.main.gameObject;
            _isCurrentDeviceMouse = _playerInput.currentControlScheme == KeyboardMouse;

            _jumpTimeOutDelta = _jumpTimeOut;
            _fallTimeoutDelta = _fallTimeOut;
        }

        private void OnValidate()
        {
            if (_heroAnimator == null)
                _heroAnimator = GetComponent<HeroAnimator>();
            
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
        }

        #region Movement

        private void Update()
                {
                    JumpAndGravity();
                    GroundCheck();
                    Move();
                }
        
                private void LateUpdate()
                {
                    CameraRotation();
                }
        
                public void BindCamera(CameraFollow camera)
                {
                    _cinemachineCamera = camera;
                    _cinemachineCamera.Follow(_cameraRoot.gameObject);
                }
                
                private void JumpAndGravity()
                {
                    if (_isGrounded)
                    {
                        _fallTimeoutDelta = _fallTimeOut;
                        
                        _heroAnimator.SetJump(false);
                        _heroAnimator.SetFreeFall(false);
        
                        if (_verticalVelocity < 0.0f)
                            _verticalVelocity = -2f;
        
                        if (_input.jump && _jumpTimeOutDelta <= 0.0f)
                        {
                            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
                            
                            _heroAnimator.SetJump(true);
                        }
        
                        if (_jumpTimeOutDelta >= 0.0f)
                        {
                            _jumpTimeOutDelta -= Time.deltaTime;
                        }
                    }
                    else
                    {
                        _jumpTimeOutDelta = _jumpTimeOut;
        
                        if (_fallTimeoutDelta >= 0.0f)
                        {
                            _fallTimeoutDelta -= Time.deltaTime;
                        }
                        else
                        {
                            _heroAnimator.SetFreeFall(true);
                        }
        
                        _input.jump = false;
                    }
        
                    if (_verticalVelocity < _terminalVelocity)
                        _verticalVelocity += _gravity * Time.deltaTime;
                }
        
                private void GroundCheck()
                {
                    Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundOffset,
                        transform.position.z);
        
                    _isGrounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundedLayers,
                        QueryTriggerInteraction.Ignore);
                    
                    _heroAnimator.SetGrounded(_isGrounded);
                }
        
                private void Move()
                {
                    float targetSpeed = _input.sprint ? _sprintSpeed : _movementSpeed;
        
                    if (_input.move == Vector2.zero)
                        targetSpeed = 0.0f;
                    
                    float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;
                    
                    float speedOffset = 0.1f;
                    float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
        
                    if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                        currentHorizontalSpeed > targetSpeed + speedOffset)
                    {
                        _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                            Time.deltaTime * _speedChangeRate);
        
                        _speed = Mathf.Round(_speed * 1000f) / 1000f;
                    }
                    else
                    {
                        _speed = targetSpeed;
                    }
                    
                    _animtionBlend = Mathf.Lerp(_animtionBlend, targetSpeed, Time.deltaTime * _speedChangeRate);
                    if (_animtionBlend < 0.01f) _animtionBlend = 0f;
                    
                    Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        
                    if (_input.move != Vector2.zero)
                    {
                        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                          _mainCamera.transform.eulerAngles.y;
        
                        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                            _rotationSmoothTime);
        
                        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    }
        
                    Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        
                    _characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                              new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
                    
                    _heroAnimator.SetSpeed(_animtionBlend);
        
                }
        
                private void CameraRotation()
                {
                    if (_input.look.sqrMagnitude >= _thresshold && !_lockCameraPosition)
                    {
                        float deltaTimeMultiplier = _isCurrentDeviceMouse ? 1.0f : Time.deltaTime;
        
                        _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                        _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
                    }
        
                    _cinemachineTargetYaw = Constraints.ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                    _cinemachineTargetPitch = Constraints.ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
                    _cameraRoot.gameObject.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride, _cinemachineTargetYaw, 0.0f);
                }

        #endregion
        

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
            {
                Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
                if(savedPosition != null)
                    Warp(savedPosition);
            }
        }

        private void Warp(Vector3Data savedPosition)
        {
            _characterController.enabled = false;
            transform.position = savedPosition.AsUnityVector().AddY(_characterController.height);
            _characterController.enabled = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(),transform.position.AsVectorData());
        }

        private static string CurrentLevel()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}