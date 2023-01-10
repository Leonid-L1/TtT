using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;
    [SerializeField] private ForestSpawnController _forestSpawnController;
    [SerializeField] private Arena _arena;
    [SerializeField] private AttackHandler _player;
    [SerializeField] private GameObject _enemyPrefab;

    private Level _currentLevel;
    private List<Level> _completeLevels;
    private int _aliveEnemiesCount;
    private List<GameObject> _enemies;
    public List<Level> Levels => _levels;

    public event UnityAction LevelComplete;
    private enum Stars
    { 
        BadResult = 0,
        AverageResult = 1,
        GoodResult = 2,
        GreatResult = 3
    }

    private void OnEnable()
    {
        _player.TargetDied += CalculatAliveEnemies;
    }

    private void OnDisable()
    {
        _player.TargetDied -= CalculatAliveEnemies;
    }

    public void SetLevel(int levelNumber)
    {
        int levelIndex = levelNumber - 1;
        _forestSpawnController.SetDuration(_levels[levelIndex].RunnerPhazeDuration);
        _enemies = _arena.SpawnEnemies(_enemyPrefab, _levels[levelIndex].EnemiesCount);
        _currentLevel = _levels[levelIndex];
        _aliveEnemiesCount = _enemies.Count;
    }

    public void Restart()
    {
        Clear();
        _forestSpawnController.SetDuration(_currentLevel.RunnerPhazeDuration);
        _enemies = _arena.SpawnEnemies(_enemyPrefab, _currentLevel.EnemiesCount);
        _aliveEnemiesCount = _enemies.Count;
    }

    public void SetToMainMenu()
    {
        Clear();
        _currentLevel = null;
    }

    public List<GameObject> GetEnemies()
    {
        return _enemies;
    }

    private void Clear()
    {
        foreach (GameObject enemy in _enemies)
            Destroy(enemy);

        _enemies.Clear();
    }

    private void CalculatAliveEnemies()
    {
        _aliveEnemiesCount--;

        if (_aliveEnemiesCount == 0)
        {
            CalculateResult();
            LevelComplete?.Invoke(); 
        }           
    }
    
    public void CalculateResult()
    {        
        float currentHealth = _player.GetComponent<PlayerHealth>().Health;
        float maxHealth = _player.GetComponent<PlayerHealth>().MaxHealth;

        float coefficientTo3Stars = 1;
        float coefficientTo2Stars = 0.66f;
        float coefficientTo1Star = 0.33f;

        int starsCount;

        if (currentHealth / maxHealth == coefficientTo3Stars)
        {
            starsCount = (int)Stars.GreatResult;
        }
        else if(currentHealth / maxHealth > coefficientTo2Stars && currentHealth / maxHealth < 1)
        {
            starsCount = (int)Stars.GoodResult;
        }
        else if(currentHealth / maxHealth > coefficientTo1Star && currentHealth / maxHealth < coefficientTo2Stars)
        {
            starsCount = (int)Stars.AverageResult;
        }
        else
        {
            starsCount = (int)Stars.BadResult;
        }

        _currentLevel.SetResult(starsCount);
    }

    public int GetResult(int levelIndex)
    {
        return _levels[levelIndex].ResultInStars;
    }

    public int GetResult()
    {
        return _currentLevel.ResultInStars;
    }
}

[System.Serializable]

public class Level
{
    [SerializeField] private float _runnerPhazeDuration;
    [SerializeField] private int _enemiesCount;

    private bool _isCompleted;
    private int _resultInStars = 0;

    public float RunnerPhazeDuration => _runnerPhazeDuration;
    public int EnemiesCount => _enemiesCount;
    public bool IsCompleted => _isCompleted;
    public int ResultInStars => _resultInStars;

    public void SetResult(int result)
    {
        _isCompleted = true;
        _resultInStars = result;
    }
}

