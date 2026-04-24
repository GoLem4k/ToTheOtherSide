using UnityEngine;

public static class G 
{
    public static AudioManager AudioManager;
    public static SceneLoader SceneLoader;
    public static GameManager GameManager;
}

[DefaultExecutionOrder(-9999)]
public static class GameBootstrapper
{
    private static bool _initialized = false;
    private static GameObject serviceHolder;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoad()
    {
        if (_initialized) return;
        
        serviceHolder = new GameObject("===Managers==="); 
        Object.DontDestroyOnLoad(serviceHolder);

        G.AudioManager = CreateSimpleService<AudioManager>();
        G.SceneLoader = CreateSimpleService<SceneLoader>();
        G.GameManager = CreateSimpleService<GameManager>();
        //G.GameState = CreateSimpleService<GameState>();
    }
    private static T CreateSimpleService<T>() where T : Component, IService
    {
        GameObject g = new GameObject(typeof(T).ToString());
        
        g.transform.parent = serviceHolder.transform;
        T t = g.AddComponent<T>();
        t.Init();
        return g.GetComponent<T>();
    }
}
public interface IService
{
    public void Init();
}