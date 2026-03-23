using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadPreviousScene();
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            ReloadCurrentScene();
        }
    }
    
    private void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;
        
        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
            Debug.Log($"Загружена предыдущая сцена: {SceneManager.GetSceneByBuildIndex(previousSceneIndex).name}");
        }
        else
        {
            Debug.LogWarning("Это первая сцена. Нет предыдущей сцены для загрузки.");
        }
    }
    
    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Debug.Log($"Загружена следующая сцена: {SceneManager.GetSceneByBuildIndex(nextSceneIndex).name}");
        }
        else
        {
            Debug.LogWarning("Это последняя сцена. Нет следующей сцены для загрузки.");
            
        }
    }
    
    private void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
        Debug.Log($"Перезагружена текущая сцена: {currentScene.name}");
    }
}