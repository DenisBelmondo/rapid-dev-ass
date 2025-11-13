using UnityEngine;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private InputSystem_Actions _playerInput;

        void Awake()
        {
            _playerInput = new InputSystem_Actions();
        }

        void OnEnable()
        {
            _playerInput.Enable();
        }

        void OnDisable()
        {
            _playerInput.Disable();
        }

        void Update()
        {
            
            var crewManager = CrewManager.Instance;
            
            if (crewManager == null || crewManager.Leader == null)
            {
                Debug.Log("Crew Manager is either null or empty!!");
                return;
            }
            
            Vector2 moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
            if (crewManager.LeaderMovement != null)
            {
                crewManager.LeaderMovement.Move(moveInput);
            }
            
        }
    }
}
