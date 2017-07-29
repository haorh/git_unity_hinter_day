using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnUse
{
    void OnUse();

    //T GetImplementor<T>();
}

//[DisallowMultipleComponent]
public abstract class ItemBehaviour : MonoBehaviour,IOnUse {

    public abstract void OnUse();

    //public abstract T GetImplementor<T>();
    //https://stackoverflow.com/questions/1771741/how-to-force-sub-classes-to-implement-a-method
}
