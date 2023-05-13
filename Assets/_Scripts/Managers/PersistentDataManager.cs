using Newtonsoft.Json;
using UnityEngine;


public class PersistentDataManager : MonoBehaviour
{
    public GameData gameData;
    public bool newData;


    #region Singleton
    public static PersistentDataManager instance;
    internal readonly object gameDataFromPlayerPrefs;


    private void Awake()
    {
        GetInstance();
    }

    private void GetInstance()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        if (!newData) { LoadData(); }
        else
        {
            SaveData();
            Invoke(nameof(LoadData), 0.1f);
        }
        // PlayerPrefs.DeleteAll();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        string gameDataString = JsonConvert.SerializeObject(gameData);
        PlayerPrefs.SetString("GameData", gameDataString);
        PlayerPrefs.Save();
        print("GameData Saved In PlayerPrefs: " + PlayerPrefs.GetString("GameData"));
    }

    public void LoadData()
    {
        string gameDataString = PlayerPrefs.GetString("GameData");
        GameData gameDataFromPlayerPrefs = JsonConvert.DeserializeObject<GameData>(gameDataString);
        if (gameDataFromPlayerPrefs == null)
        {
            print("Game is played first time. No GameData found.");
            return;
        }
        print("GameData Loaded From PlayerPrefs");

        // Set Local GameData Variables Here - Start
        gameData.objectName = gameDataFromPlayerPrefs.objectName;
        // Set Local GameData Variables Here - End
    }
}
