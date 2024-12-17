using Fishing.PlayerInput;
using UnityEngine;

namespace Fishing.FishingMechanics {
    public class HookInput : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("Amount of force to be applied per second to the hook when directional inputs are pressed.")] private float _moveForce = 20f;

        private Rigidbody2D _rigidbody;
        private float _direction = 0f;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();

            InputManager.onMoveLeft += MoveLeft;
            InputManager.onMoveRight += MoveRight;
            InputManager.releaseMoveLeft += StopLeft;
            InputManager.releaseMoveRight += StopRight;
        }

        private void Update() {
            if (_direction == 0) {
                return;
            }

            if (transform.position.y > 0f) {
                return;
            }

            _rigidbody.AddForce(new Vector2(_direction * _moveForce * Time.deltaTime, 0));
        }

        private void MoveLeft() {
            _direction = -_moveForce;
        }
        private void MoveRight() {
            _direction = _moveForce;
        }

        private void StopLeft() {
            _direction = 0f;
        }
        private void StopRight() {
            _direction = 0f;
        }
    }
}
