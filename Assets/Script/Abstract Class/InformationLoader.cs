using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public abstract class InformationLoader : MonoBehaviour
{
    protected bool loadEnd;

    public bool IsLoaded()
    {
        return loadEnd;
    }

    protected void Load<T>(out T ret, string path)
    {
        string data = (Resources.Load(path, typeof(TextAsset)) as TextAsset).text;
        ret = JsonConvert.DeserializeObject<T>(data);
        loadEnd = true;
    }
}


