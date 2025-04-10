using UnityEngine;
using Unity.Cinemachine;

namespace Kuro.GameSystem
{
    public class DynamicCameraControl : MonoBehaviour
    {
        [Header("References")]
        //public CinemachineVirtualCamera virtualCam;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private Transform _player;

        [Header("Zone Settings")]
        [SerializeField] private SpriteRenderer _cameraZoneSpriteLeft;
        [SerializeField] private float _leftZoneOffset = 0.5f;
        [SerializeField] private SpriteRenderer _cameraZoneSpriteRight;
        [SerializeField] private float _rightZoneOffset = 0.5f;
        [SerializeField] private bool _lockCameraOnStart = false;

        private bool _cameraFrozen = false;
        private Vector2 _leftZoneMin, _leftZoneMax;
        private Vector2 _rightZoneMin, _rightZoneMax;
        private float _leftCenterX, _rightCenterX;

        void Start()
        {
            if (_cameraZoneSpriteLeft != null)
            {
                Bounds bounds = _cameraZoneSpriteLeft.bounds;
                _leftZoneMin = bounds.min;
                _leftZoneMax = bounds.max;
                _leftCenterX = bounds.center.x;
            }

            if (_cameraZoneSpriteLeft != null)
            {
                Bounds bounds = _cameraZoneSpriteRight.bounds;
                _rightZoneMin = bounds.min;
                _rightZoneMax = bounds.max;
                _rightCenterX = bounds.center.x;
            }

            _cinemachineCamera.Follow = _player;
        }

        void Update()
        {
            if (_lockCameraOnStart)
            {
                _cinemachineCamera.Follow = null;
                _cameraFrozen = true;
                return;
            }

            if (_cameraZoneSpriteLeft is null || _cameraZoneSpriteRight is null)
                return;

            float playerX = _player.position.x;

            if (!_cameraFrozen && (playerX <= _leftCenterX + _leftZoneOffset) && (playerX < _rightZoneMin.x))
            {
                _cinemachineCamera.Follow = null;
                _cameraFrozen = true;
            }
            else if (_cameraFrozen && (playerX >= _leftCenterX - _leftZoneOffset) && (playerX < _rightZoneMin.x))
            {
                _cinemachineCamera.Follow = _player;
                _cameraFrozen = false;
            }

            if (!_cameraFrozen && (playerX >= _rightCenterX + _rightZoneOffset) && (playerX > _leftZoneMax.x))
            {
                _cinemachineCamera.Follow = null;
                _cameraFrozen = true;
            }
            else if (_cameraFrozen && (playerX <= _rightCenterX - _rightZoneOffset) && (playerX > _leftZoneMax.x))
            {
                _cinemachineCamera.Follow = _player;
                _cameraFrozen = false;
            }
        }
    }
}
