//===================================================
// FileName:      TaskManager.cs         
// Created:       Allent Lee	
// CreateTime:    2022-09-05 22:59:20	
// E-mail:        xiaomo_lzm@163.com
// Description:   全局特效管理器，使用对象池管理范围和受击等位置有关特效
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class FXManager : MonoSingleton<FXManager>
{
    public GameObject[] prefabs;
    private Dictionary<string, GameObject> Effects = new Dictionary<string, GameObject>();  //管理特效预制体
    private Dictionary<string, Queue<GameObject>> EffectsPool = new Dictionary<string, Queue<GameObject>>();  //特效池，管理已实例化过的特效

    private void Start()
    {
        for(int i=0; i<prefabs.Length;i++)
        {
            prefabs[i].SetActive(false);
            this.Effects[this.prefabs[i].name] = this.prefabs[i];
        }
    }
        
    EffectController CreateEffect(string name,Transform pos)
    {
        GameObject prefab;
        if(this.Effects.TryGetValue(name,out prefab))
        {
            GameObject go = Instantiate(prefab, this.transform, true);
            go.name = name;
            go.transform.position = pos.position;
            return go.GetComponent<EffectController>();
        }
        return null;
    }

    EffectController GetEffectFromEffectsPool(string name,Transform pos)
    {
        Queue<GameObject> queue = null;
        if(!this.EffectsPool.TryGetValue(name,out queue))
        {
            return CreateEffect(name,pos);
        }

        if (queue.Count == 0)
        {
            return CreateEffect(name,pos);
        }
        
        GameObject effect = queue.Dequeue();
        effect.transform.position = pos.position;
        return effect.GetComponent<EffectController>();
    }

    public void RecycleToEffectsPool(string name,GameObject effect)
    {
        Queue<GameObject> queue = null;
        if(!this.EffectsPool.TryGetValue(name,out queue))
        {
            queue = new Queue<GameObject>();
            EffectsPool.Add(name, queue);
        }
        
        // 一种对象最大为1000个
        if (queue.Count > 1000)
        {
            return;
        }
        
        queue.Enqueue(effect);
    }
    
    internal void PlayEffect(string name,Transform targetPos)
    {
        //先从特效池中获取，没有再实例化
        EffectController effect = GetEffectFromEffectsPool(name, targetPos);
        if(effect == null)
        {
            Debug.LogErrorFormat("Effect：{0} not Found",name);
            return;
        }
        //effect.Init(this.transform, target, pos, duration);
        effect.gameObject.SetActive(true);
    }
}

