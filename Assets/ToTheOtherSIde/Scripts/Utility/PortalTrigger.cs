using System.Collections;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Откладываем на следующий кадр
            StartCoroutine(LoadNextLevelDelayed());
        }
    }
    
    private IEnumerator LoadNextLevelDelayed()
    {
        yield return null;
        G.GameManager.LoadNextLevel();
    }
}