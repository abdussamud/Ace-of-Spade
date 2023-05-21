using Newtonsoft.Json;
using UnityEngine;


public class PersistentDataManager : MonoBehaviour
{
    public static PersistentDataManager Instance;

    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private bool newData;
    [SerializeField]
    private string dataKey = "GameData";


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameData);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(dataKey))
        {
            GameData saveData = JsonConvert.DeserializeObject<GameData>(PlayerPrefs.GetString(dataKey));

            gameData.m_Name = saveData.m_Name;

            Debug.Log("Data Loaded!");
        }
        else
        {
            Debug.Log("First Time Data Laod!");
        }
    }

    public void SaveData()
    {
        PlayerPrefs.SetString(dataKey, JsonConvert.SerializeObject(gameData));
        PlayerPrefs.Save();
        Debug.Log("Data Saved!");
    }
}
