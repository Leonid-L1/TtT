using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;

    public List<GameObject> SpawnEnemies(GameObject prefab,int count)
    {   
        List<GameObject> enemies = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(prefab, _spawnPoints[i]);
            enemies.Add(enemy);
        }
        return enemies;
    }
}
