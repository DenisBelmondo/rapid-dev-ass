using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Stats")]
        [SerializeField] private float baseSpeed = 5.0f;
        
        private Rigidbody2D _rb;
        
        //this list contains all the modifiers applied to the player.
        private List<float> _speedModifiers;

        private InputSystem_Actions _playerInput;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            _speedModifiers = new List<float>();
            
            _playerInput = new InputSystem_Actions();
            _playerInput.Enable();
            
        }
        
        void OnEnable()
        {
            _playerInput.Enable();
        }

        void OnDisable()
        {
            _playerInput.Disable();
        }

        void FixedUpdate()
        {
            float currentSpeed = GetCurrentModifiedSpeed();
            
            Vector2 targetVelocity = _playerInput.Player.Move.ReadValue<Vector2>().normalized * currentSpeed;

            _rb.linearVelocity = targetVelocity;
        }

        private float GetCurrentModifiedSpeed()
        {
            float finalSpeed = baseSpeed;

            foreach (float modifier in _speedModifiers)
            {
                finalSpeed *= modifier;
            }
            
            return finalSpeed;
        }

        public void AddSpeedModifier(float modifier)
        {
            _speedModifiers.Add(modifier);
        }

        public void RemoveSpeedModifier(float modifier)
        {
            _speedModifiers.Remove(modifier);
        }
    }
}
