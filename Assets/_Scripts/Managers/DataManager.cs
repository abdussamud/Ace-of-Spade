using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dm;
    [SerializeField]
    private bool resetData;
    [SerializeField]
    private string filePath;
    [SerializeField]
    private GameData gameData;

    private void Awake()
    {
        dm = this;
        filePath = Application.persistentDataPath + "/customData.json";
        if (!resetData) { LoadData(); }
        else { ResetData(); }
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

    public void ResetData()
    {
        gameData.userName = null;
        gameData.playerId = null;
        SaveData();
    }
}
