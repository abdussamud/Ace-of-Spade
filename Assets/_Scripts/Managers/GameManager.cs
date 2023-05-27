using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static string nextScene;
    public static bool loginSuccess;

    private void Awake()
    {
        Instance = this;
    }
}
