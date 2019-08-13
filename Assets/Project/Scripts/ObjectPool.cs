using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int createOnStart;

    private readonly List<GameObject> _pooledObjects = new List<GameObject>();

    public GameObject GetObject()
    {
        // NOTE: Race conditions are not possible as long as the method is called
        //       from Unity's methods such as Update().
        var obj = _pooledObjects.Find(x => x.activeInHierarchy == false);
        if (obj == null)
        {
            obj = CreateObjectAndAddToPool();
        }

        obj.SetActive(true);
        return obj;
    }

    private void Awake()
    {
        _pooledObjects.Capacity = createOnStart;
        for (var i = 0; i < createOnStart; ++i)
        {
            CreateObjectAndAddToPool();
        }
    }

    private GameObject CreateObjectAndAddToPool()
    {
        var @object = Instantiate(objectPrefab);
        @object.SetActive(false);
        _pooledObjects.Add(@object);
        return @object;
    }

}
