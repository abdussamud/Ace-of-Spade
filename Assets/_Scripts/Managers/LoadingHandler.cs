using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingHandler : MonoBehaviour
{
    public Slider loadingSlider;
    private string sceneName;


    private void Start()
    {
        loadingSlider.value = 0;
        sceneName = GameManager.nextScene;
        if (string.IsNullOrEmpty(sceneName)) { sceneName = "Lobby"; }
        _ = loadingSlider.DOValue(1, 5);
        _ = StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        yield return new WaitForSeconds(6);
        scene.allowSceneActivation = true;
    }
}
