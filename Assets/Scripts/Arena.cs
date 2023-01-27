using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;

    List<Enemy> _enemies = new List<Enemy>();

    public int EnemiesCount => _enemies.Count;

    public List<Enemy> SpawnEnemies(GameObject prefab,int count)
    {           
        
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(prefab, _spawnPoints[i]);
            _enemies.Add(enemy.GetComponent<Enemy>());
        }
        return _enemies;
    }

    public void Clear()
    {
        Debug.Log("arena clear");
        Debug.Log(_enemies.Count);
        foreach (var enemy in _enemies)
        {
            Destroy(enemy.gameObject);
            
            Debug.Log("enemy destroyed = " + enemy == null);
        }          

        _enemies.Clear();
    }

    public List<Enemy> GetEnemies()
    {
        return _enemies;
    }
}
