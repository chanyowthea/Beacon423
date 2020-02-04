using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : TSingleton<ResourceManager>
{
    public void ChangeScene()
    {
    }

    //public UnityEngine.Object GetResource(int id, System.Type type = null)
    //{
    //    var res = CSVLoader.Instance.GetData<ResourceCSV>(id.ToString());
    //    if (res == null)
    //    {
    //        return null;
    //    }
    //    UnityEngine.Object obj = type == null ? Resources.Load(res.Path)
    //               : Resources.Load(res.Path, type);
    //    return obj;
    //}

    public T LoadObject<T>(string path)
       where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    //public T LoadJson<T>(string path)
    //{
    //    var asset = Resources.Load<TextAsset>(path);
    //    if (asset != null)
    //    {
    //        return LitJson.JsonMapper.ToObject<T>(asset.text);
    //    }
    //    return default(T);
    //}

    public AsyncOperation ClearAssets()
    {
        return Resources.UnloadUnusedAssets();
    }

    //public GameObject LoadAndInstantiate(int resId, Transform parent = null)
    //{
    //    var res = CSVLoader.Instance.GetData<ResourceCSV>(resId.ToString());
    //    if (res == null)
    //    {
    //        return null;
    //    }
    //    return LoadAndInstantiate(res.Path, parent);
    //}

    public GameObject LoadAndInstantiate(string path, Transform parent = null)
    {
        var go = ResourceManager.Instance.LoadObject<GameObject>(path);
        if (go == null)
        {
            return null;
        }
        go = GameObject.Instantiate(go, parent);
        go.transform.localPosition = Vector3.zero;
        return go;
    }

    public T LoadAndInstantiate<T>(string path, Transform parent = null)
        where T : UnityEngine.Object
    {
        var go = ResourceManager.Instance.LoadObject<T>(path);
        if (go == null)
        {
            return null;
        }
        go = GameObject.Instantiate(go, parent);
        return go;
    }

    //public T LoadAndInstantiate<T>(int resId, Transform parent = null)
    //    where T : MonoBehaviour
    //{
    //    var res = CSVLoader.Instance.GetData<ResourceCSV>(resId.ToString());
    //    if (res == null)
    //    {
    //        return null;
    //    }
    //    var go = ResourceManager.Instance.LoadObject<T>(res.Path);
    //    if (go == null)
    //    {
    //        return null;
    //    }
    //    return GameObject.Instantiate(go, parent) as T;
    //}
}

