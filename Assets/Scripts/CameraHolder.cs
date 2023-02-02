using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform _player;

    [SerializeField] private float _lerpSpeed = 3;
    [SerializeField] private float _rotationSpeed = 2;
    [SerializeField] private float _distanceFromPlayer = 5;
    [SerializeField] private float _height = 3;
    [SerializeField] private float _distanceFromTarget = 4;

    private Transform _target;
    private Vector3 _newPosition;
    private Quaternion _newRotation;
    public Vector3 StartPosition { get; private set; } = new Vector3(0,3, -30);

    private void LateUpdate()
    {
        if (_target != null)
        {
            transform.position = Vector3.Lerp(transform.position, _newPosition, _lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, _rotationSpeed * Time.deltaTime);
        }   
    }

    private void FixedUpdate()
    {
        if (_target != null)
            CalculateNewPosition();
    }

    private void OnEnable()
    {
        _player.GetComponent<Player>().DeadOnActionPhaze += ClearTarget;
    }

    private void OnDisable()
    {   
        if(_player != null)
            _player.GetComponent<Player>().DeadOnActionPhaze -= ClearTarget;
    }

    public void SetTarget(Transform enemy)
    {
        _target = enemy;
    }

    public void ClearTarget()
    {   
        if(_target != null)
            _target = null;
    }

    public void CalculateFirstPosition()
    {
        if (_target != null)
            CalculateNewPosition();
    }

    private void CalculateNewPosition()
    {
        Vector3 targetToPlayer = _target.position - _player.position;
        Vector3 directionFromTarget = targetToPlayer.normalized;
        _newPosition = new Vector3(_player.position.x + directionFromTarget.x * -_distanceFromPlayer, _height, _player.position.z + directionFromTarget.z * -_distanceFromPlayer);

        Vector3 playerToTarget = _player.position - _target.position;
        Vector3 directionFromPlayer = playerToTarget.normalized;
        Vector3 toLookPosition = new Vector3(_target.position.x + directionFromPlayer.x * -_distanceFromTarget, 0, _target.position.z + directionFromPlayer.z * -_distanceFromTarget);
        _newRotation = Quaternion.LookRotation(toLookPosition - transform.position);
    }
}
