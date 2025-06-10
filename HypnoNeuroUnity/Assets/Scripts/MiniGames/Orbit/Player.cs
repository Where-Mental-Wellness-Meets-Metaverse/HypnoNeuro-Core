using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MiniGames.Orbit
{
    public class Player : MonoBehaviour
    {
        public Transform rotationTarget;
        public float rotationSpeed = 100f;
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D playerRigidbody2D;

        public int rotationDirection = 1;

        public Transform PlayerTransform => spriteRenderer.transform;

        void Update()
        {
            rotationTarget.Rotate(0, 0, rotationDirection * rotationSpeed * Time.deltaTime);

            if (!Input.GetMouseButtonDown(0)) return;
            
            rotationDirection *= -1;
            FlipSprite();
        }

        private void FlipSprite()
        {
            spriteRenderer.flipX = rotationDirection != 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.TryGetComponent<Bullet>(out var bullet)) return;
            
            Debug.Log("Game Failed");
            GameController.Instance.GameLost();
        }

        private void OnCollisionEnter2d(Collision other)
        {
            
        }
    }
}