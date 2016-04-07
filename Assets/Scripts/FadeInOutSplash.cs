using UnityEngine;
using System.Collections;

public class FadeInOutSplash : MonoBehaviour {

    public CanvasGroup fadeGroup;
    public float fadeTime;
    public float waitTime;
    public string nextScene;
    
    void Start()
    {
        if (fadeGroup == null)
        {
            fadeGroup = GetComponent<CanvasGroup>();
        }

        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0;
            StartCoroutine(FadeSplash());
        }
    }

    private IEnumerator FadeSplash()
    {
        // Fade in.
        if (fadeTime > 0)
        {
            while (fadeGroup.alpha < 1)
            {
                fadeGroup.alpha += Time.deltaTime / fadeTime;
                yield return null;
            }
        }
        else
        {
            fadeGroup.alpha = 1;
        }

        // Wait.
        float waitElapsed = 0;
        while (waitElapsed < waitTime)
        {
            waitElapsed += Time.deltaTime;
            yield return null;
        }

        // Fade out.
        if (fadeTime > 0)
        {
            while (fadeGroup.alpha > 0)
            {
                fadeGroup.alpha -= Time.deltaTime / fadeTime;
                yield return null;
            }
        }
        else
        {
            fadeGroup.alpha = 0;
        }

        if (nextScene != null && nextScene != "")
        {
            Application.LoadLevel(nextScene);
        }

        yield return null;
    }
}
