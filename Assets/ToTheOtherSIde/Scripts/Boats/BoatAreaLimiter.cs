using UnityEngine;

public class BoatAreaLimiter : MonoBehaviour
{
    [SerializeField] private Collider[] boundaryCollider;

    public void SetLocked(bool locked)
    {
        foreach (var col in boundaryCollider)
        {
            col.enabled = locked;
        }
    }
}