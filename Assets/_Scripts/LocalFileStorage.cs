using System.IO;
using Newtonsoft.Json;
using UnityEngine;


public class LocalFileStorage : MonoBehaviour
{
    public static LocalFileStorage _go;

    [SerializeField]
    private string filePath;
    [SerializeField]
    public GameData gameData;
    [SerializeField]
    //private string LFSKey = "GameData";


    private void Awake()
    {
        _go = this;
        filePath = Application.persistentDataPath + "/savedData.json";
        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }


    public void SaveData()
    {
        string json = JsonConvert.SerializeObject(gameData);
        File.WriteAllText(filePath, json);

        Debug.Log("Data saved to " + filePath);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData loadData = JsonConvert.DeserializeObject<GameData>(json);

            gameData.objectName = loadData.objectName;
            gameData.objectNumber = loadData.objectNumber;
            gameData.numberLoad = loadData.numberLoad;
            gameData.isLoaded = loadData.isLoaded;
            gameData.speedPoint = loadData.speedPoint;
            gameData.speedValue = loadData.speedValue;
            gameData.gameList = loadData.gameList;

            Debug.Log("Data loaded from " + filePath);
        }
        else
        {
            Debug.LogWarning("Save file not found at " + filePath);
        }
    }
}
