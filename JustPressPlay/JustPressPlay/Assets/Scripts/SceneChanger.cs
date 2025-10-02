using System;
using UnityEngine;
using UnityEngine.Events;

public class SceneChanger : MonoBehaviour
{
    public static UnityEvent<string> eChangeSceneRequested = new UnityEvent<string>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        NewLevelPanel.eNextLevelSelected?.AddListener(ChangeLevel);
    }
    private void OnDisable()
    {
        NewLevelPanel.eNextLevelSelected?.RemoveListener(ChangeLevel);
    }
    public void ChangeScene(string sceneName)
    {
        eChangeSceneRequested?.Invoke(sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    void ChangeLevel(int level)
    {
        ChangeScene("Level" + level);
    }
}
