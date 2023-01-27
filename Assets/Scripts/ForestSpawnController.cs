using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSpawnController : ObjectPool
{
    [SerializeField] private GameObject _template;
    [SerializeField] private GameObject _arena;
    [SerializeField] private float _offsetZ;
    [SerializeField] private float _speed;
    [SerializeField] private int _objectsCount;

    private Vector3 _offsetPosition;
    private Vector3 _arenaStartPosition;
    private Vector3 _startPosition = new Vector3(0, 0, -50);
    private bool _isAbleToMove = true;
    private bool _isSpawnRequired = true;
    private bool _isArenaMoving = false;
    private List<GameObject> _forests;
    private GameObject _lastSpawnedForest;

    private void Start()
    {
        _arenaStartPosition = _arena.transform.position;
        _offsetPosition = new Vector3(0, 0, _offsetZ);
        _arenaStartPosition = _arena.transform.position;
        _forests = InitializeList(_template,_objectsCount);
    }
    
    private void Update()
    {   
        if(TryGetObject(_forests, out GameObject result) && _isSpawnRequired)
        {
            SetForest(result);
        }

        if (_isAbleToMove)
        {
            if (_isArenaMoving)
            {
                SetArena();
            }

            foreach (GameObject forest in _forests)
            {
                forest.transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
            }
        }     
    }

    public void ChangeMoveCondition(bool isAbleToMove)
    {
        _isAbleToMove = isAbleToMove;
    }

    public void SetArena()
    {
        _arena.transform.position = _lastSpawnedForest.transform.position + _offsetPosition;
        _forests.Add(_arena);
        _isSpawnRequired = false;
    }

    public void Restart()
    {
        _arena.transform.position = _arenaStartPosition;
        _forests.Remove(_arena);
        _isSpawnRequired = true;
        ChangeMoveCondition(true);
    }

    public void SetDuration(float duration)
    {
        StartCoroutine(SetTimer(duration));
    }
    
    private void SetForest(GameObject forest)
    {
        Vector3 position = _startPosition;
        
        if(_lastSpawnedForest != null)
        {
            position = _lastSpawnedForest.transform.position + _offsetPosition;           
        }
        forest.SetActive(true);
        forest.transform.position = position;
        _lastSpawnedForest = forest;
    }

    private IEnumerator SetTimer(float duration)
    {
        float elapsedTime = 0;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        SetArena();
        yield break;
    }
}
