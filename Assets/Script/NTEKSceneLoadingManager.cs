using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NTEKSceneLoadingManager : MonoBehaviour {
    [SerializeField] private Image loadingBar;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float loadCompleteDelay;

    private static string _sceneToLoad;
    private readonly MMTweenType _tween = new (MMTween.MMTweenCurve.EaseOutCubic);
    private float _fillTarget;
    
    public static void LoadScene(string sceneToLoad, string loadingSceneName)
    {
        _sceneToLoad = sceneToLoad;					
        Application.backgroundLoadingPriority = ThreadPriority.High;
        SceneManager.LoadScene(loadingSceneName);
    }

    private void Start() {
        MMFadeOutEvent.Trigger(fadeDuration, _tween);
        if(_sceneToLoad is null) return;
        
        loadingBar.fillAmount = 0;
        StartCoroutine(LoadSceneAsynchronously(_sceneToLoad));
    }

    private void Update() {
        loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, _fillTarget, Time.deltaTime * 1);
    }

    private IEnumerator LoadSceneAsynchronously(string sceneName) {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        while(asyncOperation.progress < 0.9f) {
            _fillTarget = asyncOperation.progress;

            yield return null;
        }

        _fillTarget = 1f;

        yield return new WaitForSeconds(loadCompleteDelay);
        MMFadeInEvent.Trigger(fadeDuration, _tween);
        yield return new WaitForSeconds(fadeDuration);

        asyncOperation.allowSceneActivation = true;
    }
}