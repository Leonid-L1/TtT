using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : InteractableObject
{
    [SerializeField] private int _damage;

    private float _animationSpeed = 10;
    private float _animationPositionY = -0.2f;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent(out PlayerCollider playerCollider))
        {
            Debug.Log("trap touch");
            collider.gameObject.GetComponentInParent<Player>().ApplyDamage(_damage);
            StartCoroutine(DoAnimation());
        }
    }  
    

    private IEnumerator DoAnimation()
    {
        while(transform.position.y != _animationPositionY)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, _animationPositionY, transform.position.z), _animationSpeed * Time.deltaTime);
            yield return null;
        }
        yield break;
    }
}
