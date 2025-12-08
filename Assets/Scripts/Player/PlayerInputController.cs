using System;
using Level;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private InputSystem_Actions _playerInput;
        private World _world;

        [Header("Pylon Removal Settings")] 
        [SerializeField] private float holdDuration = 0.5f;

        private float _holdTimer = 0;
        private bool _isHoldingForRemoval = false;
        private Pylon _pylonToPotentiallyRemove;

        void Awake()
        {
            _playerInput = new InputSystem_Actions();
            _world = World.Instance;

            _playerInput.Player.PlacePylon.started += OnPylonActionStarted;
            _playerInput.Player.PlacePylon.canceled += OnPylonActionCanceled;
        }

        void OnEnable()
        {
            _playerInput.Enable();
        }

        void OnDisable()
        {
            _playerInput.Disable();
        }

        private void OnPylonActionStarted(InputAction.CallbackContext context)
        {
            GameObject markedPylonObject = _world.pylonManager.pylonToRemove;

            if (markedPylonObject != null)
            {
                if (_world.pylonManager.activePylons.Count == 3 &&
                    _world.pylonManager.activePylons[0] == markedPylonObject)
                {
                    Debug.Log("Can't remove the first pylon while all three are active!");
                }
                
                _isHoldingForRemoval = true;
                _holdTimer = 0f;
                _pylonToPotentiallyRemove = markedPylonObject.GetComponent<Pylon>();
            }
            else
            {
                _world.pylonManager.RegisterPylon(_world.crewManager.Leader.transform);
            }
        }
        
        private void OnPylonActionCanceled(InputAction.CallbackContext context)
        {
            if (_isHoldingForRemoval)
            {
                Debug.Log("Hold Canceled");
                _isHoldingForRemoval = false;
                _holdTimer = 0f;
                if (_pylonToPotentiallyRemove != null)
                {
                    _pylonToPotentiallyRemove.UpdateHoldProgress(0);
                }
                _pylonToPotentiallyRemove = null;
            }
        }

        private void Update()
        {
            
            //fuck you fuck you fuck you
            if (Input.GetKeyDown(KeyCode.Alpha1)) { _world.crewManager.UseItem(0); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { _world.crewManager.UseItem(1); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { _world.crewManager.UseItem(2); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { _world.crewManager.UseItem(3); }
            
            
            Vector2 moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
            if (_world.crewManager.LeaderMovement != null)
            {
                _world.crewManager.LeaderMovement.Move(moveInput);
            }

            if (_isHoldingForRemoval)
            {
                _holdTimer += Time.deltaTime;
                float progress = _holdTimer / holdDuration;

                if (_pylonToPotentiallyRemove != null)
                {
                    _pylonToPotentiallyRemove.UpdateHoldProgress(progress);
                }
                
                //check if it's done 
                if (_holdTimer >= holdDuration)
                {
                    Debug.Log("Hold complete! removing pylon");
                    _world.pylonManager.RemovePylon(_pylonToPotentiallyRemove.gameObject);

                    _isHoldingForRemoval = false;
                    _holdTimer = 0f;
                    _pylonToPotentiallyRemove = null;
                }
            } else if (_world.pylonManager.pylonToRemove == null && _pylonToPotentiallyRemove != null)
            {
                _pylonToPotentiallyRemove.CancelHold();
                _pylonToPotentiallyRemove = null;
                _isHoldingForRemoval = false;
            }
        }
    }
}
