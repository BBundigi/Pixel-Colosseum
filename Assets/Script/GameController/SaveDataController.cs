using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveDataController : MonoBehaviour {
    PlayerInfo UserData;

    protected void SaveGame()
    {
        //SaveData
    }

    public static string GenerateFileLocation(string file_name)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf("/"));
            return Path.Combine(Path.Combine(path, "Documents"), file_name);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf("/"));
            return Path.Combine(path, file_name);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf("/"));
            return Path.Combine(path, file_name);
        }
    }

    private IEnumerator SavePlayerData()
    {
        while(true)
        {
            SaveGame();
            yield return new WaitForSeconds(60.0f);
        }
    }
}

struct PlayerInfo
{
    int PlayerPosX;
    int PlayerPosY;//LocalPos
}

