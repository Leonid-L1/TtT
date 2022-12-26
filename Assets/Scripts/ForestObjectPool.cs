using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestObjectPool : MonoBehaviour
{
    [SerializeField] private int _forestsCount;
    [SerializeField] private GameObject _container;

    protected List<Forest> _forests = new List<Forest>();

    protected void Initialize(Forest template)
    {
        for (int i = 0; i < _forestsCount; i++)
        {
            var spawned = Instantiate(template, _container.transform);
            spawned.gameObject.SetActive(false);
            _forests.Add(spawned);
        }
    }

    protected bool TryGetForest()
    {
        var forest = _forests.FirstOrDefault(forest => forest.gameObject.activeSelf == false);
        return forest != null;    
    }
}
