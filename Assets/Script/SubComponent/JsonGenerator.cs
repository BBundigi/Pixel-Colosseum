using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JsonGenerator : MonoBehaviour
{
    ItemInfo[] ItemInfos;
    void Start()
    {
        SerializeAndSave();
    }

    private void GenerateInfo()
    {
        ItemInfos = new ItemInfo[2];
        for (int i = 0; i < ItemInfos.Length; i++)
        {
            ItemInfos[i] = new ItemInfo();
        }

        ItemInfos[0].ItemID = eItemID.FireFlask;
        ItemInfos[1].ItemID = eItemID.PoisonFlask;
    }

    private void SerializeAndSave()
    {
        GenerateInfo();
        string PATH = SaveDataController.GenerateFileLocation("ItemInfos.json");
        string data = JsonConvert.SerializeObject(ItemInfos, Formatting.Indented);
        StreamWriter streamWriter = new StreamWriter(PATH);
        streamWriter.Write(data);
        streamWriter.Close();
    }
}

