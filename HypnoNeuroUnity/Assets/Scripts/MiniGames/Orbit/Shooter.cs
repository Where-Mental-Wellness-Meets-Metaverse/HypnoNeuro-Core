using UnityEngine;

namespace MiniGames.Orbit
{
    public class Shooter : MonoBehaviour
    {
        public Bullet bulletPrefab;
        public float bulletSpeed = 10f;
        public float spawnDelay = 1f;
        public float rotationSpeed = 5f;
        public Transform bulletStartPoint;

        private Transform _playerTransform;
        private Vector3 _predictedPosition;
        private Quaternion _targetRotation;
        private float _timeSinceLastShot;

        private void Start()
        {
            if (GameController.Instance != null && GameController.Instance.player != null)
            {
                _playerTransform = GameController.Instance.player.transform;
            }
            else
            {
                Debug.LogError("Player reference not found in GameController!");
            }

            SetNewPredictedTarget();
        }

        private void Update()
        {
            if (_playerTransform)
            {
                RotateTowardsPredictedPosition();
            }

            _timeSinceLastShot += Time.deltaTime;
            if (_timeSinceLastShot >= spawnDelay)
            {
                SpawnBullet();
                _timeSinceLastShot = 0f;
                SetNewPredictedTarget();
            }
        }

        private void SetNewPredictedTarget()
        {
            if (!GameController.Instance.player)
                return;

            var randomOffset = Random.Range(0, 100f) * GameController.Instance.player.rotationDirection;
            _targetRotation = Quaternion.Euler(0, 0, (GameController.Instance.player.transform.eulerAngles.z + randomOffset));
        }

        private void RotateTowardsPredictedPosition()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void SpawnBullet()
        {
            var bullet = Instantiate(bulletPrefab, bulletStartPoint.position, bulletStartPoint.rotation);
            bullet.Initialize(transform.eulerAngles.z, bulletSpeed);
        }
    }
}
