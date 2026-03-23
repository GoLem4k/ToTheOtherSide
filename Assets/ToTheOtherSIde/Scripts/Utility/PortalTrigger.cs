using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private float teleportDelay = 1f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke("LoadNextScene", teleportDelay);
        }
    }
    
    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}