using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UI
{
    public class ProtoScorer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _percentText;
        [SerializeField] private TMP_Text _introText;
        [SerializeField] private TMP_Text _gameOverText;
        [SerializeField] private TMP_Text _winText;
        [SerializeField] private TMP_Text _timeOutText;
        
        [SerializeField] private float startTimeSeconds = 300f;
        
        [SerializeField] private Tilemap targetAreaTilemap;
        private Coroutine _fadeCoroutine;
        
        
        
        public float CurrentTime { get; private set; }
        private bool _isTimerRunning = true;
        
        private int _totalTilesInArea;
        private int _revealedTilesCount;
        

        private void Awake()
        {
            PlayIntroText();
            
        }

        private void Start()
        {
            CurrentTime = startTimeSeconds;
            _isTimerRunning = true;
            
            _totalTilesInArea = 0;
            if (targetAreaTilemap != null)
            {
                foreach (var pos in targetAreaTilemap.cellBounds.allPositionsWithin)
                {
                    if (targetAreaTilemap.HasTile(pos))
                    {
                        _totalTilesInArea++;
                    }
                }
            }
        }
        
        public void CheckAndRegisterRevealedTile(Vector2Int tilePosition)
        {
            if (_revealedTilesCount >= _totalTilesInArea) return;

            //Check if the revealed tile is part of our target area.
            if (targetAreaTilemap != null && targetAreaTilemap.HasTile((Vector3Int)tilePosition))
            {
                _revealedTilesCount++;
                
                _percentText.text = $"{(int)(((float)_revealedTilesCount / (float)_totalTilesInArea) * 100)}%";

                if (_revealedTilesCount >= _totalTilesInArea)
                {
                    _winText.gameObject.SetActive(true);
                }
            }
        }

        private void Update()
        {
            _timerText.text = FormatTime(CurrentTime);
            if (_isTimerRunning)
            {
                CurrentTime -= Time.deltaTime;

                if (CurrentTime <= 0)
                {
                    TimeIsUp();
                }
            }
        }
        
        
        private void PlayIntroText()
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            
            _introText.alpha = 1f;
            _fadeCoroutine = StartCoroutine(FadeIntroText());
            
        }

        public void PlayGameOverText()
        {
            _gameOverText.gameObject.SetActive(true);
        }

        private IEnumerator FadeIntroText() 
        {

            yield return new WaitForSeconds(2);
            
            float timer = 0f;
            while (timer < 1)
            {
                float alpha = Mathf.Lerp(1f, 0f, timer / 1);
                _introText.alpha = alpha;
                
                timer += Time.deltaTime;
                yield return null;
            }
            
            _introText.alpha = 0f;
            _fadeCoroutine = null;
        }
        
        private string FormatTime(float timeInSecs)
        {
            if (timeInSecs < 0)
            {
                timeInSecs = 0;
            }
        
            int minutes = Mathf.FloorToInt (timeInSecs / 60);
            int seconds = Mathf.FloorToInt(timeInSecs % 60);

            return $"{minutes:00}:{seconds:00}";
        }
        
        void TimeIsUp()
        {
            _isTimerRunning = false;
            CurrentTime = 0;
            
            _winText.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        
    }
}
