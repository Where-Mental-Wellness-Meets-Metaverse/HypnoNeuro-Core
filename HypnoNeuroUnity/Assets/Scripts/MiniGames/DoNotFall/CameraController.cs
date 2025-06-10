using UnityEngine;

namespace MiniGames.DoNotFall
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        public Vector3 offset = new Vector3(0, 10, -10); // Camera offset from the player
        public float smoothSpeed = 0.125f; // Smoothing speed for camera movement

        private GameController _gameController;
        
        private void Start()
        {
            _gameController = GameController.Instance;
            if (_gameController == null)
            {
                Debug.LogError("ControllerDoNotFall not found in the scene!");
            }
        }

        private void Update()
        {
            if (_gameController.Player == null)
                return;

            var targetPosition = _gameController.Player.transform.position + offset;
            var smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(_gameController.Player.transform);
        }
    }
}