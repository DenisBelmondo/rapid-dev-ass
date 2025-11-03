using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TimerUI : MonoBehaviour
{ 
    [SerializeField] private ScoreManager scoreManager;
    
    [SerializeField] private TMP_Text timerText;
    

    private void Update()
    {
        timerText.text = FormatTime(scoreManager.CurrentTime);
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
