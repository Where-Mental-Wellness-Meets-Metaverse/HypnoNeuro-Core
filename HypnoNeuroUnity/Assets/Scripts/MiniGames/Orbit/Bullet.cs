using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniGames.Orbit
{
    public class Bullet : MonoBehaviour
    {
        public float maxDistance = 10f;
        public float lifetime = 2f; 

        private Vector2 _spawnPosition;
        private float _distanceTraveled; 
        private float _timeAlive;
        private Vector3 _direction;
        private float _speed;
        public Rigidbody2D rb;

        public void Initialize(float angleDegrees, float speed)
        {
            _direction = AngleToDirection(angleDegrees);
            _speed = speed;
            rb.velocity = _direction * _speed;
        }

        private Vector3 AngleToDirection(float angleDegrees)
        {
            var adjustedAngle = angleDegrees + 90f;
            var angleRadians = adjustedAngle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians), 0);
        }

        private void Start()
        {
            _spawnPosition = transform.position;
        }

        private void Update()
        {
            _distanceTraveled = Vector2.Distance(_spawnPosition, transform.position);

            if (_distanceTraveled >= maxDistance)
            {
                Destroy(gameObject);
                return;
            }

            _timeAlive += Time.deltaTime;

            if (_timeAlive >= lifetime)
            {
                Destroy(gameObject);
            }
        }
        
    }
}