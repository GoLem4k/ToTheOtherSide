using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        G.GameManager.LoadFirstLevel();
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
