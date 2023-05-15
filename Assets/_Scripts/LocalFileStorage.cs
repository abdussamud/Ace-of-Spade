using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class LocalFileStorage : MonoBehaviour
{
    [SerializeField]
    private string filePath;
    public GameVariables gameVariables;
    public GameData gameData;


    private void Start()
    {
        filePath = Application.persistentDataPath + "/savedData.dat";
        gameVariables = LoadData();
        gameData.objectName = gameVariables.name;
        gameData.objectNumber = gameVariables.number;
    }

    private void OnApplicationQuit()
    {
        gameVariables.name = gameData.objectName;
        gameVariables.number = gameData.objectNumber;
        SaveData(gameVariables);
    }

    public void SaveData(GameVariables gameVariables)
    {
        BinaryFormatter bf = new();
        FileStream file = File.Create(filePath);
        bf.Serialize(file, gameVariables);
        file.Close();

        Debug.Log("Data saved to " + filePath);
    }

    public GameVariables LoadData()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new();
            FileStream file = File.Open(filePath, FileMode.Open);

            GameVariables gameVariables = (GameVariables)bf.Deserialize(file);
            file.Close();

            Debug.Log("Data loaded from " + filePath);
            return gameVariables;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + filePath);
            return null;
        }
    }
}

