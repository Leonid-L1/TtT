using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Forest : MonoBehaviour
{
    private float _destroyPositionZ = -150;

    private void Update()
    {
        if (transform.position.z <= _destroyPositionZ )
        {
            gameObject.SetActive(false);
        }
    }
}
