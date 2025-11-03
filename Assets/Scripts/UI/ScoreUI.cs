using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    public void UpdateScore(float score)
    {
        scoreText.text = $"You uncovered {score:F2}% of the map!";
    }
}
