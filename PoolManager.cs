using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private Dictionary<Object, Queue<Object>> poolsDic = new Dictionary<Object, Queue<Object>>();
    private void Awake()
    {
        Instance = this;
    }
    public void InitPool(Object obj,int count)
    {
        if (poolsDic.ContainsKey(obj)) return;
        Queue<Object> queue = new Queue<Object>();
        for (int i = 0; i < count; i++)
        {
            Object cloneGo=Instantiate(obj);
            CreateObjectAndSetActive(cloneGo);
            queue.Enqueue(cloneGo);
        }
        poolsDic[obj] = queue;
    }
    private void CreateObjectAndSetActive(Object obj,bool active=false)
    {
        GameObject itemGo = null;
        if (obj is Component)
        {
            Component componentGo = obj as Component;
            itemGo = componentGo.gameObject;
        }
        else
        {
            itemGo = obj as GameObject;
        }
        //itemGo.transform.SetParent(transform);
        itemGo.SetActive(active);
    }
    public Queue<Object> queue;
    public T GetInstance<T>(Object obj)where T : Object
    {        
        if(poolsDic.TryGetValue(obj,out queue))
        {
            Object itemGo = null;
            if (queue.Count > 0)
                itemGo = queue.Dequeue();
            else
                itemGo = Instantiate(obj);
            CreateObjectAndSetActive(itemGo, true);
            queue.Enqueue(itemGo);
            return itemGo as T;
        }
        Debug.LogError("Not CurrentType Reources in Dic!!!");
        return null;
    }
}
