using System.Collections;
using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DebugTexter : Singleton<DebugTexter>
    {
        private TMP_Text _bigText;
        private Coroutine _fadeCoroutine;

        protected override void Awake()
        {
            base.Awake();
            _bigText = GetComponent<TMP_Text>();
            _bigText.alpha = 0f;
        }

        public void UpdateText(string text, Color color)
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            
            _bigText.text = text;
            _bigText.alpha = 1f;
            _bigText.color = color;
            _fadeCoroutine = StartCoroutine(FadeText());
            
        }

        private IEnumerator FadeText() 
        {

            yield return new WaitForSeconds(2);
            
            float timer = 0f;
            while (timer < 1)
            {
                float alpha = Mathf.Lerp(1f, 0f, timer / 1);
                _bigText.alpha = alpha;
                
                timer += Time.deltaTime;
                yield return null;
            }
            
            _bigText.alpha = 0f;
            _fadeCoroutine = null;
        }
    }
}
