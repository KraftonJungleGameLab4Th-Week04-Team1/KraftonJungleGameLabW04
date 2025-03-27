using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutPanel : MonoBehaviour
{
    public Image fadeImage;
    public TMP_Text text;
    public float fadeDuration = 2f;

    [SerializeField] private float timer = 0f;
    private float startTiming = 15f;
    private float startTextTiming = 20f;
    private float endTiming = 25f;
    private bool isFading = false;

    void Start()
    {
        Color c = fadeImage.color;
        Color c2 = text.color;
        c.a = 0f;
        c2.a = 0f;
        fadeImage.color = c;
        text.color = c2;
        StartFadeOut();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (isFading)
        {
            if(timer > startTiming)
            {
                float alpha = Mathf.Clamp01((timer - startTiming) / fadeDuration);
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;

                if (alpha == 1f)
                {
                    isFading = false;
                }
            }
        }

        if(timer > startTextTiming)
        {
            float alpha = Mathf.Clamp01((timer - startTextTiming) / fadeDuration);
            Color c2 = text.color;
            c2.a = alpha;
            text.color = c2;
        }

        if(timer > endTiming)
        {
            if(Input.anyKeyDown)
            {
                Application.Quit();
            }
        }
    }

    private void StartFadeOut()
    {
        timer = 0f;
        isFading = true;
    }
}
