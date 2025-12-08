using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
	private int _tutorialCounter;
	[SerializeField]private TMP_Text tutorialText;
	[SerializeField]private SpriteRenderer titleSplashRenderer;
	[SerializeField] private Sprite successSprite;
	[SerializeField] private PylonManager pylonSystem;

	public void Start()
	{
		if(pylonSystem == null) Debug.LogError("TUTORIAL: Missing pylon system");
		if(titleSplashRenderer == null) Debug.LogError("TUTORIAL: Missing title splash renderer");
		if(tutorialText == null) Debug.LogError("TUTORIAL: Missing tutorial text");
		if(successSprite == null) Debug.LogError("TUTORIAL: Missing success sprite");

		pylonSystem.onPylonRegistered.AddListener(OnPylonRegistered);
		pylonSystem.onTriangleFormed.AddListener(OnTriangleFormed);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			StartCoroutine("Exit");
		}
	}

	private void OnPylonRegistered(GameObject pylon)
	{
		_tutorialCounter += 1;
	}

	private void OnTriangleFormed(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		titleSplashRenderer.sprite = successSprite;
		tutorialText.text = "";
		if (_tutorialCounter > 5)
		{
			tutorialText.text = "";
		}
		else
		{
			tutorialText.text = "Hold Space on a placed pylon to remove it";
		}
	}

	private IEnumerator Exit()
	{
		ScreenTransitionCanvas.Instance.StartFadeOut();
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("Camilo Gym");
		ScreenTransitionCanvas.Instance.StartFadeIn();
	}
}
