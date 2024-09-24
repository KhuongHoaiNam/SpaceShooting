using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPooling : SingletonMono<UIPooling>
{
    public string strRoot;// Vi tri chua cac prefab
    public List<PoolingView> listUiPooling;
    public Transform tfParent;


    public void OnSpawnerView()
    {
        listUiPooling.ForEach(x => { 
        
            if(x.preInstance)
            {
                Spawner(x);
            }
        });
    }
    

    public Transform Spawner(string prefabNames)
    {
        var pool = listUiPooling.Find(x => x.prefabName.Contains(prefabNames));
        if (pool != null) {
            if (!pool.rfSpawner)
            {
                Spawner(pool);
            }
            if (pool.rfSpawner) { 
            
                pool.rfSpawner.gameObject.SetActive(true);
            }
            return pool.rfSpawner;
        }
        return null;
    }

    //sinh ra theo loadresource
    public void Spawner(PoolingView uiPooling)
    {
        var resource = Resources.Load<Transform>($"{strRoot}/{uiPooling.prefabName}");
        if (resource)
        {
            uiPooling.rfSpawner = Instantiate(resource, this.gameObject.transform.position, Quaternion.identity, tfParent);
            uiPooling.rfSpawner.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Khong tim thay Gameobject");
        }
    }
    public void DeSpawner(Transform instance)
    {
        instance.gameObject.SetActive(false);
    }
}
[Serializable]
public class PoolingView
{
    public string prefabName;
    internal Transform rfSpawner;
    public bool preInstance;
}
