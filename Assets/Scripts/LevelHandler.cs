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
    private int _aliveEnemiesCount;
    private float _coefficientTo3Stars = 1;
    private float _coefficientTo2Stars = 0.66f;
    private float _coefficientTo1Star = 0.33f;

    public List<Level> Levels => _levels;

    public event UnityAction<int> LevelComplete;
    private enum Stars
    { 
        BadResult = 0,
        AverageResult = 1,
        GoodResult = 2,
        GreatResult = 3
    }

    public void SetLevel(int levelNumber)
    {   
        int levelIndex = levelNumber - 1;

        _forestSpawnController.SetDuration(_levels[levelIndex].RunnerPhazeDuration);
        _arena.SpawnEnemies(_enemyPrefab, _levels[levelIndex].EnemiesCount);

        foreach (var enemy in _arena.GetEnemies())
            enemy.EnemyDied += CalculateAliveEnemies;

        _currentLevel = _levels[levelIndex];
        _aliveEnemiesCount = _arena.EnemiesCount;
    }

    public void Restart()
    {   
        _arena.Clear();

        _forestSpawnController.SetDuration(_currentLevel.RunnerPhazeDuration);
        _arena.SpawnEnemies(_enemyPrefab, _currentLevel.EnemiesCount);

        foreach (var enemy in _arena.GetEnemies())
            enemy.EnemyDied += CalculateAliveEnemies;

        _aliveEnemiesCount = _arena.EnemiesCount;
    }

    public void SetToMainMenu()
    {
        _arena.Clear();
        _currentLevel = null;
    }

    public List<Enemy> GetEnemies()
    {   
        return _arena.GetEnemies();
    }

    private void CalculateAliveEnemies(Enemy enemy)
    {   
        enemy.EnemyDied -= CalculateAliveEnemies;
        _aliveEnemiesCount--;

        if (_aliveEnemiesCount == 0)
            CalculateResult();
    }
    
    public void CalculateResult()
    {        
        float currentHealth = _player.GetComponent<PlayerHealth>().Health;
        float maxHealth = _player.GetComponent<PlayerHealth>().MaxHealth;        

        int starsCount;

        if (currentHealth / maxHealth == _coefficientTo3Stars)
        {
            starsCount = (int)Stars.GreatResult;
        }
        else if(currentHealth / maxHealth > _coefficientTo2Stars && currentHealth / maxHealth < 1)
        {
            starsCount = (int)Stars.GoodResult;
        }
        else if(currentHealth / maxHealth > _coefficientTo1Star && currentHealth / maxHealth < _coefficientTo2Stars)
        {
            starsCount = (int)Stars.AverageResult;
        }
        else
        {
            starsCount = (int)Stars.BadResult;
        }
        LevelComplete?.Invoke(starsCount);
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

    //public Level GetSavingLevelData()
    //{

    //}
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

    public void SetResult(int newResult)
    {
        if (this._isCompleted && newResult < _resultInStars)
            return;

        _isCompleted = true;
        _resultInStars = newResult;
    }
}

