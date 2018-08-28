using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataController : MonoBehaviour {
    PlayerInfo UserData;

    protected void SaveGame()
    {
        //SaveData
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

