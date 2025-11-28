using Level;
using Managers;
using UnityEngine;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private InputSystem_Actions _playerInput;
        private World _world;

        void Awake()
        {
            _playerInput = new InputSystem_Actions();
            _playerInput.Player.PlacePylon.performed += context => PlacePylon();
            _world = World.Instance;
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
            Vector2 moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
            if (_world.crewManager.LeaderMovement != null)
            {
                _world.crewManager.LeaderMovement.Move(moveInput);
            }
        }

        void PlacePylon()
        {
            _world.pylonManager.RegisterPylon(_world.crewManager.Leader.transform);
        }
    }
}
