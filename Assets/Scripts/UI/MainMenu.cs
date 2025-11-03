using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        private InputSystem_Actions _playerInput;

        private void Awake()
        {
            _playerInput = new InputSystem_Actions();
            _playerInput.Enable();
            
            _playerInput.Player.PlacePylon.performed += context => StartGame();
        }
        
        void OnEnable()
        {
            _playerInput.Enable();
        }

        void OnDisable()
        {
            _playerInput.Disable();
        }

        private void StartGame()
        {
            SceneManager.LoadScene("ExplorationProtoScene");
        }
    }
}
