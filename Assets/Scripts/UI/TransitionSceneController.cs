using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    const string TransitionSceneName = "TransitionScene"; // must match the scene's name

    [SerializeField] CanvasGroup group;
    [SerializeField] float fadeInTime = 5f;
    [SerializeField] float fadeOutTime = 2f;

    static string nextLevel;
    static bool froze;

    public static void TransitionScene(string next, bool freezeTime = false)
    {
        if (SceneManager.GetSceneByName(TransitionSceneName).isLoaded) return; // already transitioning

        nextLevel = next;
        froze = freezeTime;
        if (freezeTime) Time.timeScale = 0f;

        SceneManager.LoadSceneAsync(TransitionSceneName, LoadSceneMode.Additive);
    }

    IEnumerator Start()
    {
        // Force this canvas to draw above every other canvas and camera, including the next level's
        Canvas canvas = group.GetComponentInParent<Canvas>().rootCanvas;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32767; // max allowed

        group.alpha = 0f;
        group.blocksRaycasts = true; // block clicks on the level underneath

        // The transition scene loaded additively, so the active scene is still the level we're leaving
        Scene oldScene = SceneManager.GetActiveScene();

        // 1. Fade to black
        Debug.Log(fadeInTime);
        yield return group.DOFade(1f, fadeInTime).SetUpdate(true).WaitForCompletion();
        

        // 2. Swap levels while the screen is covered
        SceneManager.SetActiveScene(gameObject.scene);
        yield return SceneManager.UnloadSceneAsync(oldScene);
        yield return SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextLevel));

        if (froze) Time.timeScale = 1f;

        // 3. Fade out, then remove this scene
        Debug.Log(fadeOutTime);
        yield return group.DOFade(0f, fadeOutTime).SetUpdate(true).WaitForCompletion();
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}