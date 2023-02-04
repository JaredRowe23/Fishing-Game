using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fishing.IO;

namespace Fishing.FishingMechanics
{
    public class HookInput : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 500f;

        private Rigidbody2D rb;
        private float direction = 0f;
        private bool isMoving;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            Controls _controls = new Controls();
            _controls.FishingLevelInputs.Enable();
            _controls.FishingLevelInputs.MoveHook.performed += MoveHook;
            _controls.FishingLevelInputs.MoveHook.canceled += StopMovingHook;
        }

        private void Update()
        {
            if (!isMoving) return;
            if (transform.position.y > 0f) return;
            rb.AddForce(new Vector2(direction * moveSpeed * Time.deltaTime, 0));
        }

        public void MoveHook(InputAction.CallbackContext _context)
        {
            if (!_context.performed) return;
            direction = _context.ReadValue<float>();
            isMoving = true;
        }

        public void StopMovingHook(InputAction.CallbackContext _context)
        {
            if (!_context.canceled) return;
            direction = 0f;
            isMoving = false;
        }
    }
}
