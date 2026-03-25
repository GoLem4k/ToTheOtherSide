using UnityEngine;
using UnityEngine.SceneManagement;

public class WaterTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
            Debug.Log($"Перезагружена текущая сцена: {currentScene.name}");
        }

        if (other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
    }
}
