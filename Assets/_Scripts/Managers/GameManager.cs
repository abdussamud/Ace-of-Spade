using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public static string nextScene;
    public static bool loginSuccess;
    public static Mode mode;

    private void Awake()
    {
        gm = this;
    }
}
