using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleAnimation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime, Space.World);
    }
}
