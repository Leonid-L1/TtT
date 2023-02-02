using UnityEngine;

public class Fireball : MonoBehaviour
{
    private bool _isTargetSet = false;
    private Transform _target;
    private float _speed = 10;
    private int _damage = 40;
    private float _offsetY = 1.5f;

    private void Update()
    {
        if (!_isTargetSet)
            return;

        Vector3 targetPosition = new Vector3(_target.position.x, _offsetY, _target.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out EnemyCollider enemyCollider))
        {
            Destroy(gameObject);
            enemyCollider.GetComponentInParent<EnemyHealth>().ApplyDamage(_damage);
        }
    }

    public void Init(Transform target)
    {
        transform.parent = null;
        _target = target;
        _isTargetSet = true;
    }
}
