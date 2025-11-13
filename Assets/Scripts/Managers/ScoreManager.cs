using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Timer Settings")] 
        [SerializeField] private float startTimeSeconds = 300f;
        
        [Header("Game Settings")]
        [SerializeField] private string mainMenuSceneName = "MainMenuScene";
        [SerializeField] private float returnToMenuDelay = 7f;
        
        [FormerlySerializedAs("finalScoreUI")]
        [Header("Score UI")]
        [SerializeField] private GameObject finalScoreScreen;
        
        [Header("Fog Cutters")]
        [SerializeField] private GameObject fogCutterParent; //TODO- Do this better lolol.
        [SerializeField] private Tilemap backgroundTilemap; //TODO- Do this better lolol.

        //private float _scorePercent;
        
        public float CurrentTime { get; private set; }

        private bool _isTimerRunning = true;
        
        void Start()
        {
            //_scorePercent = 0;
            CurrentTime = startTimeSeconds;
            _isTimerRunning = true;

            if (finalScoreScreen != null)
            {
                finalScoreScreen.SetActive(false);
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
            
            finalScoreScreen.SetActive(true);
            finalScoreScreen.GetComponentInChildren<ScoreUI>().UpdateScore(GetScore()); //TODO- Jesus christ...
            
            StartCoroutine(ReturnToMenu(returnToMenuDelay));


        }

        float GetScore()
        {
            float bgTileCount = 0;
            
            foreach (var pos in backgroundTilemap.cellBounds.allPositionsWithin)
            {
                // Check if a tile *actually* exists at this position in the grid
                if (backgroundTilemap.HasTile(pos))
                {
                    bgTileCount++;
                }
            }
            //Debug.Log("Tiles there are:" + bgTileCount + "Tiles");
            //Debug.Log("Fog Cutters: " + fogCutterParent.transform.childCount);
            return fogCutterParent.transform.childCount / bgTileCount * 100;
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
