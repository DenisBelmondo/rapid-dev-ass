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
		
		pylonSystem.OnPylonRegistered.AddListener(OnPylonRegistered);
		pylonSystem.OnTriangleFormed.AddListener(OnTriangleFormed);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			SceneManager.LoadScene("Camilo Gym");
		}
	}

	public void OnPylonRegistered(GameObject pylon)
	{
		_tutorialCounter += 1;
	}

	public void OnTriangleFormed(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		titleSplashRenderer.sprite = successSprite;
		tutorialText.text = "";
	}
}
