using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Простой UI Manager для примера
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [Header("Экраны")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject pauseMenu;
    
    [Header("Подсказки взаимодействия")]
    [SerializeField] private GameObject InteractionPrimary;
    [SerializeField] private GameObject InteractionSecondary;
    [SerializeField] private GameObject InteractionContext;
    
    [Header("Элементы UI")]
    [SerializeField] private Slider loadingProgressBar;
    [SerializeField] private Text timerText;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // public void ShowInteractions(List<InteractionType> interactions)
    // {
    //     if (interactions.Contains(InteractionType.Primary)) InteractionPrimary.SetActive(true);
    //     if (interactions.Contains(InteractionType.Secondary)) InteractionSecondary.SetActive(true);
    //     if (interactions.Contains(InteractionType.Context)) InteractionContext.SetActive(true);
    // }

    public void HideInteractions()
    {
        InteractionPrimary.SetActive(false);
        InteractionSecondary.SetActive(false);
        InteractionContext.SetActive(false);
    }
    
    public void ShowLoadingScreen()
    {
        loadingScreen?.SetActive(true);
        gameplayUI?.SetActive(false);
        pauseMenu?.SetActive(false);
    }
    
    public void HideLoadingScreen()
    {
        loadingScreen?.SetActive(false);
    }
    
    public void ShowGameplayUI()
    {
        gameplayUI?.SetActive(true);
        pauseMenu?.SetActive(false);
    }
    
    public void ShowPauseMenu(bool show)
    {
        pauseMenu?.SetActive(show);
    }
    
    public void UpdateLoadingProgress(float progress)
    {
        if (loadingProgressBar != null)
        {
            loadingProgressBar.value = progress;
        }
    }
    
    public void UpdateGameplayUI(string timeString)
    {
        if (timerText != null)
        {
            timerText.text = timeString;
        }
    }
}