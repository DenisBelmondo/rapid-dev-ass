using System;
using System.Collections;
using Level;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UI
{
    public class ProtoScorer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _percentText;
        [SerializeField] private TMP_Text _targetPercentText;
        [SerializeField] private TMP_Text _introText;
        [SerializeField] private TMP_Text _gameOverText;
        [SerializeField] private TMP_Text _winText;
        
        [Header("Score")]
        [SerializeField] private int _targetScorePercent = 80;

        private Coroutine _fadeCoroutine;
        private int _totalTilesInArea;
        private int _revealedTilesCount;

        private int _targetPercent = 0;
        private Tilemap _targetTilemap;

        private void Awake()
        {
            PlayIntroText();

            _targetTilemap = World.Instance.targetTilemap;
            
            foreach (var pos in _targetTilemap.cellBounds.allPositionsWithin)
            {
                if (_targetTilemap.HasTile(pos))
                {
                    _totalTilesInArea++;
                }
            }
            Debug.Log("Total tiles:  " + _totalTilesInArea);
            _targetPercentText.text = $"Target: {_targetScorePercent}%";
            
            
            World.Instance.fogController.OnTileRevealed += UpdatePercent;
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

        //NOTE- ONLY WORKS IF THE ENTIRE MAP IS THE TARGETTILEMAP.. LOL
        private void UpdatePercent()
        {
            _revealedTilesCount++;
            _targetPercent = (int)(((float)_revealedTilesCount / (float)_totalTilesInArea) * 100);
        }

        void Update()
        {
            _percentText.text = _targetPercent + "%";
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
    }
}
