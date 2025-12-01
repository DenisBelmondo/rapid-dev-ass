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
        [SerializeField] private TMP_Text _pylonText;
        
        [Header("Score")]
        [SerializeField] private int _targetScorePercent = 80;
        
        [Header("Sound")]
        [SerializeField] private AudioClip _winSound;
        [SerializeField] private AudioClip _loseSound;
        private AudioSource _audioSource;

        private Coroutine _fadeCoroutine;
        private int _totalTilesInArea;
        private int _revealedTilesCount;

        private int _targetPercent = 0;
        private Tilemap _targetTilemap;

        private int _pylonCount;
        private bool _isWon = false;

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
            World.Instance.pylonManager.onPylonRegistered.AddListener(UpdatePylons);
            World.Instance.pylonManager.onPylonsCleared.AddListener(ClearPylons);
            World.Instance.crewManager.onGameOver.AddListener(PlayGameOverText);
            
            
            _audioSource = GetComponent<AudioSource>();
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
            if (_targetPercent >= _targetScorePercent)
            {
                if (_isWon) return;
                PlayWinText();
                _isWon = true;
            }
        }

        private void UpdatePylons(GameObject pylon)
        {
            _pylonCount++;
        }

        private void ClearPylons()
        {
            _pylonCount = 0;
        }

        void Update()
        {
            _percentText.text = _targetPercent + "%";
            _pylonText.text = $"{_pylonCount}/3";
        }

        private void PlayGameOverText()
        {
            _gameOverText.gameObject.SetActive(true);
            _audioSource.PlayOneShot(_loseSound);
        }

        private void PlayWinText()
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            
            _winText.gameObject.SetActive(true);
            _winText.alpha = 1f;
            _audioSource.PlayOneShot(_winSound);
            _fadeCoroutine = StartCoroutine(FadeWinText());
            
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
        
        private IEnumerator FadeWinText() 
        {

            yield return new WaitForSeconds(3);
            
            float timer = 0f;
            while (timer < 1)
            {
                float alpha = Mathf.Lerp(1f, 0f, timer / 1);
                _winText.alpha = alpha;
                
                timer += Time.deltaTime;
                yield return null;
            }
            
            _winText.alpha = 0f;
            _fadeCoroutine = null;
        }
        
    }
}
