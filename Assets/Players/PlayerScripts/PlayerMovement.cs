using System;
using System.Collections;
using UnityEngine;
using Zlipacket.CoreZlipacket.Player.Input;

namespace Players.PlayerScripts
{
    public class PlayerMovement : PlayerScript
    {
        private Vector2 movementInput;
        private bool isSprinting = false;
        
        [Header("Movement")]
        public float walkSpeed = 5f;
        public float runSpeed = 10f;
        public float jumpForce = 10f;
        public float jumpCooldown = 0.2f;
        private bool jumpAble = true;
        
        [Space]
        [Header("Stamina")]
        public float staminaConsumeRate = 0.4f;
        public float staminaRegenRate = 0.4f;
        public float staminaCooldown = 2f;

        private Coroutine co_StaminaRegen = null;
        private bool isStaminaRegening => co_StaminaRegen != null;
        
        [Space]
        [Header("Grounded Check")]
        public float playerHeight;
        public Vector3 groundBoxSize;
        public LayerMask groundLayer;
        
        private void OnEnable()
        {
            inputReader.movementEvent += Move;
            inputReader.sprintEvent += Sprint;
            inputReader.jumpEvent += Jump;
        }

        private void OnDisable()
        {
            inputReader.movementEvent -= Move;
            inputReader.sprintEvent -= Sprint;
            inputReader.jumpEvent -= Jump;
        }

        private void Update()
        {
            UpdateRotation();
            UpdateMovement();
        }

        private void Move(Vector2 movement) => movementInput = movement;

        private void UpdateMovement()
        {
            if (movementInput == Vector2.zero && rb.linearVelocity.magnitude <= 0f)
                return;
            
            Vector3 directionMovement = transform.forward * movementInput.y + transform.right * movementInput.x;
            directionMovement.Normalize();
            
            float speed = 1 + Time.deltaTime;
            
            if (isSprinting && stamina > 0)
            {
                Debug.Log($"Stamina: {stamina}");
                stamina -= staminaConsumeRate;
                speed *= runSpeed;
            }
            else
                speed *= walkSpeed;
            
            var newVelocity = new  Vector3(
                directionMovement.x * speed
                , rb.linearVelocity.y
                , directionMovement.z * speed);
            
            rb.linearVelocity = newVelocity;
        }

        private void UpdateRotation()
        {
            Vector3 cameraForward = cam.transform.forward;
            cameraForward.y = 0f; // Ignore the y-axis rotation

            if (cameraForward != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(cameraForward);
                rb.MoveRotation(newRotation);
            }
        }
        
        private void Sprint(bool isHolding)
        {
            isSprinting = isHolding;

            if (!isHolding)
            {
                StartRegenStamina();
            }
            else if (isHolding && isStaminaRegening)
            {
                StopRegenStamina();
            }
        }
        
        private Coroutine StartRegenStamina()
        {
            if (isStaminaRegening)
                StopCoroutine(co_StaminaRegen);

            co_StaminaRegen = StartCoroutine(StaminaRegenerating());
            return co_StaminaRegen;
        }

        private Coroutine StopRegenStamina()
        {
            if (!isStaminaRegening)
                return null;
            
            StopCoroutine(co_StaminaRegen);
            
            co_StaminaRegen = null;
            return co_StaminaRegen;
        }
        
        private IEnumerator StaminaRegenerating()
        {
            yield return new WaitForSeconds(staminaCooldown);
            
            while (stamina < maxStamina)
            {
                Debug.Log("Stamina: " + stamina);
                stamina += staminaRegenRate;
                yield return null;
            }
            
            co_StaminaRegen = null;
        }
        
        private void Jump()
        {
            if (!jumpAble)
                return;
            
            if (Physics.BoxCastAll(transform.position, groundBoxSize, -transform.up, Quaternion.identity,
                    playerHeight * 0.5f, groundLayer).Length == 0)
                return;
            
            jumpAble = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        private void ResetJump()
        {
            jumpAble = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, groundBoxSize * 2);
        }
    }
}