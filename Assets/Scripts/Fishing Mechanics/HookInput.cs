using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.IO;

namespace Fishing.FishingMechanics
{
    public class HookInput : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 20f;

        private Rigidbody2D rb;
        private float direction = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            InputManager.onMoveLeft += MoveLeft;
            InputManager.onMoveRight += MoveRight;
            InputManager.releaseMoveLeft += StopLeft;
            InputManager.releaseMoveRight += StopRight;
        }

        private void Update()
        {
            Debug.Log(direction);
            if (direction == 0) return;
            if (transform.position.y > 0f) return;
            rb.AddForce(new Vector2(direction * moveSpeed * Time.deltaTime, 0));
        }

        private void MoveLeft()
        {
            direction = -moveSpeed;
        }

        private void MoveRight()
        {
            direction = moveSpeed;
        }

        private void StopLeft()
        {
            direction = 0f;
        }

        private void StopRight()
        {
            direction = 0f;
        }
    }
}
