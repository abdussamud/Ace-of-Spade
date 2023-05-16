using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class LocalFileStorage : MonoBehaviour
{
    public static LocalFileStorage _go;

    [SerializeField]
    private string filePath;
    [SerializeField]
    public GameVariables dataToTest;


    private void Awake()
    {
        _go = this;
        filePath = Application.persistentDataPath + "/savedData.dat";
        dataToTest = LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData(dataToTest);
    }

    public void SaveDataOut()
    {
        SaveData(dataToTest);
    }

    public void SaveData(GameVariables dataToTest)
    {
        BinaryFormatter bf = new();
        FileStream file = File.Create(filePath);
        bf.Serialize(file, dataToTest);
        file.Close();

        Debug.Log("Data saved to " + filePath);
    }

    public GameVariables LoadData()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new();
            FileStream file = File.Open(filePath, FileMode.Open);

            GameVariables dataToTest = (GameVariables)bf.Deserialize(file);
            file.Close();

            Debug.Log("Data loaded from " + filePath);
            return dataToTest;
        }
        else
        {
            Debug.LogWarning("Save file not found at " + filePath);
            return null;
        }
    }
}

[Serializable]
public class DataToTest
{
    public int Number;
    public string Name;
    public bool IsSaved;
}
