using System;
using UnityEngine;
using System.Collections.Generic;

namespace JengaTest.Utils
{
    [Serializable]
    public class ObjectPoolCollection
    {
        public string tag;
        public ObjectPoolItem item;
    }
    [Serializable]
    public class ObjectPoolItem
    {
        public string objectToPoolPath;
        public int amountToPool;
        public bool shouldExpand;
    }

    public class ObjectPooler : BaseSingleton<ObjectPooler>
    {
        public List<ObjectPoolCollection> collection;

        private Dictionary<string, List<GameObject>> pooledObjects;
        private Dictionary<string, GameObject> rObjs;

        protected override void OnAwake()
        {
            pooledObjects = new Dictionary<string, List<GameObject>>();
            rObjs = new Dictionary<string, GameObject>();

            foreach (ObjectPoolCollection item in collection)
            {
                ObjectPoolItem collectionItem = item.item;
                List<GameObject> objs = new();
                rObjs[item.tag] = Resources.Load<GameObject>(collectionItem.objectToPoolPath);
                for (int i = 0; i < collectionItem.amountToPool; i++)
                {
                    GameObject obj = Instantiate(rObjs[item.tag]);
                    obj.SetActive(false);
                    objs.Add(obj);
                }
                pooledObjects.Add(item.tag, objs);
            }
        }
        public GameObject GetPooledObject(string tag)
        {
            GameObject obj = null;
            if (pooledObjects.TryGetValue(tag, out List<GameObject> objs))
            {
                obj = objs.Find(a => !a.activeInHierarchy);
                if (obj == null)
                {
                    ObjectPoolCollection pool = collection.Find(a => a.tag == tag);
                    if (pool.item.shouldExpand)
                    {
                        obj = Instantiate(rObjs[tag]);
                        objs.Add(obj);
                    }
                }
                else
                {
                    obj.SetActive(true);
                }
            }
            else
            {
                throw new Exception("No Pool With This Tag");
            }
            return obj;
        }
    }
}