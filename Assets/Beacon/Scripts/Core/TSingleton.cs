using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSingleton<T>
    where T : class, new()
{
    static T _Instance;
    public static T Instance
    {
        protected set { _Instance = value; }
        get
        {
            if (_Instance == null)
            {
                _Instance = new T();
            }
            return _Instance;
        }
    }

    public TSingleton()
    {
        Instance = this as T;
        OnInit();
    }

    ~TSingleton()
    {
        OnClear();
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnClear()
    {
    }
}
