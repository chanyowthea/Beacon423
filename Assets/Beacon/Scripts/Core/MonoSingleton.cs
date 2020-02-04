using UnityEngine;
using System.Collections;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { protected set; get; }

    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this as T; 
        OnInit();
    }

    protected virtual void OnInit()
    {

    }

    protected virtual void OnClear()
    {

    }

    protected void OnDestroy()
    {
        if (Instance != null && Instance.GetHashCode() == this.GetHashCode())
        {
            OnClear();
            Instance = null;
        }
    }
}