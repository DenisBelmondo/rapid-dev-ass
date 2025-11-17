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
        
        [SerializeField] private float startTimeSeconds = 300f;
        private Coroutine _fadeCoroutine;
        
        
        
        public float CurrentTime { get; private set; }
        private bool _isTimerRunning = true;
        private float _scorePercent;

        private int _tileCount = 0;
        private int _fullMapCount = 0;
        [SerializeField] private Tilemap _gameTilemap;

        private void Awake()
        {
            PlayIntroText();
            foreach (var pos in _gameTilemap.cellBounds.allPositionsWithin)
            {
                if (_gameTilemap.HasTile(pos))
                {
                    _fullMapCount++;
                }
            }
        }

        private void Start()
        {
            _scorePercent = 0;
            CurrentTime = startTimeSeconds;
            _isTimerRunning = true;
            Debug.Log($"{_fullMapCount}");
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
            
            _scorePercent = (int)(((float)_tileCount / (float)_fullMapCount) * 100);
            Debug.Log(_tileCount);
            _percentText.text = $"{_scorePercent}%";
        }

        public void IncrementTiles()
        {
            _tileCount++;
        }
        
        private void PlayIntroText()
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            
            //_introText.text = text;
            _introText.alpha = 1f;
            //_introText.color = color;
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
