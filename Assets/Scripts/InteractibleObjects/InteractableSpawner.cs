using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSpawner : ObjectPool
{
    [SerializeField] private Trap _trapTemplate;
    [SerializeField] private int _trapsCount;
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private int _coinsCount;
    [SerializeField] private Gem _gemPrefab;
    [SerializeField] private int _gemsCount;
    [SerializeField] private float _gemSpawnProbability;
    [SerializeField] private float _spawnDelay;

    private Vector3[] _collectibleSpawnPositions = new Vector3[3] { new Vector3(3, 2, 30), new Vector3(0, 2, 30), new Vector3(-3, 2, 30) };
    private Vector3[] _trapSpawnPositions  = new Vector3[3] { new Vector3(3, -1.4f, 30), new Vector3(0, -1.4f, 30), new Vector3(-3, -1.4f, 30) };

    private float _elapsedTime;
    private bool _isSpawnRequired;
    private bool _isMoveRequired;

    private List<GameObject> _traps;
    private List<GameObject> _coins;
    private List<GameObject> _gems;

    private void Start()
    {
        _traps = InitializeList(_trapTemplate.gameObject, _trapsCount);
        _coins = InitializeList(_coinPrefab.gameObject, _coinsCount);
        _gems = InitializeList(_gemPrefab.gameObject, _gemsCount);
    }

    private void Update()
    {
        SetMoveCondition();
        if (!_isSpawnRequired)
            return;

        _elapsedTime += Time.deltaTime;
        int currentProbability = Random.Range(0, 100);

        if (_elapsedTime >= _spawnDelay)
        {
            if(TryGetObject(_traps, out GameObject trap))
                SetTrap(trap);

            if (currentProbability <= _gemSpawnProbability && TryGetObject(_gems, out GameObject gem))
            {
                SetCollectible(gem);
            }
            else if (TryGetObject(_coins, out GameObject coin))
            {
                SetCollectible(coin);
            }
            _elapsedTime = 0;
        }
    }

    public void SetSpawnCondition(bool isSpawnRequired)
    {
        _isSpawnRequired = isSpawnRequired;
    }

    public void DisableAllInteractables()
    {
        DisableObjects(_traps);
        DisableObjects(_coins);
        DisableObjects(_gems);
    }

    private void SetCollectible(GameObject Collectible)
    {
        Collectible.SetActive(true);
        Collectible.transform.position = _collectibleSpawnPositions[Random.Range(0, _collectibleSpawnPositions.Length)];
    }

    private void SetTrap(GameObject trap)
    {
        trap.SetActive(true);
        trap.transform.position = _trapSpawnPositions[Random.Range(0, _trapSpawnPositions.Length)];
    }

    public void ChangeMoveCondition(bool isMoveRequied)
    {
        _isMoveRequired = isMoveRequied;
    }

    public void SetMoveCondition()
    {
        foreach  (GameObject coin in _coins)
        {
            coin.GetComponent<InteractableObject>().SetMoveCondition(_isMoveRequired);
        }

        foreach (GameObject gem in _gems)
        {
            gem.GetComponent<InteractableObject>().SetMoveCondition(_isMoveRequired);
        }

        foreach (GameObject trap in _traps)
        {
            trap.GetComponent<InteractableObject>().SetMoveCondition(_isMoveRequired);
        }
    }
}
