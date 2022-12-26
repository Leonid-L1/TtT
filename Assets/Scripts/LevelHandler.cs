using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _enemies = _arena.SpawnEnemies(_enemyPrefab,_levels[levelIndex].EnemiesCount);
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

        if(_aliveEnemiesCount == 0)
            Debug.Log("LevelComplete");
    }
}

[System.Serializable]

public class Level
{
    [SerializeField] private float _runnerPhazeDuration;
    [SerializeField] private int _enemiesCount;

    //private bool _isCompleted;
   
    public float RunnerPhazeDuration => _runnerPhazeDuration;
    public int EnemiesCount => _enemiesCount;
    //public bool IsCompleted => _isCompleted;
}
