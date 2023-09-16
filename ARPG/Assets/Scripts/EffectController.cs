//===================================================
// FileName:      EventTest.cs         
// Author:        Allent Lee	
// CreateTime:    2023-01-07 20:07:41	
// E-mail:        xiaomo_lzm@163.com
// Description:   挂载在特效上，控制特效生存周期
//===================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public float lifeTime = 1f;     //生命周期
    public bool isRecycle = false;  //是否回收，对于FXManager管理的特效（范围技能、受击特效等），在失效时回收到FXManager的EffectsPool中
    //float time = 0;
    // EffectType type;
    // Transform target;
    // Vector3 targetPos;
    // Vector3 startPos;
    // Vector3 offset; //偏移量，控制子弹高度，如子弹和地面的高度
    void OnEnable()
    {
        //if (type != EffectType.Bullet)
        //{
            StartCoroutine(Run());
        //}
    }

    IEnumerator Run()
    {
        yield return new WaitForSeconds(this.lifeTime);
        this.gameObject.SetActive(false);
        //this.transform.position = startPos;
        if (isRecycle)
        {
            FXManager.Instance.RecycleToEffectsPool(this.name,this.gameObject);
        }
    }

    // internal void Init(EffectType type, Transform source, Transform target, Vector3 offset, float duration)
    // {
    //     this.type = type;
    //     this.target = target;
    //     if (duration > 0)   //如果有逻辑控制生命周期，使用逻辑生命周期，否则，使用特效挂载脚本时属性组件界面设定的生命周期
    //         this.lifeTime = duration;
    //     this.time = 0;
    //     if (type == EffectType.Bullet)
    //     {
    //         this.startPos = this.transform.position;
    //         this.offset = offset;
    //         this.targetPos = target.position + offset;
    //     }
    //     else if( type == EffectType.Hit)
    //     {
    //         this.transform.position = target.position + offset;
    //     }
    //     
    // }
    // void Update()
    // {
    //     if(type == EffectType.Bullet)
    //     {
    //         this.time += Time.deltaTime;
    //         if(this.target != null) //目标如果存在，更新目标位置，防止在子弹命中目标前目标死亡，为空
    //         {
    //             this.targetPos = this.target.position + this.offset;
    //
    //         }
    //         this.transform.LookAt(this.targetPos);  //面向目标（让子弹朝着目标飞）
    //         if (Vector3.Distance(this.targetPos,this.transform.position) < 0.5f)
    //         {
    //             Destroy(this.gameObject);
    //             return;
    //         }
    //         if(this.lifeTime > 0 && this.time >= this.lifeTime)
    //         {
    //             Destroy(this.gameObject);
    //             return;
    //         }
    //         this.transform.position = Vector3.Lerp(this.transform.position, this.targetPos, Time.deltaTime / (this.lifeTime - this.time));
    //     }
    // }
}

