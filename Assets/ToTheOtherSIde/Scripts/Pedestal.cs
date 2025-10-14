using System;
using UnityEngine;

public class Pedestal : Entity
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _hasContentColor;
    [SerializeField] private Collider _collider;
    [SerializeField] private Transform _contentT;
    [SerializeField] private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "PuzzleObject")
        {
            _material.color = _hasContentColor;
            other.transform.position = _contentT.position;
            other.transform.rotation = _contentT.rotation;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "PuzzleObject" && other.transform.position.magnitude > Mathf.Epsilon)
        {
            other.transform.position = _contentT.position;
            other.transform.rotation = _contentT.rotation;
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
