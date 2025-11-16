using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private InputSystem_Actions _playerInput;
        private CrewManager _crewManager;

        void Awake()
        {
            _playerInput = new InputSystem_Actions();
            _playerInput.Player.PlacePylon.performed += context => PlacePylon();
            _crewManager = CrewManager.Instance;
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
            if (_crewManager == null || _crewManager.Leader == null)
            {
                Debug.Log("Crew Manager is either null or empty!!");
                return;
            }
            
            Vector2 moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
            if (_crewManager.LeaderMovement != null)
            {
                _crewManager.LeaderMovement.Move(moveInput);
            }
        }

        void PlacePylon()
        {
            PylonManager.Instance.RegisterPylon(CrewManager.Instance.Leader.transform);
        }
    }
}
