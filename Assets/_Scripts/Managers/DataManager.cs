using System.IO;
using Newtonsoft.Json;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [SerializeField]
    private string filePath;
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private bool resetData;


    private void Awake()
    {
        Instance = this;
        filePath = Application.persistentDataPath + "/savedData.json";
        if (!resetData)
        {
            LoadData();
        }
        else
        {
            SaveData();
        }
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
            GameData loadedData = JsonConvert.DeserializeObject<GameData>(json);

            gameData.userName = loadedData.userName;
            gameData.playerId = loadedData.playerId;

            Debug.Log("Data loaded from " + filePath);
        }
        else
        {
            Debug.LogWarning("Save file not found at " + filePath);
        }
    }
}
