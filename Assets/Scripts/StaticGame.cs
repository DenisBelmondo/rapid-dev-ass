using UnityEngine;

/// <summary>
/// The singleton that lives from the time the game is booted up to the time
/// the game is shut down. The things that go here should be only things that
/// should be kept track of for the entire duration of the _application_, not
/// the playsim. In that case, please refer to `class Game`.
/// </summary>
public sealed class StaticGame : MonoBehaviour
{
    public static StaticGame Instance { get; private set; }

    [SerializeField]
    public GameObject InitialGameScreen;

    public GameObject CurrentGameRoot { get; private set; }

    private bool _isBeingDeleted;

    public void Awake()
    {
        // begin singleton stuff

        if (Instance is not null)
        {
            _isBeingDeleted = true;
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;

        // end singleton stuff. put other code here vvv
    }

    public void Start()
    {
        // begin singleton stuff

        if (_isBeingDeleted)
        {
            return;
        }

        ChangeCurrentGame(InitialGameScreen);

        // end singleton stuff. put other code here vvv
    }

    /// <summary>
    /// Change the current "game" or "scene".
    /// </summary>
    /// <param name="go">
    /// A reference (ideally to a prefab) to a GameObject to be placed under
    /// my root transform.
    /// </param>
    public void ChangeCurrentGame(GameObject go)
    {
        GameObject newGameRoot = go;

        if (go.scene.name == null || go.scene.name == go.name)
        {
            newGameRoot = Instantiate(go);
        }

        Destroy(CurrentGameRoot);
        CurrentGameRoot = newGameRoot;
        CurrentGameRoot.transform.SetParent(transform);
    }
}
