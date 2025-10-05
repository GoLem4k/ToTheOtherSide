using System;
using Unity.VisualScripting;
using UnityEngine;

public class Pedestal : Entity
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _hasContentColor;
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform _contentT;
    [SerializeField] private Material _material;
    


    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "PuzzleObject")
        {
            _material.color = _hasContentColor;
            other.transform.position = _contentT.position;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.tag == "PuzzleObject")
        {
            _material.color = _baseColor;
        }
    }
}
