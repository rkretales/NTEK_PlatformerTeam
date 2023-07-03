using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class NTEKSceneLoader : MonoBehaviour {
    [Header("Bindings")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string loadingSceneName;
    [Header("Settings")]
    [SerializeField] private float fadeDuration;
    
    private readonly MMTweenType _tween = new (MMTween.MMTweenCurve.EaseOutCubic);

    public void LoadScene() {
        StartCoroutine(EnterLoadingScene());
    }

    private IEnumerator EnterLoadingScene() {
        MMFadeInEvent.Trigger(fadeDuration, _tween);
        yield return new WaitForSeconds(fadeDuration);
        NTEKSceneLoadingManager.LoadScene(sceneToLoad, loadingSceneName);
    }
}