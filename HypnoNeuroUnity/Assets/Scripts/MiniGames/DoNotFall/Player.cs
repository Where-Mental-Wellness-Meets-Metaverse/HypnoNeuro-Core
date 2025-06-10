using UnityEngine;

namespace MiniGames.DoNotFall
{
    public class Player : MonoBehaviour
    {
        private GameController _gameController;
        private CharacterController _characterController;
        private Vector2 _swipeStartPosition;
        private Vector3 _movement;

        public float moveSpeed = 5f; // Speed of the character movement
        public float rotationSpeed = 10f; // Speed of character rotation
        public float touchSensitivity = 10f; // Minimum distance for touch input to register

        
        private void Start()
        {
            _gameController = GameController.Instance;
            if (_gameController == null)
            {
                Debug.LogError("ControllerDoNotFall not found in the scene!");
            }

            // Get the CharacterController component
            _characterController = GetComponent<CharacterController>();
            if (_characterController == null)
            {
                Debug.LogError("CharacterController component not found on the player!");
            }
        }

        private void Update()
        {
            HandleTouchInput();
        }


        private void FixedUpdate()
        {
            MoveCharacter();
            RotateCharacter();
        }

        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.TryGetComponent<GridElement>(out var tile))
            {
                tile.SetPlayerOnTile();
            }
        }
        
        private void HandleTouchInput()
        {
            if (Input.touchCount > 0) // Check if there is at least one touch
            {
                Touch touch = Input.GetTouch(0); // Get the first touch

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _swipeStartPosition = touch.position;
                        break;

                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        Vector2 touchDelta = touch.position - _swipeStartPosition;

                        if (touchDelta.magnitude > touchSensitivity)
                        {
                            _movement = new Vector3(touchDelta.x, 0, touchDelta.y).normalized;
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        _movement = Vector3.zero;
                        break;
                }
            }
        }
        
        private void MoveCharacter()
        {
            Vector3 moveDirection = new Vector3(_movement.x, 0, _movement.z);

            if (!_characterController.isGrounded)
            {
                moveDirection.y += -50f * Time.fixedDeltaTime;
            }

            Vector3 targetPosition = transform.position + moveDirection * (moveSpeed * Time.fixedDeltaTime);

            if (IsPositionWithinGrid(targetPosition))
            {
                _characterController.Move(moveDirection * (moveSpeed * Time.fixedDeltaTime));
            }
        }

        private void RotateCharacter()
        {
            if (_movement != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }

        private bool IsPositionWithinGrid(Vector3 position)
        {
            var gridSize = _gameController.GridSize;
            var spacing = _gameController.spacing;

            float minX = 0;
            var maxX = (gridSize.x - 1) * spacing;
            float minZ = 0;
            var maxZ = (gridSize.y - 1) * spacing;

            if (position.x >= minX && position.x <= maxX &&
                position.z >= minZ && position.z <= maxZ)
            {
                return true;
            }

            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.z = Mathf.Clamp(position.z, minZ, maxZ);
            transform.position = new Vector3(position.x, transform.position.y, position.z);

            return false;
        }
        
    }
}