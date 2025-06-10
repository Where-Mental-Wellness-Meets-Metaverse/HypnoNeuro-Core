using System;
using UnityEngine;

namespace MiniGames.ZenSlider
{
    public class Player : MonoBehaviour
    {
        public float moveSpeed = 5f;

        private Rigidbody _rb;
        private Vector3 _moveDirection = Vector3.zero;
        private bool _isMoving = false;

        private Vector2 _startTouchPos;
        private bool _touchStarted;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleInput();
            MovePlayer();
        }

        private void HandleInput()
        {
            if (_isMoving) return;

            if (Input.GetMouseButtonDown(0))
            {
                _startTouchPos = Input.mousePosition;
                _touchStarted = true;
            }

            if (Input.GetMouseButtonUp(0) && _touchStarted)
            {
                _touchStarted = false;
                Vector2 endTouchPos = Input.mousePosition;
                var swipeDirection = endTouchPos - _startTouchPos;

                if (!(swipeDirection.magnitude > 50f)) return;
                
                if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
                {
                    _moveDirection = swipeDirection.x > 0 ? Vector3.right : Vector3.left;
                }
                else
                {
                    _moveDirection = swipeDirection.y > 0 ? Vector3.forward : Vector3.back;
                }

                _isMoving = true;
                _rb.isKinematic = false;
            }
        }
        private void MovePlayer()
        {
            if (!_isMoving) return;

            _rb.velocity = _moveDirection * moveSpeed;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                var collisionNormal = collision.contacts[0].normal;

                if (!(Vector3.Dot(_moveDirection, collisionNormal) < -0.5f)) return;

                _isMoving = false;
                _moveDirection = Vector3.zero;
                _rb.velocity = Vector3.zero;
                _rb.isKinematic = true;

                transform.position = new Vector3(
                    Mathf.Round(transform.position.x),
                    Mathf.Round(transform.position.y),
                    Mathf.Round(transform.position.z)
                );
            }
            else if (collision.gameObject.CompareTag("Finish"))
            {
                var collisionNormal = collision.contacts[0].normal;

                if (!(Vector3.Dot(_moveDirection, collisionNormal) < -0.5f)) return;
                
                _isMoving = false;
                _moveDirection = Vector3.zero;
                _rb.velocity = Vector3.zero;
                _rb.isKinematic = true;

                transform.position = new Vector3(
                    Mathf.Round(transform.position.x),
                    Mathf.Round(transform.position.y),
                    Mathf.Round(transform.position.z)
                );

                GameController.Instance.LevelCompleted();

            }
        }



    }
}