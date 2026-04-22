using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelList", menuName = "Scriptable Objects/LevelList")]
public class LevelList : ScriptableObject
{
    public List<string> levelsScenes;
}
