using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _container;

    protected List<GameObject> InitializeList(GameObject prefab, int count)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            var spawned = Instantiate(prefab, _container.transform);
            spawned.SetActive(false);
            gameObjects.Add(spawned);
        }
        return gameObjects;
    }

    protected bool TryGetObject(List<GameObject> templates, out GameObject result)
    {   
        result = templates.FirstOrDefault(template => template.activeSelf == false);
        
        return result != null;
    }

    protected void DisableObjects(List<GameObject> objects)
    {
        foreach (GameObject gameObjects in objects)
        {
            if (gameObjects.activeSelf == true)
                gameObjects.SetActive(false);
        }
    }
}
