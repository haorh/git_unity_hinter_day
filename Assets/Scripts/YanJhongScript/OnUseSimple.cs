using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUseSimple : ItemBehaviour
{
    [Header("On Use")]
    public float affectHP;
    public float affectMP;

    public override void OnUse()
    {
        Debug.Log("On Used");
    }

    //public override T GetImplementor<T>()
    //{
    //    return this;
    //}
}
