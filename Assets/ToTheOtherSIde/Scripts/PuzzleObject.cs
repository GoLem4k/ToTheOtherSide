using UnityEngine;

public class PuzzleObject : PausedBehaviour, IHasInfo
{
    [SerializeField] private string _info = "Описание отсутствует";
    public string GetInfo()
    {
        return _info;
    }
}
