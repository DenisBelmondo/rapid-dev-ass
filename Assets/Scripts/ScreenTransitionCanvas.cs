using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransitionCanvas : MonoBehaviour
{
    public static ScreenTransitionCanvas Instance { get; private set; }

    [SerializeField]
    public Image Image;

    private float _t;

    public void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void Start()
    {
        StartFadeIn();
    }

    public void StartFadeOut()
    {
        _t = 0f;
        StartCoroutine("FadeOut");
    }

    public void StartFadeIn()
    {
        _t = 1f;
        StartCoroutine("FadeIn");
    }

    protected IEnumerator FadeOut()
    {
        while (_t < 1)
        {
            _t += Time.unscaledDeltaTime;

            var c = Image.color;

            c.a = _t;
            Image.color = c;

            yield return new WaitForEndOfFrame();
        }

        _t = 1;
    }

    protected IEnumerator FadeIn()
    {
        while (_t > 0)
        {
            _t -= Time.unscaledDeltaTime;

            var c = Image.color;

            c.a = _t;
            Image.color = c;

            yield return new WaitForEndOfFrame();
        }

        _t = 0;
    }
}
