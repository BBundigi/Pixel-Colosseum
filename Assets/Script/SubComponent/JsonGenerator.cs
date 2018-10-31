using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using  ;

public class JsonGenerator : MonoBehaviour
{ 
    
    void Start()
    {
        SerializeAndSave();
    }

    private void GenerateInfo()
    {
        RelicInfos = new RelicInfo[3];
        for (int i = 0; i < RelicInfos.Length; i++)
        {
            RelicInfos[i] = new RelicInfo();
            RelicInfos[i].ID = (eRelicNameID)i;
        }
    }

    private void SerializeAndSave()
    {
        GenerateInfo();
        string PATH = SaveDataController.GenerateFileLocation("RelicInfo.json");
        string data = JsonConvert.SerializeObject(RelicInfos, Formatting.Indented);
        StreamWriter streamWriter = new StreamWriter(PATH);
        streamWriter.Write(data);
        streamWriter.Close();
    }

    private void LoadAndDeserialize()
    {
        string PATH = SaveDataController.GenerateFileLocation("shopInfo.json");
        StreamReader streamReader = new StreamReader(PATH);
        string data = streamReader.ReadToEnd();
        streamReader.Close();

        //infos = JsonConvert.DeserializeObject<StageInfo[]>(data);

        //Debug.Log(infos.Length);
    }
}
