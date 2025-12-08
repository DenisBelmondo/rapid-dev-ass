using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boat : MonoBehaviour
{
    private bool isActivated = false;
    public void Activate()
    {
        isActivated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated && collision.CompareTag("Player"))
        {
            StartCoroutine("NextLevel");
            
        }
    }
    
    private IEnumerator NextLevel()
    {
        ScreenTransitionCanvas.Instance.StartFadeOut();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level 02");
        ScreenTransitionCanvas.Instance.StartFadeIn();
    }
}
