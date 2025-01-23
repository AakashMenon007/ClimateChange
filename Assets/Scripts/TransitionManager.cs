using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class PokeButtonSceneTransitionWithFade : MonoBehaviour
{
    public string targetScene; // Name of the scene to transition to
    public CanvasGroup fadeCanvasGroup; // Canvas Group for the fade effect
    public float fadeDuration = 1f; // Duration of the fade effect

    private bool isTransitioning = false;

    // Called when the button is "pressed" or "selected"
    public void OnPokeButtonPressed()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToScene());
        }
    }

    private IEnumerator TransitionToScene()
    {
        isTransitioning = true;

        // Step 1: Fade to black
        yield return StartCoroutine(Fade(1f));

        // Step 2: Load the target scene
        SceneManager.LoadScene(targetScene);

        // Step 3: Fade back in
        yield return StartCoroutine(Fade(0f));

        isTransitioning = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
