using UnityEngine;

public class PuzzleObject : Entity, IHasInfo
{
    [SerializeField] private string _info = "Описание отсутствует";
    public string GetInfo()
    {
        return _info;
    }
}
