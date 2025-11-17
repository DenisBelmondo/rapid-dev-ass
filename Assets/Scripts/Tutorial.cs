using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Tutorial : Game
{
	private int _tutorialCounter;
	private TMP_Text _tutorialText;
	private SpriteRenderer _titleSplashRenderer;

	[SerializeField]
	private Sprite _yaySprite;

	public override void Start()
	{
		base.Start();

		_titleSplashRenderer = transform.Find("TitleSplash").GetComponent<SpriteRenderer>();
		_tutorialText = transform.Find("Canvas/TutorialText").GetComponent<TMP_Text>();
	}

	public override void Update()
	{
		base.Update();

		if (Input.GetKeyDown(KeyCode.Return))
		{
			/*
			var nextGame = Resources.Load<GameObject>("Prefabs/GameScreens/Game");
			StaticGame.Instance.ChangeCurrentGame(nextGame);
			*/
			
			//Debug.Log("LOAD THE NEW SCENE HERE!");
			SceneManager.LoadScene("Camilo Gym");
		}
	}

	public override void OnPylonRegistered(GameObject pylon)
	{
		base.OnPylonRegistered(pylon);

		_tutorialCounter += 1;
	}

	public override void OnTriangleFormed(Vector3 v1, Vector3 v2, Vector3 v3)
	{
		base.OnTriangleFormed(v1, v2, v3);

		_titleSplashRenderer.sprite = _yaySprite;
		_tutorialText.text = "";
	}
}
