using System.Collections;
using UnityEngine;

public class Trap : InteractableObject
{
    [SerializeField] private int _damage;

    private float _animationSpeed = 10;
    private float _animationPositionY = -0.2f;

    protected override void InteractWithPlayer(PlayerCollider playerCollider)
    {
        playerCollider.gameObject.GetComponentInParent<PlayerHealth>().ApplyDamage(_damage);
        StartCoroutine(DoAnimation());
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
