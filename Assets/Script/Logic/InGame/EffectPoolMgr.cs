using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectPoolMgr : SingletonMB<EffectPoolMgr>
{
    private Dictionary<int, Queue<EffectObject>> pooledObjects;

    public List<EffectObject> objectToPool;

    private void Start()
    {
        pooledObjects = new Dictionary<int, Queue<EffectObject>>();

        for (int i = 0; i < objectToPool.Count; ++i)
        {
            Queue<EffectObject> effectObj = new Queue<EffectObject>();
            pooledObjects.Add(i, effectObj);
        }
    }

    public int GetEffectCount()
    {
        return objectToPool.Count;
    }

    public void InitEffect()
    {
        if (pooledObjects != null)
        {
            foreach (var obj in pooledObjects)
            {
                Queue<EffectObject> objQueue = obj.Value;
                objQueue.Clear();
            }
        }
    }

    public EffectObject GetPooledObject(int id, float time, Transform holder)
    {
        EffectObject ret;
        Queue<EffectObject> objQueue;
        if (pooledObjects.TryGetValue(id, out objQueue))
        {
            if (objQueue.Count > 0)
            {
                ret = objQueue.Dequeue();
            }
            else
            {
                ret = Instantiate(objectToPool[id], holder);
            }

            ret.StartEffect(id, time);
            ret.transform.SetParent(holder);
            ret.transform.position = holder.position;
            ret.gameObject.SetActive(true);
            ret.transform.rotation = Quaternion.identity;
            ret.transform.localRotation = objectToPool[id].transform.localRotation;

            return ret;
        }
       
        return null;
    }

    // 위치를 직접 입력할 경우 사용
    public EffectObject GetPooledObject(int id, float time)
    {
        EffectObject ret;
        Queue<EffectObject> objQueue;
        
        if (pooledObjects.TryGetValue(id, out objQueue))
        {
            if (objQueue.Count > 0)
            {
                ret = objQueue.Dequeue();
                if (ret == null)
                {
                    ret = Instantiate(objectToPool[id]);
                }
            }
            else
            {
                ret = Instantiate(objectToPool[id]);
            }

            ret.StartEffect(id, time);
            ret.gameObject.SetActive(true);

            return ret;
        }
        
        return null;
    }

    public void FreeObject(EffectObject obj)
    {
        obj.transform.SetParent(transform);
        Queue<EffectObject> objQueue;
        if (pooledObjects.TryGetValue(obj._id, out objQueue))
        {
            objQueue.Enqueue(obj);
            obj.gameObject.SetActive(false);
        }
    }
}