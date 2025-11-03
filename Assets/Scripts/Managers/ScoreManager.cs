using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Timer Settings")] 
        [SerializeField] private float startTimeSeconds = 300f;
        
        [Header("Game Settings")]
        [SerializeField] private string mainMenuSceneName = "MainMenuScene";
        [SerializeField] private float returnToMenuDelay = 7f;
        
        [Header("Score UI")]
        [SerializeField] private GameObject finalScoreUI;
        
        public float CurrentTime { get; private set; }

        private bool _isTimerRunning = true;
        
        void Start()
        {
            CurrentTime = startTimeSeconds;
            _isTimerRunning = true;

            if (finalScoreUI != null)
            {
                finalScoreUI.SetActive(false);
            }
            else
            {
                Debug.Log("Final Score UI is not assigned in the scoreManager!!");
            }

            if (string.IsNullOrEmpty(mainMenuSceneName))
            {
                Debug.Log("Main Menu Scene is not assigned in the scoreManager!!");
            }
        }
        
        void Update()
        {
            if (_isTimerRunning)
            {
                CurrentTime -= Time.deltaTime;

                if (CurrentTime <= 0)
                {
                    TimeIsUp();
                }
            }
        }

        void TimeIsUp()
        {
            _isTimerRunning = false;
            CurrentTime = 0;
            
            Debug.Log("Time's up!");
            
            finalScoreUI.SetActive(true);
            StartCoroutine(ReturnToMenu(returnToMenuDelay));


        }

        IEnumerator ReturnToMenu(float secs)
        {
            float timer = 0f;

            while (timer < secs)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
