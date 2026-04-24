using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneLoader : MonoBehaviour, IService
{
    public string currentSceneName = null;
    public Action onLoadAction;
    
    private GameObject _fadeCanvas;
    private bool showFade;
    
    public void Init()
    {
        CreateFadeCanvas();
        
        currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.sceneLoaded += (scene, sceneMode) => onLoadAction?.Invoke();
        _ = Unfade(0.5f);
    }
    public async UniTask Load(string n, float fadeSpeed = 0.5f)
    {
        await Fade(fadeSpeed);
        LoadScene(n);
        await Unfade(fadeSpeed);
    }
    private void LoadScene(string n)
    {
        if(currentSceneName == null) return;
        SceneManager.LoadScene(n);
        currentSceneName = n;
    }
    
    private void CreateFadeCanvas()
    {
        _fadeCanvas = new GameObject("Canvas - FadeCanvas");
        DontDestroyOnLoad(_fadeCanvas);
        _fadeCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        _fadeCanvas.GetComponent<Canvas>().sortingOrder = 1000; 

        _fadeCanvas.AddComponent<GraphicRaycaster>();

        GameObject fadeImage = new GameObject("FadeImage");
        fadeImage.transform.parent = _fadeCanvas.transform;
        
        fadeImage.AddComponent<Image>().color = Color.black;

        fadeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(100000, 100000);
    }
    private async UniTask Fade(float duration)
    {
        if (_fadeCanvas == null)
        {
            await UniTask.Yield();
            return;
        }

        _fadeCanvas.GetComponentInChildren<Image>().raycastTarget = true;
        await _fadeCanvas.transform.GetChild(0).GetComponent<Image>().DOFade(1, duration)
            .AsyncWaitForCompletion().AsUniTask();
    }
    private async UniTask Unfade(float duration)
    {
        if (_fadeCanvas == null)
        {
            await UniTask.Yield();
            return;
        }
        
        await _fadeCanvas.transform.GetChild(0).GetComponent<Image>().DOFade(0, duration)
            .OnComplete(() => _fadeCanvas.GetComponentInChildren<Image>().raycastTarget = false)
            .AsyncWaitForCompletion().AsUniTask();
    }
}

